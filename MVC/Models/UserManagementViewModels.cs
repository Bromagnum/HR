using System.ComponentModel.DataAnnotations;

namespace MVC.Models;

public class UserManagementViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public List<string> Roles { get; set; } = new();
    public int? PersonId { get; set; }
    public int? DepartmentId { get; set; }
    
    public string FullName => $"{FirstName} {LastName}".Trim();
    public string RolesDisplay => string.Join(", ", Roles);
    public string StatusDisplay => IsActive ? "Aktif" : "Pasif";
    public string StatusClass => IsActive ? "text-success" : "text-danger";
}

public class CreateUserViewModel
{
    [Required(ErrorMessage = "Ad alanı zorunludur.")]
    [Display(Name = "Ad")]
    [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Soyad alanı zorunludur.")]
    [Display(Name = "Soyad")]
    [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email adresi zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
    [Display(Name = "Email Adresi")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şifre zorunludur.")]
    [StringLength(100, ErrorMessage = "Şifre en az {2} karakter olmalıdır.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Şifre")]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Şifre Tekrarı")]
    [Compare("Password", ErrorMessage = "Şifre ve şifre tekrarı uyuşmuyor.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Display(Name = "Personel")]
    public int? PersonId { get; set; }

    [Display(Name = "Departman")]
    public int? DepartmentId { get; set; }

    [Display(Name = "Şube")]
    public int? BranchId { get; set; }

    [Required(ErrorMessage = "Rol seçimi zorunludur.")]
    [Display(Name = "Rol")]
    public string Role { get; set; } = "Employee";

    [Display(Name = "Notlar")]
    [StringLength(500, ErrorMessage = "Notlar en fazla 500 karakter olabilir.")]
    public string? Notes { get; set; }
}

public class EditUserViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ad alanı zorunludur.")]
    [Display(Name = "Ad")]
    [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Soyad alanı zorunludur.")]
    [Display(Name = "Soyad")]
    [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email adresi zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
    [Display(Name = "Email Adresi")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Personel")]
    public int? PersonId { get; set; }

    [Display(Name = "Departman")]
    public int? DepartmentId { get; set; }

    [Display(Name = "Şube")]
    public int? BranchId { get; set; }

    [Required(ErrorMessage = "Rol seçimi zorunludur.")]
    [Display(Name = "Rol")]
    public string Role { get; set; } = "Employee";

    [Display(Name = "Aktif")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Notlar")]
    [StringLength(500, ErrorMessage = "Notlar en fazla 500 karakter olabilir.")]
    public string? Notes { get; set; }
}

public class UserResetPasswordViewModel
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "Yeni şifre zorunludur.")]
    [StringLength(100, ErrorMessage = "Şifre en az {2} karakter olmalıdır.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Yeni Şifre")]
    public string NewPassword { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Yeni Şifre Tekrarı")]
    [Compare("NewPassword", ErrorMessage = "Şifre ve şifre tekrarı uyuşmuyor.")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}
