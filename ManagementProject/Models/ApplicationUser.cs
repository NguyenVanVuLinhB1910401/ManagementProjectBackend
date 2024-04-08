using ManagementProject.Models;
using Microsoft.AspNetCore.Identity;

namespace ManagementProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string? CCCD { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Address { get; set; }

        public string? Gender { get; set; }

        public string? MobilePhone { get; set; }

        public string? Location { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExprityTime { get; set; }
        public string? ResetPasswordToken { get; set; }

        public string DepartmentId { get; set; }
        public string Position { get; set; }
        public Department Department { get; set; }

        public int Status { get; set; }

        public int isLock { get; set; }

        public List<Project> Projects { get; set; }

        public List<ProjectMember> ProjectMembers { get; set; }
        public List<Work> CreatedWorks { get; set; }
        public List<Work> AssignWorks { get; set; }
        public List<Comment> Comments { get; set; }
        public List<QuyTrinh> QuyTrinhs { get; set; }
        public List<BuocThucHien> BuocThucHiens { get; set; }
    }
}
