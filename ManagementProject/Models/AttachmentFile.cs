namespace ManagementProject.Models
{
    public class AttachmentFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public int WorkId { get; set; }
        public Work Work { get; set; }
        
        
    }
}
