using ManagementProject.Models;

namespace ManagementProject.DTOs
{
    public class ProjectMemberRes
    {
        public string ProjectId { get; set; }
        public string MemberId { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string PhongBan { get; set; }
        public string ViTriTrongPhongBan { get; set; }
    }
}
