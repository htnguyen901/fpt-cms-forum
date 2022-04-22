using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Events.web.Models
{
    public class View
    {
        [Key]
        public int viewId { get; set; }
        public string UserId { get; set; }
        
        public int IdeaId { get; set; }
        public DateTime LastVisitedDate { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Idea Ideas { get; set; }
    }
}