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
    public class LikeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Like
        public string LikeThis(int id)
        {
            Idea idea = db.Ideas.FirstOrDefault(x => x.IdeaId == id);
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            idea.LikeCount++;
            Like like = new Like();
            like.IdeaId = id;
            like.UserId = user.Id;
            like.IsLike = true;
            db.Likes.Add(like);
            db.SaveChanges();
            
            return idea.Likes.ToString();
        }

        public string UnlikeThis(int id)
        {
            Idea idea = db.Ideas.FirstOrDefault(x => x.IdeaId == id);
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            Like like = db.Likes.FirstOrDefault(x => x.IdeaId == id && x.UserId == user.Id);
            idea.LikeCount--;
            db.Likes.Remove(like);
            db.SaveChanges();

            return idea.Likes.ToString();

        }
    }
}