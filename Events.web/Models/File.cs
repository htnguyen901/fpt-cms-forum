using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Events.web.Models
{
    public class File
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int Ideaid { get; set; }
        public virtual Idea Idea { get; set; }
    }
}