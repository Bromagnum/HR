using System.Security.Claims;
using BLL.DTOs;

namespace BLL.Services;

public interface IJwtService
{
    string GenerateAccessToken(UserClaimsDto user);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    bool ValidateToken(string token);
    DateTime GetTokenExpiration(string token);
}
