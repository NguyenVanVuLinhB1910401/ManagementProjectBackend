using System.ComponentModel.DataAnnotations;

namespace ManagementProject.DTOs
{
    public class DepartmentReq
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Name is requied")]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
