using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Events.web.Models;

namespace Events.web.Areas.Admin.Controllers
{
    // /admin/submission
    public class SubmissionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Submissions
        public ActionResult Index()
        {
            return View(db.Submissions.ToList());
        }

        //upload
        //public ActionResult Download(int submissionId)
        //{
        //    //ExportExcel(submissionId);
        //    ExportZIP(submissionId);
        //    return RedirectToAction("Index");
        //}

        //public ActionResult ExportZIP(int submissionId)
        //{
        //    var path = Path.Combine("file", "submission_" + submissionId);
        //    if (Directory.Exists(path))
        //    {
        //        var zipPath = Path.Combine("file", "submission_" + submissionId + ".zip");
        //        using (FileStream zipToOpen = new FileStream(zipPath, FileMode.Create))
        //        {
        //            using(ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
        //            {
        //                archive.CreateEntryFromFile(path, path);  
        //            }
        //        }
        //        byte[] fileBytes = System.IO.File.ReadAllBytes(zipPath);
        //        return File(fileBytes, MediaTypeNames.Application.Zip, Path.GetFileName(zipPath));
        //    }
        //    return NoContent();
            
        //}

        // GET: Admin/Submissions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Submission submission = db.Submissions.Find(id);
            if (submission == null)
            {
                return HttpNotFound();
            }
            return View(submission);
        }

        // GET: Admin/Submissions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Submissions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubmissionId,SubmissionName,SubmissionDescription,ClosureDate,FinalClosureDate")] Submission submission)
        {
            if (ModelState.IsValid)
            {
                db.Submissions.Add(submission);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(submission);
        }

        // GET: Admin/Submissions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Submission submission = db.Submissions.Find(id);
            if (submission == null)
            {
                return HttpNotFound();
            }
            return View(submission);
        }

        // POST: Admin/Submissions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubmissionId,SubmissionName,SubmissionDescription,ClosureDate,FinalClosureDate")] Submission submission)
        {
            if (ModelState.IsValid)
            {
                db.Entry(submission).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(submission);
        }

        // GET: Admin/Submissions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Submission submission = db.Submissions.Find(id);
            if (submission == null)
            {
                return HttpNotFound();
            }
            return View(submission);
        }

        // POST: Admin/Submissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Submission submission = db.Submissions.Find(id);
            db.Submissions.Remove(submission);
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
