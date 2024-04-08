using ManagementProject.Interfaces;
using ManagementProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagementProject.Repositories
{
    public class BuocThucHienRepository : GenericRepository<BuocThucHien>, IBuocThucHienRepository
    {
        public BuocThucHienRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<BuocThucHien?> GetBuocHienTaiByCode(string code)
        {
            return await _context.BuocThucHiens.FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<BuocThucHien?> GetBuocHienTaiById(int id)
        {
            return await _context.BuocThucHiens.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<BuocThucHien?> GetBuocKhoiTao(int quyTrinhId)
        {
            return await _context.BuocThucHiens.FirstOrDefaultAsync(b => b.QuyTrinhId == quyTrinhId && b.BuocTruocDo == "0");
        }

        public async Task<BuocThucHien?> GetBuocTiepTheo(string code)
        {
            return await _context.BuocThucHiens.FirstOrDefaultAsync(b => b.BuocTruocDo.Equals(code));
        }

        public async Task<BuocThucHien?> GetBuocTruocDo(string code)
        {
            return await _context.BuocThucHiens.FirstOrDefaultAsync(b => b.BuocTiepTheo.Equals(code));
        }
    }
}
