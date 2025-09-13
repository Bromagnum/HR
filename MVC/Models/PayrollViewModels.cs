using System.ComponentModel.DataAnnotations;

namespace MVC.Models;

/// <summary>
/// Bordro listesi görünümü için ViewModel
/// </summary>
public class PayrollListViewModel
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    
    [Display(Name = "Personel")]
    public string PersonFullName { get; set; } = string.Empty;
    
    [Display(Name = "Personel No")]
    public string EmployeeNumber { get; set; } = string.Empty;
    
    [Display(Name = "Departman")]
    public string DepartmentName { get; set; } = string.Empty;
    
    [Display(Name = "Yıl")]
    public int Year { get; set; }
    
    [Display(Name = "Ay")]
    public int Month { get; set; }
    
    [Display(Name = "Dönem")]
    public string PayrollPeriod { get; set; } = string.Empty;
    
    [Display(Name = "Temel Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal BasicSalary { get; set; }
    
    [Display(Name = "Ek Ödemeler")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal Allowances { get; set; }
    
    [Display(Name = "Brüt Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal GrossSalary { get; set; }
    
    [Display(Name = "Kesintiler")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal Deductions { get; set; }
    
    [Display(Name = "Net Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal NetSalary { get; set; }
    
    [Display(Name = "Hazırlama Tarihi")]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
    public DateTime PreparedDate { get; set; }
    
    [Display(Name = "Hazırlayan")]
    public string? PreparedByName { get; set; }
    
    public bool IsActive { get; set; }
}

/// <summary>
/// Bordro detayı görünümü için ViewModel
/// </summary>
public class PayrollDetailViewModel
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    
    [Display(Name = "Personel")]
    public string PersonFullName { get; set; } = string.Empty;
    
    [Display(Name = "Ad")]
    public string PersonFirstName { get; set; } = string.Empty;
    
    [Display(Name = "Soyad")]
    public string PersonLastName { get; set; } = string.Empty;
    
    [Display(Name = "TC Kimlik No")]
    public string PersonTcKimlikNo { get; set; } = string.Empty;
    
    [Display(Name = "Personel No")]
    public string PersonEmployeeNumber { get; set; } = string.Empty;
    
    [Display(Name = "Departman")]
    public string DepartmentName { get; set; } = string.Empty;
    
    [Display(Name = "Pozisyon")]
    public string PositionName { get; set; } = string.Empty;
    
    [Display(Name = "Yıl")]
    public int Year { get; set; }
    
    [Display(Name = "Ay")]
    public int Month { get; set; }
    
    [Display(Name = "Ay Adı")]
    public string MonthName { get; set; } = string.Empty;
    
    [Display(Name = "Bordro Dönemi")]
    public string PayrollPeriod { get; set; } = string.Empty;
    
    [Display(Name = "Temel Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal BasicSalary { get; set; }
    
    [Display(Name = "Ek Ödemeler")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal Allowances { get; set; }
    
    [Display(Name = "Brüt Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal GrossSalary { get; set; }
    
    [Display(Name = "Kesintiler")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal Deductions { get; set; }
    
    [Display(Name = "Net Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal NetSalary { get; set; }
    
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }
    
    public int? PreparedById { get; set; }
    
    [Display(Name = "Hazırlayan")]
    public string? PreparedByName { get; set; }
    
    [Display(Name = "Hazırlama Tarihi")]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
    public DateTime PreparedDate { get; set; }
    
    [Display(Name = "Oluşturulma Tarihi")]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
    public DateTime CreatedAt { get; set; }
    
    [Display(Name = "Güncellenme Tarihi")]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
    public DateTime? UpdatedAt { get; set; }
    
    public bool IsActive { get; set; }
}

/// <summary>
/// Yeni bordro oluşturma formu için ViewModel
/// </summary>
public class PayrollCreateViewModel
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
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
    public decimal BasicSalary { get; set; }

    [Range(0, 999999, ErrorMessage = "Ek ödemeler 0-999999 arasında olmalıdır")]
    [Display(Name = "Ek Ödemeler (Prim, İkramiye vb.)")]
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
    public decimal Allowances { get; set; } = 0;

    [Range(0, 999999, ErrorMessage = "Kesintiler 0-999999 arasında olmalıdır")]
    [Display(Name = "Kesintiler (Vergi, SGK vb.)")]
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
    public decimal Deductions { get; set; } = 0;

    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    [Display(Name = "Açıklama")]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    [Display(Name = "Bordroyu Hazırlayan")]
    public int? PreparedById { get; set; }

    // Hesaplanan özellikler (sadece görüntüleme için)
    [Display(Name = "Brüt Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal GrossSalary => BasicSalary + Allowances;

    [Display(Name = "Net Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal NetSalary => GrossSalary - Deductions;
}

/// <summary>
/// Bordro güncelleme formu için ViewModel
/// </summary>
public class PayrollEditViewModel
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
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
    public decimal BasicSalary { get; set; }

    [Range(0, 999999, ErrorMessage = "Ek ödemeler 0-999999 arasında olmalıdır")]
    [Display(Name = "Ödenekler")]
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
    public decimal Allowances { get; set; }

    [Range(0, 999999, ErrorMessage = "Primler 0-999999 arasında olmalıdır")]
    [Display(Name = "Primler")]
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
    public decimal Bonuses { get; set; } = 0;

    [Range(0, 999999, ErrorMessage = "SGK kesintisi 0-999999 arasında olmalıdır")]
    [Display(Name = "SGK Kesintisi")]
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
    public decimal SocialSecurityDeduction { get; set; } = 0;

    [Range(0, 999999, ErrorMessage = "Gelir vergisi 0-999999 arasında olmalıdır")]
    [Display(Name = "Gelir Vergisi")]
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
    public decimal IncomeTaxDeduction { get; set; } = 0;

    [Range(0, 999999, ErrorMessage = "Diğer kesintiler 0-999999 arasında olmalıdır")]
    [Display(Name = "Diğer Kesintiler")]
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
    public decimal OtherDeductions { get; set; } = 0;

    [Display(Name = "Ödeme Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? PaymentDate { get; set; }

    [StringLength(500, ErrorMessage = "Notlar en fazla 500 karakter olabilir")]
    [Display(Name = "Notlar")]
    [DataType(DataType.MultilineText)]
    public string? Notes { get; set; }

    [Display(Name = "Bordroyu Hazırlayan")]
    public int? PreparedById { get; set; }

    // Mevcut personel bilgileri (salt okunur)
    [Display(Name = "Personel")]
    public string PersonFullName { get; set; } = string.Empty;

    [Display(Name = "Departman")]
    public string DepartmentName { get; set; } = string.Empty;

    // Hesaplanan özellikler (sadece görüntüleme için)
    [Display(Name = "Brüt Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal GrossSalary => BasicSalary + Allowances + Bonuses;

    [Display(Name = "Toplam Kesintiler")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal TotalDeductions => SocialSecurityDeduction + IncomeTaxDeduction + OtherDeductions;

    [Display(Name = "Net Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal NetSalary => GrossSalary - TotalDeductions;
}

/// <summary>
/// Bordro filtreleme formu için ViewModel
/// </summary>
public class PayrollFilterViewModel
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
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
    public decimal? MinNetSalary { get; set; }

    [Display(Name = "Maksimum Net Maaş")]
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
    public decimal? MaxNetSalary { get; set; }

    // Sayfalama
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

/// <summary>
/// Bordro özet raporu görünümü için ViewModel
/// </summary>
public class PayrollSummaryViewModel
{
    [Display(Name = "Yıl")]
    public int Year { get; set; }
    
    [Display(Name = "Ay")]
    public int Month { get; set; }
    
    [Display(Name = "Ay Adı")]
    public string MonthName { get; set; } = string.Empty;
    
    [Display(Name = "Bordro Dönemi")]
    public string PayrollPeriod { get; set; } = string.Empty;
    
    [Display(Name = "Toplam Personel")]
    public int TotalEmployees { get; set; }
    
    [Display(Name = "Toplam Temel Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal TotalBasicSalary { get; set; }
    
    [Display(Name = "Toplam Ek Ödemeler")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal TotalAllowances { get; set; }
    
    [Display(Name = "Toplam Brüt Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal TotalGrossSalary { get; set; }
    
    [Display(Name = "Toplam Kesintiler")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal TotalDeductions { get; set; }
    
    [Display(Name = "Toplam Net Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal TotalNetSalary { get; set; }
    
    [Display(Name = "Ortalama Net Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal AverageNetSalary { get; set; }
    
    [Display(Name = "Minimum Net Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal MinNetSalary { get; set; }
    
    [Display(Name = "Maksimum Net Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal MaxNetSalary { get; set; }
    
    [Display(Name = "Rapor Tarihi")]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
    public DateTime ReportDate { get; set; } = DateTime.Now;
    
    // Departman bazlı özet listesi
    public List<DepartmentPayrollSummaryViewModel>? DepartmentSummary { get; set; }
}

/// <summary>
/// Departman bazlı bordro özeti
/// </summary>
public class DepartmentPayrollSummaryViewModel
{
    [Display(Name = "Departman")]
    public string DepartmentName { get; set; } = string.Empty;
    
    [Display(Name = "Çalışan Sayısı")]
    public int EmployeeCount { get; set; }
    
    [Display(Name = "Toplam Brüt Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal TotalGrossSalary { get; set; }
    
    [Display(Name = "Toplam Kesintiler")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal TotalDeductions { get; set; }
    
    [Display(Name = "Toplam Net Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal TotalNetSalary { get; set; }
    
    [Display(Name = "Ortalama Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal AverageSalary { get; set; }
}

/// <summary>
/// Personel yıllık bordro özeti görünümü için ViewModel
/// </summary>
public class PersonYearlyPayrollSummaryViewModel
{
    public int PersonId { get; set; }
    
    [Display(Name = "Personel")]
    public string PersonFullName { get; set; } = string.Empty;
    
    [Display(Name = "Personel Adı")]
    public string PersonName { get; set; } = string.Empty;
    
    [Display(Name = "Personel No")]
    public string EmployeeNumber { get; set; } = string.Empty;
    
    [Display(Name = "Departman")]
    public string DepartmentName { get; set; } = string.Empty;
    
    [Display(Name = "Pozisyon")]
    public string PositionName { get; set; } = string.Empty;
    
    [Display(Name = "Yıl")]
    public int Year { get; set; }
    
    [Display(Name = "Bordro Sayısı")]
    public int PayrollCount { get; set; }
    
    [Display(Name = "Toplam Bordro")]
    public int TotalPayrolls { get; set; }
    
    [Display(Name = "Toplam Brüt Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal TotalGrossSalary { get; set; }
    
    [Display(Name = "Toplam Kesintiler")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal TotalDeductions { get; set; }
    
    [Display(Name = "Toplam Net Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal TotalNetSalary { get; set; }
    
    [Display(Name = "Ortalama Net Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal AverageNetSalary { get; set; }
    
    // Aylık detay listesi
    public List<MonthlyPayrollSummaryViewModel>? MonthlyBreakdown { get; set; }
}

/// <summary>
/// Aylık bordro özeti
/// </summary>
public class MonthlyPayrollSummaryViewModel
{
    public int PayrollId { get; set; }
    
    [Display(Name = "Ay")]
    public int Month { get; set; }
    
    [Display(Name = "Temel Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal BasicSalary { get; set; }
    
    [Display(Name = "Brüt Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal GrossSalary { get; set; }
    
    [Display(Name = "Kesintiler")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal Deductions { get; set; }
    
    [Display(Name = "Net Maaş")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public decimal NetSalary { get; set; }
    
    [Display(Name = "Ödeme Tarihi")]
    public DateTime? PaymentDate { get; set; }
    
    [Display(Name = "Rapor Tarihi")]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
    public DateTime ReportDate { get; set; } = DateTime.Now;
}
