using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Events.web.Models;
using Ionic.Zip;
using PagedList;

namespace Events.web.Areas.Admin.Controllers
{
    // /admin/submission


    public class SubmissionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Submissions
        public ActionResult Index(int? page)
        {
            var sub = db.Submissions.ToList();

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(sub.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Download(int id, int? page)
        {
            ViewBag.page = page;
            var ideas = db.Ideas.Where(i => i.SubmissionId == id).ToList();
            var files = db.Files.Where(f => f.Idea.SubmissionId == id).ToList();
            var sub = db.Submissions.Where(s => s.SubmissionId == id).FirstOrDefault();

            if (sub.FinalClosureDate <= DateTime.Now)
            {
                using (ZipFile zip = new ZipFile())
                {
                    foreach (var file in files)
                    {
                        string filePath = file.FilePath;

                        byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                        var fileName = filePath.Split('\\').Last();


                        zip.AddFile(filePath, "Files");

                    }


                    string zipName = String.Format("submission_{0}.zip", id);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        zip.Save(memoryStream);
                        return File(memoryStream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Octet, zipName);
                    }

                }
            }
            return RedirectToAction("Index", new { page = page });

        }

        public ActionResult ExportToCSV(int id)
        {
            var ideas = db.Ideas.Where(i => i.SubmissionId == id).ToList();
            var files = db.Files.Where(f => f.Idea.SubmissionId == id).ToList();
            var sub = db.Submissions.Where(s => s.SubmissionId == id).FirstOrDefault();

            if (sub.FinalClosureDate <= DateTime.Now)
            {

                List<object> lstIdea = (from Idea in ideas
                                        select new[] { Idea.Title,
                                                                Idea.Description,
                                                                Idea.Content,
                                                                Idea.CreateDate.ToString(),
                                                                Idea.LastModifiedDate.ToString(),
                                                                Idea.UserId.ToString(),
                                                                Idea.Categories.CategoryName,
                                                                Idea.Submissions.SubmissionName,
                                                                Idea.ApplicationUser.FullName,
                                                                Idea.LikeCount.ToString(),
                                                                Idea.Views.Count.ToString(),
                                                                Idea.isAnon.ToString(),
                                                                Idea.Files.Count.ToString()
                                  }).ToList<object>();


                List<string> names = new List<string>
                        {
                            "Title",
                            "Description",
                            "Content",
                            "Created Date",
                            "Updated Date",
                            "UserId",
                            "Category",
                            "Submission",
                            "User FullName",
                            "No. of Likes",
                            "No. of Views",
                            "Anonymously",
                            "File Attached"
                        };


                StringBuilder sb = new StringBuilder();


                lstIdea.Insert(0, names.ToArray());
                foreach (var item in lstIdea)
                {
                    string[] arrIdea = (string[])item;
                    foreach (var data in arrIdea)
                    {
                        //Append data with separator.
                        sb.Append(data + ',');
                    }

                    //Append new line character.
                    sb.Append("\r\n");

                }

                string toFileName = "idealist_" + "sub_" + id + ".csv";

                return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", toFileName);
            }
            return RedirectToAction("Index");
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
                if (dt.Rows.Count == 0)
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
            }
            catch (Exception e)
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
