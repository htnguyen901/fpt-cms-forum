using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Events.web.Models
{
    public class Comment
    {
        public Comment()
        {
            this.Ideas = new HashSet<Idea>();
        }
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string UserId { get; set; }
        public int IdeaId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Idea> Ideas { get; set; }


    }
}