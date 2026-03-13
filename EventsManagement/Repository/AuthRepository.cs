using Dapper;
using EventsManagement.DTOs;
using EventsManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EventsManagement.Repository
{
    public class AuthRepository(IDbConnection dbConnection,IConfiguration configuration) : IAuthRepository
    {
        public async Task<UserLoginOutDto?> CreateUserAsync(UserCreateDto request)
        {
            var query = @"SELECT email FROM users WHERE users.email = @email";
            var existingAccount =await dbConnection.QueryFirstOrDefaultAsync(query,new{ email = request.Email});

            if (existingAccount != null) return null;

            var hashPassword = GenerateHashPassword(request);

            var createUserQuery = @"
INSERT INTO users(firstName,lastName,email,password)
VALUES(@firstName,@lastName,@email,@password);

SELECT * FROM users WHERE userId = LAST_INSERT_ID();
";
            var user = await dbConnection.QueryFirstAsync< UserOutDto>(
                createUserQuery,new {firstName = request.FirstName,lastName =request.LastName,email =request.Email,password = hashPassword});


            return new UserLoginOutDto
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserId = user.UserId,
            ProfileImage= user.ProfileImage, 
                Token = new UserTokenDto(refreshToken:GenerateRefreshToken(),GenerateAccessToken(new(user.UserId,user.Email)))};
        }

        public async Task<UserLoginOutDto?> LoginUserAsync(UserLoginDto request)
        {
            var query = @"SELECT * FROM users WHERE email = @email;";
            var user =await dbConnection.QueryFirstOrDefaultAsync<User>(
                query,new {email = request.Email});

            if (user is null) return null;

            if (!VerifyHashPassword(user, request.Password)) return null;

             return new UserLoginOutDto
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserId = user.UserId,
            ProfileImage= user.ProfileImage, 
                Token = new UserTokenDto(refreshToken:GenerateRefreshToken(),GenerateAccessToken(new(user.UserId,user.Email)))};


        }

        public Task<UserTokenDto?> RefreshTokenAsync(UserRefreshTokenRequestDto request)
        {
            throw new NotImplementedException();
        }

        private static string GenerateHashPassword(UserBase user)
        {
            return new PasswordHasher<UserBase>().HashPassword(user, user.Password);
        }
        private static bool VerifyHashPassword(User user, string providedPassword) { 
          var verifyPassword =  new PasswordHasher<UserBase>().VerifyHashedPassword(user,user.Password, providedPassword);

            return verifyPassword == PasswordVerificationResult.Success;
        }
        private string GenerateAccessToken(UserJwt user) {
            var claims = new List<Claim> { 
                new(ClaimTypes.NameIdentifier,user.UserId),
                new (ClaimTypes.Email,user.Email ),
                new(ClaimTypes.Role,user.Role ?? "User"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:Token"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration["AppSettings:Issuer"],
                audience: configuration["AppSettings:Audience"],
                claims:claims,
                signingCredentials: creds,
                expires:DateTime.UtcNow.AddDays(1)
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

    }
}
