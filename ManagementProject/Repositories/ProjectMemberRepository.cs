using ManagementProject.Interfaces;
using ManagementProject.Models;

namespace ManagementProject.Repositories
{
    public class ProjectMemberRepository : GenericRepository<ProjectMember>, IProjectMemberRepository
    {
        public ProjectMemberRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
