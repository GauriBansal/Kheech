using Kheech.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Kheech.Web.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using Kheech.Web.Clients;
using System.Threading.Tasks;
using System.Configuration;
using Humanizer;

namespace Kheech.Web.Controllers
{
    [RoutePrefix("Friends")]
    public class FriendController : Controller
    {
        protected readonly ApplicationDbContext _context;

        public FriendController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Friend
        [Route("", Name = "FriendsHome")]
        public async Task<ActionResult> Index()
        {
            var currentUserId = User.Identity.GetUserId();

            var friends = await _context.Friendships
                                        .Include(f => f.Initiator)
                                        .Include(f => f.Recipient)
                                        .Include(f => f.FriendshipStatus)
                                        .Where(f => (f.InitiatorId == currentUserId || f.RecipientId == currentUserId) && f.FriendshipStatusId == 2).Distinct().ToListAsync();

            var friendsIndexViewModel = new FriendsIndexViewModel
            {
                FriendsCount = friends.Count,
                FriendViewModels = new List<FriendViewModel>()
            };

            foreach (var item in friends)
            {
                var friendshipLength = await _context.Friendships.Where(f => f.Id == item.Id)
                                                                 .Select(f => f.InsertDate)
                                                                 .FirstOrDefaultAsync();
                TimeSpan difference = (DateTime.UtcNow - friendshipLength);
                if (item.RecipientId == currentUserId)
                {
                    var friendViewModel1 = new FriendViewModel
                    {
                        Id = item.InitiatorId,
                        Name = item.Initiator.FirstName.Humanize() + " " + item.Initiator.LastName.Humanize(),
                        FriendshipLength = string.Format(
                                                         "{0} days, {1} hours",
                                                         difference.Days,
                                                         difference.Hours)
                    };
                    friendsIndexViewModel.FriendViewModels.Add(friendViewModel1);
                }
                else
                {
                    var friendViewModel = new FriendViewModel
                    {
                        Id = item.RecipientId,
                        Name = item.Recipient.FirstName.Humanize() + " " + item.Recipient.LastName.Humanize(),
                        FriendshipLength = string.Format(
                                                         "{0} days, {1} hours",
                                                         difference.Days,
                                                         difference.Hours)
                    };
                    friendsIndexViewModel.FriendViewModels.Add(friendViewModel);
                }
            }
            return View(friendsIndexViewModel);
        }

        [Route("Details/{friendId}", Name = "FriendsDetail")]
        public async Task<ActionResult> Details(string friendId)
        {
            var currentUserId = User.Identity.GetUserId();
            if (friendId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var friend = await _context.Friendships.Include(f => f.Initiator)
                                             .Include(f => f.Recipient)
                                             .FirstOrDefaultAsync(f => ((f.InitiatorId == currentUserId && f.RecipientId == friendId) ||
                                                                   (f.InitiatorId == friendId && f.RecipientId == currentUserId)) &&
                                                                   (f.FriendshipStatusId == 2));
            if (friend == null)
            {
                return HttpNotFound();
            }

            var friendinformation = await _context.Users.FirstOrDefaultAsync(u => u.Id == friendId);
            var friendActivity = await _context.KheechUsers.Include(k => k.ApplicationUser)
                                                      .Include(k => k.KheechEvent.Location)
                                                      .Include(k => k.KheechEvent.Group)
                                                      .Include(k => k.KheechEvent.ApplicationUser)
                                                      .Where(k => (k.ApplicationUserId == friendId))
                                                      .ToListAsync();

            var commonActivity = await _context.KheechUsers.Include(k => k.ApplicationUser)
                                         .Include(k => k.KheechEvent.Location)
                                         .Include(k => k.KheechEvent.Group)
                                         .Include(k => k.KheechEvent.ApplicationUser)
                                         .Where(k => ((k.ApplicationUserId == friendId) && (k.KheechEvent.ApplicationUserId == currentUserId)))
                                         .ToListAsync();

            var friendsDetailViewModel = new FriendsDetailViewModel
            {
                FriendInformation = new FriendViewModel
                {
                    Id = friendId,
                    Name = friendinformation.FirstName.Humanize() + " " + friendinformation.LastName.Humanize()
                },
                FriendActivity = friendActivity,
                CommonActivity = commonActivity
            };

            return View(friendsDetailViewModel);
        }

        [Route("Create", Name = "InviteAFriend")]
        public async Task<ActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Create", Name = "InviteAFriendPost")]
        public async Task<ActionResult> Create(InviteFriend model)
        {
            var currentUserId = User.Identity.GetUserId();
            var currentUserName = User.Identity.GetUserName();

            if (currentUserName == model.Email)
            {
                TempData["InviteMessage"] = $"This is you {model.Email} and already a member of Kheech.";
                return View();
            }

            var invitedFriendId = await _context.Users.Where(u => u.Email == model.Email).Select(u => u.Id).FirstOrDefaultAsync();

            var isAlreadyFriend = await _context.Friendships.Include(f => f.Recipient)
                                                .Include(f => f.Initiator)
                                                .FirstOrDefaultAsync(f => (f.InitiatorId == currentUserId && f.RecipientId == invitedFriendId) ||
                                                            (f.InitiatorId == invitedFriendId && f.RecipientId == currentUserId));
            if (isAlreadyFriend != null)
            {
                TempData["InviteMessage"] = $"You are already friends with {model.Email}";
                return View();
            }

            var invite = new InviteFriend
            {
                ApplicationUserId = currentUserId,
                Email = model.Email,
                FriendshipStatusId = 1,
                InsertDate = DateTime.UtcNow
            };

            _context.InviteFriends.Add(invite);
            await _context.SaveChangesAsync();

            var subject = "Invitation to Kheech - Breaking Bread";
            var message = $"you are invited to join the best app to achedule a meeting with friends. Please click the link below: {ConfigurationManager.AppSettings["SiteUrlBase"]}";

            var sendGridClient = new SendGridEmailClient(ConfigurationManager.AppSettings["SendGridApiKey"]);
            await sendGridClient.SendEmailAsync(model.Email, subject, message);

            TempData["InviteMessage"] = "Your invite has been sent. Do you wanna invite more?";
            return View();
        }

        [HttpPost]
        [Route("InviteFriend", Name = "InviteAFriendAjax")]
        public async Task<JsonResult> InviteFriend(InviteFriend model)
        {
            var currentUserId = User.Identity.GetUserId();
            var currentUserName = User.Identity.GetUserName();

            if (currentUserName == model.Email)
            {
                TempData["InviteMessage"] = $"This is you {model.Email} and already a member of Kheech.";
                return Json(new { message = TempData["InviteMessage"] });
            }

            var invitedFriendId = await _context.Users.Where(u => u.Email == model.Email).Select(u => u.Id).FirstOrDefaultAsync();

            var isAlreadyFriend = await _context.Friendships.Include(f => f.Recipient)
                                                .Include(f => f.Initiator)
                                                .FirstOrDefaultAsync(f => (f.InitiatorId == currentUserId && f.RecipientId == invitedFriendId) ||
                                                            (f.InitiatorId == invitedFriendId && f.RecipientId == currentUserId));
            if (isAlreadyFriend != null)
            {
                TempData["InviteMessage"] = $"You are already friends with {model.Email}";
                return Json(new { message = TempData["InviteMessage"] });
            }

            var invite = new InviteFriend
            {
                ApplicationUserId = currentUserId,
                Email = model.Email,
                FriendshipStatusId = 1,
                InsertDate = DateTime.UtcNow
            };

            _context.InviteFriends.Add(invite);
            await _context.SaveChangesAsync();

            var subject = "Invitation to Kheech - Breaking Bread";
            var message = $"you are invited to join the best app to achedule a meeting with friends. Please click the link below: {ConfigurationManager.AppSettings["SiteUrlBase"]}";

            var sendGridClient = new SendGridEmailClient(ConfigurationManager.AppSettings["SendGridApiKey"]);
            await sendGridClient.SendEmailAsync(model.Email, subject, message);

            TempData["InviteMessage"] = "Your invite has been sent.";
            return Json(new { result = true, message = TempData["InviteMessage"] });
        }

        [Route("PendingFriendRequests", Name = "PendingFriendRequests")]
        public async Task<ActionResult> PendingFriendRequests()
        {
            var currentUserId = User.Identity.GetUserId();
            var currentUserName = User.Identity.GetUserName();

            var pendingFriends = await _context.InviteFriends.Include(f => f.ApplicationUser)
                        .Include(f => f.FriendshipStatus)
                        .Where(f => f.Email == currentUserName)
                        .ToListAsync();

            if (pendingFriends.Count != 0)
            {
                return View(pendingFriends);
                //foreach (var friend in pendingFriends)
                //{
                //    if (friend.FriendshipStatus.status == "Pending")
                //    {
                //    }
                //}
            }
            return View(pendingFriends);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("PendingFriendRequests", Name = "PendingFriendRequestsPost")]
        public async Task<ActionResult> PendingFriendRequests(int pendingFriendId, bool isAccepted)
        {
            var currentUserId = User.Identity.GetUserId();
            var currentUserName = User.Identity.GetUserName();

            var pendingFriend = await _context.InviteFriends.Include(f => f.FriendshipStatus).FirstOrDefaultAsync(i => i.Id == pendingFriendId);
            if (pendingFriend == null)
            {
                return HttpNotFound();
            }

            if (isAccepted == true)
            {
                var pendingFriendship = new Friendship
                {
                    InitiatorId = pendingFriend.ApplicationUserId,
                    RecipientId = currentUserId,
                    FriendshipStatusId = 2,
                    InsertDate = DateTime.UtcNow
                };

                _context.Friendships.Add(pendingFriendship);
                _context.InviteFriends.Remove(pendingFriend);
            }
            else
            {
                var pendingFriendship1 = new Friendship
                {
                    InitiatorId = pendingFriend.ApplicationUserId,
                    RecipientId = currentUserId,
                    FriendshipStatusId = 3,
                    InsertDate = DateTime.UtcNow
                };

                _context.Friendships.Add(pendingFriendship1);
                _context.InviteFriends.Remove(pendingFriend);
            }

            await _context.SaveChangesAsync();

            return RedirectToRoute("HomePage");
        }
    }
}