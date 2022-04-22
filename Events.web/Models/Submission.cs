using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Events.web.Models
{
    public class Submission
    {
        public Submission()
        {
            this.Ideas = new HashSet<Idea>();
        }

        public int SubmissionId { get; set; }
        public string SubmissionName { get; set; }
        public string SubmissionDescription { get; set; }
        public DateTime ClosureDate { get; set; }
        public DateTime FinalClosureDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Idea> Ideas { get; set; }
    }
}