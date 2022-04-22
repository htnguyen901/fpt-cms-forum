using System.Collections.Generic;
using System.Data.Entity;
using System.Web;
using Events.web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Events.web.Repository
{
    //Like
    public class PostLikeRepository : Repository<Like>
    {
        public PostLikeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Like> GetByPostId(int id)
        {
            return Find(e => e.IdeaId == id);
        }

        public bool Exists(int ideaId)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var result = First(e => e.IdeaId == ideaId && e.UserId == user.Id);
            return result != null;
        }

        public int CountByPostId(int id)
        {
            return Count(e => e.IdeaId == id);
        }
    }
}