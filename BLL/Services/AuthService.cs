using System.Security.Claims;
using AutoMapper;
using BLL.DTOs;
using BLL.Utilities;
using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BLL.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IJwtService _jwtService;
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManager,
        IJwtService jwtService,
        AppDbContext context,
        IMapper mapper,
        ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _jwtService = jwtService;
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !user.IsActive)
            {
                _logger.LogWarning("Login attempt with invalid email: {Email}", request.Email);
                return Result<LoginResponseDto>.Fail("Geçersiz email veya şifre.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Failed login attempt for user: {Email}", request.Email);
                
                if (result.IsLockedOut)
                    return Result<LoginResponseDto>.Fail("Hesabınız geçici olarak kilitlendi. Lütfen daha sonra tekrar deneyin.");
                
                return Result<LoginResponseDto>.Fail("Geçersiz email veya şifre.");
            }

            // Update last login info
            user.LastLoginAt = DateTime.UtcNow;
            user.LastLoginIp = request.IpAddress;
            user.DeviceInfo = request.UserAgent;
            await _userManager.UpdateAsync(user);

            // Log successful login
            await LogUserLoginAsync(user.Id, request.IpAddress, request.UserAgent, true);

            // Generate tokens
            var userClaims = await BuildUserClaimsAsync(user);
            var accessToken = _jwtService.GenerateAccessToken(userClaims);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Save refresh token
            await SaveRefreshTokenAsync(user.Id, refreshToken, request.IpAddress ?? "Unknown");

            var response = new LoginResponseDto
            {
                UserId = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                FirstName = user.FirstName ?? "",
                LastName = user.LastName ?? "",
                FullName = user.FullName,
                PersonId = user.PersonId,
                DepartmentId = user.DepartmentId,
                DepartmentName = userClaims.DepartmentName,
                BranchId = user.BranchId,
                Roles = userClaims.Roles,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(15), // Default 15 minutes
                RefreshTokenExpiry = DateTime.UtcNow.AddDays(7) // Default 7 days
            };

            _logger.LogInformation("User {Email} logged in successfully", request.Email);
            return Result<LoginResponseDto>.Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user: {Email}", request.Email);
            return Result<LoginResponseDto>.Fail("Giriş sırasında bir hata oluştu.");
        }
    }

    public async Task<Result<LoginResponseDto>> RegisterAsync(RegisterRequestDto request)
    {
        try
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return Result<LoginResponseDto>.Fail("Bu email adresi zaten kullanımda.");
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PersonId = request.PersonId,
                DepartmentId = request.DepartmentId,
                BranchId = request.BranchId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("User registration failed for {Email}: {Errors}", request.Email, errors);
                return Result<LoginResponseDto>.Fail($"Kayıt işlemi başarısız: {errors}");
            }

            // Assign default role
            await _userManager.AddToRoleAsync(user, "Employee");

            // Log successful registration
            await LogUserLoginAsync(user.Id, request.IpAddress, request.UserAgent, true, "Registration");

            // Generate tokens
            var userClaims = await BuildUserClaimsAsync(user);
            var accessToken = _jwtService.GenerateAccessToken(userClaims);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Save refresh token
            await SaveRefreshTokenAsync(user.Id, refreshToken, request.IpAddress ?? "Unknown");

            var response = new LoginResponseDto
            {
                UserId = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                FirstName = user.FirstName ?? "",
                LastName = user.LastName ?? "",
                FullName = user.FullName,
                PersonId = user.PersonId,
                DepartmentId = user.DepartmentId,
                BranchId = user.BranchId,
                Roles = userClaims.Roles,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(15),
                RefreshTokenExpiry = DateTime.UtcNow.AddDays(7)
            };

            _logger.LogInformation("User {Email} registered successfully", request.Email);
            return Result<LoginResponseDto>.Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for user: {Email}", request.Email);
            return Result<LoginResponseDto>.Fail("Kayıt sırasında bir hata oluştu.");
        }
    }

    public async Task<Result<LoginResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        try
        {
            var refreshToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);

            if (refreshToken == null || !refreshToken.IsActive)
            {
                _logger.LogWarning("Invalid refresh token used: {Token}", request.RefreshToken);
                return Result<LoginResponseDto>.Fail("Geçersiz refresh token.");
            }

            var user = refreshToken.User;
            if (user == null || !user.IsActive)
            {
                return Result<LoginResponseDto>.Fail("Kullanıcı bulunamadı veya aktif değil.");
            }

            // Revoke old token
            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;
            refreshToken.RevokedByIp = request.IpAddress;

            // Generate new tokens
            var userClaims = await BuildUserClaimsAsync(user);
            var newAccessToken = _jwtService.GenerateAccessToken(userClaims);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            // Save new refresh token
            await SaveRefreshTokenAsync(user.Id, newRefreshToken, request.IpAddress ?? "Unknown");

            await _context.SaveChangesAsync();

            var response = new LoginResponseDto
            {
                UserId = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                FirstName = user.FirstName ?? "",
                LastName = user.LastName ?? "",
                FullName = user.FullName,
                PersonId = user.PersonId,
                DepartmentId = user.DepartmentId,
                BranchId = user.BranchId,
                Roles = userClaims.Roles,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(15),
                RefreshTokenExpiry = DateTime.UtcNow.AddDays(7)
            };

            _logger.LogInformation("Token refreshed for user: {UserId}", user.Id);
            return Result<LoginResponseDto>.Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            return Result<LoginResponseDto>.Fail("Token yenileme sırasında bir hata oluştu.");
        }
    }

    public async Task<Result<bool>> RevokeTokenAsync(string token, string ipAddress)
    {
        try
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token);

            if (refreshToken == null || !refreshToken.IsActive)
            {
                return Result<bool>.Fail("Token bulunamadı veya zaten iptal edilmiş.");
            }

            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReasonRevoked = "Revoked by user";

            await _context.SaveChangesAsync();

            _logger.LogInformation("Token revoked: {Token}", token);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revoking token");
            return Result<bool>.Fail("Token iptal edilirken hata oluştu.");
        }
    }

    public async Task<Result<bool>> RevokeAllTokensAsync(int userId, string ipAddress)
    {
        try
        {
            var refreshTokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && rt.IsActive)
                .ToListAsync();

            foreach (var token in refreshTokens)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
                token.RevokedByIp = ipAddress;
                token.ReasonRevoked = "Revoked all tokens";
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("All tokens revoked for user: {UserId}", userId);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revoking all tokens for user: {UserId}", userId);
            return Result<bool>.Fail("Tüm token'lar iptal edilirken hata oluştu.");
        }
    }

    public async Task<Result<bool>> LogoutAsync(int userId, string ipAddress)
    {
        try
        {
            await RevokeAllTokensAsync(userId, ipAddress);
            await _signInManager.SignOutAsync();

            _logger.LogInformation("User logged out: {UserId}", userId);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout for user: {UserId}", userId);
            return Result<bool>.Fail("Çıkış işlemi sırasında hata oluştu.");
        }
    }

    public async Task<Result<bool>> ChangePasswordAsync(ChangePasswordDto request)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return Result<bool>.Fail("Kullanıcı bulunamadı.");
            }

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<bool>.Fail($"Şifre değiştirme başarısız: {errors}");
            }

            _logger.LogInformation("Password changed for user: {UserId}", request.UserId);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password for user: {UserId}", request.UserId);
            return Result<bool>.Fail("Şifre değiştirme sırasında hata oluştu.");
        }
    }

    public async Task<Result<bool>> ForgotPasswordAsync(ForgotPasswordDto request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return Result<bool>.Ok(true);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // Here you would send an email with the reset token
            // For now, just log it (in production, implement email service)
            _logger.LogInformation("Password reset token generated for user: {Email}, Token: {Token}", request.Email, token);

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating password reset token for: {Email}", request.Email);
            return Result<bool>.Fail("Şifre sıfırlama sırasında hata oluştu.");
        }
    }

    public async Task<Result<bool>> ResetPasswordAsync(ResetPasswordDto request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result<bool>.Fail("Kullanıcı bulunamadı.");
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<bool>.Fail($"Şifre sıfırlama başarısız: {errors}");
            }

            _logger.LogInformation("Password reset successfully for user: {Email}", request.Email);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting password for: {Email}", request.Email);
            return Result<bool>.Fail("Şifre sıfırlama sırasında hata oluştu.");
        }
    }

    public async Task<Result<UserProfileDto>> GetCurrentUserProfileAsync(int userId)
    {
        try
        {
            var user = await _userManager.Users
                .Include(u => u.Person)
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return Result<UserProfileDto>.Fail("Kullanıcı bulunamadı.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var profile = new UserProfileDto
            {
                UserId = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                FirstName = user.FirstName ?? "",
                LastName = user.LastName ?? "",
                FullName = user.FullName,
                PersonId = user.PersonId,
                PersonName = user.Person?.FirstName + " " + user.Person?.LastName,
                DepartmentId = user.DepartmentId,
                DepartmentName = user.Department?.Name,
                BranchId = user.BranchId,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                Roles = roles.ToList()
            };

            return Result<UserProfileDto>.Ok(profile);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user profile: {UserId}", userId);
            return Result<UserProfileDto>.Fail("Profil bilgileri alınırken hata oluştu.");
        }
    }

    public async Task<Result<bool>> UpdateProfileAsync(UpdateProfileDto request)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return Result<bool>.Fail("Kullanıcı bulunamadı.");
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.UserName = request.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<bool>.Fail($"Profil güncelleme başarısız: {errors}");
            }

            _logger.LogInformation("Profile updated for user: {UserId}", request.UserId);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating profile for user: {UserId}", request.UserId);
            return Result<bool>.Fail("Profil güncelleme sırasında hata oluştu.");
        }
    }

    #region Private Methods

    private async Task<UserClaimsDto> BuildUserClaimsAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        
        // Get department name if department exists
        string? departmentName = null;
        if (user.DepartmentId.HasValue)
        {
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.Id == user.DepartmentId.Value);
            departmentName = department?.Name;
        }

        return new UserClaimsDto
        {
            UserId = user.Id,
            UserName = user.UserName ?? "",
            Email = user.Email ?? "",
            FirstName = user.FirstName ?? "",
            LastName = user.LastName ?? "",
            PersonId = user.PersonId,
            DepartmentId = user.DepartmentId,
            DepartmentName = departmentName,
            BranchId = user.BranchId,
            Roles = roles.ToList()
        };
    }

    private async Task SaveRefreshTokenAsync(int userId, string token, string ipAddress)
    {
        var refreshToken = new RefreshToken
        {
            UserId = userId,
            Token = token,
            ExpiryDate = DateTime.UtcNow.AddDays(7), // 7 days
            CreatedByIp = ipAddress,
            CreatedAt = DateTime.UtcNow
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();
    }

    private async Task LogUserLoginAsync(int userId, string? ipAddress, string? userAgent, bool isSuccessful, string? notes = null)
    {
        var loginLog = new UserLoginLog
        {
            UserId = userId,
            LoginTime = DateTime.UtcNow,
            IpAddress = ipAddress ?? "Unknown",
            UserAgent = userAgent ?? "Unknown",
            IsSuccessful = isSuccessful,
            FailureReason = isSuccessful ? null : "Login failed",
            CreatedAt = DateTime.UtcNow
        };

        if (!string.IsNullOrEmpty(notes))
        {
            loginLog.UserAgent += $" - {notes}";
        }

        _context.UserLoginLogs.Add(loginLog);
        await _context.SaveChangesAsync();
    }

    #endregion
}
