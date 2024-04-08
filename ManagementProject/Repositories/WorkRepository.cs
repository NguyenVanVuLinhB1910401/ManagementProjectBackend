using ManagementProject.Interfaces;
using ManagementProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace ManagementProject.Repositories
{
    public class WorkRepository : GenericRepository<Work>, IWorkRepository

    {
        public WorkRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Work?> Get(int id)
        {
            return await _context.Works.FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<IEnumerable<Work>> GetAllSubWork(int parentWorkId)
        {
            return await _context.Works.Include(w => w.CreatedUser).Include(w => w.AssignUser).Include(w => w.Project)
                .Where(w => w.ParentWorkId == parentWorkId)
                .Select(w => new Work()
                {
                    Id = w.Id,
                    Title = w.Title,
                    //Content = w.Content,
                    StartDate = w.StartDate,
                    EndDate = w.EndDate,
                    CompleteDate = w.CompleteDate,
                    Status = w.Status,
                    Progress = w.Progress,
                    Created = w.Created,
                    Project = new Project() { Id = w.ProjectId, Name = w.Project.Name },
                    CreatedUser = new ApplicationUser() { Id = w.CreatedUser.Id, FirstName = w.CreatedUser.FirstName, LastName = w.CreatedUser.LastName },
                    AssignUser = new ApplicationUser() { Id = w.AssignUser.Id, FirstName = w.AssignUser.FirstName, LastName = w.AssignUser.LastName }
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<Work>> GetAllWork()
        {
            return await _context.Works.Include(w => w.CreatedUser).Include(w => w.AssignUser).Include(w => w.Project)
                .Select(w => new Work()
                {
                    Id = w.Id,
                    Title = w.Title,
                    Content = w.Content,
                    StartDate = w.StartDate,
                    EndDate = w.EndDate,
                    CompleteDate = w.CompleteDate,
                    Status = w.Status,
                    Progress = w.Progress,
                    Created = w.Created,
                    Project = new Project() { Id = w.ProjectId, Name = w.Project.Name },
                    CreatedUser = new ApplicationUser() { Id = w.CreatedUser.Id, FirstName = w.CreatedUser.FirstName, LastName = w.CreatedUser.LastName },
                    AssignUser = new ApplicationUser() { Id = w.AssignUser.Id, FirstName= w.AssignUser.FirstName, LastName= w.AssignUser.LastName }
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<Work>> GetAllWorkAssignedToMe(string userId)
        {
            return await _context.Works.Include(w => w.CreatedUser).Include(w => w.AssignUser).Include(w => w.Project)
                .Where(w => w.AssignUserId == userId)
                .Select(w => new Work()
                {
                    Id = w.Id,
                    Title = w.Title,
                    //Content = w.Content,
                    StartDate = w.StartDate,
                    EndDate = w.EndDate,
                    CompleteDate = w.CompleteDate,
                    Status = w.Status,
                    Progress = w.Progress,
                    Created = w.Created,
                    Project = new Project() { Id = w.ProjectId, Name = w.Project.Name },
                    CreatedUser = new ApplicationUser() { Id = w.CreatedUser.Id, FirstName = w.CreatedUser.FirstName, LastName = w.CreatedUser.LastName },
                    AssignUser = new ApplicationUser() { Id = w.AssignUser.Id, FirstName = w.AssignUser.FirstName, LastName = w.AssignUser.LastName }
                })
                .ToListAsync();
        }

        public async  Task<IEnumerable<Work>> GetAllWorkIAssign(string userId)
        {
            return await _context.Works.Where(w => w.CreatedUserId == userId)
                .Include(w => w.CreatedUser).Include(w => w.AssignUser).Include(w => w.Project)
                .Select(w => new Work()
                {
                    Id = w.Id,
                    Title = w.Title,
                    //Content = w.Content,
                    StartDate = w.StartDate,
                    EndDate = w.EndDate,
                    CompleteDate = w.CompleteDate,
                    Status = w.Status,
                    Progress = w.Progress,
                    Created = w.Created,
                    Project = new Project() { Id = w.ProjectId, Name = w.Project.Name },
                    CreatedUser = new ApplicationUser() { Id = w.CreatedUser.Id, FirstName = w.CreatedUser.FirstName, LastName = w.CreatedUser.LastName },
                    AssignUser = new ApplicationUser() { Id = w.AssignUser.Id, FirstName = w.AssignUser.FirstName, LastName = w.AssignUser.LastName }
                })
                .ToListAsync();
        }

        public async Task<Work?> GetWorkById(int id)
        {
            return await _context.Works.Where(w => w.Id == id)
                .Include(w => w.CreatedUser).Include(w => w.AssignUser).Include(w => w.Project).Include(w => w.AttachmentFiles)
                .Select(w => new Work()
                {
                    Id = w.Id,
                    Title = w.Title,
                    Content = w.Content,
                    StartDate = w.StartDate,
                    EndDate = w.EndDate,
                    CompleteDate = w.CompleteDate,
                    Status = w.Status,
                    Progress = w.Progress,
                    Created = w.Created,
                    Project = new Project() { Id = w.ProjectId, Name = w.Project.Name, BuocHienTai = w.Project.BuocHienTai },
                    CreatedUser = new ApplicationUser() { Id = w.CreatedUser.Id, FirstName = w.CreatedUser.FirstName, LastName = w.CreatedUser.LastName },
                    AssignUser = new ApplicationUser() { Id = w.AssignUser.Id, FirstName = w.AssignUser.FirstName, LastName = w.AssignUser.LastName },
                    AttachmentFiles = w.AttachmentFiles,
                    isChuyenBuoc = w.isChuyenBuoc
                }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Work>> LichSuThucHienCongViec(string projectId)
        {
            return await _context.Works
                                    .Where(w => w.ProjectId == projectId)
                                    .OrderByDescending(w => w.isChuyenBuoc)
                                    .Select(w => new Work()
                                    {
                                        Id = w.Id,
                                        Title = w.Title,
                                        Content = w.Content,
                                        StartDate = w.StartDate,
                                        EndDate = w.EndDate,
                                        CompleteDate = w.CompleteDate,
                                        Status = w.Status,
                                        Progress = w.Progress,
                                        Created = w.Created,
                                        //Project = new Project() { Id = w.ProjectId, Name = w.Project.Name, BuocHienTai = w.Project.BuocHienTai },
                                        CreatedUser = new ApplicationUser() { Id = w.CreatedUser.Id, FirstName = w.CreatedUser.FirstName, LastName = w.CreatedUser.LastName },
                                        AssignUser = new ApplicationUser() { Id = w.AssignUser.Id, FirstName = w.AssignUser.FirstName, LastName = w.AssignUser.LastName },
                                        //AttachmentFiles = w.AttachmentFiles,
                                        isChuyenBuoc = w.isChuyenBuoc,
                                        ParentWorkId = w.ParentWorkId
                                    })
                                    .ToListAsync();
        }
    }
}
