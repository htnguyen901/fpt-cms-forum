using System.Data.Entity;
using Events.web.Models;

namespace Events.web.Repository
{
    //Idea
    public class PostRepository: Repository<Idea>
    {
        public PostRepository(): this(new ApplicationDbContext()) { }
        public PostRepository(DbContext context) : base(context)
        {
        }

        public Idea GetById(int? id)
        {
            return First(e => e.IdeaId == id);
        }
    }
}