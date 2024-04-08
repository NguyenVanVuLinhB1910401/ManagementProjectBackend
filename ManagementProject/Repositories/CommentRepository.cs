using ManagementProject.Interfaces;
using ManagementProject.Models;

namespace ManagementProject.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
