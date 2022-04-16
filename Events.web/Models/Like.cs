using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Events.web.Models
{
    public class Like
    {
        [Key]
        public int LikeId { get; set; }

        public string UserId { get; set; }
        public bool IsLike { get; set; }

        public int IdeaId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Idea Idea { get; set; }

    }
}