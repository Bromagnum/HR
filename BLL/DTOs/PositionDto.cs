using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class PositionListDto
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

    public class PositionDetailDto
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
        public List<PositionPersonDto> AssignedPersons { get; set; } = new();
    }

    public class PositionCreateDto
    {
        [Required(ErrorMessage = "Pozisyon adı gereklidir")]
        [StringLength(100, ErrorMessage = "Pozisyon adı en fazla 100 karakter olabilir")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Departman seçimi gereklidir")]
        public int DepartmentId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Minimum maaş 0'dan büyük olmalıdır")]
        public decimal? MinSalary { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Maksimum maaş 0'dan büyük olmalıdır")]
        public decimal? MaxSalary { get; set; }

        [Range(0, 50, ErrorMessage = "Tecrübe süresi 0-50 yıl arasında olmalıdır")]
        public int? RequiredExperience { get; set; }

        [StringLength(1000, ErrorMessage = "Gereksinimler en fazla 1000 karakter olabilir")]
        public string? Requirements { get; set; }

        [StringLength(1000, ErrorMessage = "Sorumluluklar en fazla 1000 karakter olabilir")]
        public string? Responsibilities { get; set; }

        [StringLength(50, ErrorMessage = "İstihdam türü en fazla 50 karakter olabilir")]
        public string? EmploymentType { get; set; }

        [StringLength(50, ErrorMessage = "Seviye en fazla 50 karakter olabilir")]
        public string? Level { get; set; }

        public bool IsAvailable { get; set; } = true;
    }

    public class PositionUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pozisyon adı gereklidir")]
        [StringLength(100, ErrorMessage = "Pozisyon adı en fazla 100 karakter olabilir")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Departman seçimi gereklidir")]
        public int DepartmentId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Minimum maaş 0'dan büyük olmalıdır")]
        public decimal? MinSalary { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Maksimum maaş 0'dan büyük olmalıdır")]
        public decimal? MaxSalary { get; set; }

        [Range(0, 50, ErrorMessage = "Tecrübe süresi 0-50 yıl arasında olmalıdır")]
        public int? RequiredExperience { get; set; }

        [StringLength(1000, ErrorMessage = "Gereksinimler en fazla 1000 karakter olabilir")]
        public string? Requirements { get; set; }

        [StringLength(1000, ErrorMessage = "Sorumluluklar en fazla 1000 karakter olabilir")]
        public string? Responsibilities { get; set; }

        [StringLength(50, ErrorMessage = "İstihdam türü en fazla 50 karakter olabilir")]
        public string? EmploymentType { get; set; }

        [StringLength(50, ErrorMessage = "Seviye en fazla 50 karakter olabilir")]
        public string? Level { get; set; }

        public bool IsAvailable { get; set; }
    }

    public class PositionPersonDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string? EmployeeNumber { get; set; }
        public DateTime? HireDate { get; set; }
        public decimal? Salary { get; set; }
    }
}
