using ManagementProject.DTOs;
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

        public async Task<IEnumerable<ProjectMemberRes>> GetInfoMembersByProject(string projectId)
        {
            return await _context.ProjectMembers.Include(pm => pm.Member)
                .Where(x => x.ProjectId == projectId)
                .Select(pm => new ProjectMemberRes
                {
                    MemberId = pm.MemberId,
                    ProjectId = pm.ProjectId,
                    Position = pm.Position,
                    FullName = pm.Member.FirstName + " " + pm.Member.LastName,
                    PhongBan = pm.Member.Department.Name,
                    ViTriTrongPhongBan = pm.Member.Position
                })
                .ToListAsync();
        }
    }
}
