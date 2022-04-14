using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Events.web.Models;

namespace Events.web.Controllers
{
    public class CategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categoriess
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        // GET: Categoriess/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category Categories = db.Categories.Find(id);
            //var Categories2 = db.Categoriess.Where(c => c.Id == id.Value).Include(i => i.CategoriesInstructor.Id;
            //Categories Categories = db.Categoriess.AsQueryable<Categoriess>().Select(x => x.id == id.Value);
            string query = "Select dbo.AspNetUsers.UserName from dbo.AspNetUsers,dbo.Categoriess where (dbo.AspNetUsers.Id = dbo.Categoriess.Instructor) AND (dbo.Categoriess.Id = " + id.Value + ")";
            var queryResult = db.Database.SqlQuery<string>(query);
            string name = queryResult.FirstOrDefault();
            if (name == null) name = "Not Assigned";
            if (Categories == null)
            {
                return HttpNotFound();
            }

            ViewBag.instructor_name = name;
            return View(Categories);
        }


        // GET: Categoriess/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,CategoryId")] Category Categories)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(Categories);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(Categories);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category Categories = db.Categories.Find(id);
            if (Categories == null)
            {
                return HttpNotFound();
            }
            return View(Categories);
        }

        // POST: Categoriess/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,CategoryId")] Category Categories)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Categories).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Categories);
        }

        // GET: Categoriess/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category Categories = db.Categories.Find(id);
            if (Categories == null)
            {
                return HttpNotFound();
            }
            return View(Categories);
        }

        // POST: Categoriess/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category Categories = db.Categories.Find(id);
            db.Categories.Remove(Categories);
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
