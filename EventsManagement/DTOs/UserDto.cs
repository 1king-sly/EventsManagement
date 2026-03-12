using EventsManagement.Models;

namespace EventsManagement.DTOs
{
    public class UserLoginDto(string email,string password):UserBase(email,password)
    {
    }

    public class UserCreateDto(string firstName, string lastName,string email,string password) : UserBase(email, password)
    {
        public string FirstName { get; set; } = firstName;
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
