
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Events.web.Models;
using Events.web.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Events.web.Hubs
{
    public class PostHub : Hub
    {
        public Task Like(string postId)
        {
            var likePost = SaveLike(postId);
            return Clients.All.updateLikeCount(likePost);
        }
        private LikePost SaveLike(string id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var postId = int.Parse(id);
            var baseContext = Context.Request.GetHttpContext();
            var postRepository = new PostRepository();
            var item = postRepository.GetById(postId);
            var liked = new Like
            {
                UserId = user.Id,
                IdeaId = item.IdeaId,
                IsLike = true
            };
            var dupe = item.Likes.FirstOrDefault(e => e.UserId == liked.UserId);
            if (dupe == null)
            {
                item.Likes.Add(liked);
            }
            else
            {
                dupe.IsLike = !dupe.IsLike;
            }
            postRepository.SaveChanges();
            var post = postRepository.GetById(postId);

            return new LikePost
            {
                LikeCount = post.Likes.Count(e => e.IsLike)
            };
        }
    }
}