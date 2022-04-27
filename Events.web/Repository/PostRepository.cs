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
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

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

        public void SendMail(ApplicationUser user, string content, string subject)
        {
            //var currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            MailboxAddress from = new MailboxAddress(user.FullName, user.Email);

            var qa = context.Users.Where(u => u.DepartmentId == user.DepartmentId && u.Role == "QACoordinator").FirstOrDefault();

            MailboxAddress to = new MailboxAddress(qa.FullName, qa.Email);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = content;

            MimeMessage message = new MimeMessage();
            message.From.Add(from);
            message.To.Add(to);
            message.Subject = subject;
            message.Body = bodyBuilder.ToMessageBody();

            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 465, true);
            client.Authenticate(user.Email, user.tokenPass);

            client.Send(message);
            client.Disconnect(true);
            client.Dispose();

        }

        public void NotiComment(ApplicationUser user, int ideaid, string content, string subject)
        {
            //var currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            
            var thisIdea = context.Ideas.Where(i => i.IdeaId == ideaid).FirstOrDefault();

            var authorId = thisIdea.UserId;

            var ideaOwner = context.Users.Where(u => u.Id == authorId).FirstOrDefault();

            MailboxAddress from = new MailboxAddress(user.FullName, user.Email);

            var qa = context.Users.Where(u => u.DepartmentId == user.DepartmentId && u.Role == "QACoordinator").FirstOrDefault();

            MailboxAddress to = new MailboxAddress(ideaOwner.FullName, ideaOwner.Email);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = content;

            MimeMessage message = new MimeMessage();
            message.From.Add(from);
            message.To.Add(to);
            message.Subject = subject;
            message.Body = bodyBuilder.ToMessageBody();

            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 465, true);
            client.Authenticate(user.Email, user.tokenPass);

            client.Send(message);
            client.Disconnect(true);
            client.Dispose();

        }



        public Submission GetSubmission(int ideaid)
        {
            var idea = context.Ideas.Where(i => i.IdeaId == ideaid).FirstOrDefault();
            return context.Submissions
                .Include(s => s.Ideas)
                .FirstOrDefault(s => s.SubmissionId == idea.SubmissionId);
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