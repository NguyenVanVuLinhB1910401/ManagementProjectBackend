using System.ComponentModel.DataAnnotations;

namespace ManagementProject.DTOs
{
    public class UpdateProgressReq
    {
        [Required(ErrorMessage = "Id is requied")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Progress is requied")]
        public int Progress { get; set; }
    }
}
