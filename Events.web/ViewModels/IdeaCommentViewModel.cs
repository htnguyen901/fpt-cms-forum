using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Events.web.Models;

namespace Events.web.ViewModels
{
    public class IdeaCommentViewModel
    {
        public string userId { get; set; }

        [Display(Name = "Author")]

        public string FullName { get; set; }
        public int IdeaId { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public string CategoryName { get; set; }
        public List<CommentDisplayViewModel> Comments { get; set; }
    }
}