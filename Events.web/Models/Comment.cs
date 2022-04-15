using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Events.web.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string UserId { get; set; }
        public int IdeaId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Idea Ideas { get; set; }


    }
}