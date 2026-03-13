using Dapper;
using EventsManagement.DTOs;
using EventsManagement.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EventsManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthRepository authRepository, IDbConnection dbConnection) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserLoginOutDto>> CreateUser(UserCreateDto userCreate)
        {
            if (userCreate == null)
            {
                return BadRequest();
            }

            var user = await authRepository.CreateUserAsync(userCreate);
            if (user == null)
            {
                return BadRequest("Account with email already exists");
            }

            return Ok(user);

        }

        [HttpGet]
        public async Task<ActionResult> TestAsync()
        {
            var query = @"SELECT * FROM institutions";

            var data =  await dbConnection.QueryAsync(
                query);
            return Ok(data);
        }

        [HttpPost("login")]

        public async Task<ActionResult<UserLoginOutDto>> LoginUserAsync(UserLoginDto userLogin)
        {
            if (userLogin == null)
            {
                return BadRequest();

            }

            var user =await authRepository.LoginUserAsync(userLogin);

            if (user == null)
            {
                return Unauthorized("Invalid email or Password");
            }
            return Ok(user);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<UserTokenDto>> RefreshToken(UserRefreshTokenRequestDto request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var refreshToken = await authRepository.RefreshTokenAsync(request);

            if (refreshToken == null)
            {
                return NotFound("Invalid user or refresh token");
            }
            return Ok(refreshToken);
        }
    }
}
