using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Events.web.Models
{
    public class Like
    {
        public int LikeId { get; set; }

        public string UserId { get; set; }
        public bool IsLike { get; set; }

        public int IdeaId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Idea Idea { get; set; }

    }
}