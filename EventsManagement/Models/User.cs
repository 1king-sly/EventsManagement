using System.ComponentModel.DataAnnotations;

namespace EventsManagement.Models
{
    public class UserBase {
        [Required (ErrorMessage ="Email must be provided"), EmailAddress(ErrorMessage ="Invalid email format")]
        public required string Email { get; set; } 
       [Required(ErrorMessage = "Password must be provided"),StringLength(20,MinimumLength =8,ErrorMessage ="{0} must be atleast {2} characters and maximum {1}")]
        public required string Password { get; set; }
    }
    public class User: UserBase
    {
        public required string UserId { get; set; } 
        public required string FirstName { get; set; } 
        public required string LastName { get; set; }
        public string? ProfileImage { get; set; } 
        public string? InstitutionId { get; set; }
        public string? SchoolId {  get; set; }
    }

}
