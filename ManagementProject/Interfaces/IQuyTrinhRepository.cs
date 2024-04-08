using ManagementProject.Models;

namespace ManagementProject.Interfaces
{
    public interface IQuyTrinhRepository : IGenericRepository<QuyTrinh>
    {
        Task<QuyTrinh?> GetById(int id);
        Task<QuyTrinh?> GetDetail(int id);

        Task<IEnumerable<QuyTrinh>> GetAllQuyTrinh();
    }
}
