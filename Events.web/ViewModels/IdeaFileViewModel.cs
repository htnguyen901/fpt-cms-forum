using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Events.web.Models;

namespace Events.web.ViewModels
{
    public class IdeaFileViewModel
    {
        public int CategoryId { get; set; }
        public virtual Idea Idea { get; set; }
        public HttpPostedFileBase File { get; set; }
    }
}