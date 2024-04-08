using System.ComponentModel.DataAnnotations;

namespace ManagementProject.DTOs
{
    public class UpdateStatusWorkReq
    {
        [Required(ErrorMessage = "Id is requied")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Status is requied")]
        public int Status { get; set; }
    }
}
