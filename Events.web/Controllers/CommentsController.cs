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
using Events.web.Repository;
using System.Threading.Tasks;
using PusherServer;
using Events.web.ViewModels;

namespace Events.web.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments

        public ActionResult Index(int id)
        {
            var repo = new CommentRepository();
            var model = repo.GetIdeaCommentViewModel(id);
            return View(model);
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommentId,Content,CreateDate,LastModifiedDate,UserId,IdeaId")] Comment comment)
        {

            if (ModelState.IsValid)
            {
                //comment.CreateDate = DateTime.Now;
                //comment.LastModifiedDate = DateTime.Now;
                //comment.UserId = "ce2c2928-d805-4d4b-8996-399f11d839f3";
                //comment.IdeaId = 1;

                //var currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                //var currentU = db.Users.Find(currentUser.Id);
                //comment.ApplicationUser = currentU;

                //db.Comments.Add(comment);
                //db.SaveChanges();
                //return RedirectToAction("Index", "Comments", new {id=1});
            }

            return View(comment);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentId,Content,CreateDate,LastModifiedDate,UserId,IdeaId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Comment(IdeaCommentViewModel ideaCommentViewModel)
        {


            //var repo = new PostRepository(db);
            //var idea = repo.GetIdea(ideaCommentViewModel.Idea.IdeaId);
            var ideaid = ideaCommentViewModel.Idea.IdeaId;
            var currentIdea = db.Ideas.Find(ideaid);

            //var comment = ideaCommentViewModel.Comments;

            var currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var currentU = db.Users.Find(currentUser.Id);
            //comment.ApplicationUser = currentU;

            //_comment.Ideas = idea;

            var _comment = new Comment
            {
                Ideas = currentIdea,
                Content = ideaCommentViewModel.Comments.Content,
                UserId = currentU.Id,
                IdeaId = ideaCommentViewModel.Idea.IdeaId,
                CreateDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                ApplicationUser = currentU,
                isAnon = ideaCommentViewModel.Comments.isAnon
            };


            db.Comments.Add(_comment);
            db.SaveChanges();

            return RedirectToAction("Index", "Comments", new { id=ideaCommentViewModel.Idea.IdeaId});
        }

        //public ActionResult CreateComment(IdeaCommentViewModel ideaCommentVM)
        //{
        //    ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

        //    var repo = new PostRepository(db);
        //    var idea = repo.GetById(ideaCommentVM.Idea.IdeaId);
        //    var comment = ideaCommentVM.Comments;

        //    comment.UserId = user.Id;
        //    comment.Ideas = idea;
        //    comment.CreateDate = DateTime.Now;

        //    //var result = repo.Add(comment);

        //    return repo.Add(comment);

        //}

        public string LikeThis(int id)
        {
            Idea idea = db.Ideas.FirstOrDefault(x => x.IdeaId == id);
            var currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var currentU = db.Users.Find(currentUser.Id);

            idea.LikeCount++;
            Like like = new Like();
            like.IdeaId = id;
            like.ApplicationUser = currentU;
            like.UserId = currentU.Id;
            like.IsLike = true;

            db.Likes.Add(like);

            db.Entry(idea).State = EntityState.Modified;
            db.SaveChanges();

            return idea.LikeCount.ToString();
        }

        public string UnlikeThis(int id)
        {
            Idea idea = db.Ideas.FirstOrDefault(x => x.IdeaId == id);
            var currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var currentU = db.Users.Find(currentUser.Id);

            Like like = db.Likes.FirstOrDefault(x => x.IdeaId == id && x.UserId == currentU.Id);
            idea.LikeCount--;
            db.Likes.Remove(like);
            db.SaveChanges();

            return idea.LikeCount.ToString();

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
