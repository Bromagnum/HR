using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BLL.DTOs;
using DAL.Entities;

namespace MVC.Controllers.Api;

/// <summary>
/// Authentication API Controller
/// </summary>
[Route("api/[controller]")]
public class AuthController : BaseApiController
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,
        ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Kullanıcı girişi
    /// </summary>
    /// <param name="model">Giriş bilgileri</param>
    /// <returns>JWT Token</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        var validationResult = ValidateModel();
        if (validationResult != null)
            return validationResult;

        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                _logger.LogWarning("Giriş denemesi başarısız: {Email}", model.Email);
                return Error("Geçersiz email veya şifre", 401);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Şifre doğrulama başarısız: {Email}", model.Email);
                return Error("Geçersiz email veya şifre", 401);
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("Pasif kullanıcı giriş denemesi: {Email}", model.Email);
                return Error("Hesabınız pasif durumda", 401);
            }

            // Token oluştur
            var token = await GenerateJwtToken(user);
            
            // Son giriş bilgilerini güncelle
            user.LastLoginAt = DateTime.UtcNow;
            user.LastLoginIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            await _userManager.UpdateAsync(user);

            _logger.LogInformation("Başarılı giriş: {Email}", model.Email);

            return Success(new LoginResponse
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(GetTokenExpiryMinutes()),
                User = new UserInfo
                {
                    Id = user.Id,
                    Email = user.Email!,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = (await _userManager.GetRolesAsync(user)).ToList()
                }
            }, "Giriş başarılı");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Giriş işlemi sırasında hata: {Email}", model.Email);
            return Error("Giriş işlemi sırasında hata oluştu", 500);
        }
    }

    /// <summary>
    /// Token yenileme
    /// </summary>
    /// <returns>Yeni JWT Token</returns>
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Error("Geçersiz token", 401);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !user.IsActive)
            {
                return Error("Kullanıcı bulunamadı veya pasif", 401);
            }

            var token = await GenerateJwtToken(user);

            return Success(new TokenResponse
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(GetTokenExpiryMinutes())
            }, "Token yenilendi");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Token yenileme hatası");
            return Error("Token yenileme sırasında hata oluştu", 500);
        }
    }

    /// <summary>
    /// Kullanıcı bilgilerini getir
    /// </summary>
    /// <returns>Mevcut kullanıcı bilgileri</returns>
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Error("Geçersiz token", 401);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Error("Kullanıcı bulunamadı", 404);
            }

            return Success(new UserInfo
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = (await _userManager.GetRolesAsync(user)).ToList()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı bilgileri getirme hatası");
            return Error("Kullanıcı bilgileri alınamadı", 500);
        }
    }

    /// <summary>
    /// Çıkış yapma
    /// </summary>
    /// <returns>Başarı yanıtı</returns>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("Kullanıcı çıkış yaptı");
            return Success("Çıkış başarılı");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Çıkış işlemi hatası");
            return Error("Çıkış işlemi sırasında hata oluştu", 500);
        }
    }

    private async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var jwtSettings = _configuration.GetSection("JWT");
        var secretKey = jwtSettings["SecretKey"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];

        var key = Encoding.ASCII.GetBytes(secretKey!);
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!),
            new("FirstName", user.FirstName),
            new("LastName", user.LastName)
        };

        // Rolleri ekle
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(GetTokenExpiryMinutes()),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private int GetTokenExpiryMinutes()
    {
        var jwtSettings = _configuration.GetSection("JWT");
        return int.Parse(jwtSettings["AccessTokenExpiryMinutes"] ?? "15");
    }
}

/// <summary>
/// Giriş isteği modeli
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Email adresi
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Şifre
    /// </summary>
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Giriş yanıt modeli
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// JWT Token
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Token bitiş tarihi
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Kullanıcı bilgileri
    /// </summary>
    public UserInfo User { get; set; } = new();
}

/// <summary>
/// Token yanıt modeli
/// </summary>
public class TokenResponse
{
    /// <summary>
    /// JWT Token
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Token bitiş tarihi
    /// </summary>
    public DateTime ExpiresAt { get; set; }
}

/// <summary>
/// Kullanıcı bilgi modeli
/// </summary>
public class UserInfo
{
    /// <summary>
    /// Kullanıcı ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Ad
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Soyad
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Roller
    /// </summary>
    public List<string> Roles { get; set; } = new();

    /// <summary>
    /// Tam ad
    /// </summary>
    public string FullName => $"{FirstName} {LastName}".Trim();
}
