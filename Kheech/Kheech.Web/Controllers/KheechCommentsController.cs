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
    public class KheechCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: KheechComments
        public ActionResult Index()
        {
            var kheechComments = db.KheechComments.Include(k => k.KheechEvent);
            return View(kheechComments.ToList());
        }

        // GET: KheechComments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KheechComment kheechComment = db.KheechComments.Find(id);
            if (kheechComment == null)
            {
                return HttpNotFound();
            }
            return View(kheechComment);
        }

        // GET: KheechComments/Create
        public ActionResult Create()
        {
            ViewBag.KheechEventId = new SelectList(db.KheechEvents, "Id", "ApplicationUserId");
            return View();
        }

        // POST: KheechComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Discussion,KheechEventId,InsertDate")] KheechComment kheechComment)
        {
            if (ModelState.IsValid)
            {
                db.KheechComments.Add(kheechComment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KheechEventId = new SelectList(db.KheechEvents, "Id", "ApplicationUserId", kheechComment.KheechEventId);
            return View(kheechComment);
        }

        // GET: KheechComments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KheechComment kheechComment = db.KheechComments.Find(id);
            if (kheechComment == null)
            {
                return HttpNotFound();
            }
            ViewBag.KheechEventId = new SelectList(db.KheechEvents, "Id", "ApplicationUserId", kheechComment.KheechEventId);
            return View(kheechComment);
        }

        // POST: KheechComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Discussion,KheechEventId,InsertDate")] KheechComment kheechComment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kheechComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KheechEventId = new SelectList(db.KheechEvents, "Id", "ApplicationUserId", kheechComment.KheechEventId);
            return View(kheechComment);
        }

        // GET: KheechComments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KheechComment kheechComment = db.KheechComments.Find(id);
            if (kheechComment == null)
            {
                return HttpNotFound();
            }
            return View(kheechComment);
        }

        // POST: KheechComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KheechComment kheechComment = db.KheechComments.Find(id);
            db.KheechComments.Remove(kheechComment);
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
