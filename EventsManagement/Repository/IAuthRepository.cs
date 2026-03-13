using EventsManagement.DTOs;

namespace EventsManagement.Repository
{
    public interface IAuthRepository
    {
        Task<UserLoginOutDto?> CreateUserAsync(UserCreateDto user);
        Task<UserLoginOutDto?> LoginUserAsync(UserLoginDto user);
        Task<UserTokenDto?> RefreshTokenAsync(UserRefreshTokenRequestDto request);
        Task<bool?> ResetPasswordAsync(ResetPasswordDto request);

    }
}
