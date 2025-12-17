using BlogApp.Application.DTOs.Auth;

namespace BlogApp.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> ResgiterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    }

}