using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Models
{
    public class PositionListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public string? Level { get; set; }
        public string? EmploymentType { get; set; }
        public bool IsAvailable { get; set; }
        public int PersonCount { get; set; }
        public bool IsActive { get; set; }
    }

    public class PositionDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public int? RequiredExperience { get; set; }
        public string? Requirements { get; set; }
        public string? Responsibilities { get; set; }
        public string? EmploymentType { get; set; }
        public string? Level { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<PositionPersonViewModel> AssignedPersons { get; set; } = new();
    }

    public class PositionCreateViewModel
    {
        [Required(ErrorMessage = "Pozisyon adı gereklidir")]
        [Display(Name = "Pozisyon Adı")]
        [StringLength(100, ErrorMessage = "Pozisyon adı en fazla 100 karakter olabilir")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Açıklama")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Departman seçimi gereklidir")]
        [Display(Name = "Departman")]
        public int DepartmentId { get; set; }

        [Display(Name = "Minimum Maaş")]
        [Range(0, double.MaxValue, ErrorMessage = "Minimum maaş 0'dan büyük olmalıdır")]
        public decimal? MinSalary { get; set; }

        [Display(Name = "Maksimum Maaş")]
        [Range(0, double.MaxValue, ErrorMessage = "Maksimum maaş 0'dan büyük olmalıdır")]
        public decimal? MaxSalary { get; set; }

        [Display(Name = "Gerekli Tecrübe (Yıl)")]
        [Range(0, 50, ErrorMessage = "Tecrübe süresi 0-50 yıl arasında olmalıdır")]
        public int? RequiredExperience { get; set; }

        [Display(Name = "Gereksinimler")]
        [StringLength(1000, ErrorMessage = "Gereksinimler en fazla 1000 karakter olabilir")]
        public string? Requirements { get; set; }

        [Display(Name = "Sorumluluklar")]
        [StringLength(1000, ErrorMessage = "Sorumluluklar en fazla 1000 karakter olabilir")]
        public string? Responsibilities { get; set; }

        [Display(Name = "İstihdam Türü")]
        [StringLength(50, ErrorMessage = "İstihdam türü en fazla 50 karakter olabilir")]
        public string? EmploymentType { get; set; }

        [Display(Name = "Seviye")]
        [StringLength(50, ErrorMessage = "Seviye en fazla 50 karakter olabilir")]
        public string? Level { get; set; }

        [Display(Name = "Müsait Pozisyon")]
        public bool IsAvailable { get; set; } = true;

        // For dropdown
        public List<SelectListItem> Departments { get; set; } = new();
        public List<SelectListItem> EmploymentTypes { get; set; } = new();
        public List<SelectListItem> Levels { get; set; } = new();
    }

    public class PositionUpdateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pozisyon adı gereklidir")]
        [Display(Name = "Pozisyon Adı")]
        [StringLength(100, ErrorMessage = "Pozisyon adı en fazla 100 karakter olabilir")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Açıklama")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Departman seçimi gereklidir")]
        [Display(Name = "Departman")]
        public int DepartmentId { get; set; }

        [Display(Name = "Minimum Maaş")]
        [Range(0, double.MaxValue, ErrorMessage = "Minimum maaş 0'dan büyük olmalıdır")]
        public decimal? MinSalary { get; set; }

        [Display(Name = "Maksimum Maaş")]
        [Range(0, double.MaxValue, ErrorMessage = "Maksimum maaş 0'dan büyük olmalıdır")]
        public decimal? MaxSalary { get; set; }

        [Display(Name = "Gerekli Tecrübe (Yıl)")]
        [Range(0, 50, ErrorMessage = "Tecrübe süresi 0-50 yıl arasında olmalıdır")]
        public int? RequiredExperience { get; set; }

        [Display(Name = "Gereksinimler")]
        [StringLength(1000, ErrorMessage = "Gereksinimler en fazla 1000 karakter olabilir")]
        public string? Requirements { get; set; }

        [Display(Name = "Sorumluluklar")]
        [StringLength(1000, ErrorMessage = "Sorumluluklar en fazla 1000 karakter olabilir")]
        public string? Responsibilities { get; set; }

        [Display(Name = "İstihdam Türü")]
        [StringLength(50, ErrorMessage = "İstihdam türü en fazla 50 karakter olabilir")]
        public string? EmploymentType { get; set; }

        [Display(Name = "Seviye")]
        [StringLength(50, ErrorMessage = "Seviye en fazla 50 karakter olabilir")]
        public string? Level { get; set; }

        [Display(Name = "Müsait Pozisyon")]
        public bool IsAvailable { get; set; }

        // For dropdown
        public List<SelectListItem> Departments { get; set; } = new();
        public List<SelectListItem> EmploymentTypes { get; set; } = new();
        public List<SelectListItem> Levels { get; set; } = new();
    }

    public class PositionPersonViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? EmployeeNumber { get; set; }
        public DateTime? HireDate { get; set; }
        public decimal? Salary { get; set; }
    }

    public class PositionFilterViewModel
    {
        public string? SearchTerm { get; set; }
        public int? DepartmentId { get; set; }
        public string? Level { get; set; }
        public string? EmploymentType { get; set; }
        public bool? IsAvailable { get; set; }
        public bool? IsActive { get; set; }

        // For dropdowns
        public List<SelectListItem> Departments { get; set; } = new();
        public List<SelectListItem> Levels { get; set; } = new();
        public List<SelectListItem> EmploymentTypes { get; set; } = new();

        // Results
        public List<PositionListViewModel> Positions { get; set; } = new();
    }
}
