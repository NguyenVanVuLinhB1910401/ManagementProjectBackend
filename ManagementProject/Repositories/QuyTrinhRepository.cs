using ManagementProject.Interfaces;
using ManagementProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagementProject.Repositories
{
    public class QuyTrinhRepository : GenericRepository<QuyTrinh>, IQuyTrinhRepository
    {
        public QuyTrinhRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<QuyTrinh>> GetAllQuyTrinh()
        {
            return await _context.QuyTrinhs.Where(q => q.isDelete == 0).Include(q => q.NguoiTao).Select(q => new QuyTrinh()
            {
                Id = q.Id,
                TenQuyTrinh = q.TenQuyTrinh,
                NgayTao = q.NgayTao,
                NguoiTaoId = q.NguoiTaoId,
                NguoiTao = new ApplicationUser()
                {
                    Id = q.NguoiTao.Id,
                    FirstName = q.NguoiTao.FirstName,
                    LastName = q.NguoiTao.LastName
                }
            }).ToListAsync();
        }

        public async Task<QuyTrinh?> GetById(int id)
        {
            return await _context.QuyTrinhs.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<QuyTrinh?> GetDetail(int id)
        {
            return await _context.QuyTrinhs.Include(q => q.BuocThucHiens).Select(q => new QuyTrinh()
            {
                Id = q.Id,
                TenQuyTrinh = q.TenQuyTrinh,
                NgayTao = q.NgayTao,
                NguoiTaoId = q.NguoiTaoId,
                NguoiTao = new ApplicationUser()
                {
                    Id = q.NguoiTao.Id,
                    FirstName = q.NguoiTao.FirstName,
                    LastName = q.NguoiTao.LastName
                },
                BuocThucHiens = q.BuocThucHiens.Select(b => new BuocThucHien() // Chỉ lấy một số trường bạn muốn
                {
                    Id = b.Id,
                    Code = b.Code,
                    TenBuoc = b.TenBuoc,
                    NguoiThucHienId = b.NguoiThucHienId,
                    NguoiThucHien = new ApplicationUser()
                    {
                        Id = b.NguoiThucHien.Id,
                        FirstName = b.NguoiThucHien.FirstName,
                        LastName = b.NguoiThucHien.LastName
                    },
                    BuocTiepTheo = b.BuocTiepTheo,
                    BuocTruocDo = b.BuocTruocDo

                }).ToList(),

            }).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
