using EventsManagement.DTOs;
using EventsManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EventsManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthRepository authRepository) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserLoginOutDto>> CreateUserAsync(UserCreateDto userCreate)
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
        public async Task<ActionResult<UserTokenDto>> RefreshTokenAsync(UserRefreshTokenRequestDto request)
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

        [Authorize]
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPasswordAsync(ResetPasswordDto request)
        {
            var resetPassword = await authRepository.ResetPasswordAsync(request);

            if (resetPassword is null || resetPassword is false)
            {
                return BadRequest(resetPassword is null?"New password can't be same as old password":"Password or internet error occurred! Please try again");
            }
            return Ok("Password changed success");
        }
    }
}
