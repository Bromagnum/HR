using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.Entities;

namespace MVC.Models;

#region List & Filter ViewModels

public class JobApplicationListViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public int PositionId { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public DateTime ApplicationDate { get; set; }
    public JobApplicationStatus Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public string StatusClass { get; set; } = string.Empty;
    public int? ExperienceYears { get; set; }
    public decimal? ExpectedSalary { get; set; }
    public int? Rating { get; set; }
    public bool HasCV { get; set; }
    public int DocumentCount { get; set; }
    public string? ReviewedByName { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public DateTime? InterviewDate { get; set; }
    public string ApplicationPeriod { get; set; } = string.Empty;
    public string? EducationLevel { get; set; }
    public string? University { get; set; }
    public string? Department { get; set; }
    public int? GraduationYear { get; set; }
    public string? CurrentCompany { get; set; }
    public string? CurrentPosition { get; set; }
    public bool IsActive { get; set; }
}

public class JobApplicationFilterViewModel
{
    [Display(Name = "Arama")]
    public string? SearchTerm { get; set; }

    [Display(Name = "Durum")]
    public JobApplicationStatus? Status { get; set; }

    [Display(Name = "Pozisyon")]
    public int? PositionId { get; set; }

    [Display(Name = "Başlangıç Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? StartDate { get; set; }

    [Display(Name = "Bitiş Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    [Display(Name = "Değerlendiren")]
    public int? ReviewedById { get; set; }

    [Display(Name = "Min. Puan")]
    [Range(1, 10)]
    public int? MinRating { get; set; }

    [Display(Name = "Max. Puan")]
    [Range(1, 10)]
    public int? MaxRating { get; set; }

    // For dropdown lists
    public List<SelectListItem> Positions { get; set; } = new();
    public List<SelectListItem> Reviewers { get; set; } = new();
    public List<SelectListItem> StatusOptions { get; set; } = new();
}

#endregion

#region Detail ViewModels

public class JobApplicationDetailViewModel
{
    public int Id { get; set; }
    public int PositionId { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? CoverLetter { get; set; }
    public DateTime ApplicationDate { get; set; }
    public string ApplicationPeriod { get; set; } = string.Empty;
    public JobApplicationStatus Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public string StatusClass { get; set; } = string.Empty;
    public int? ExperienceYears { get; set; }
    public decimal? ExpectedSalary { get; set; }
    public int? ReviewedById { get; set; }
    public string? ReviewedByName { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public int? Rating { get; set; }
    public string? ReviewNotes { get; set; }
    public DateTime? InterviewDate { get; set; }
    public string? InterviewNotes { get; set; }
    public string? EducationLevel { get; set; }
    public string? University { get; set; }
    public string? Department { get; set; }
    public int? GraduationYear { get; set; }
    public string? CurrentCompany { get; set; }
    public string? CurrentPosition { get; set; }
    public List<ApplicationDocumentViewModel> Documents { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}

public class ApplicationDocumentViewModel
{
    public int Id { get; set; }
    public int JobApplicationId { get; set; }
    public DocumentType DocumentType { get; set; }
    public string DocumentTypeName { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string FileSizeFormatted { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
    public bool IsVerified { get; set; }
    public string? Notes { get; set; }
}

#endregion

#region Create & Edit ViewModels

public class JobApplicationCreateViewModel
{
    [Required(ErrorMessage = "Pozisyon seçimi zorunludur")]
    [Display(Name = "Pozisyon")]
    public int PositionId { get; set; }

    [Required(ErrorMessage = "Ad alanı zorunludur")]
    [StringLength(100, ErrorMessage = "Ad en fazla 100 karakter olabilir")]
    [Display(Name = "Ad")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Soyad alanı zorunludur")]
    [StringLength(100, ErrorMessage = "Soyad en fazla 100 karakter olabilir")]
    [Display(Name = "Soyad")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta alanı zorunludur")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
    [StringLength(200, ErrorMessage = "E-posta en fazla 200 karakter olabilir")]
    [Display(Name = "E-posta")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
    [StringLength(50, ErrorMessage = "Telefon en fazla 50 karakter olabilir")]
    [Display(Name = "Telefon")]
    public string? PhoneNumber { get; set; }

    [StringLength(500, ErrorMessage = "Ön yazı en fazla 500 karakter olabilir")]
    [Display(Name = "Ön Yazı")]
    [DataType(DataType.MultilineText)]
    public string? CoverLetter { get; set; }

    [Display(Name = "Deneyim (Yıl)")]
    [Range(0, 50, ErrorMessage = "Deneyim 0-50 yıl arasında olmalıdır")]
    public int? ExperienceYears { get; set; }

    [Display(Name = "Beklenen Maaş")]
    [Range(0, 999999, ErrorMessage = "Geçerli bir maaş miktarı giriniz")]
    public decimal? ExpectedSalary { get; set; }

    [Display(Name = "Eğitim Seviyesi")]
    public EducationLevel? EducationLevel { get; set; }

    [Display(Name = "İhbar Süresi (Hafta)")]
    [Range(0, 52, ErrorMessage = "İhbar süresi 0-52 hafta arasında olmalıdır")]
    public int? NoticePeriodWeeks { get; set; }

    // File uploads
    [Display(Name = "CV Dosyası")]
    public IFormFile? CvFile { get; set; }

    [Display(Name = "Ek Belgeler")]
    public List<IFormFile>? AdditionalDocuments { get; set; }

    // For dropdown lists
    public List<SelectListItem> Positions { get; set; } = new();

    // Computed
    public string FullName => $"{FirstName} {LastName}".Trim();
}

public class JobApplicationEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Pozisyon seçimi zorunludur")]
    [Display(Name = "Pozisyon")]
    public int PositionId { get; set; }

    [Required(ErrorMessage = "Ad alanı zorunludur")]
    [StringLength(100, ErrorMessage = "Ad en fazla 100 karakter olabilir")]
    [Display(Name = "Ad")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Soyad alanı zorunludur")]
    [StringLength(100, ErrorMessage = "Soyad en fazla 100 karakter olabilir")]
    [Display(Name = "Soyad")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta alanı zorunludur")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
    [StringLength(200, ErrorMessage = "E-posta en fazla 200 karakter olabilir")]
    [Display(Name = "E-posta")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
    [StringLength(50, ErrorMessage = "Telefon en fazla 50 karakter olabilir")]
    [Display(Name = "Telefon")]
    public string? PhoneNumber { get; set; }

    [StringLength(500, ErrorMessage = "Ön yazı en fazla 500 karakter olabilir")]
    [Display(Name = "Ön Yazı")]
    [DataType(DataType.MultilineText)]
    public string? CoverLetter { get; set; }

    [Display(Name = "Deneyim (Yıl)")]
    [Range(0, 50, ErrorMessage = "Deneyim 0-50 yıl arasında olmalıdır")]
    public int? ExperienceYears { get; set; }

    [Display(Name = "Beklenen Maaş")]
    [Range(0, 999999, ErrorMessage = "Geçerli bir maaş miktarı giriniz")]
    public decimal? ExpectedSalary { get; set; }

    [Display(Name = "Eğitim Seviyesi")]
    public EducationLevel? EducationLevel { get; set; }

    [Display(Name = "İhbar Süresi (Hafta)")]
    [Range(0, 52, ErrorMessage = "İhbar süresi 0-52 hafta arasında olmalıdır")]
    public int? NoticePeriodWeeks { get; set; }

    [Display(Name = "Durum")]
    public JobApplicationStatus Status { get; set; }

    [Display(Name = "Değerlendiren")]
    public int? ReviewedById { get; set; }

    [Display(Name = "Değerlendirme Puanı")]
    [Range(1, 10, ErrorMessage = "Puan 1-10 arasında olmalıdır")]
    public int? Rating { get; set; }

    [Display(Name = "Değerlendirme Notları")]
    [StringLength(1000, ErrorMessage = "Notlar en fazla 1000 karakter olabilir")]
    [DataType(DataType.MultilineText)]
    public string? ReviewNotes { get; set; }

    [Display(Name = "Mülakat Tarihi")]
    [DataType(DataType.DateTime)]
    public DateTime? InterviewDate { get; set; }

    [Display(Name = "Mülakat Notları")]
    [StringLength(1000, ErrorMessage = "Notlar en fazla 1000 karakter olabilir")]
    [DataType(DataType.MultilineText)]
    public string? InterviewNotes { get; set; }

    // File uploads
    [Display(Name = "Yeni CV Dosyası")]
    public IFormFile? NewCvFile { get; set; }

    [Display(Name = "Yeni Ek Belgeler")]
    public List<IFormFile>? NewAdditionalDocuments { get; set; }

    // Existing documents
    public List<ApplicationDocumentViewModel> ExistingDocuments { get; set; } = new();

    // For dropdown lists
    public List<SelectListItem> Positions { get; set; } = new();
    public List<SelectListItem> StatusOptions { get; set; } = new();
    public List<SelectListItem> Reviewers { get; set; } = new();

    // Computed
    public string FullName => $"{FirstName} {LastName}".Trim();
}

#endregion

#region Public Application ViewModels

public class PublicJobApplicationViewModel
{
    [Required(ErrorMessage = "Pozisyon seçimi zorunludur")]
    public int JobPostingId { get; set; }

    [Required(ErrorMessage = "Ad alanı zorunludur")]
    [StringLength(100, ErrorMessage = "Ad en fazla 100 karakter olabilir")]
    [Display(Name = "Adınız")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Soyad alanı zorunludur")]
    [StringLength(100, ErrorMessage = "Soyad en fazla 100 karakter olabilir")]
    [Display(Name = "Soyadınız")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta alanı zorunludur")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
    [StringLength(200, ErrorMessage = "E-posta en fazla 200 karakter olabilir")]
    [Display(Name = "E-posta Adresiniz")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
    [StringLength(50, ErrorMessage = "Telefon en fazla 50 karakter olabilir")]
    [Display(Name = "Telefon Numaranız")]
    public string? PhoneNumber { get; set; }

    [StringLength(500, ErrorMessage = "Ön yazı en fazla 500 karakter olabilir")]
    [Display(Name = "Ön Yazı")]
    [DataType(DataType.MultilineText)]
    public string? CoverLetter { get; set; }

    [Display(Name = "Deneyim Süreniz (Yıl)")]
    [Range(0, 50, ErrorMessage = "Deneyim 0-50 yıl arasında olmalıdır")]
    public int? ExperienceYears { get; set; }

    [Display(Name = "Beklenen Maaş")]
    [Range(0, 999999, ErrorMessage = "Geçerli bir maaş miktarı giriniz")]
    public decimal? ExpectedSalary { get; set; }

    [Required(ErrorMessage = "CV dosyası zorunludur")]
    [Display(Name = "CV Dosyanız")]
    public IFormFile CvFile { get; set; } = null!;

    [Display(Name = "Ek Belgeler (Opsiyonel)")]
    public List<IFormFile>? AdditionalDocuments { get; set; }

    [Required(ErrorMessage = "KVKK onayı zorunludur")]
    [Display(Name = "Kişisel Verilerin Korunması Kanunu kapsamında verilerimin işlenmesine onay veriyorum")]
    public bool KvkkConsent { get; set; }

    // Job posting info (populated from controller)
    public string JobTitle { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string? Location { get; set; }

    // Computed
    public string FullName => $"{FirstName} {LastName}".Trim();
}

public class ApplicationSuccessViewModel
{
    public string FullName { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime ApplicationDate { get; set; }
    public string ApplicationId { get; set; } = string.Empty;
}

#endregion

#region Status Management ViewModels

public class JobApplicationStatusUpdateViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public JobApplicationStatus CurrentStatus { get; set; }

    [Required(ErrorMessage = "Yeni durum seçimi zorunludur")]
    [Display(Name = "Yeni Durum")]
    public JobApplicationStatus NewStatus { get; set; }

    [Required(ErrorMessage = "Değerlendiren seçimi zorunludur")]
    [Display(Name = "Değerlendiren")]
    public int ReviewedById { get; set; }

    [StringLength(1000, ErrorMessage = "Notlar en fazla 1000 karakter olabilir")]
    [Display(Name = "Değerlendirme Notları")]
    [DataType(DataType.MultilineText)]
    public string? Notes { get; set; }

    [Display(Name = "Mülakat Tarihi")]
    [DataType(DataType.DateTime)]
    public DateTime? InterviewDate { get; set; }

    [Display(Name = "Değerlendirme Puanı")]
    [Range(1, 10, ErrorMessage = "Puan 1-10 arasında olmalıdır")]
    public int? Rating { get; set; }

    // For dropdown lists
    public List<SelectListItem> StatusOptions { get; set; } = new();
    public List<SelectListItem> Reviewers { get; set; } = new();
}

public class BulkStatusUpdateViewModel
{
    [Required(ErrorMessage = "En az bir başvuru seçilmelidir")]
    public List<int> ApplicationIds { get; set; } = new();

    [Required(ErrorMessage = "Durum seçimi zorunludur")]
    [Display(Name = "Yeni Durum")]
    public JobApplicationStatus Status { get; set; }

    [Required(ErrorMessage = "Değerlendiren seçimi zorunludur")]
    [Display(Name = "Değerlendiren")]
    public int ReviewedById { get; set; }

    [StringLength(1000, ErrorMessage = "Notlar en fazla 1000 karakter olabilir")]
    [Display(Name = "Notlar")]
    [DataType(DataType.MultilineText)]
    public string? Notes { get; set; }

    // For dropdown lists
    public List<SelectListItem> StatusOptions { get; set; } = new();
    public List<SelectListItem> Reviewers { get; set; } = new();
}

#endregion

#region Statistics & Dashboard ViewModels

public class JobApplicationSummaryViewModel
{
    public int TotalApplications { get; set; }
    public int PendingApplications { get; set; }
    public int UnderReviewApplications { get; set; }
    public int ShortlistedApplications { get; set; }
    public int InterviewApplications { get; set; }
    public int ApprovedApplications { get; set; }
    public int RejectedApplications { get; set; }
    public decimal ApprovalRate { get; set; }
    public int TodaysApplications { get; set; }
    public int WeeklyApplications { get; set; }
    public int MonthlyApplications { get; set; }
}

public class PositionApplicationSummaryViewModel
{
    public int PositionId { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public int ApplicationCount { get; set; }
    public int PendingCount { get; set; }
    public int ApprovedCount { get; set; }
    public int RejectedCount { get; set; }
    public decimal ApprovalRate { get; set; }
}

public class MonthlyApplicationSummaryViewModel
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public int ApplicationCount { get; set; }
    public int ApprovedCount { get; set; }
    public int RejectedCount { get; set; }
    public decimal ApprovalRate { get; set; }
}

public class ApplicationDashboardViewModel
{
    public JobApplicationSummaryViewModel Summary { get; set; } = new();
    public List<JobApplicationListViewModel> RecentApplications { get; set; } = new();
    public List<JobApplicationListViewModel> PendingApplications { get; set; } = new();
    public List<JobApplicationListViewModel> UpcomingInterviews { get; set; } = new();
    public List<PositionApplicationSummaryViewModel> PositionSummaries { get; set; } = new();
    public List<MonthlyApplicationSummaryViewModel> MonthlySummaries { get; set; } = new();
    public DateTime LastUpdated { get; set; } = DateTime.Now;
}

#endregion

#region Document Management ViewModels

public class DocumentUploadViewModel
{
    public int JobApplicationId { get; set; }
    public string ApplicantName { get; set; } = string.Empty;
    public DocumentType DocumentType { get; set; }
    public IFormFile File { get; set; } = null!;
    public string? Notes { get; set; }

    // For dropdown lists
    public List<SelectListItem> DocumentTypes { get; set; } = new();
}

public class DocumentListViewModel
{
    public int JobApplicationId { get; set; }
    public string ApplicantName { get; set; } = string.Empty;
    public List<ApplicationDocumentViewModel> Documents { get; set; } = new();
}

#endregion
