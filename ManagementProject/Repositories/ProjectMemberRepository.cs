using ManagementProject.Interfaces;
using ManagementProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagementProject.Repositories
{
    public class ProjectMemberRepository : GenericRepository<ProjectMember>, IProjectMemberRepository
    {
        public ProjectMemberRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ProjectMember>> GetMembersByProject(string projectId)
        {
            return await _context.ProjectMembers.Where(x => x.ProjectId == projectId).ToListAsync();
        }
    }
}
