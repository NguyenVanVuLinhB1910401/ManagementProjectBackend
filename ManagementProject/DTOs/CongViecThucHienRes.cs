using ManagementProject.Models;

namespace ManagementProject.DTOs
{
    public class CongViecThucHienRes
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public int Progress { get; set; }
        public string CreatedUserId { get; set; }
        public ApplicationUser CreatedUser { get; set; }
        public string AssignUserId { get; set; }
        public ApplicationUser AssignUser { get; set; }
        public string ProjectId { get; set; }
        public Project Project { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public List<AttachmentFile> AttachmentFiles { get; set; }
        public List<Comment> Comments { get; set; }

        public int? ParentWorkId { get; set; }
        public int isChuyenBuoc { get; set; }
    }
}
