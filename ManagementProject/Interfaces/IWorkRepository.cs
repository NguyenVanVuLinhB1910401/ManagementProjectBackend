using ManagementProject.Models;

namespace ManagementProject.Interfaces
{
    public interface IWorkRepository : IGenericRepository<Work>
    {
        Task<IEnumerable<Work>> GetAllWork();
        Task<IEnumerable<Work>> GetAllWorkAssignedToMe(string userId);
        Task<IEnumerable<Work>> GetAllWorkIAssign(string userId);
        Task<Work> GetWorkById(int id);
        Task<Work> Get(int id);
        Task<IEnumerable<Work>> GetAllSubWork(int parentWorkId);
        Task<IEnumerable<Work>> LichSuThucHienCongViec(string projectId);
    }
}
