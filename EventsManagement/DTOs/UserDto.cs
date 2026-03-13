using EventsManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace EventsManagement.DTOs
{
    public class UserLoginDto(string email,string password):UserBase(email,password)
    {
    }

    public class UserCreateDto(string firstName, string lastName,string email,string password) : UserBase(email, password)
    {
        [Required(ErrorMessage ="First name is required"),StringLength(20,ErrorMessage ="Name cannot exceed 20 characters")]
        public string FirstName { get; set; } = firstName;
        [Required(ErrorMessage = "Last name is required"), StringLength(20, ErrorMessage = "Name cannot exceed 20 characters")]

        public string LastName { get; set; } = lastName;
    }

    public class UserOutDto(Guid userId, string firstName,string lastName,string email,string? profileImage)
    {
        public Guid UserId { get; set; } = userId;
        public string FirstName { get; set; } = firstName;
        public string LastName { get; set; } = lastName;
        public string Email { get; set; } = email;
        public string? ProfileImage { get; set; } = profileImage;

    }
    public class UserJwt(Guid userId,string email,string? role = null)
    {
        public Guid UserId { get; set; } = userId;
        public string Email { get; set; } = email;
        public string? Role { get; set; } = role;

    }

    public class UserRefreshTokenRequestDto(Guid userId,string refreshToken)
    {
        public Guid userId { get; set; } = userId;
        public string RefreshToken { get; set; } = refreshToken;
    }

    public class UserTokenDto(string refreshToken, string accessToken)
    {
        public string RefreshToken { get; set; } = refreshToken;
        public string AccessToken { get; set; } = accessToken;  
    }

    public class UserLoginOutDto(Guid userId, string firstName, string lastName, string email, string? profileImage,string refreshToken,string accessToken) : UserOutDto(userId, firstName, lastName, email, profileImage)
    {
        public UserTokenDto Token { get; set; } = new(refreshToken,accessToken);
    }
}
