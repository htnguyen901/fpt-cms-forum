﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Events.web.Models;

namespace Events.web.ViewModels
{
    public class IdeaCommentViewModel
    {
        public virtual Idea Idea { get; set; }
        public virtual Comment Comments { get; set; }

        public virtual Like Like { get; set; }

        public virtual Submission Submission { get; set; }
    }
}