using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs;

/// <summary>
/// Bordro listesi için basit DTO
/// </summary>
public class PayrollListDto
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public string PersonFullName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public string PayrollPeriod { get; set; } = string.Empty;
    public decimal BasicSalary { get; set; }
    public decimal Allowances { get; set; }
    public decimal GrossSalary { get; set; }
    public decimal Deductions { get; set; }
    public decimal NetSalary { get; set; }
    public DateTime PreparedDate { get; set; }
    public string? PreparedByName { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// Bordro detayı için DTO
/// </summary>
public class PayrollDetailDto
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public string PersonFullName { get; set; } = string.Empty;
    public string PersonFirstName { get; set; } = string.Empty;
    public string PersonLastName { get; set; } = string.Empty;
    public string PersonTcKimlikNo { get; set; } = string.Empty;
    public string PersonEmployeeNumber { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string PositionName { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public string PayrollPeriod { get; set; } = string.Empty;
    public decimal BasicSalary { get; set; }
    public decimal Allowances { get; set; }
    public decimal Bonuses { get; set; }
    public decimal GrossSalary { get; set; }
    public decimal Deductions { get; set; }
    public decimal NetSalary { get; set; }
    public string? Description { get; set; }
    public int? PreparedById { get; set; }
    public string? PreparedByName { get; set; }
    public DateTime PreparedDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// Yeni bordro oluşturma için DTO
/// </summary>
public class PayrollCreateDto
{
    [Required(ErrorMessage = "Personel seçimi zorunludur")]
    [Display(Name = "Personel")]
    public int PersonId { get; set; }

    [Required(ErrorMessage = "Yıl belirtilmelidir")]
    [Range(2020, 2030, ErrorMessage = "Yıl 2020-2030 arasında olmalıdır")]
    [Display(Name = "Yıl")]
    public int Year { get; set; } = DateTime.Now.Year;

    [Required(ErrorMessage = "Ay belirtilmelidir")]
    [Range(1, 12, ErrorMessage = "Ay 1-12 arasında olmalıdır")]
    [Display(Name = "Ay")]
    public int Month { get; set; } = DateTime.Now.Month;

    [Required(ErrorMessage = "Temel maaş belirtilmelidir")]
    [Range(0, 999999, ErrorMessage = "Temel maaş 0-999999 arasında olmalıdır")]
    [Display(Name = "Temel Maaş")]
    public decimal BasicSalary { get; set; }

    [Range(0, 999999, ErrorMessage = "Ek ödemeler 0-999999 arasında olmalıdır")]
    [Display(Name = "Ek Ödemeler (Prim, Yardım vb.)")]
    public decimal Allowances { get; set; } = 0;

    [Range(0, 999999, ErrorMessage = "İkramiyeler 0-999999 arasında olmalıdır")]
    [Display(Name = "İkramiyeler")]
    public decimal Bonuses { get; set; } = 0;

    [Range(0, 999999, ErrorMessage = "Kesintiler 0-999999 arasında olmalıdır")]
    [Display(Name = "Kesintiler (Vergi, SGK vb.)")]
    public decimal Deductions { get; set; } = 0;

    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }

    [Display(Name = "Bordroyu Hazırlayan")]
    public int? PreparedById { get; set; }
}

/// <summary>
/// Bordro güncelleme için DTO
/// </summary>
public class PayrollUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "Personel seçimi zorunludur")]
    [Display(Name = "Personel")]
    public int PersonId { get; set; }

    [Required(ErrorMessage = "Yıl belirtilmelidir")]
    [Range(2020, 2030, ErrorMessage = "Yıl 2020-2030 arasında olmalıdır")]
    [Display(Name = "Yıl")]
    public int Year { get; set; }

    [Required(ErrorMessage = "Ay belirtilmelidir")]
    [Range(1, 12, ErrorMessage = "Ay 1-12 arasında olmalıdır")]
    [Display(Name = "Ay")]
    public int Month { get; set; }

    [Required(ErrorMessage = "Temel maaş belirtilmelidir")]
    [Range(0, 999999, ErrorMessage = "Temel maaş 0-999999 arasında olmalıdır")]
    [Display(Name = "Temel Maaş")]
    public decimal BasicSalary { get; set; }

    [Range(0, 999999, ErrorMessage = "Ek ödemeler 0-999999 arasında olmalıdır")]
    [Display(Name = "Ek Ödemeler (Prim, Yardım vb.)")]
    public decimal Allowances { get; set; }

    [Range(0, 999999, ErrorMessage = "İkramiyeler 0-999999 arasında olmalıdır")]
    [Display(Name = "İkramiyeler")]
    public decimal Bonuses { get; set; }

    [Range(0, 999999, ErrorMessage = "Kesintiler 0-999999 arasında olmalıdır")]
    [Display(Name = "Kesintiler (Vergi, SGK vb.)")]
    public decimal Deductions { get; set; }

    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }

    [Display(Name = "Bordroyu Hazırlayan")]
    public int? PreparedById { get; set; }
}

/// <summary>
/// Bordro filtreleme için DTO
/// </summary>
public class PayrollFilterDto
{
    [Display(Name = "Personel")]
    public int? PersonId { get; set; }

    [Display(Name = "Departman")]
    public int? DepartmentId { get; set; }

    [Display(Name = "Yıl")]
    public int? Year { get; set; }

    [Display(Name = "Ay")]
    public int? Month { get; set; }

    [Display(Name = "Minimum Net Maaş")]
    public decimal? MinNetSalary { get; set; }

    [Display(Name = "Maksimum Net Maaş")]
    public decimal? MaxNetSalary { get; set; }

    // Sayfalama
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

/// <summary>
/// Bordro özet raporu için DTO
/// </summary>
public class PayrollSummaryDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public string PayrollPeriod { get; set; } = string.Empty;
    public int TotalEmployees { get; set; }
    public decimal TotalBasicSalary { get; set; }
    public decimal TotalAllowances { get; set; }
    public decimal TotalGrossSalary { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal TotalNetSalary { get; set; }
    public decimal AverageNetSalary { get; set; }
    public decimal MinNetSalary { get; set; }
    public decimal MaxNetSalary { get; set; }
    public DateTime ReportDate { get; set; } = DateTime.Now;
}

/// <summary>
/// Personel yıllık bordro özeti için DTO
/// </summary>
public class PersonYearlyPayrollSummaryDto
{
    public int PersonId { get; set; }
    public string PersonFullName { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public int Year { get; set; }
    public int PayrollCount { get; set; }
    public decimal YearlyBasicSalary { get; set; }
    public decimal YearlyAllowances { get; set; }
    public decimal YearlyGrossSalary { get; set; }
    public decimal YearlyDeductions { get; set; }
    public decimal YearlyNetSalary { get; set; }
    public decimal AverageMonthlyNet { get; set; }
    public DateTime ReportDate { get; set; } = DateTime.Now;
}
