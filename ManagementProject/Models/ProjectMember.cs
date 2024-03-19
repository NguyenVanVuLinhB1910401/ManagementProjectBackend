using ManagementProject.Models;

namespace ManagementProject.Models
{
    public class ProjectMember
    {
        public int Id { get; set; }

        public string ProjectId { get; set; }
        public Project Project { get; set; }

        public string MemberId { get; set; }
        public ApplicationUser Member { get; set; }

        public string Position { get; set; }
    }
}
