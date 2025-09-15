using System.ComponentModel.DataAnnotations;
using DAL.Entities;

namespace BLL.DTOs;

public class JobApplicationListDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string PositionName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public JobApplicationStatus Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public string StatusClass { get; set; } = string.Empty;
    public DateTime ApplicationDate { get; set; }
    public int? ExperienceYears { get; set; }
    public decimal? ExpectedSalary { get; set; }
    public bool HasCV { get; set; }
    public int DocumentCount { get; set; }
    public string? ReviewedByName { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public int? Rating { get; set; }
    public string ApplicationPeriod { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class JobApplicationDetailDto
{
    public int Id { get; set; }
    
    // Başvuru Sahibi Bilgileri
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? NationalId { get; set; }
    public string? Address { get; set; }
    public int? Age { get; set; }
    public string? Gender { get; set; }
    
    // Başvuru Bilgileri
    public int PositionId { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public JobApplicationStatus Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public string StatusClass { get; set; } = string.Empty;
    public DateTime ApplicationDate { get; set; }
    public string? CoverLetter { get; set; }
    
    // İş Deneyimi
    public int? ExperienceYears { get; set; }
    public string? CurrentCompany { get; set; }
    public string? CurrentPosition { get; set; }
    public decimal? ExpectedSalary { get; set; }
    
    // Eğitim Bilgileri
    public string? EducationLevel { get; set; }
    public string? University { get; set; }
    public string? Department { get; set; }
    public int? GraduationYear { get; set; }
    
    // Beceriler
    public string? Skills { get; set; }
    public string? Languages { get; set; }
    
    // Çalışma Durumu
    public bool IsAvailableImmediately { get; set; }
    public DateTime? AvailableStartDate { get; set; }
    
    // Admin Değerlendirmesi
    public int? ReviewedById { get; set; }
    public string? ReviewedByName { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? ReviewNotes { get; set; }
    public int? Rating { get; set; }
    public DateTime? InterviewDate { get; set; }
    public string? InterviewNotes { get; set; }
    
    // Belgeler
    public List<ApplicationDocumentDto> Documents { get; set; } = new();
    public bool HasCV { get; set; }
    public bool HasCoverLetter { get; set; }
    public int DocumentCount { get; set; }
    public string ApplicationPeriod { get; set; } = string.Empty;
    
    // Audit
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}

public class JobApplicationCreateDto
{
    // Başvuru Sahibi Bilgileri
    [Required(ErrorMessage = "Ad gereklidir")]
    [StringLength(100, ErrorMessage = "Ad en fazla 100 karakter olabilir")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Soyad gereklidir")]
    [StringLength(100, ErrorMessage = "Soyad en fazla 100 karakter olabilir")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta gereklidir")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
    [StringLength(200, ErrorMessage = "E-posta en fazla 200 karakter olabilir")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefon gereklidir")]
    [StringLength(20, ErrorMessage = "Telefon en fazla 20 karakter olabilir")]
    public string Phone { get; set; } = string.Empty;

    [StringLength(11, ErrorMessage = "TC Kimlik numarası 11 karakter olmalıdır")]
    public string? NationalId { get; set; }

    [StringLength(500, ErrorMessage = "Adres en fazla 500 karakter olabilir")]
    public string? Address { get; set; }

    [Range(18, 70, ErrorMessage = "Yaş 18-70 arasında olmalıdır")]
    public int? Age { get; set; }

    public string? Gender { get; set; }

    // Başvuru Bilgileri
    [Required(ErrorMessage = "Pozisyon seçimi gereklidir")]
    public int PositionId { get; set; }

    [StringLength(2000, ErrorMessage = "Ön yazı en fazla 2000 karakter olabilir")]
    public string? CoverLetter { get; set; }

    // İş Deneyimi
    [Range(0, 50, ErrorMessage = "Deneyim 0-50 yıl arasında olmalıdır")]
    public int? ExperienceYears { get; set; }

    [StringLength(200, ErrorMessage = "Şirket adı en fazla 200 karakter olabilir")]
    public string? CurrentCompany { get; set; }

    [StringLength(100, ErrorMessage = "Pozisyon en fazla 100 karakter olabilir")]
    public string? CurrentPosition { get; set; }

    [Range(0, 999999, ErrorMessage = "Maaş beklentisi geçerli bir miktar olmalıdır")]
    public decimal? ExpectedSalary { get; set; }

    // Eğitim Bilgileri
    [StringLength(100, ErrorMessage = "Eğitim seviyesi en fazla 100 karakter olabilir")]
    public string? EducationLevel { get; set; }

    [StringLength(200, ErrorMessage = "Üniversite adı en fazla 200 karakter olabilir")]
    public string? University { get; set; }

    [StringLength(100, ErrorMessage = "Bölüm adı en fazla 100 karakter olabilir")]
    public string? Department { get; set; }

    [Range(1950, 2030, ErrorMessage = "Mezuniyet yılı 1950-2030 arasında olmalıdır")]
    public int? GraduationYear { get; set; }

    // Beceriler
    [StringLength(1000, ErrorMessage = "Beceriler en fazla 1000 karakter olabilir")]
    public string? Skills { get; set; }

    // Dosya Upload
    public string? CvFile { get; set; }
    public List<string> AdditionalDocuments { get; set; } = new();

    [StringLength(500, ErrorMessage = "Dil bilgileri en fazla 500 karakter olabilir")]
    public string? Languages { get; set; }

    // Çalışma Durumu
    public bool IsAvailableImmediately { get; set; } = true;
    public DateTime? AvailableStartDate { get; set; }
}

public class JobApplicationUpdateDto
{
    public int Id { get; set; }

    // Admin tarafından güncellenen alanlar
    [Required]
    public JobApplicationStatus Status { get; set; }

    [StringLength(2000, ErrorMessage = "İnceleme notları en fazla 2000 karakter olabilir")]
    public string? ReviewNotes { get; set; }

    [Range(1, 10, ErrorMessage = "Değerlendirme 1-10 arasında olmalıdır")]
    public int? Rating { get; set; }

    public DateTime? InterviewDate { get; set; }

    [StringLength(1000, ErrorMessage = "Mülakat notları en fazla 1000 karakter olabilir")]
    public string? InterviewNotes { get; set; }

    public int? ReviewedById { get; set; }
}

public class JobApplicationFilterDto
{
    public JobApplicationStatus? Status { get; set; }
    public int? PositionId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? SearchTerm { get; set; }
    public int? ReviewedById { get; set; }
    public int? MinExperience { get; set; }
    public int? MaxExperience { get; set; }
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public string? EducationLevel { get; set; }
    public bool? HasCV { get; set; }
    public int? Rating { get; set; }
}

public class ApplicationDocumentDto
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public DocumentType DocumentType { get; set; }
    public string DocumentTypeText { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public string FileSizeText { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime UploadDate { get; set; }
    public bool IsVerified { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public string? VerificationNotes { get; set; }
    public int DownloadCount { get; set; }
    public string FileExtension { get; set; } = string.Empty;
    public bool IsImageFile { get; set; }
    public bool IsPdfFile { get; set; }
    public bool IsDocumentFile { get; set; }
    public long FileSize { get; set; }
}

public class JobApplicationSummaryDto
{
    public int TotalApplications { get; set; }
    public int PendingApplications { get; set; }
    public int UnderReviewApplications { get; set; }
    public int ShortlistedApplications { get; set; }
    public int InterviewApplications { get; set; }
    public int ApprovedApplications { get; set; }
    public int RejectedApplications { get; set; }
    public List<PositionApplicationSummaryDto> PositionSummary { get; set; } = new();
    public List<MonthlyApplicationSummaryDto> MonthlyBreakdown { get; set; } = new();
}

public class PositionApplicationSummaryDto
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

public class MonthlyApplicationSummaryDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public int ApplicationCount { get; set; }
    public int ApprovedCount { get; set; }
    public int RejectedCount { get; set; }
    public decimal ApprovalRate { get; set; }
}
