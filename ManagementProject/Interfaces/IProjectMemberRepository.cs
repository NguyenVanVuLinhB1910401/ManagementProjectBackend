using ManagementProject.Interfaces;
using ManagementProject.Models;

namespace ManagementProject.Interfaces
{
    public interface IProjectMemberRepository : IGenericRepository<ProjectMember>
    {
        Task<IEnumerable<ProjectMember>> GetMembersByProject(string projectId);
    }
}
