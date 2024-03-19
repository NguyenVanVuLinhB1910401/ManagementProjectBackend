using ManagementProject.Models;
using ManagementProject.Repositories;
using ManagementProject.Interfaces;
using ManagementProject.Models;
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
            return await _context.Projects.Where(d => d.Status == 1).OrderByDescending(d => d.Created).ToListAsync();
        }

        public async Task<Project> GetById(string id)
        {
            return await _context.Projects.Where(p => p.Id == id).Include(p => p.Members).FirstOrDefaultAsync();
        }
    }
}
