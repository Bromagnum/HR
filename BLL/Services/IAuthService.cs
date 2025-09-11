using BLL.DTOs;
using BLL.Utilities;

namespace BLL.Services;

public interface IAuthService
{
    Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request);
    Task<Result<LoginResponseDto>> RegisterAsync(RegisterRequestDto request);
    Task<Result<LoginResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request);
    Task<Result<bool>> RevokeTokenAsync(string token, string ipAddress);
    Task<Result<bool>> RevokeAllTokensAsync(int userId, string ipAddress);
    Task<Result<bool>> LogoutAsync(int userId, string ipAddress);
    Task<Result<bool>> ChangePasswordAsync(ChangePasswordDto request);
    Task<Result<bool>> ForgotPasswordAsync(ForgotPasswordDto request);
    Task<Result<bool>> ResetPasswordAsync(ResetPasswordDto request);
    Task<Result<UserProfileDto>> GetCurrentUserProfileAsync(int userId);
    Task<Result<bool>> UpdateProfileAsync(UpdateProfileDto request);
}
