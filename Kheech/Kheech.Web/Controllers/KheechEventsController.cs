using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kheech.Web.Models;

namespace Kheech.Web.Controllers
{
    public class KheechEventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: KheechEvents
        public async Task<ActionResult> Index()
        {
            var kheechEvents = db.KheechEvents.Include(k => k.ApplicationUser).Include(k => k.Group).Include(k => k.Location);
            return View(await kheechEvents.ToListAsync());
        }

        // GET: KheechEvents/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KheechEvent kheechEvent = await db.KheechEvents.FindAsync(id);
            if (kheechEvent == null)
            {
                return HttpNotFound();
            }
            return View(kheechEvent);
        }

        // GET: KheechEvents/Create
        public ActionResult Create()
        {
            //ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "FirstName");
            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name");
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name");
            return View();
        }

        // POST: KheechEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ApplicationUserId,EventName,LocationId,StartDate,EndDate,GroupId,IsGroupEvent")] KheechEvent kheechEvent)
        {
            if (ModelState.IsValid)
            {
                db.KheechEvents.Add(kheechEvent);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            //ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "FirstName", kheechEvent.ApplicationUserId);
            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name", kheechEvent.GroupId);
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", kheechEvent.LocationId);
            return View(kheechEvent);
        }

        // GET: KheechEvents/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KheechEvent kheechEvent = await db.KheechEvents.FindAsync(id);
            if (kheechEvent == null)
            {
                return HttpNotFound();
            }
            //ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "FirstName", kheechEvent.ApplicationUserId);
            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name", kheechEvent.GroupId);
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", kheechEvent.LocationId);
            return View(kheechEvent);
        }

        // POST: KheechEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ApplicationUserId,EventName,LocationId,StartDate,EndDate,GroupId,IsGroupEvent")] KheechEvent kheechEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kheechEvent).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "FirstName", kheechEvent.ApplicationUserId);
            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name", kheechEvent.GroupId);
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", kheechEvent.LocationId);
            return View(kheechEvent);
        }

        // GET: KheechEvents/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KheechEvent kheechEvent = await db.KheechEvents.FindAsync(id);
            if (kheechEvent == null)
            {
                return HttpNotFound();
            }
            return View(kheechEvent);
        }

        // POST: KheechEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            KheechEvent kheechEvent = await db.KheechEvents.FindAsync(id);
            db.KheechEvents.Remove(kheechEvent);
            await db.SaveChangesAsync();
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
