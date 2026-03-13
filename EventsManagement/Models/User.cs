using System.ComponentModel.DataAnnotations;

namespace EventsManagement.Models
{
    public class UserBase(string email,string password) {
        [Required (ErrorMessage ="Email must be provided"), EmailAddress(ErrorMessage ="Invalid email format")]
        public string Email { get; set; } = email;
       [Required(ErrorMessage = "Password must be provided"),StringLength(20,MinimumLength =8,ErrorMessage ="{0} must be atleast {2} characters and maximum {1}")]
        public string Password { get; set; } = password;
    }
    public class User(string userId, string firstName,string lastName,string email,string password,string profileImage,string? institutionId,string? schoolId):UserBase(email,password)
    {
        public string UserId { get; set; } = userId;
        public string FirstName { get; set; } = firstName;
        public string LastName { get; set; } = lastName;
        public string ProfileImage { get; set; } = profileImage;
        public string? InstitutionId { get; set; }=institutionId;
        public string? SchoolId {  get; set; }=schoolId; 
    }

}
