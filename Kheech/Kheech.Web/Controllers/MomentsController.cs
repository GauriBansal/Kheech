using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kheech.Web.Models;

namespace Kheech.Web.Controllers
{
    [RoutePrefix("Moments")]
    public class MomentsController : Controller
    {
        protected readonly ApplicationDbContext _context;

        protected readonly ApplicationUserManager _userManager;

        public HomeController(ApplicationDbContext context, ApplicationUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        // GET: Moments
        [Route("", Name = "MomentsHome")]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToRoute("IndexPage");
            }
            
            var moments = _context.Moments.Include(m => m.KheechEvent).OrderByDescending((m => m.Id).ToList();
            return View(moments);
        }

        // GET: Moments/Details/5
        [Route("Details/{id}", Name = "MomentsDetail")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Moment moment = _context.Moments.Find(id).Include(m => m.KheechEvent);
            if (moment == null)
            {
                return HttpNotFound();
            }
            return View(moment);
        }

        // GET: Moments/Create
        public ActionResult Create()
        {
            ViewBag.KheechEventId = new SelectList(db.KheechEvents, "Id", "ApplicationUserId");
            return View();
        }

        // POST: Moments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,KheechEventId,Capture")] Moment moment)
        {
            if (ModelState.IsValid)
            {
                db.Moments.Add(moment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KheechEventId = new SelectList(db.KheechEvents, "Id", "ApplicationUserId", moment.KheechEventId);
            return View(moment);
        }

        // GET: Moments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Moment moment = db.Moments.Find(id);
            if (moment == null)
            {
                return HttpNotFound();
            }
            ViewBag.KheechEventId = new SelectList(db.KheechEvents, "Id", "ApplicationUserId", moment.KheechEventId);
            return View(moment);
        }

        // POST: Moments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,KheechEventId,Capture")] Moment moment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(moment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KheechEventId = new SelectList(db.KheechEvents, "Id", "ApplicationUserId", moment.KheechEventId);
            return View(moment);
        }

        // GET: Moments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Moment moment = db.Moments.Find(id);
            if (moment == null)
            {
                return HttpNotFound();
            }
            return View(moment);
        }

        // POST: Moments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Moment moment = db.Moments.Find(id);
            db.Moments.Remove(moment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
