using System.ComponentModel.DataAnnotations;
using DAL.Entities;

namespace BLL.DTOs;

public class JobPostingListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string PositionName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public JobPostingStatus Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public string StatusClass { get; set; } = string.Empty;
    public EmploymentType EmploymentType { get; set; }
    public string EmploymentTypeText { get; set; } = string.Empty;
    public string? Location { get; set; }
    public bool IsRemoteWork { get; set; }
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public string SalaryRange { get; set; } = string.Empty;
    public DateTime PublishDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public DateTime? LastApplicationDate { get; set; }
    public int ViewCount { get; set; }
    public int ApplicationCount { get; set; }
    public int? OpenPositions { get; set; }
    public bool IsActive { get; set; }
    public bool IsExpired { get; set; }
    public int DaysUntilExpiry { get; set; }
    public int DaysUntilApplicationDeadline { get; set; }
    public string? CreatedByName { get; set; }
}

public class JobPostingDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Position & Department
    public int PositionId { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    
    // Status & Type
    public JobPostingStatus Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public string StatusClass { get; set; } = string.Empty;
    public EmploymentType EmploymentType { get; set; }
    public string EmploymentTypeText { get; set; } = string.Empty;
    
    // Requirements & Responsibilities
    public string? Requirements { get; set; }
    public string? Responsibilities { get; set; }
    public string? Benefits { get; set; }
    
    // Experience & Education
    public int? MinExperience { get; set; }
    public int? MaxExperience { get; set; }
    public string ExperienceRange { get; set; } = string.Empty;
    public string? MinEducation { get; set; }
    
    // Salary & Location
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public string SalaryRange { get; set; } = string.Empty;
    public string? Location { get; set; }
    public bool IsRemoteWork { get; set; }
    
    // Positions & Dates
    public int? OpenPositions { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public DateTime? LastApplicationDate { get; set; }
    
    // Contact & SEO
    public string? ContactInfo { get; set; }
    public string? Slug { get; set; }
    public string? MetaDescription { get; set; }
    public string? Tags { get; set; }
    
    // Statistics
    public int ViewCount { get; set; }
    public int ApplicationCount { get; set; }
    
    // Computed Properties
    public bool IsActive { get; set; }
    public bool IsExpired { get; set; }
    public bool IsApplicationDeadlinePassed { get; set; }
    public int DaysUntilExpiry { get; set; }
    public int DaysUntilApplicationDeadline { get; set; }
    
    // Creator Info
    public int? CreatedById { get; set; }
    public string? CreatedByName { get; set; }
    public int? UpdatedById { get; set; }
    public string? UpdatedByName { get; set; }
    
    // Applications (if included)
    public List<JobApplicationListDto> Applications { get; set; } = new();
    
    // Audit
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class JobPostingCreateDto
{
    [Required(ErrorMessage = "İlan başlığı gereklidir")]
    [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "İş tanımı gereklidir")]
    [StringLength(5000, ErrorMessage = "İş tanımı en fazla 5000 karakter olabilir")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Pozisyon seçimi gereklidir")]
    public int PositionId { get; set; }

    [Required(ErrorMessage = "Departman seçimi gereklidir")]
    public int DepartmentId { get; set; }

    [Required(ErrorMessage = "Çalışma türü seçimi gereklidir")]
    public EmploymentType EmploymentType { get; set; } = EmploymentType.FullTime;

    [StringLength(1000, ErrorMessage = "Gereksinimler en fazla 1000 karakter olabilir")]
    public string? Requirements { get; set; }

    [StringLength(1000, ErrorMessage = "Sorumluluklar en fazla 1000 karakter olabilir")]
    public string? Responsibilities { get; set; }

    [StringLength(1000, ErrorMessage = "Yan haklar en fazla 1000 karakter olabilir")]
    public string? Benefits { get; set; }

    [Range(0, 50, ErrorMessage = "Minimum deneyim 0-50 yıl arasında olmalıdır")]
    public int? MinExperience { get; set; }

    [Range(0, 50, ErrorMessage = "Maksimum deneyim 0-50 yıl arasında olmalıdır")]
    public int? MaxExperience { get; set; }

    [StringLength(100, ErrorMessage = "Eğitim seviyesi en fazla 100 karakter olabilir")]
    public string? MinEducation { get; set; }

    [Range(0, 999999, ErrorMessage = "Minimum maaş 0 veya pozitif olmalıdır")]
    public decimal? MinSalary { get; set; }

    [Range(0, 999999, ErrorMessage = "Maksimum maaş 0 veya pozitif olmalıdır")]
    public decimal? MaxSalary { get; set; }

    [StringLength(200, ErrorMessage = "Lokasyon en fazla 200 karakter olabilir")]
    public string? Location { get; set; }

    public bool IsRemoteWork { get; set; } = false;

    [Range(1, 100, ErrorMessage = "Açık pozisyon sayısı 1-100 arasında olmalıdır")]
    public int? OpenPositions { get; set; } = 1;

    public DateTime? ExpiryDate { get; set; }
    public DateTime? LastApplicationDate { get; set; }

    [StringLength(500, ErrorMessage = "İletişim bilgileri en fazla 500 karakter olabilir")]
    public string? ContactInfo { get; set; }

    [StringLength(200, ErrorMessage = "URL slug en fazla 200 karakter olabilir")]
    public string? Slug { get; set; }

    [StringLength(500, ErrorMessage = "Meta açıklama en fazla 500 karakter olabilir")]
    public string? MetaDescription { get; set; }

    [StringLength(200, ErrorMessage = "Etiketler en fazla 200 karakter olabilir")]
    public string? Tags { get; set; }

    public int? CreatedById { get; set; }
}

public class JobPostingUpdateDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "İlan başlığı gereklidir")]
    [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "İş tanımı gereklidir")]
    [StringLength(5000, ErrorMessage = "İş tanımı en fazla 5000 karakter olabilir")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Pozisyon seçimi gereklidir")]
    public int PositionId { get; set; }

    [Required(ErrorMessage = "Departman seçimi gereklidir")]
    public int DepartmentId { get; set; }

    [Required(ErrorMessage = "Durum seçimi gereklidir")]
    public JobPostingStatus Status { get; set; }

    [Required(ErrorMessage = "Çalışma türü seçimi gereklidir")]
    public EmploymentType EmploymentType { get; set; }

    [StringLength(1000, ErrorMessage = "Gereksinimler en fazla 1000 karakter olabilir")]
    public string? Requirements { get; set; }

    [StringLength(1000, ErrorMessage = "Sorumluluklar en fazla 1000 karakter olabilir")]
    public string? Responsibilities { get; set; }

    [StringLength(1000, ErrorMessage = "Yan haklar en fazla 1000 karakter olabilir")]
    public string? Benefits { get; set; }

    [Range(0, 50, ErrorMessage = "Minimum deneyim 0-50 yıl arasında olmalıdır")]
    public int? MinExperience { get; set; }

    [Range(0, 50, ErrorMessage = "Maksimum deneyim 0-50 yıl arasında olmalıdır")]
    public int? MaxExperience { get; set; }

    [StringLength(100, ErrorMessage = "Eğitim seviyesi en fazla 100 karakter olabilir")]
    public string? MinEducation { get; set; }

    [Range(0, 999999, ErrorMessage = "Minimum maaş 0 veya pozitif olmalıdır")]
    public decimal? MinSalary { get; set; }

    [Range(0, 999999, ErrorMessage = "Maksimum maaş 0 veya pozitif olmalıdır")]
    public decimal? MaxSalary { get; set; }

    [StringLength(200, ErrorMessage = "Lokasyon en fazla 200 karakter olabilir")]
    public string? Location { get; set; }

    public bool IsRemoteWork { get; set; }

    [Range(1, 100, ErrorMessage = "Açık pozisyon sayısı 1-100 arasında olmalıdır")]
    public int? OpenPositions { get; set; }

    public DateTime? ExpiryDate { get; set; }
    public DateTime? LastApplicationDate { get; set; }

    [StringLength(500, ErrorMessage = "İletişim bilgileri en fazla 500 karakter olabilir")]
    public string? ContactInfo { get; set; }

    [StringLength(200, ErrorMessage = "URL slug en fazla 200 karakter olabilir")]
    public string? Slug { get; set; }

    [StringLength(500, ErrorMessage = "Meta açıklama en fazla 500 karakter olabilir")]
    public string? MetaDescription { get; set; }

    [StringLength(200, ErrorMessage = "Etiketler en fazla 200 karakter olabilir")]
    public string? Tags { get; set; }

    public int? UpdatedById { get; set; }
}

public class JobPostingFilterDto
{
    public JobPostingStatus? Status { get; set; }
    public int? DepartmentId { get; set; }
    public int? PositionId { get; set; }
    public EmploymentType? EmploymentType { get; set; }
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public string? Location { get; set; }
    public string? SearchTerm { get; set; }
    public bool? IsRemoteWork { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsExpiring { get; set; } // Next 7 days
    public int? CreatedById { get; set; }
    public DateTime? PublishStartDate { get; set; }
    public DateTime? PublishEndDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class JobPostingSummaryDto
{
    public int TotalPostings { get; set; }
    public int ActivePostings { get; set; }
    public int DraftPostings { get; set; }
    public int PausedPostings { get; set; }
    public int ClosedPostings { get; set; }
    public int FilledPostings { get; set; }
    public int ExpiringPostings { get; set; } // Next 7 days
    public int TotalViews { get; set; }
    public int TotalApplications { get; set; }
    public List<DepartmentPostingSummaryDto> DepartmentSummary { get; set; } = new();
    public List<MonthlyPostingSummaryDto> MonthlyBreakdown { get; set; } = new();
}

public class DepartmentPostingSummaryDto
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public int PostingCount { get; set; }
    public int ActivePostingCount { get; set; }
    public int TotalApplications { get; set; }
    public decimal AverageApplicationsPerPosting { get; set; }
}

public class MonthlyPostingSummaryDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public int PostingCount { get; set; }
    public int ApplicationCount { get; set; }
    public int TotalViews { get; set; }
    public decimal AverageApplicationsPerPosting { get; set; }
}

public class PublicJobPostingDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PositionName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public EmploymentType EmploymentType { get; set; }
    public string EmploymentTypeText { get; set; } = string.Empty;
    public string? Requirements { get; set; }
    public string? Responsibilities { get; set; }
    public string? Benefits { get; set; }
    public string ExperienceRange { get; set; } = string.Empty;
    public string? MinEducation { get; set; }
    public string SalaryRange { get; set; } = string.Empty;
    public string? Location { get; set; }
    public bool IsRemoteWork { get; set; }
    public int? OpenPositions { get; set; }
    public DateTime PublishDate { get; set; }
    public List<string> Tags { get; set; } = new();
    public int? MinExperience { get; set; }
    public int? MaxExperience { get; set; }
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public DateTime? LastApplicationDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public JobPostingStatus Status { get; set; }
    public string? ContactInfo { get; set; }
    public string? Slug { get; set; }
    public int ViewCount { get; set; }
    public int ApplicationCount { get; set; }
    public int DaysUntilApplicationDeadline { get; set; }
    public bool CanApply { get; set; }
}

public class PublicJobPostingFilterDto
{
    public int? DepartmentId { get; set; }
    public int? PositionId { get; set; }
    public EmploymentType? EmploymentType { get; set; }
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public string? Location { get; set; }
    public string? SearchTerm { get; set; }
    public bool? IsRemoteWork { get; set; }
    public string? Tags { get; set; }
    public int? MinExperience { get; set; }
    public int? MaxExperience { get; set; }
    public string? MinEducation { get; set; }
    public string? Category { get; set; } // For position categories
    public string? SortBy { get; set; } = "newest"; // newest, oldest, salary-high, salary-low, deadline
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
