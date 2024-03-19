using ManagementProject.Models;

namespace ManagementProject.Models
{
    public class Department
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }

        public int Status { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }

    }
}
