using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Events.web.Models;

namespace Events.web.ViewModels
{
    public class IdeaSortViewModel
    {
        public virtual Idea Idea { get; set; }

        public string sortIdeaBy { get; set; }
    }
}