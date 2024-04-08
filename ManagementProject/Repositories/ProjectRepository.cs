using ManagementProject.Models;
using ManagementProject.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ManagementProject.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async  Task<IEnumerable<Project>> GetAllProject()
        {
            return await _context.Projects.Where(d => d.isProject == 1 && d.isDelete == 0).OrderByDescending(d => d.Created)
            .Select(p => new Project()
            {
                Id = p.Id,
                Name = p.Name,
                Address = p.Address,
                Type = p.Type,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Status = p.Status,
                Created = p.Created,
                CreatedId = p.CreatedId,
                isProject = 1,
                QuyTrinhId = p.QuyTrinhId,
                BuocHienTai = new BuocThucHien () { Id = p.BuocHienTai.Id, TenBuoc = p.BuocHienTai.TenBuoc },
            }).ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetAllProjectJoined(string userId)
        {
            return await _context.Projects.Include(p => p.Members).Where(d => (d.isProject == 1 && d.isDelete == 0 && (d.CreatedId == userId || d.Members.Any(m => m.MemberId == userId)))).OrderByDescending(d => d.Created).ToListAsync();
        }

        public async Task<Project?> GetById(string id)
        {
            return await _context.Projects
                                          .Where(p => p.Id == id)
                                          .Include(p => p.Members)
                                          .ThenInclude(p => p.Member)
                                          .FirstOrDefaultAsync();
        }

        public async Task<Project?> GetDetailProject(string id)
        {
            return await _context.Projects.Where(p => p.Id == id)
                .Include(p => p.BuocHienTai).FirstOrDefaultAsync();    
        }

        public async Task<Project?> GetProject(string id)
        {
            return await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
        }

        
    }
}
