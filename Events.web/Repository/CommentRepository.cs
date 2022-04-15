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

namespace Events.web.Repository
{
    public class CommentRepository
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public IdeaCommentViewModel GetIdeaCommentViewModel(int ideaid)
        {
 
            if (ideaid == 0)
            {
                return null;
            }

            var repo = new PostRepository(db);
            var idea = repo.GetIdea(ideaid);
            //var comment = repo.GetComment(ideaid);

            if (idea is null)
            {
                return null;
            }

            return new IdeaCommentViewModel
            {
                Idea = idea,
                //Comments = comment
            };

        }
    }
}