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
using PagedList;
using Ionic.Zip;

namespace Events.web.Controllers
{
    public class ViewSubmissionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Submissions
        public ActionResult Index(int? page)
        {
            var sub = db.Submissions.ToList();

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(sub.ToPagedList(pageNumber,pageSize));
        }


        //No longer used
        //public ActionResult ViewIdea(int id)
        //{

        //    var submission = db.Submissions
        //        .Include(i => i.Ideas)
        //        .FirstOrDefault(i => i.SubmissionId == id);

        //    var idea = db.Ideas
        //        .Include(i => i.Categories)
        //        .FirstOrDefault(i => i.SubmissionId == id);

        //    var model =  new SubmissionIdeaViewModel
        //    {
        //        Submission = submission,
        //        Idea = idea
        //    };

        //    return View(model);
        //}

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

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }
    }
}
