using ManagementProject.Interfaces;
using ManagementProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace ManagementProject.Repositories
{
    public class AttachmentFileRepository : GenericRepository<AttachmentFile>, IAttachmentFileRepository
    {
        public AttachmentFileRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task AddRange(List<AttachmentFile> attachments)
        {
            await _context.AttachmentFiles.AddRangeAsync(attachments);
        }

        public async Task<IEnumerable<AttachmentFile>> GetAllFileByWork(int workId)
        {
            return await _context.AttachmentFiles.Where(a => a.WorkId == workId).ToListAsync();
        }
    }
}
