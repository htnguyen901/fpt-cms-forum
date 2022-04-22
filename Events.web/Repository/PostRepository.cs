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
using Events.web.ViewModels;
using System.Threading.Tasks;

namespace Events.web.Repository
{
    //Idea
    public class PostRepository : Repository<Idea>
    {
        private readonly ApplicationDbContext context;

        public PostRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public Idea GetById(int? id)
        {
            return First(e => e.IdeaId == id);
        }
        public Idea GetIdea(int id)
        {
            return context.Ideas
                .Include(i => i.ApplicationUser)
                .Include(i => i.Comments.Select(c => c.ApplicationUser))
                .FirstOrDefault(i => i.IdeaId == id);
        }

        public Comment GetComment(int commentId)
        {
            return context.Comments
                .Include(c => c.ApplicationUser)
                .Include(c => c.Ideas)
                .FirstOrDefault(c => c.CommentId == commentId);
        }

        public Like GetLike(int likeId)
        {
            return context.Likes
                .Include(c => c.ApplicationUser)
                .Include(c => c.Idea)
                .FirstOrDefault(c => c.LikeId == likeId);
        }


        public Comment Add(Comment comment)
        {
            context.Comments.Add(comment);
            context.SaveChangesAsync();
            return comment;

        }
    }
}