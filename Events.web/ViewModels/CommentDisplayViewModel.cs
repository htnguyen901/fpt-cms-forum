using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Events.web.Models;

namespace Events.web.ViewModels
{
    public class CommentDisplayViewModel
    {
        public string UserId { get; set; }

        public int IdeaId { get; set; }

        [Display(Name = "From User")]
        public string FullName { get; set; }

        public DateTime CreateDate { get; set; }
        public int CommentId { get; set; }

        [Display(Name = "User Comment")]

        public string Content { get; set; }

        public Idea Idea { get; set; }
    }
}