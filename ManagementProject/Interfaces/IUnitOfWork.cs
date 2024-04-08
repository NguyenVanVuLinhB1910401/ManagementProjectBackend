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
        IWorkRepository Works { get; }
        IAttachmentFileRepository AttachmentFiles { get; }
        ICommentRepository Comments { get; }
        IQuyTrinhRepository QuyTrinhs { get; }
        IBuocThucHienRepository BuocThucHiens { get; }
        int Complete();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
