using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using BLL.Services;
using BLL.DTOs;
using DAL.Entities;
using MVC.Models;
using AutoMapper;

namespace MVC.Controllers;

[Authorize(Roles = "Admin,Manager")]
public class UserManagementController : Controller
{
    private readonly IAuthService _authService;
    private readonly IPersonService _personService;
    private readonly IDepartmentService _departmentService;
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly ILogger<UserManagementController> _logger;

    public UserManagementController(
        IAuthService authService,
        IPersonService personService,
        IDepartmentService departmentService,
        ICurrentUserService currentUserService,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IMapper mapper,
        ILogger<UserManagementController> logger)
    {
        _authService = authService;
        _personService = personService;
        _departmentService = departmentService;
        _currentUserService = currentUserService;
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _logger = logger;
    }

    #region User List & Management

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            // Apply department filtering for Managers
            IQueryable<ApplicationUser> usersQuery = _userManager.Users;
            
            if (_currentUserService.IsInRole("Manager"))
            {
                var managerDepartmentId = _currentUserService.DepartmentId;
                _logger.LogInformation("Manager Department Filtering - DepartmentId: {DepartmentId}", managerDepartmentId);
                
                if (!managerDepartmentId.HasValue)
                {
                    TempData["ErrorMessage"] = "Departman bilginiz bulunamadı. Lütfen yöneticinize başvurun.";
                    return View(new List<UserManagementViewModel>());
                }
                
                // Filter users by department
                usersQuery = usersQuery.Where(u => u.DepartmentId == managerDepartmentId.Value);
                _logger.LogInformation("Filtered users query created for department: {DepartmentId}", managerDepartmentId.Value);
            }

            var users = usersQuery.ToList();
            var userViewModels = new List<UserManagementViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userViewModel = new UserManagementViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName ?? "",
                    LastName = user.LastName ?? "",
                    Email = user.Email ?? "",
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt,
                    LastLoginAt = user.LastLoginAt,
                    Roles = roles.ToList(),
                    PersonId = user.PersonId,
                    DepartmentId = user.DepartmentId
                };
                userViewModels.Add(userViewModel);
            }

            return View(userViewModels);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading users");
            TempData["ErrorMessage"] = "Kullanıcı listesi yüklenirken hata oluştu.";
            return View(new List<UserManagementViewModel>());
        }
    }

    #endregion

    #region Create User

    [HttpGet]
    public async Task<IActionResult> CreateUser()
    {
        try
        {
            await LoadDropdownDataAsync();
            return View(new CreateUserViewModel());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading create user page");
            TempData["ErrorMessage"] = "Sayfa yüklenirken hata oluştu.";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(CreateUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdownDataAsync();
            return View(model);
        }

        try
        {
            // Check if email already exists
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Bu email adresi zaten kullanımda.");
                await LoadDropdownDataAsync();
                return View(model);
            }

            // Create the user
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PersonId = model.PersonId,
                DepartmentId = model.DepartmentId,
                BranchId = model.BranchId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (result.Succeeded)
            {
                // Assign role (default to Employee if not specified)
                var roleToAssign = string.IsNullOrEmpty(model.Role) ? "Employee" : model.Role;
                await _userManager.AddToRoleAsync(user, roleToAssign);

                _logger.LogInformation("User {Email} created successfully by {CurrentUser}", 
                    model.Email, User.Identity?.Name);
                
                TempData["SuccessMessage"] = $"Kullanıcı {model.FirstName} {model.LastName} başarıyla oluşturuldu!";
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user: {Email}", model.Email);
            ModelState.AddModelError(string.Empty, "Kullanıcı oluşturulurken hata oluştu.");
        }

        await LoadDropdownDataAsync();
        return View(model);
    }

    #endregion

    #region Edit User

    [HttpGet]
    public async Task<IActionResult> EditUser(int id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Index");
            }

            var roles = await _userManager.GetRolesAsync(user);
            
            var model = new EditUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName ?? "",
                LastName = user.LastName ?? "",
                Email = user.Email ?? "",
                PersonId = user.PersonId,
                DepartmentId = user.DepartmentId,
                BranchId = user.BranchId,
                IsActive = user.IsActive,
                Role = roles.FirstOrDefault() ?? "Employee"
            };

            await LoadDropdownDataAsync();
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading user for edit: {UserId}", id);
            TempData["ErrorMessage"] = "Kullanıcı bilgileri yüklenirken hata oluştu.";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(EditUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdownDataAsync();
            return View(model);
        }

        try
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Index");
            }

            // Update user properties
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.PersonId = model.PersonId;
            user.DepartmentId = model.DepartmentId;
            user.BranchId = model.BranchId;
            user.IsActive = model.IsActive;

            var result = await _userManager.UpdateAsync(user);
            
            if (result.Succeeded)
            {
                // Update role if changed
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (!currentRoles.Contains(model.Role))
                {
                    if (currentRoles.Any())
                    {
                        await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    }
                    await _userManager.AddToRoleAsync(user, model.Role);
                }

                _logger.LogInformation("User {Email} updated successfully by {CurrentUser}", 
                    model.Email, User.Identity?.Name);
                
                TempData["SuccessMessage"] = $"Kullanıcı {model.FirstName} {model.LastName} başarıyla güncellendi!";
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user: {UserId}", model.Id);
            ModelState.AddModelError(string.Empty, "Kullanıcı güncellenirken hata oluştu.");
        }

        await LoadDropdownDataAsync();
        return View(model);
    }

    #endregion

    #region Toggle User Status

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleUserStatus(int id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return Json(new { success = false, message = "Kullanıcı bulunamadı." });
            }

            user.IsActive = !user.IsActive;
            var result = await _userManager.UpdateAsync(user);
            
            if (result.Succeeded)
            {
                var status = user.IsActive ? "aktif" : "pasif";
                _logger.LogInformation("User {Email} status changed to {Status} by {CurrentUser}", 
                    user.Email, status, User.Identity?.Name);
                
                return Json(new { 
                    success = true, 
                    message = $"Kullanıcı durumu {status} olarak güncellendi.",
                    isActive = user.IsActive 
                });
            }
            else
            {
                return Json(new { success = false, message = "Kullanıcı durumu güncellenirken hata oluştu." });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error toggling user status: {UserId}", id);
            return Json(new { success = false, message = "İşlem sırasında hata oluştu." });
        }
    }

    #endregion

    #region Reset Password

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetUserPassword(int id, string newPassword)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
            {
                return Json(new { success = false, message = "Şifre en az 6 karakter olmalıdır." });
            }

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return Json(new { success = false, message = "Kullanıcı bulunamadı." });
            }

            // Remove old password and set new one
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            
            if (result.Succeeded)
            {
                _logger.LogInformation("Password reset for user {Email} by {CurrentUser}", 
                    user.Email, User.Identity?.Name);
                
                return Json(new { 
                    success = true, 
                    message = $"Kullanıcı {user.FirstName} {user.LastName} için şifre başarıyla sıfırlandı." 
                });
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Json(new { success = false, message = $"Şifre sıfırlama hatası: {errors}" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting password for user: {UserId}", id);
            return Json(new { success = false, message = "Şifre sıfırlama sırasında hata oluştu." });
        }
    }

    #endregion

    #region Helper Methods

    private async Task LoadDropdownDataAsync()
    {
        try
        {
            // Load Persons
            var personsResult = await _personService.GetAllAsync();
            ViewBag.Persons = personsResult.IsSuccess && personsResult.Data != null
                ? personsResult.Data.Select(p => new SelectListItem 
                { 
                    Value = p.Id.ToString(), 
                    Text = $"{p.FirstName} {p.LastName} ({p.TcKimlikNo})" 
                }).ToList()
                : new List<SelectListItem>();

            // Load Departments
            var departmentsResult = await _departmentService.GetAllAsync();
            ViewBag.Departments = departmentsResult.IsSuccess && departmentsResult.Data != null
                ? departmentsResult.Data.Select(d => new SelectListItem 
                { 
                    Value = d.Id.ToString(), 
                    Text = d.Name 
                }).ToList()
                : new List<SelectListItem>();

            // Load Roles
            var roles = _roleManager.Roles.Where(r => r.IsActive).ToList();
            ViewBag.Roles = roles.Select(r => new SelectListItem 
            { 
                Value = r.Name, 
                Text = r.Name 
            }).ToList();

            // Branch data (for future use)
            ViewBag.Branches = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Ana Şube" },
                new SelectListItem { Value = "2", Text = "İkinci Şube" }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading dropdown data");
            ViewBag.Persons = new List<SelectListItem>();
            ViewBag.Departments = new List<SelectListItem>();
            ViewBag.Roles = new List<SelectListItem>();
            ViewBag.Branches = new List<SelectListItem>();
        }
    }

    #endregion
}
