using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
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

       public ActionResult DashBoard()
        {
            return View();
        }

        public JsonResult DashBoardcounts()
        {
            try
            {
                string[] DashBoardcounts = new string[2];
                string mann = ConfigurationManager.ConnectionStrings["CMSEntities"].ConnectionString;
                SqlConnection con = new SqlConnection(mann);
                con.Open();
                SqlCommand cmd = new SqlCommand("select count(CONVERT(date, ClosureDate)) as date,(select count(CONVERT(date, ClosureDate)) from Submissions where ClosureDate like '%2020%') as closuredate from Submissions where ClosureDate like '%2019%'", con);
                DataTable dt = new DataTable();
                SqlDataAdapter cmd1 = new SqlDataAdapter(cmd);
                cmd1.Fill(dt);
                if(dt.Rows.Count == 0)
                {
                    DashBoardcounts[0] = "0";
                    DashBoardcounts[1] = "0";
                }
                else
                {
                    DashBoardcounts[0] = dt.Rows[0]["date"].ToString();
                    DashBoardcounts[1] = dt.Rows[0]["closuredate"].ToString();
                }
                return Json(new { DashBoardcounts }, JsonRequestBehavior.AllowGet);
            } catch(Exception e)
            {
                throw e;
            }
        }

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
