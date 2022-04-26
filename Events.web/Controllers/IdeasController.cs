using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Events.web.Models;
using Events.web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;

namespace Events.web.Controllers
{
    public class IdeasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Ideas
        public ActionResult Index(int id, int? page, string sortBy)
        {
            ViewBag.currentSort = sortBy;
            ViewBag.submission = db.Submissions.Where(s => s.SubmissionId == id).FirstOrDefault();

            var ideas = db.Ideas.Include(i => i.Categories).Include(i => i.Submissions).Include(i => i.Comments).Include(i => i.Views).AsQueryable();


            //var ideas = db.Ideas.AsQueryable();

            ideas = ideas.Where(i => i.Submissions.SubmissionId == id);

            switch (sortBy)
            {
                case "date":
                    ideas = ideas.OrderByDescending(i => i.CreateDate);
                    break;
                case "comment":
                    ideas = ideas.OrderByDescending(i => i.Comments.Select(c => c.CreateDate).FirstOrDefault());
                    break;
                case "view":
                    ideas = ideas.OrderByDescending(i => i.Views.Count());
                    break;
                default:
                    ideas = ideas.OrderByDescending(i => i.CreateDate);
                    break;

            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(ideas.ToPagedList(pageNumber, pageSize));

            //return View(ideas.ToList());
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
        public ActionResult Create(int id)
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            ViewBag.SubmissionId = id;
            return View();
        }

        // POST: Ideas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IdeaFileViewModel ideaFileVM, int id)
        {
            if (ModelState.IsValid)
            {
                var file = ideaFileVM.File;
                //TempData["Message"] = "files uploaded successfully";


                var currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                var currentU = db.Users.Find(currentUser.Id);
                var idea = new Idea()
                {
                    SubmissionId = id,
                    Title = ideaFileVM.Idea.Title,
                    CategoryId = ideaFileVM.CategoryId,
                    Description = ideaFileVM.Idea.Description,
                    Content = ideaFileVM.Idea.Content,
                    UserId = currentU.Id,
                    CreateDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    ApplicationUser = currentU,
                    isAnon = ideaFileVM.Idea.isAnon
                };

                var submission = db.Submissions.Where(s => s.SubmissionId == id).FirstOrDefault();


                if (submission.ClosureDate >= DateTime.Now)
                {
                    db.Ideas.Add(idea);
                    db.SaveChanges();

                    //find this idea in database
                    var thisIdea = db.Ideas.Where(i => i.SubmissionId == id && i.UserId == currentU.Id && i.CategoryId == ideaFileVM.CategoryId )
                        .OrderByDescending(i => i.IdeaId)
                        .FirstOrDefault();
                    //save file to server folder
                    if (file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(Server.MapPath("~/Files"), "submission_" + submission.SubmissionId, "idea_" + thisIdea.IdeaId);
                        if (!Directory.Exists(filePath)) { Directory.CreateDirectory(filePath); }
                        filePath = Path.Combine(filePath, fileName);
                        file.SaveAs(filePath);
                    }
                    // insert file to db
                    var newFile = new Models.File()
                    {
                        FilePath = Path.Combine(Server.MapPath("~/Files"), "submission_" + submission.SubmissionId, "idea_" + thisIdea.IdeaId, Path.GetFileName(file.FileName)),
                        CreateDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        Ideaid = thisIdea.IdeaId,
                        Idea = thisIdea

                    };

                    db.Files.Add(newFile);
                    db.SaveChanges();
                }

                return RedirectToAction("Index", new { id = id });
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", ideaFileVM.CategoryId);
            ViewBag.SubmissionId = id;
            return View(ideaFileVM);
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
