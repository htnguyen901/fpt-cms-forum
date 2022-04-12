using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
//using Events.Data;

namespace Events.web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<File> Files { get; set; }

        public virtual DbSet<Idea> Ideas { get; set; }

        public virtual DbSet<Reaction> Reactions { get; set; }

        public virtual DbSet<Submission> Submissions { get; set; }

        public virtual DbSet<View> Views { get; set; }

        //public IDbSet<CourseCategory> CourseCategories { get; set; }

        //public IDbSet<CoursesAssigned> CoursesAssigned { get; set; }

        public ApplicationDbContext()
            : base("CMSEntities", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}