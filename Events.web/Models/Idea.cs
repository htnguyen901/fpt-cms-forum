using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Events.web.Models
{
    public class Idea
    {
        public Idea()
        {
            this.Comments = new HashSet<Comment>();
            this.Files = new HashSet<File>();
            this.Likes = new HashSet<Like>();
            this.Views = new HashSet<View>();
        }
        public int IdeaId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string UserId { get; set; }
        public int CategoryId { get; set; }
        public int SubmissionId { get; set; }

        public int? LikeCount    { get; set; }

        public int? DislikeCount { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Category Categories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<File> Files { get; set; }
        public virtual Submission Submissions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Like> Likes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<View> Views { get; set; }
    }
}