using ManagementProject.Models;

namespace ManagementProject.Interfaces
{
    public interface IAttachmentFileRepository : IGenericRepository<AttachmentFile>
    {
        Task AddRange(List<AttachmentFile> attachments);
        Task<IEnumerable<AttachmentFile>> GetAllFileByWork(int workId);
    }
}
