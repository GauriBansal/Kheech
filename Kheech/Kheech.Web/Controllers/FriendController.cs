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
        public ActionResult Index()
        {
            var currentUserId = User.Identity.GetUserId();
            
            var friends = _context.Friendships
                        .Include(f => f.Initiator)
                        .Include(f => f.Recipient)
                        .Include(f => f.FriendshipStatus)
                        .Where(f => (f.InitiatorId == currentUserId || f.RecipientId == currentUserId) && f.FriendshipStatusId == 1).Distinct().ToList();

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
        public ActionResult Details(string friendId)
        {
            var currentUserId = User.Identity.GetUserId();
            if (friendId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var friend = _context.Friendships.Include(f => f.Initiator)
                                             .Include(f => f.Recipient)
                                             .FirstOrDefault(f => ((f.InitiatorId == currentUserId && f.RecipientId == friendId) ||
                                                                   (f.InitiatorId == friendId && f.RecipientId == currentUserId)) &&
                                                                   (f.FriendshipStatusId == 1));            
            if (friend == null)
            {
                return HttpNotFound();
            }
            
            var friendinformation = _context.Users.FirstOrDefault(u => u.Id == friendId);
            var friendActivity = _context.KheechUsers.Include(k => k.ApplicationUser)
                                                      .Include(k => k.KheechEvent.Location)
                                                      .Include(k => k.KheechEvent.Group)
                                                      .Include(k => k.KheechEvent.ApplicationUser)
                                                      .Where(k => k.ApplicationUserId == friendId && k.KheechEvent.ApplicationUserId != currentUserId)
                                                      .ToList();
         
            var commonActivity = _context.KheechUsers.Include(k => k.ApplicationUser)
                                         .Include(k => k.KheechEvent.Location)
                                         .Include(k => k.KheechEvent.Group)
                                         .Include(k => k.KheechEvent.ApplicationUser)
                                         .Where(k => k.ApplicationUserId == friendId && k.IsAccepted == true && k.KheechEvent.ApplicationUserId == currentUserId)
                                         .ToList();

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
        public ActionResult Create()
        {
            return View();
        }
        
        [Route("Create", Name = "InviteAFriendPost")]
        public ActionResult Create(Invitation model)
        {
            using (MailMessage mm = new MailMessage(model.Email, model.To))
            {
                mm.Subject = model.Subject;
                mm.Body = model.Body;
                mm.IsBodyHtml = false;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(model.Email, model.Password);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                    ViewBag.Message = "Email sent.";
                }
            }
            return View();
        }

    }
}
