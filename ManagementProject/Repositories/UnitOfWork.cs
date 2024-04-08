using ManagementProject.Interfaces;
using ManagementProject.Models;
using ManagementProject.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace ManagementProject.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManager<ApplicationUser> UserManager => _userManager;
        public RoleManager<IdentityRole> RoleManager => _roleManager;

        public IDepartmentRepository Departments { get; }

        public IProjectRepository Projects { get; }

        public IProjectMemberRepository ProjectMembers { get; }
        public IWorkRepository Works { get; }
        public IAttachmentFileRepository AttachmentFiles { get; }
        public ICommentRepository Comments { get; }
        public IQuyTrinhRepository QuyTrinhs { get; }
        public IBuocThucHienRepository BuocThucHiens { get; }

        public UnitOfWork(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IDepartmentRepository departments, 
            IProjectRepository projects, 
            IProjectMemberRepository projectMembers, 
            IWorkRepository works,
            IAttachmentFileRepository attachmentFiles,
            ICommentRepository comments,
            IQuyTrinhRepository quyTrinhs,
            IBuocThucHienRepository buocThucHiens
            
            )
        {
            this._context = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this.Departments = departments;
            this.Projects = projects;
            this.ProjectMembers = projectMembers;
            this.Works = works;
            this.AttachmentFiles = attachmentFiles;
            this.Comments = comments;
            this.QuyTrinhs = quyTrinhs;
            this.BuocThucHiens = buocThucHiens;
            
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _transaction.Commit();
            _transaction = null;
        }

        public void RollbackTransaction()
        {
            _transaction.Rollback();
            _transaction = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
