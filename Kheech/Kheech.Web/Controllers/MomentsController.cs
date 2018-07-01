using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kheech.Web.Models;
using Kheech.Web.ViewModels;
using Microsoft.AspNet.Identity;

namespace Kheech.Web.Controllers
{
    [RoutePrefix("Moments")]
    public class MomentsController : Controller
    {
        protected readonly ApplicationDbContext _context;

        protected readonly ApplicationUserManager _userManager;

        public MomentsController(ApplicationDbContext context, ApplicationUserManager userManager)
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

            var currentUserId = User.Identity.GetUserId();

            var moments = _context.Moments
                .Include(m => m.KheechEvent.Location)
                .Include(m => m.ApplicationUser)
                .Where(m => m.ApplicationUserId == currentUserId)
                .OrderByDescending(m => m.Id)
                .ToList();

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
            Moment moment = _context.Moments.Include(m => m.KheechEvent.Location)
                                            .FirstOrDefault(m => m.Id == id);
            if (moment == null)
            {
                return HttpNotFound();
            }
            
            moment.KheechEvent.Location = _context.Locations.FirstOrDefault(l => l.Id == moment.KheechEvent.LocationId);
         
            return View(moment);
        }

        // GET: Moments/Create
        [Route("Create/{kheechId}", Name = "MomentsCreate")]
        public ActionResult Create(int kheechId)
        {
            var moment = new MomentsUploadViewModel
            {
                KheechId = kheechId
            };
            //moment.InsertDate = DateTime.UtcNow;
            
            return View(moment);
        }

        // POST: Moments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create/{kheechId}", Name = "MomentsCreatePost")]
        public ActionResult Create(MomentsUploadViewModel momentupload)
        {
            var currentUserId = User.Identity.GetUserId();

            var allowedExtensions = new[] { ".png", ".jpg", ".gif" };
            var fileExtension = Path.GetExtension(momentupload.File.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                ViewBag.Message = "This FileType is not supported by our site.";
                return View(momentupload);
            }

            string fileName = Path.GetFileNameWithoutExtension(momentupload.File.FileName);
            string extension = Path.GetExtension(momentupload.File.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

            var path = Server.MapPath("~/uploads/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            momentupload.ImagePath = "~/uploads/" + fileName;
            momentupload.File.SaveAs(Path.Combine(path, fileName));

            var momentToBeCreated = new Moment
            {
                KheechEventId = momentupload.KheechId,
                Description = momentupload.Description,
                ApplicationUserId = currentUserId,
                Capture = momentupload.ImagePath,
                InsertDate = DateTime.UtcNow
            };

            //var momentToBeAdded = new Moment
            //{
            //    FileName = moment.File.FileName,
            //    ContentType = moment.File.ContentType,
            //ContentLength = moment.File.ContentLength,
            //Data = data
            ////}

            _context.Moments.Add(momentToBeCreated);
            _context.SaveChanges();
            return RedirectToRoute("MomentsDetail", new { id = momentToBeCreated.Id });
        }

        [Route("Download/{id}", Name = "FileDownload")]
        public FileResult Download(int id)
        {
            var file = _context.Moments.FirstOrDefault(f => f.Id == id);
            var fileName = Path.GetFileName(file.Capture);

            return File(file.Capture, fileName);
        }

        // GET: Moments/Edit/5
        [Route("Edit/{id}", Name = "MomentsEdit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Moment moment = _context.Moments.Include(m => m.KheechEvent).FirstOrDefault(m => m.Id == id);
            if (moment == null)
            {
                return HttpNotFound();
            }
            return View(moment);
        }

        // POST: Moments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{id}", Name = "MomentsEditPost")]
        public ActionResult Edit(Moment moment)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(moment).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Details", new { id = moment.Id});
            }
            return View(moment);
        }

        // GET: Moments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Moment moment = _context.Moments.Find(id);
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
            Moment moment = _context.Moments.Find(id);
            _context.Moments.Remove(moment);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
