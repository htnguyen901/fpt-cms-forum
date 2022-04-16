using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Events.web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Events.web.Controllers
{
    public class IdeasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Ideas
        public ActionResult Index()
        {
            var ideas = db.Ideas.Include(i => i.Categories).Include(i => i.Submissions);
            return View(ideas.ToList());
        }

        // GET: Ideas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Idea idea = db.Ideas.Find(id);
            if (idea == null)
            {
                return HttpNotFound();
            }
            return View(idea);
        }

        // GET: Ideas/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            ViewBag.SubmissionId = new SelectList(db.Submissions, "SubmissionId", "SubmissionName");
            return View();
        }

        // POST: Ideas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdeaId,Title,Description,Content,CategoryId,SubmissionId")] Idea idea)
        {
            if (ModelState.IsValid)
            {
                var currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                var currentU = db.Users.Find(currentUser.Id);
                idea = new Idea()
                {
                    IdeaId = idea.IdeaId,
                    SubmissionId = idea.SubmissionId,
                    Title = idea.Title,
                    CategoryId = idea.CategoryId,
                    Description = idea.Description,
                    Content = idea.Content,
                    UserId = currentU.Id,
                    CreateDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    ApplicationUser = currentU
                };
                db.Ideas.Add(idea);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", idea.CategoryId);
            ViewBag.SubmissionId = new SelectList(db.Submissions, "SubmissionId", "SubmissionName", idea.SubmissionId);
            return View(idea);
        }

        // GET: Ideas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Idea idea = db.Ideas.Find(id);
            if (idea == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", idea.CategoryId);
            ViewBag.SubmissionId = new SelectList(db.Submissions, "SubmissionId", "SubmissionName", idea.SubmissionId);
            return View(idea);
        }

        // POST: Ideas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdeaId,Title,Description,Content,CreateDate,LastModifiedDate,UserId,CategoryId,SubmissionId")] Idea idea)
        {
            if (ModelState.IsValid)
            {
                db.Entry(idea).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", idea.CategoryId);
            ViewBag.SubmissionId = new SelectList(db.Submissions, "SubmissionId", "SubmissionName", idea.SubmissionId);
            return View(idea);
        }

        // GET: Ideas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Idea idea = db.Ideas.Find(id);
            if (idea == null)
            {
                return HttpNotFound();
            }
            return View(idea);
        }

        // POST: Ideas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Idea idea = db.Ideas.Find(id);
            db.Ideas.Remove(idea);
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
