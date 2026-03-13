using EventsManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace EventsManagement.DTOs
{
    public class UserLoginDto:UserBase
    {
    }

    public class UserCreateDto : UserBase
    {
        [Required(ErrorMessage ="First name is required"),StringLength(20,ErrorMessage ="Name cannot exceed 20 characters")]
        public required string FirstName { get; set; } 
        [Required(ErrorMessage = "Last name is required"), StringLength(20, ErrorMessage = "Name cannot exceed 20 characters")]

        public required string LastName { get; set; } 
    }

    public class UserOutDto
    {
        public required string UserId { get; set; } 
        public required string FirstName { get; set; }
        public required string LastName { get; set; }  
        public required string Email { get; set; } 
        public string? ProfileImage { get; set; }

    }
    public class UserJwt(string userId,string email,string? role = null)
    {
        public string UserId { get; set; } = userId;
        public string Email { get; set; } = email;
        public string? Role { get; set; } = role;

    }

    public class UserRefreshTokenRequestDto
    {
        public required string UserId { get; set; } 
        public required string RefreshToken { get; set; } 
    }

    public class UserTokenDto
    {
        public required string RefreshToken { get; set; } 
        public required string AccessToken { get; set; }   
    }

    public class UserLoginOutDto: UserOutDto
    {
        public required UserTokenDto Token { get; set; }
    }
}
