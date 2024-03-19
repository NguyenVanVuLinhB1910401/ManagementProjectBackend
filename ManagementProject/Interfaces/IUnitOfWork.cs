using ManagementProject.Models;
using ManagementProject.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ManagementProject.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        UserManager<ApplicationUser> UserManager { get; }
        RoleManager<IdentityRole> RoleManager { get; }
        IDepartmentRepository Departments { get; }
        IProjectRepository Projects { get; }
        IProjectMemberRepository ProjectMembers { get; }
        int Complete();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
