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
                if (item.RecipientId == currentUserId)
                {
                var friendViewModel1 = new FriendViewModel
                    {
                        Id = item.InitiatorId,
                        Name = item.Initiator.FirstName + " " + item.Initiator.LastName
                    };
                    friendsIndexViewModel.FriendViewModels.Add(friendViewModel1);
                }
                else
                {
                    var friendViewModel = new FriendViewModel
                    {
                        Id = item.RecipientId,
                        Name = item.Recipient.FirstName + " " + item.Recipient.LastName
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
                                                      .Where(k => k.ApplicationUserId == friendId && k.KheechEvent.ApplicationUserId != currentUserId)
                                                      .ToListAsync();
         
            var commonActivity = await _context.KheechUsers.Include(k => k.ApplicationUser)
                                         .Include(k => k.KheechEvent.Location)
                                         .Include(k => k.KheechEvent.Group)
                                         .Include(k => k.KheechEvent.ApplicationUser)
                                         .Where(k => k.ApplicationUserId == friendId && k.IsAccepted == true && k.KheechEvent.ApplicationUserId == currentUserId)
                                         .ToListAsync();

            var friendsDetailViewModel = new FriendsDetailViewModel
            {
                FriendInformation = new FriendViewModel
                {
                    Id = friendId,
                    Name = friendinformation.FirstName + " " + friendinformation.LastName
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
    }
}
