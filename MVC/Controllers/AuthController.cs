using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BLL.Services;
using BLL.DTOs;
using DAL.Entities;
using MVC.Models;
using AutoMapper;

namespace MVC.Controllers;

[AllowAnonymous]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthController> _logger;
    private readonly IWebHostEnvironment _hostEnvironment;

    public AuthController(
        IAuthService authService,
        ICurrentUserService currentUserService,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IMapper mapper,
        ILogger<AuthController> logger,
        IWebHostEnvironment hostEnvironment)
    {
        _authService = authService;
        _currentUserService = currentUserService;
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _logger = logger;
        _hostEnvironment = hostEnvironment;
    }

    #region Simple Login (Debug)
    
    [HttpGet]
    public IActionResult SimpleLogin(string? returnUrl = null)
    {
        _logger.LogInformation("SimpleLogin GET called. ReturnUrl: {ReturnUrl}", returnUrl);
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel());
    }
    
    #endregion

    #region Login

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        _logger.LogInformation("GET Login action called. ReturnUrl: {ReturnUrl}, IsAuthenticated: {IsAuth}", 
            returnUrl, User.Identity?.IsAuthenticated);
            
        if (User.Identity?.IsAuthenticated == true)
        {
            _logger.LogInformation("User already authenticated, redirecting to home");
            return RedirectToAction("Index", "Home");
        }

        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost]
    [IgnoreAntiforgeryToken] // Temporarily disable for debugging 405 error
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        _logger.LogInformation("POST Login action called. Email: {Email}, ReturnUrl: {ReturnUrl}", model?.Email, returnUrl);
        
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Login model validation failed. Errors: {Errors}", 
                string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            return View(model);
        }

        try
        {
            var request = new LoginRequestDto
            {
                Email = model.Email,
                Password = model.Password,
                RememberMe = model.RememberMe,
                IpAddress = GetClientIpAddress(),
                UserAgent = Request.Headers.UserAgent.ToString()
            };

            var result = await _authService.LoginAsync(request);
            
            _logger.LogInformation("Login attempt for {Email}: Success={Success}, Message={Message}", 
                model.Email, result.IsSuccess, result.Message);
            
            if (result.IsSuccess && result.Data != null)
            {
                // For cookie-based authentication, sign in the user with custom claims
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    // Create additional claims for PersonId, DepartmentId, etc.
                    var additionalClaims = new List<System.Security.Claims.Claim>();
                    
                    if (user.PersonId.HasValue)
                    {
                        additionalClaims.Add(new System.Security.Claims.Claim("PersonId", user.PersonId.Value.ToString()));
                    }
                    
                    if (user.DepartmentId.HasValue)
                    {
                        additionalClaims.Add(new System.Security.Claims.Claim("DepartmentId", user.DepartmentId.Value.ToString()));
                    }
                    
                    if (user.BranchId.HasValue)
                    {
                        additionalClaims.Add(new System.Security.Claims.Claim("BranchId", user.BranchId.Value.ToString()));
                    }

                    // Sign in with additional claims
                    await _signInManager.SignInWithClaimsAsync(user, model.RememberMe, additionalClaims);
                    _logger.LogInformation("User {Email} logged in successfully with custom claims", model.Email);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _logger.LogWarning("User not found after successful login for {Email}", model.Email);
                    ModelState.AddModelError(string.Empty, "Kullanıcı bilgileri alınamadı.");
                    return View(model);
                }
            }

            _logger.LogWarning("Login failed for {Email}: {Message}", model.Email, result.Message);
            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user: {Email}", model.Email);
            ModelState.AddModelError(string.Empty, "Giriş sırasında bir hata oluştu.");
            return View(model);
        }
    }

    // Alternative login endpoint for troubleshooting
    [HttpPost]
    [Route("Auth/LoginPost")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginPost(LoginViewModel model, string? returnUrl = null)
    {
        _logger.LogInformation("Alternative POST LoginPost action called. Email: {Email}", model?.Email);
        return await Login(model, returnUrl);
    }

    #endregion

    #region Register (Admin Only)

    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var request = new RegisterRequestDto
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                IpAddress = GetClientIpAddress(),
                UserAgent = Request.Headers.UserAgent.ToString()
            };

            var result = await _authService.RegisterAsync(request);
            
            if (result.IsSuccess && result.Data != null)
            {
                // For cookie-based authentication, sign in the user with custom claims
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    // Create additional claims for PersonId, DepartmentId, etc.
                    var additionalClaims = new List<System.Security.Claims.Claim>();
                    
                    if (user.PersonId.HasValue)
                    {
                        additionalClaims.Add(new System.Security.Claims.Claim("PersonId", user.PersonId.Value.ToString()));
                    }
                    
                    if (user.DepartmentId.HasValue)
                    {
                        additionalClaims.Add(new System.Security.Claims.Claim("DepartmentId", user.DepartmentId.Value.ToString()));
                    }
                    
                    if (user.BranchId.HasValue)
                    {
                        additionalClaims.Add(new System.Security.Claims.Claim("BranchId", user.BranchId.Value.ToString()));
                    }

                    // Sign in with additional claims
                    await _signInManager.SignInWithClaimsAsync(user, false, additionalClaims);
                    _logger.LogInformation("User {Email} registered and logged in successfully with custom claims", model.Email);
                    
                    TempData["SuccessMessage"] = "Hesabınız başarıyla oluşturuldu!";
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for user: {Email}", model.Email);
            ModelState.AddModelError(string.Empty, "Kayıt sırasında bir hata oluştu.");
            return View(model);
        }
    }

    #endregion

    #region Logout

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var userId = _currentUserService.UserId;
            if (userId.HasValue)
            {
                await _authService.LogoutAsync(userId.Value, GetClientIpAddress());
            }

            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out");
            
            TempData["InfoMessage"] = "Başarıyla çıkış yaptınız.";
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            return RedirectToAction("Index", "Home");
        }
    }

    #endregion

    #region Access Denied

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    #endregion

    #region Profile Management

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        try
        {
            var userId = _currentUserService.UserId;
            if (!userId.HasValue)
            {
                return RedirectToAction("Login");
            }

            var result = await _authService.GetCurrentUserProfileAsync(userId.Value);
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["ErrorMessage"] = "Profil bilgileri alınamadı.";
                return RedirectToAction("Index", "Home");
            }

            var viewModel = _mapper.Map<UserProfileViewModel>(result.Data);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user profile");
            TempData["ErrorMessage"] = "Profil bilgileri alınırken hata oluştu.";
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfile(UserProfileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Profile", model);
        }

        try
        {
            var userId = _currentUserService.UserId;
            if (!userId.HasValue)
            {
                return RedirectToAction("Login");
            }

            var request = new UpdateProfileDto
            {
                UserId = userId.Value,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email
            };

            var result = await _authService.UpdateProfileAsync(request);
            
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Profil bilgileriniz başarıyla güncellendi.";
                return RedirectToAction("Profile");
            }

            ModelState.AddModelError(string.Empty, result.Message);
            return View("Profile", model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user profile");
            ModelState.AddModelError(string.Empty, "Profil güncellenirken hata oluştu.");
            return View("Profile", model);
        }
    }

    [HttpGet]
    [Authorize]
    public IActionResult ChangePassword()
    {
        return View(new ChangePasswordViewModel());
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var userId = _currentUserService.UserId;
            if (!userId.HasValue)
            {
                return RedirectToAction("Login");
            }

            var request = new ChangePasswordDto
            {
                UserId = userId.Value,
                CurrentPassword = model.CurrentPassword,
                NewPassword = model.NewPassword,
                ConfirmNewPassword = model.ConfirmNewPassword
            };

            var result = await _authService.ChangePasswordAsync(request);
            
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirildi.";
                return RedirectToAction("Profile");
            }

            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password");
            ModelState.AddModelError(string.Empty, "Şifre değiştirme sırasında hata oluştu.");
            return View(model);
        }
    }

    #endregion

    #region Password Reset

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View(new ForgotPasswordViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var request = new ForgotPasswordDto
            {
                Email = model.Email
            };

            var result = await _authService.ForgotPasswordAsync(request);
            
            // Always show success message for security reasons
            TempData["InfoMessage"] = "Şifre sıfırlama bağlantısı email adresinize gönderildi.";
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during forgot password");
            ModelState.AddModelError(string.Empty, "İşlem sırasında hata oluştu.");
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult ResetPassword(string token, string email)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
        {
            TempData["ErrorMessage"] = "Geçersiz şifre sıfırlama bağlantısı.";
            return RedirectToAction("Login");
        }

        var model = new ResetPasswordViewModel
        {
            Token = token,
            Email = email
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var request = new ResetPasswordDto
            {
                Email = model.Email,
                Token = model.Token,
                NewPassword = model.NewPassword,
                ConfirmNewPassword = model.ConfirmNewPassword
            };

            var result = await _authService.ResetPasswordAsync(request);
            
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Şifreniz başarıyla sıfırlandı. Yeni şifrenizle giriş yapabilirsiniz.";
                return RedirectToAction("Login");
            }

            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset");
            ModelState.AddModelError(string.Empty, "Şifre sıfırlama sırasında hata oluştu.");
            return View(model);
        }
    }

    #endregion

    #region Helper Methods

    private string GetClientIpAddress()
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1")
        {
            ipAddress = "127.0.0.1"; // localhost
        }
        return ipAddress;
    }

    #endregion
}
