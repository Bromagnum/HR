using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.Entities;

namespace MVC.Models;

#region List & Filter ViewModels

public class JobPostingListViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int PositionId { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string? Location { get; set; }
    public EmploymentType EmploymentType { get; set; }
    public string EmploymentTypeName { get; set; } = string.Empty;
    public string EmploymentTypeText { get; set; } = string.Empty;
    public JobPostingStatus Status { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public string StatusText { get; set; } = string.Empty;
    public string StatusClass { get; set; } = string.Empty;
    public DateTime PublishDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public DateTime? LastApplicationDate { get; set; }
    public bool IsRemoteWork { get; set; }
    public int ViewCount { get; set; }
    public int ApplicationCount { get; set; }
    public int OpenPositions { get; set; }
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public string SalaryRange { get; set; } = string.Empty;
    public bool IsExpired { get; set; }
    public bool IsApplicationDeadlinePassed { get; set; }
    public int DaysUntilExpiry { get; set; }
    public string? CreatedByName { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public bool IsUrgent { get; set; }
    public bool IsFeatured { get; set; }
}

public class JobPostingFilterViewModel
{
    [Display(Name = "Arama")]
    public string? SearchTerm { get; set; }

    [Display(Name = "Durum")]
    public JobPostingStatus? Status { get; set; }

    [Display(Name = "Departman")]
    public int? DepartmentId { get; set; }

    [Display(Name = "Pozisyon")]
    public int? PositionId { get; set; }

    [Display(Name = "Çalışma Şekli")]
    public EmploymentType? EmploymentType { get; set; }

    [Display(Name = "Lokasyon")]
    public string? Location { get; set; }

    [Display(Name = "Uzaktan Çalışma")]
    public bool? IsRemoteWork { get; set; }

    [Display(Name = "Min. Maaş")]
    public decimal? MinSalary { get; set; }

    [Display(Name = "Max. Maaş")]
    public decimal? MaxSalary { get; set; }

    [Display(Name = "Yayın Başlangıç")]
    [DataType(DataType.Date)]
    public DateTime? PublishDateStart { get; set; }

    [Display(Name = "Yayın Bitiş")]
    [DataType(DataType.Date)]
    public DateTime? PublishDateEnd { get; set; }

    // For dropdown lists
    public List<SelectListItem> Departments { get; set; } = new();
    public List<SelectListItem> Positions { get; set; } = new();
    public List<SelectListItem> StatusOptions { get; set; } = new();
    public List<SelectListItem> EmploymentTypes { get; set; } = new();
    public List<SelectListItem> Locations { get; set; } = new();
}

#endregion

#region Detail ViewModels

public class JobPostingDetailViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int PositionId { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public EmploymentType EmploymentType { get; set; }
    public string EmploymentTypeName { get; set; } = string.Empty;
    public string? Requirements { get; set; }
    public string? Responsibilities { get; set; }
    public string? Benefits { get; set; }
    public int? MinExperience { get; set; }
    public int? MaxExperience { get; set; }
    public EducationLevel? MinEducation { get; set; }
    public string? MinEducationName { get; set; }
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public string SalaryRange { get; set; } = string.Empty;
    public string? Location { get; set; }
    public bool IsRemoteWork { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public DateTime? LastApplicationDate { get; set; }
    public JobPostingStatus Status { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public string StatusClass { get; set; } = string.Empty;
    public int ViewCount { get; set; }
    public int ApplicationCount { get; set; }
    public int OpenPositions { get; set; }
    public string? ContactInfo { get; set; }
    public string? Tags { get; set; }
    public List<string> TagList { get; set; } = new();
    public bool IsExpired { get; set; }
    public bool IsApplicationDeadlinePassed { get; set; }
    public int DaysUntilExpiry { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
    
    // Related applications
    public List<JobApplicationListViewModel> Applications { get; set; } = new();
    public int PendingApplicationsCount { get; set; }
    public int ApprovedApplicationsCount { get; set; }
    public int RejectedApplicationsCount { get; set; }
}

#endregion

#region Create & Edit ViewModels

public class JobPostingCreateViewModel
{
    [Required(ErrorMessage = "İlan başlığı zorunludur")]
    [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir")]
    [Display(Name = "İlan Başlığı")]
    public string Title { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "URL en fazla 200 karakter olabilir")]
    [Display(Name = "URL (Slug)")]
    [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "URL sadece küçük harf, rakam ve tire içerebilir")]
    public string? Slug { get; set; }

    [Required(ErrorMessage = "İş tanımı zorunludur")]
    [StringLength(4000, ErrorMessage = "İş tanımı en fazla 4000 karakter olabilir")]
    [Display(Name = "İş Tanımı")]
    [DataType(DataType.MultilineText)]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Pozisyon seçimi zorunludur")]
    [Display(Name = "Pozisyon")]
    public int PositionId { get; set; }

    [Required(ErrorMessage = "Departman seçimi zorunludur")]
    [Display(Name = "Departman")]
    public int DepartmentId { get; set; }

    [Required(ErrorMessage = "Çalışma şekli seçimi zorunludur")]
    [Display(Name = "Çalışma Şekli")]
    public EmploymentType EmploymentType { get; set; }

    [StringLength(2000, ErrorMessage = "Gereksinimler en fazla 2000 karakter olabilir")]
    [Display(Name = "Aranan Nitelikler")]
    [DataType(DataType.MultilineText)]
    public string? Requirements { get; set; }

    [StringLength(2000, ErrorMessage = "Sorumluluklar en fazla 2000 karakter olabilir")]
    [Display(Name = "İş Sorumlulukları")]
    [DataType(DataType.MultilineText)]
    public string? Responsibilities { get; set; }

    [StringLength(1000, ErrorMessage = "Yan haklar en fazla 1000 karakter olabilir")]
    [Display(Name = "Yan Haklar")]
    [DataType(DataType.MultilineText)]
    public string? Benefits { get; set; }

    [Display(Name = "Min. Deneyim (Yıl)")]
    [Range(0, 50, ErrorMessage = "Deneyim 0-50 yıl arasında olmalıdır")]
    public int? MinExperience { get; set; }

    [Display(Name = "Max. Deneyim (Yıl)")]
    [Range(0, 50, ErrorMessage = "Deneyim 0-50 yıl arasında olmalıdır")]
    public int? MaxExperience { get; set; }

    [Display(Name = "Min. Eğitim Seviyesi")]
    public EducationLevel? MinEducation { get; set; }

    [Display(Name = "Min. Maaş")]
    [Range(0, 999999, ErrorMessage = "Geçerli bir maaş miktarı giriniz")]
    public decimal? MinSalary { get; set; }

    [Display(Name = "Max. Maaş")]
    [Range(0, 999999, ErrorMessage = "Geçerli bir maaş miktarı giriniz")]
    public decimal? MaxSalary { get; set; }

    [StringLength(200, ErrorMessage = "Lokasyon en fazla 200 karakter olabilir")]
    [Display(Name = "Çalışma Yeri")]
    public string? Location { get; set; }

    [Display(Name = "Uzaktan Çalışma İmkanı")]
    public bool IsRemoteWork { get; set; }

    [Display(Name = "Son Başvuru Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? LastApplicationDate { get; set; }

    [Display(Name = "İlan Bitiş Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? ExpiryDate { get; set; }

    [Required(ErrorMessage = "Açık pozisyon sayısı zorunludur")]
    [Display(Name = "Açık Pozisyon Sayısı")]
    [Range(1, 100, ErrorMessage = "1-100 arasında bir değer giriniz")]
    public int OpenPositions { get; set; } = 1;

    [StringLength(500, ErrorMessage = "İletişim bilgisi en fazla 500 karakter olabilir")]
    [Display(Name = "İletişim Bilgileri")]
    [DataType(DataType.MultilineText)]
    public string? ContactInfo { get; set; }

    [StringLength(200, ErrorMessage = "Etiketler en fazla 200 karakter olabilir")]
    [Display(Name = "Etiketler (virgülle ayırın)")]
    public string? Tags { get; set; }

    [Display(Name = "Durumu")]
    public JobPostingStatus Status { get; set; } = JobPostingStatus.Draft;

    // For dropdown lists
    public List<SelectListItem> Positions { get; set; } = new();
    public List<SelectListItem> Departments { get; set; } = new();
    public List<SelectListItem> EmploymentTypes { get; set; } = new();
    public List<SelectListItem> EducationLevels { get; set; } = new();
    public List<SelectListItem> StatusOptions { get; set; } = new();

    // Validation
    public bool IsValid => ValidateModel();

    private bool ValidateModel()
    {
        if (MinExperience.HasValue && MaxExperience.HasValue && MinExperience > MaxExperience)
            return false;

        if (MinSalary.HasValue && MaxSalary.HasValue && MinSalary > MaxSalary)
            return false;

        if (LastApplicationDate.HasValue && LastApplicationDate.Value <= DateTime.Now)
            return false;

        if (ExpiryDate.HasValue && ExpiryDate.Value <= DateTime.Now)
            return false;

        return true;
    }
}

public class JobPostingEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "İlan başlığı zorunludur")]
    [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir")]
    [Display(Name = "İlan Başlığı")]
    public string Title { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "URL en fazla 200 karakter olabilir")]
    [Display(Name = "URL (Slug)")]
    [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "URL sadece küçük harf, rakam ve tire içerebilir")]
    public string? Slug { get; set; }

    [Required(ErrorMessage = "İş tanımı zorunludur")]
    [StringLength(4000, ErrorMessage = "İş tanımı en fazla 4000 karakter olabilir")]
    [Display(Name = "İş Tanımı")]
    [DataType(DataType.MultilineText)]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Pozisyon seçimi zorunludur")]
    [Display(Name = "Pozisyon")]
    public int PositionId { get; set; }

    [Required(ErrorMessage = "Departman seçimi zorunludur")]
    [Display(Name = "Departman")]
    public int DepartmentId { get; set; }

    [Required(ErrorMessage = "Çalışma şekli seçimi zorunludur")]
    [Display(Name = "Çalışma Şekli")]
    public EmploymentType EmploymentType { get; set; }

    [StringLength(2000, ErrorMessage = "Gereksinimler en fazla 2000 karakter olabilir")]
    [Display(Name = "Aranan Nitelikler")]
    [DataType(DataType.MultilineText)]
    public string? Requirements { get; set; }

    [StringLength(2000, ErrorMessage = "Sorumluluklar en fazla 2000 karakter olabilir")]
    [Display(Name = "İş Sorumlulukları")]
    [DataType(DataType.MultilineText)]
    public string? Responsibilities { get; set; }

    [StringLength(1000, ErrorMessage = "Yan haklar en fazla 1000 karakter olabilir")]
    [Display(Name = "Yan Haklar")]
    [DataType(DataType.MultilineText)]
    public string? Benefits { get; set; }

    [Display(Name = "Min. Deneyim (Yıl)")]
    [Range(0, 50, ErrorMessage = "Deneyim 0-50 yıl arasında olmalıdır")]
    public int? MinExperience { get; set; }

    [Display(Name = "Max. Deneyim (Yıl)")]
    [Range(0, 50, ErrorMessage = "Deneyim 0-50 yıl arasında olmalıdır")]
    public int? MaxExperience { get; set; }

    [Display(Name = "Min. Eğitim Seviyesi")]
    public EducationLevel? MinEducation { get; set; }

    [Display(Name = "Min. Maaş")]
    [Range(0, 999999, ErrorMessage = "Geçerli bir maaş miktarı giriniz")]
    public decimal? MinSalary { get; set; }

    [Display(Name = "Max. Maaş")]
    [Range(0, 999999, ErrorMessage = "Geçerli bir maaş miktarı giriniz")]
    public decimal? MaxSalary { get; set; }

    [StringLength(200, ErrorMessage = "Lokasyon en fazla 200 karakter olabilir")]
    [Display(Name = "Çalışma Yeri")]
    public string? Location { get; set; }

    [Display(Name = "Uzaktan Çalışma İmkanı")]
    public bool IsRemoteWork { get; set; }

    [Display(Name = "Son Başvuru Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? LastApplicationDate { get; set; }

    [Display(Name = "İlan Bitiş Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? ExpiryDate { get; set; }

    [Required(ErrorMessage = "Açık pozisyon sayısı zorunludur")]
    [Display(Name = "Açık Pozisyon Sayısı")]
    [Range(1, 100, ErrorMessage = "1-100 arasında bir değer giriniz")]
    public int OpenPositions { get; set; } = 1;

    [StringLength(500, ErrorMessage = "İletişim bilgisi en fazla 500 karakter olabilir")]
    [Display(Name = "İletişim Bilgileri")]
    [DataType(DataType.MultilineText)]
    public string? ContactInfo { get; set; }

    [StringLength(200, ErrorMessage = "Etiketler en fazla 200 karakter olabilir")]
    [Display(Name = "Etiketler (virgülle ayırın)")]
    public string? Tags { get; set; }

    [Display(Name = "Durumu")]
    public JobPostingStatus Status { get; set; }

    [Display(Name = "Güncelleme Nedeni")]
    [StringLength(500, ErrorMessage = "Güncelleme nedeni en fazla 500 karakter olabilir")]
    public string? UpdateReason { get; set; }

    // Read-only properties
    public DateTime PublishDate { get; set; }
    public int ViewCount { get; set; }
    public int ApplicationCount { get; set; }
    public string? CreatedByName { get; set; }
    public DateTime CreatedAt { get; set; }

    // For dropdown lists
    public List<SelectListItem> Positions { get; set; } = new();
    public List<SelectListItem> Departments { get; set; } = new();
    public List<SelectListItem> EmploymentTypes { get; set; } = new();
    public List<SelectListItem> EducationLevels { get; set; } = new();
    public List<SelectListItem> StatusOptions { get; set; } = new();

    // Validation
    public bool IsValid => ValidateModel();

    private bool ValidateModel()
    {
        if (MinExperience.HasValue && MaxExperience.HasValue && MinExperience > MaxExperience)
            return false;

        if (MinSalary.HasValue && MaxSalary.HasValue && MinSalary > MaxSalary)
            return false;

        return true;
    }
}

#endregion

#region Public ViewModels

public class PublicJobPostingListViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string PositionName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string? Location { get; set; }
    public EmploymentType EmploymentType { get; set; }
    public string EmploymentTypeName { get; set; } = string.Empty;
    public bool IsRemoteWork { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime? LastApplicationDate { get; set; }
    public string SalaryRange { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public List<string> TagList { get; set; } = new();
    public bool IsNew { get; set; }
    public bool IsUrgent { get; set; }
    public int DaysRemaining { get; set; }
}

public class PublicJobPostingDetailViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PositionName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public EmploymentType EmploymentType { get; set; }
    public string EmploymentTypeName { get; set; } = string.Empty;
    public string? Requirements { get; set; }
    public string? Responsibilities { get; set; }
    public string? Benefits { get; set; }
    public string ExperienceRange { get; set; } = string.Empty;
    public string? MinEducationName { get; set; }
    public string SalaryRange { get; set; } = string.Empty;
    public string? Location { get; set; }
    public bool IsRemoteWork { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime? LastApplicationDate { get; set; }
    public int OpenPositions { get; set; }
    public string? ContactInfo { get; set; }
    public List<string> TagList { get; set; } = new();
    public bool IsNew { get; set; }
    public bool IsUrgent { get; set; }
    public int DaysRemaining { get; set; }
    public int ViewCount { get; set; }
    
    // Company info
    public string CompanyName { get; set; } = "Şirket Adı";
    public string CompanyDescription { get; set; } = string.Empty;
    
    // Related postings
    public List<PublicJobPostingListViewModel> RelatedPostings { get; set; } = new();
    
    // Application info
    public bool CanApply { get; set; } = true;
    public string? CannotApplyReason { get; set; }
}

public class PublicJobPostingFilterViewModel
{
    [Display(Name = "Arama")]
    public string? SearchTerm { get; set; }

    [Display(Name = "Departman")]
    public int? DepartmentId { get; set; }

    [Display(Name = "Pozisyon")]
    public int? PositionId { get; set; }

    [Display(Name = "Çalışma Şekli")]
    public EmploymentType? EmploymentType { get; set; }

    [Display(Name = "Lokasyon")]
    public string? Location { get; set; }

    [Display(Name = "Uzaktan Çalışma")]
    public bool? IsRemoteWork { get; set; }

    [Display(Name = "Min. Maaş")]
    public decimal? MinSalary { get; set; }

    [Display(Name = "Max. Maaş")]
    public decimal? MaxSalary { get; set; }

    // For dropdown lists
    public List<SelectListItem> Departments { get; set; } = new();
    public List<SelectListItem> Positions { get; set; } = new();
    public List<SelectListItem> EmploymentTypes { get; set; } = new();
    public List<SelectListItem> Locations { get; set; } = new();
}

#endregion

#region Status Management ViewModels

public class JobPostingStatusUpdateViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public JobPostingStatus CurrentStatus { get; set; }

    [Required(ErrorMessage = "Yeni durum seçimi zorunludur")]
    [Display(Name = "Yeni Durum")]
    public JobPostingStatus NewStatus { get; set; }

    [StringLength(500, ErrorMessage = "Notlar en fazla 500 karakter olabilir")]
    [Display(Name = "Güncelleme Nedeni")]
    [DataType(DataType.MultilineText)]
    public string? Notes { get; set; }

    // For dropdown lists
    public List<SelectListItem> StatusOptions { get; set; } = new();
}

public class BulkJobPostingUpdateViewModel
{
    [Required(ErrorMessage = "En az bir ilan seçilmelidir")]
    public List<int> PostingIds { get; set; } = new();

    [Required(ErrorMessage = "İşlem türü seçimi zorunludur")]
    [Display(Name = "İşlem")]
    public string Action { get; set; } = string.Empty;

    [Display(Name = "Yeni Durum")]
    public JobPostingStatus? Status { get; set; }

    [Display(Name = "Yeni Son Başvuru Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? NewLastApplicationDate { get; set; }

    [Display(Name = "Yeni Bitiş Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? NewExpiryDate { get; set; }

    [StringLength(500, ErrorMessage = "Notlar en fazla 500 karakter olabilir")]
    [Display(Name = "Notlar")]
    [DataType(DataType.MultilineText)]
    public string? Notes { get; set; }

    // For dropdown lists
    public List<SelectListItem> Actions { get; set; } = new();
    public List<SelectListItem> StatusOptions { get; set; } = new();
}

#endregion

#region Statistics & Dashboard ViewModels

public class JobPostingSummaryViewModel
{
    public int TotalPostings { get; set; }
    public int ActivePostings { get; set; }
    public int DraftPostings { get; set; }
    public int PausedPostings { get; set; }
    public int ClosedPostings { get; set; }
    public int FilledPostings { get; set; }
    public int ExpiringPostings { get; set; }
    public int TotalViews { get; set; }
    public int TotalApplications { get; set; }
    public decimal AverageApplicationsPerPosting { get; set; }
    public decimal AverageViewsPerPosting { get; set; }
    public decimal ConversionRate { get; set; }
}

public class DepartmentPostingSummaryViewModel
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public int PostingCount { get; set; }
    public int ActivePostingCount { get; set; }
    public int TotalApplications { get; set; }
    public decimal AverageApplicationsPerPosting { get; set; }
}

public class MonthlyPostingSummaryViewModel
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public int PostingCount { get; set; }
    public int ApplicationCount { get; set; }
    public int TotalViews { get; set; }
    public decimal AverageApplicationsPerPosting { get; set; }
}

public class PostingDashboardViewModel
{
    public JobPostingSummaryViewModel Summary { get; set; } = new();
    public List<JobPostingListViewModel> RecentPostings { get; set; } = new();
    public List<JobPostingListViewModel> ExpiringPostings { get; set; } = new();
    public List<JobPostingListViewModel> TopPerformingPostings { get; set; } = new();
    public List<DepartmentPostingSummaryViewModel> DepartmentSummaries { get; set; } = new();
    public List<MonthlyPostingSummaryViewModel> MonthlySummaries { get; set; } = new();
    public DateTime LastUpdated { get; set; } = DateTime.Now;
}

#endregion

#region Performance & Analytics ViewModels

public class PostingPerformanceViewModel
{
    public int PostingId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int ViewCount { get; set; }
    public int ApplicationCount { get; set; }
    public decimal ConversionRate { get; set; }
    public int DaysActive { get; set; }
    public decimal AverageApplicationsPerDay { get; set; }
    public decimal AverageViewsPerDay { get; set; }
    public List<DailyMetric> DailyMetrics { get; set; } = new();
}

public class DailyMetric
{
    public DateTime Date { get; set; }
    public int Views { get; set; }
    public int Applications { get; set; }
}

public class PostingAnalyticsViewModel
{
    public List<PostingPerformanceViewModel> TopPerformers { get; set; } = new();
    public List<PostingPerformanceViewModel> PoorPerformers { get; set; } = new();
    public Dictionary<string, int> PopularLocations { get; set; } = new();
    public Dictionary<EmploymentType, int> EmploymentTypeDistribution { get; set; } = new();
    public Dictionary<string, int> PopularTags { get; set; } = new();
}

#endregion
