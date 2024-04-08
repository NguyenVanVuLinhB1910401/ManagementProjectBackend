namespace ManagementProject.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int WorkId { get; set; }
        public Work Work { get; set; }
        public string Content { get; set; }
        public List<AttachmentFile> AttachmentFiles { get; set; }
        public DateTime Created { get; set; }

    }
}
