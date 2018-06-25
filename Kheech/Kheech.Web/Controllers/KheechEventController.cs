﻿using Kheech.Web.Models;
using Kheech.Web.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;

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

            var kheechIndexViewModel = new KheechIndexViewModel();
            
            kheechIndexViewModel.ActiveKheechEvents = _context.KheechEvents.Include(k => k.ApplicationUser)
                                                    .Include(k => k.Location)
                                                    .Include(k => k.Group)
                                                    .Where(k => k.ApplicationUserId == currentUserId && k.EndDate > DateTime.UtcNow)
                                                    .Take(5)
                                                    .ToList();
            
            if (kheechIndexViewModel.ActiveKheechEvents.Count() == 0)
            {
                ViewBag.Message = "You do not have any Kheech at the moment. Would you like to create?";
            }
            
            kheechIndexViewModel.RecentSchedules = _context.KheechEvents.Include(k => k.ApplicationUser)
                                                    .Where(k => k.ApplicationUserId == currentUserId && k.EndDate <= DateTime.UtcNow)
                                                    .OrderByDescending(k => k.EndDate)
                                                    .Take(3)
                                                    .ToList();
            
            kheechIndexViewModel.RecentMoments = _context.Moments.Include(m => m.KheechEvent)
                                                                 .OrderByDescending(m => m.InsertDate)
                                                                 .Take(3)
                                                                 .ToList();

            kheechIndexViewModel.RecentFriends = _context.KheechUsers.Include(k => k.ApplicationUser)
                                                   .Include(k => k.KheechEvent)
                                                   .Where(m => m.KheechEvent.ApplicationUserId == currentUserId && m.KheechEvent.EndDate > DateTime.UtcNow)
                                                   .Distinct().Take(3).ToList();

            return View(kheechIndexViewModel);

        }

       // GET: KheechEvents/Details/5
       [Route("details/{id}", Name = "KheechDetails")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            KheechEvent kheechEvent = _context.KheechEvents.Include(k => k.ApplicationUser)
                                                           .Include(k => k.KheechComments)
                                                           .FirstOrDefault(k => k.Id == id);
            if (kheechEvent == null)
            {
                return HttpNotFound();
            }
            
            return View(kheechEvent);
        }
        
        [Route("schedule", Name = "ScheduleMeeting")]
        public ActionResult Schedule()
        {
            var currentUserId = User.Identity.GetUserId();

            var scheduleViewModel = new ScheduleViewModel();
            
            //scheduleViewModel.Friends = Enumerable.Empty<Friendship>();
            
            scheduleViewModel.Friends = _context.Friendships.Where(f => f.InitiatorId == currentUserId || f.RecipientId == currentUserId).Distinct().ToList();

            return View(scheduleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("schedule/{id}", Name = "ScheduleMeetingPost")]
        public ActionResult Schedule(int id, ScheduleViewModel model)
        {
            var currentUserId = User.Identity.GetUserId();
            var kheechEvent = new KheechEvent();

            kheechEvent.EventName = model.EventName;
            kheechEvent.ApplicationUserId = currentUserId;
            kheechEvent.StartDate = model.WhenToMeet;
            kheechEvent.EndDate = kheechEvent.StartDate.AddHours(2);
            kheechEvent.IsGroupEvent = false;
            kheechEvent.InsertDate = DateTime.UtcNow;

            var location = _context.Locations.FirstOrDefault(l => l.Name == model.WhereToMeet);
            if (location == null)
            {
                location = new Location 
                {   
                    Name = model.WhereToMeet,
                    Country = "USA",
                    City = "Little Rock",
                    State = "AR",
                    InsertDate = DateTime.UtcNow
                };
                _context.Locations.Add(location);
                _context.SaveChanges();
            }
            
            kheechEvent.LocationId = location.Id;

            _context.KheechEvents.Add(kheechEvent);
            _context.SaveChanges();

            TempData["ScheduleMessage"] = "Congratulations, you have successfully added a Kheech. Keep going!";
            return RedirectToRoute("HomePage");
        }
    }
}
