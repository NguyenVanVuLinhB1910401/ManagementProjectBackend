using ManagementProject.Models;

namespace ManagementProject.Interfaces
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        Task<IEnumerable<Department>> GetAllDepartment();
    }
}
