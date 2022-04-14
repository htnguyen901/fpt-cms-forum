using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Events.web.Models;

namespace Events.web.ViewModels
{
    public class PostLikeViewModel
    {
        public Idea Idea { get; set; }
        public int Like { get; set; }
    }
}