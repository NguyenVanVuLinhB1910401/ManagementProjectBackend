using ManagementProject.Models;

using ManagementProject.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace ManagementProject.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Department>> GetAllDepartment()
        {
            return await _context.Departments.Where(d => d.Status == 1).OrderByDescending(d => d.Created).ToListAsync();
        }
    }
}
