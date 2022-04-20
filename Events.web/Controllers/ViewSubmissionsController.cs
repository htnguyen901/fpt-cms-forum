using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Events.web.Models;
using Events.web.ViewModels;

namespace Events.web.Controllers
{
    public class ViewSubmissionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Submissions
        public ActionResult Index()
        {
            return View(db.Submissions.ToList());
        }

        public ActionResult ViewIdea(int id)
        {

            var submission = db.Submissions
                .Include(i => i.Ideas)
                .FirstOrDefault(i => i.SubmissionId == id);

            var idea = db.Ideas
                .Include(i => i.Categories)
                .FirstOrDefault(i => i.SubmissionId == id);

            var model =  new SubmissionIdeaViewModel
            {
                Submission = submission,
                Idea = idea
            };

            return View(model);
        }

        // GET: Submissions/Details/5
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
