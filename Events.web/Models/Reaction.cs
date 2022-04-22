using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Events.web.Models
{
    public class Reaction
    {
        [Key]
        public int Reaction1 { get; set; }
        public DateTime CreateDate { get; set; }
        public string UserId { get; set; }
        public int IdeaId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Idea Ideas { get; set; }
    }
}