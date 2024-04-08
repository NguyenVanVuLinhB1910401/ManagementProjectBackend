using System.ComponentModel.DataAnnotations;

namespace ManagementProject.DTOs
{
    public class ChuyenBuocTiepTheoReq
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200)]
        public string Title { get; set; }

        public string Content { get; set; }

        [Required(ErrorMessage = "StartDate is required")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required")]
        public DateTime? EndDate { get; set; }

        //[Required(ErrorMessage = "AssignUserId is required")]
        //public string AssignUserId { get; set; }

        [Required(ErrorMessage = "ProjectId is required")]
        public string ProjectId { get; set; }

        [Required(ErrorMessage = "Created is required")]
        public DateTime? Created { get; set; }
        public List<IFormFile>? AttachmentFiles { get; set; }

    }
}
