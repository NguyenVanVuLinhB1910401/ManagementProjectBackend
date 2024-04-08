using ManagementProject.Models;
using System.ComponentModel.DataAnnotations;

namespace ManagementProject.DTOs
{
    public class ProjectReq
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; }

        [Required(ErrorMessage = "StartDate is required")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required")]
        public DateTime? EndDate { get; set; }

        public DateTime? CompleteDate { get; set; }
        //public int Status { get; set; }
        //public int isProject { get; set; }
        //public string CreatedId { get; set; }
        public List<Member>? Members { get; set; }

        //public int isDelete { get; set; }
        public int QuyTrinhId { get; set; }
    }

    public class Member 
    {
        public string MemberId { get; set; }
        public string Position { get; set; }
    }



}
