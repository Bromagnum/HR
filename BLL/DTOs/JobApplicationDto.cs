using System.ComponentModel.DataAnnotations;
using DAL.Entities;

namespace BLL.DTOs;

// List DTO - JobApplication listesi için
public class JobApplicationListDto
{
    public int Id { get; set; }
    public int CandidateId { get; set; }
    public string CandidateName { get; set; } = string.Empty;
    public string CandidateEmail { get; set; } = string.Empty;
    public int PositionId { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public ApplicationStatus Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public DateTime ApplicationDate { get; set; }
    public decimal? RequestedSalary { get; set; }
    public int? InitialScore { get; set; }
    public int? OverallScore { get; set; }
    public string? ReviewedByName { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public DateTime? InterviewDate { get; set; }
    public string? InterviewerName { get; set; }
    public int DaysInProcess { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Detail DTO - JobApplication detayları için
public class JobApplicationDetailDto
{
    public int Id { get; set; }
    public int CandidateId { get; set; }
    public CandidateListDto Candidate { get; set; } = new();
    public int PositionId { get; set; }
    public PositionDetailDto Position { get; set; } = new();
    public ApplicationStatus Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public DateTime ApplicationDate { get; set; }
    
    // Başvuru Detayları
    public string? CoverLetter { get; set; }
    public string? WhyInterested { get; set; }
    public decimal? RequestedSalary { get; set; }
    public DateTime? AvailableStartDate { get; set; }
    public string? PreferredWorkType { get; set; }
    
    // Dosyalar
    public string? CvFilePath { get; set; }
    public string? CoverLetterFilePath { get; set; }
    public string? PortfolioFilePath { get; set; }
    
    // Değerlendirme
    public int? InitialScore { get; set; }
    public int? InterviewScore { get; set; }
    public int? TechnicalScore { get; set; }
    public int? OverallScore { get; set; }
    
    // İşlem Bilgileri
    public int? ReviewedById { get; set; }
    public string? ReviewedByName { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? ReviewNotes { get; set; }
    
    public int? InterviewerId { get; set; }
    public string? InterviewerName { get; set; }
    public DateTime? InterviewDate { get; set; }
    public string? InterviewNotesText { get; set; }
    
    // Karar Bilgileri
    public int? DecisionById { get; set; }
    public string? DecisionByName { get; set; }
    public DateTime? DecisionDate { get; set; }
    public string? DecisionReason { get; set; }
    
    // Teklif Bilgileri
    public decimal? OfferedSalary { get; set; }
    public DateTime? OfferDate { get; set; }
    public DateTime? OfferExpiryDate { get; set; }
    public string? OfferDetails { get; set; }
    
    // Red Bilgileri
    public string? RejectionReason { get; set; }
    public DateTime? RejectionDate { get; set; }
    
    // İstatistik
    public int ViewCount { get; set; }
    public DateTime? LastViewedAt { get; set; }
    public string? LastViewedByName { get; set; }
    public string? Source { get; set; }
    public string? InternalNotes { get; set; }
    public int DaysInProcess { get; set; }
    
    // Navigation Properties
    public List<InterviewNoteDto> InterviewNotes { get; set; } = new();
    public List<ApplicationDocumentDto> Documents { get; set; } = new();
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}

// Create DTO - Yeni job application oluşturma için
public class JobApplicationCreateDto
{
    [Required(ErrorMessage = "Aday seçilmelidir")]
    public int CandidateId { get; set; }
    
    [Required(ErrorMessage = "Pozisyon seçilmelidir")]
    public int PositionId { get; set; }
    
    [StringLength(2000, ErrorMessage = "Ön yazı en fazla 2000 karakter olabilir")]
    public string? CoverLetter { get; set; }
    
    [StringLength(1000, ErrorMessage = "İlgi nedeni en fazla 1000 karakter olabilir")]
    public string? WhyInterested { get; set; }
    
    [Range(0, double.MaxValue, ErrorMessage = "Talep edilen maaş pozitif olmalıdır")]
    public decimal? RequestedSalary { get; set; }
    
    public DateTime? AvailableStartDate { get; set; }
    
    [StringLength(20)]
    public string? PreferredWorkType { get; set; }
    
    [StringLength(500)]
    public string? CvFilePath { get; set; }
    
    [StringLength(500)]
    public string? CoverLetterFilePath { get; set; }
    
    [StringLength(500)]
    public string? PortfolioFilePath { get; set; }
    
    [StringLength(100)]
    public string? Source { get; set; }
}

// Update DTO - JobApplication güncelleme için
public class JobApplicationUpdateDto : JobApplicationCreateDto
{
    public int Id { get; set; }
    public ApplicationStatus Status { get; set; }
}

// Review DTO - Başvuru değerlendirme için
public class JobApplicationReviewDto
{
    public int Id { get; set; }
    
    [Range(1, 10, ErrorMessage = "Puan 1-10 arasında olmalıdır")]
    public int? InitialScore { get; set; }
    
    [StringLength(1000, ErrorMessage = "Değerlendirme notu en fazla 1000 karakter olabilir")]
    public string? ReviewNotes { get; set; }
    
    [Required(ErrorMessage = "Durum seçilmelidir")]
    public ApplicationStatus Status { get; set; }
    
    public int ReviewedById { get; set; }
}

// Interview Schedule DTO - Mülakat planlama için
public class JobApplicationInterviewDto
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Mülakat tarihi zorunludur")]
    public DateTime InterviewDate { get; set; }
    
    [Required(ErrorMessage = "Mülakatçı seçilmelidir")]
    public int InterviewerId { get; set; }
    
    [StringLength(1000)]
    public string? InterviewNotesText { get; set; }
}

// Decision DTO - Karar verme için
public class JobApplicationDecisionDto
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Karar durumu seçilmelidir")]
    public ApplicationStatus Status { get; set; }
    
    [StringLength(1000, ErrorMessage = "Karar gerekçesi en fazla 1000 karakter olabilir")]
    public string? DecisionReason { get; set; }
    
    // Teklif bilgileri (eğer teklif veriliyorsa)
    [Range(0, double.MaxValue, ErrorMessage = "Teklif edilen maaş pozitif olmalıdır")]
    public decimal? OfferedSalary { get; set; }
    
    public DateTime? OfferExpiryDate { get; set; }
    
    [StringLength(1000)]
    public string? OfferDetails { get; set; }
    
    // Red bilgileri (eğer reddediliyorsa)
    [StringLength(500)]
    public string? RejectionReason { get; set; }
    
    public int DecisionById { get; set; }
}

// Filter DTO - JobApplication filtreleme için
public class JobApplicationFilterDto
{
    public string? SearchTerm { get; set; }
    public ApplicationStatus? Status { get; set; }
    public int? CandidateId { get; set; }
    public int? PositionId { get; set; }
    public int? DepartmentId { get; set; }
    public int? ReviewedById { get; set; }
    public int? InterviewerId { get; set; }
    public DateTime? ApplicationDateFrom { get; set; }
    public DateTime? ApplicationDateTo { get; set; }
    public DateTime? InterviewDateFrom { get; set; }
    public DateTime? InterviewDateTo { get; set; }
    public int? MinScore { get; set; }
    public int? MaxScore { get; set; }
    public decimal? MinRequestedSalary { get; set; }
    public decimal? MaxRequestedSalary { get; set; }
    public string? Source { get; set; }
    public bool? HasInterview { get; set; }
    public bool? IsExpiredOffer { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string SortBy { get; set; } = "ApplicationDate";
    public bool SortDescending { get; set; } = true;
}

// Statistics DTO - İstatistikler için
public class JobApplicationStatisticsDto
{
    public int TotalApplications { get; set; }
    public int PendingApplications { get; set; }
    public int UnderReviewApplications { get; set; }
    public int ShortlistedApplications { get; set; }
    public int InterviewScheduledApplications { get; set; }
    public int OfferedApplications { get; set; }
    public int HiredApplications { get; set; }
    public int RejectedApplications { get; set; }
    public decimal AverageProcessingDays { get; set; }
    public decimal OfferAcceptanceRate { get; set; }
    public Dictionary<string, int> ApplicationsByStatus { get; set; } = new();
    public Dictionary<string, int> ApplicationsByMonth { get; set; } = new();
    public Dictionary<string, int> ApplicationsBySource { get; set; } = new();
    public List<TopPositionDto> TopPositions { get; set; } = new();
}

public class TopPositionDto
{
    public int PositionId { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public int ApplicationCount { get; set; }
}
