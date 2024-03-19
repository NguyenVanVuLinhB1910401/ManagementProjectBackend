using ManagementProject.Interfaces;
using ManagementProject.Models;

namespace ManagementProject.Interfaces
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<IEnumerable<Project>> GetAllProject();
        Task<Project> GetById(string id);
    }
}
