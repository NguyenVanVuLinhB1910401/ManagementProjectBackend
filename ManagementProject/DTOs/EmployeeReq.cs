using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace ManagementProject.DTOs
{
    public class EmployeeReq
    {
        //[Required(ErrorMessage = "User Name is required")]
        //public string? Username { get; set; }
        public string? Id { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        //[Required(ErrorMessage = "Password is required")]
        //public string? Password { get; set; }

        [Required(ErrorMessage = "FistName is required")]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        [MaxLength(20)]
        public string LastName { get; set; }

        public string? CCCD { get; set; }

        [Required(ErrorMessage = "DateOfBirth is required")]
        public DateTime DateOfBirth { get; set; }

        public string? Address { get; set; }

        public string Gender { get; set; }

        [MaxLength(12)]
        public string? MobilePhone { get; set; }

        public string? Location { get; set; }

        public string? DepartmentId { get; set; }
        public string? Position { get; set; }
    }
}
