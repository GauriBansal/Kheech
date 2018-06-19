using Kheech.Web.Models;
using Kheech.Web.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Kheech.Web.Controllers
{
    [RoutePrefix("Kheech")]
    [Authorize]
    public class KheechEventController : Controller
    {
        protected readonly ApplicationDbContext _context;

        public KheechEventController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: KheechEvent
        [Route("", Name = "HomePage")]
        public ActionResult Index()
        {
            var currentUserId = User.Identity.GetUserId();

            var kheechEvents = _context.KheechEvents.Include(k => k.ApplicationUser).Where(k => k.ApplicationUserId == currentUserId).ToList();
            
            if (kheechEvents.Count == 0)
            {
                ViewBag.Message = "You do not have any Kheech at the moment. Would you like to create?";
            }
            return View(kheechEvents);
        }

        [Route("schedule", Name = "ScheduleMeeting")]
        public ActionResult Schedule()
        {
            var currentUserId = User.Identity.GetUserId();
            //var friendUser = new ApplicationUser();

            var scheduleViewModel = new ScheduleViewModel();
            
            scheduleViewModel.Friends = Enumerable.Empty<Friendship>();
            
            // _context.Friendships.Where(f => f.ApplicationUserId1 == currentUserId).ToList();

            //foreach (var friend in friends)
            //{
            //    friendUser = _context.Users.FirstOrDefault(x => x.Id == friend.ApplicationUserId2);
            //    friend.ApplicationUser2 = friendUser;
            //}

            return View(scheduleViewModel);
        }

        [HttpPost]
        [Route("schedule/{id}", Name = "ScheduleMeetingPost")]
        public ActionResult Schedule(int id, ScheduleViewModel model)
        {
            var currentUserId = User.Identity.GetUserId();
            var kheechEvent = new KheechEvent();

            kheechEvent.EventName = model.EventName;
            kheechEvent.ApplicationUserId = currentUserId;
            kheechEvent.StartDate = model.WhenToMeet;
            kheechEvent.IsGroupEvent = false;

            var location = _context.Locations.Where(l => l.Name == model.WhereToMeet).Take(1).ToList();

            kheechEvent.LocationId = location[0].Id;

            _context.KheechEvents.Add(kheechEvent);
            _context.SaveChanges();

            return RedirectToRoute("HomePage");
        }

        [Route("detail/{id}", Name = "KheechDetails")]
        public ActionResult Detail(int id)
        {
            var kheechEvent = _context.KheechEvents.FirstOrDefault(k => k.Id == id);

            return View(kheechEvent);
        }
    }
}
