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
            var transaction = dbConnection.BeginTransaction();

            try
            {
                var query = @"SELECT email FROM users WHERE users.email = @email";
                var existingAccount = await dbConnection.QueryFirstOrDefaultAsync(query, new { email = request.Email });

                if (existingAccount != null) return null;

                var hashPassword = GenerateHashPassword(request);

                var createUserQuery = @"
                                        INSERT INTO users(firstName,lastName,email,password)
                                        VALUES(@firstName,@lastName,@email,@password);

                                        SELECT * FROM users WHERE userId = LAST_INSERT_ID();";
                                                                                            
                var user = await dbConnection.QueryFirstAsync<UserOutDto>(
                    createUserQuery, new { firstName = request.FirstName, lastName = request.LastName, email = request.Email, password = hashPassword }, transaction);

                var refreshToken = GenerateRefreshToken();

                await InsertRefreshToken(transaction, new RefreshToken {UserId = user.UserId,Token = refreshToken,ExpiresAt = DateTime.UtcNow.AddDays(30)});
                transaction.Commit();
                return new UserLoginOutDto
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserId = user.UserId,
                    ProfileImage = user.ProfileImage,
                    Token =new UserTokenDto { RefreshToken = refreshToken, AccessToken = GenerateAccessToken(new(user.UserId, user.Email)) }
                };
            }
            catch (Exception) {
                transaction.Dispose();
                return null;
            }
           
        }

        public async Task<UserLoginOutDto?> LoginUserAsync(UserLoginDto request)
        {
            var transaction = dbConnection.BeginTransaction();

            try {

                var user = await GetUserFromEmailOrId(value: request.Email,isId:false, transaction: transaction);


                if (user is null) return null;

                if (!VerifyHashPassword(user, request.Password)) return null;

                var refreshToken = "";


                var tokenQuery = @"SELECT * FROM refreshTokens WHERE userId = @userId;";
                var existingRefreshToken = await dbConnection.QueryFirstOrDefaultAsync<RefreshToken>(tokenQuery,new {userId = user.UserId},transaction );
                if (existingRefreshToken is null || existingRefreshToken.ExpiresAt >= DateTime.UtcNow)
                {
                    refreshToken = GenerateRefreshToken();
                    await InsertRefreshToken(transaction, new RefreshToken { UserId = user.UserId, Token = refreshToken, ExpiresAt = DateTime.UtcNow.AddDays(30) });
                }
                else
                {
                    refreshToken = existingRefreshToken.Token;
                }


                transaction.Commit();
                return new UserLoginOutDto
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserId = user.UserId,
                    ProfileImage = user.ProfileImage,
                    Token = new UserTokenDto { RefreshToken = refreshToken, AccessToken = GenerateAccessToken(new(user.UserId, user.Email)) }
                };

            }
            catch(Exception) {
                transaction.Dispose( );
                return null;
            }




        }

        public async Task<UserTokenDto?> RefreshTokenAsync(UserRefreshTokenRequestDto request)
        {
            try {
                IDbTransaction transaction = dbConnection.BeginTransaction();
                var tokenQuery = @"SELECT * FROM refreshTokens WHERE userId = @userId;";
                var existingRefreshToken = await dbConnection.QueryFirstOrDefaultAsync<RefreshToken>(tokenQuery, new { userId = request.UserId }, transaction);

                if (existingRefreshToken is null || existingRefreshToken.Token != request.RefreshToken) 
                    return null;


                var user = await GetUserFromEmailOrId(value:request.UserId,transaction:transaction);

                if (existingRefreshToken.ExpiresAt >= DateTime.UtcNow)
                {
                    var newRefreshToken = GenerateRefreshToken();

                    await InsertRefreshToken(transaction, new RefreshToken { ExpiresAt = DateTime.UtcNow.AddDays(30),Token = newRefreshToken,UserId = request.UserId});
                    transaction.Commit();
                    return new UserTokenDto { AccessToken = GenerateAccessToken(new UserJwt(user!.UserId,user.Email)),RefreshToken = newRefreshToken
                    };
                }


                return new UserTokenDto
                {
                    AccessToken = GenerateAccessToken(new UserJwt(user!.UserId, user.Email)),
                    RefreshToken = existingRefreshToken.Token
                };
                } catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool?> ResetPasswordAsync(ResetPasswordDto request)
        {
            try
            {
                if (request.Password == request.NewPassword) return null;

                var user = await GetUserFromEmailOrId(request.UserId);


                if(!VerifyHashPassword(user!,request.Password)) return false;

                var query = @"UPDATE users WHERE userId = @userId SET password = @newPassword;";

                await dbConnection.ExecuteAsync(query,new {userId = user!.UserId, newPassword = request.NewPassword});
                return true;

            }catch (Exception)
            {
                return false;
            }
        }

        private async Task InsertRefreshToken(IDbTransaction transaction, RefreshToken token)
        {
            var insertRefreshToken = @"INSERT INTO refreshTokens(userId,token,expiresAt)
                                            VALUES(@userId,@token,@expiresAt);";
            await dbConnection.ExecuteAsync(insertRefreshToken, token, transaction);
        }

        private async Task<User?> GetUserFromEmailOrId(string value, bool isId = true,IDbTransaction? transaction = null)
        {
            try
            {
                User? user = null;
                if (isId)
                {
                   var query = @"SELECT * FROM users WHERE userId = @userId;";
                   user = await dbConnection.QueryFirstOrDefaultAsync<User>(
                        query, new { userId = value }, transaction);
                }
                else
                {
                    var query = @"SELECT * FROM users WHERE email = @email;";
                    user = await dbConnection.QueryFirstOrDefaultAsync<User>(
                         query, new { email = value }, transaction);
                }
                return user;

            }
            catch (Exception)
            {
                return null;

            }


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
                expires:DateTime.UtcNow.AddMinutes(1)
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
