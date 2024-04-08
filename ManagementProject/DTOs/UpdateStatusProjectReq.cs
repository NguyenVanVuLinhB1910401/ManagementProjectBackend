using System.ComponentModel.DataAnnotations;

namespace ManagementProject.DTOs
{
    public class UpdateStatusProjectReq
    {
        [Required(ErrorMessage = "Id is requied")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Status is requied")]
        public int Status { get; set; }
    }
}
