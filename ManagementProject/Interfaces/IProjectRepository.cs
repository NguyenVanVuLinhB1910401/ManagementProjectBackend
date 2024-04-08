using ManagementProject.Interfaces;
using ManagementProject.Models;

namespace ManagementProject.Interfaces
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<IEnumerable<Project>> GetAllProject();
        Task<IEnumerable<Project>> GetAllProjectJoined(string userId);
        Task<Project> GetById(string id);
        Task<Project> GetDetailProject(string id);
        Task<Project> GetProject(string  id);
       
    }
}
