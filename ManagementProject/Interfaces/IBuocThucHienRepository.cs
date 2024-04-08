using ManagementProject.Models;

namespace ManagementProject.Interfaces
{
    public interface IBuocThucHienRepository : IGenericRepository<BuocThucHien>
    {
        Task<BuocThucHien> GetBuocKhoiTao(int quyTrinhId);
        Task<BuocThucHien?> GetBuocTruocDo(string code);
        Task<BuocThucHien?> GetBuocTiepTheo(string code);
        Task<BuocThucHien?> GetBuocHienTaiByCode(string code);
        Task<BuocThucHien?> GetBuocHienTaiById(int id);
    }
}
