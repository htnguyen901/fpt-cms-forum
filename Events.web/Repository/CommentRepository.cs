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
        public IdeaCommentViewModel GetIdeaCommentDisplay(int ideaid)
        {
            if (ideaid != null)
            {
                using (var db = new ApplicationDbContext())
                {
                    var comment = db.Comments.Where(c => c.IdeaId == ideaid).FirstOrDefault();
                    var idea = db.Ideas.Where(c => c.IdeaId == ideaid).FirstOrDefault();
                    if (comment != null)
                    {
                        var author = db.Users.Where(u => u.Id == idea.UserId).FirstOrDefault();
                        var user = db.Users.Where(u => u.Id == comment.UserId).FirstOrDefault();
                        var ideaCommentListVM = new IdeaCommentViewModel()
                        {
                            userId = comment.UserId,
                            IdeaId = comment.IdeaId,
                            Title = idea.Title,
                            Content = idea.Content,
                            CategoryName = idea.Categories.CategoryName,
                            FullName = author.FullName
                        };

                        List<CommentDisplayViewModel> commentList = db.Comments.AsNoTracking()
                            .Where(x => x.IdeaId == ideaid)
                            //.OrderBy(x => x.CreateDate)
                            .Select(x =>
                           new CommentDisplayViewModel
                           {
                               UserId = x.UserId,
                               IdeaId = x.IdeaId,
                               CommentId = x.CommentId,
                               CreateDate = x.CreateDate,
                               Content = x.Content,
                               FullName = user.FullName
                           }).ToList();
                        ideaCommentListVM.Comments = commentList;
                        return ideaCommentListVM;
                    }
                }
            }
            return null;
        }
    }
}