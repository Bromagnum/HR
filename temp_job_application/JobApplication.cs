using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public enum ApplicationStatus
{
    Applied = 1,
    UnderReview = 2,
    Shortlisted = 3,
    InterviewScheduled = 4,
    InterviewCompleted = 5,
    SecondInterview = 6,
    Offered = 7,
    Accepted = 8,
    Rejected = 9,
    Withdrawn = 10,
    Hired = 11
}

public class JobApplication : BaseEntity
{
    // Başvuru Bilgileri
    [Required]
    public int CandidateId { get; set; }
    public virtual Candidate Candidate { get; set; } = null!;
    
    [Required]
    public int PositionId { get; set; }
    public virtual Position Position { get; set; } = null!;
    
    [Required]
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;
    
    [Required]
    public DateTime ApplicationDate { get; set; } = DateTime.Now;
    
    // Başvuru Detayları
    [StringLength(2000)]
    public string? CoverLetter { get; set; }
    
    [StringLength(1000)]
    public string? WhyInterested { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal? RequestedSalary { get; set; }
    
    public DateTime? AvailableStartDate { get; set; }
    
    [StringLength(20)]
    public string? PreferredWorkType { get; set; } // Remote, Office, Hybrid
    
    // Ek Dosyalar
    [StringLength(500)]
    public string? CvFilePath { get; set; }
    
    [StringLength(500)]
    public string? CoverLetterFilePath { get; set; }
    
    [StringLength(500)]
    public string? PortfolioFilePath { get; set; }
    
    // Değerlendirme
    [Range(1, 10)]
    public int? InitialScore { get; set; }
    
    [Range(1, 10)]
    public int? InterviewScore { get; set; }
    
    [Range(1, 10)]
    public int? TechnicalScore { get; set; }
    
    [Range(1, 10)]
    public int? OverallScore { get; set; }
    
    // İşlem Bilgileri
    public int? ReviewedById { get; set; }
    public virtual ApplicationUser? ReviewedBy { get; set; }
    
    public DateTime? ReviewedAt { get; set; }
    
    [StringLength(1000)]
    public string? ReviewNotes { get; set; }
    
    public int? InterviewerId { get; set; }
    public virtual ApplicationUser? Interviewer { get; set; }
    
    public DateTime? InterviewDate { get; set; }
    
    [StringLength(1000)]
    public string? InterviewNotesText { get; set; }
    
    // Karar Bilgileri
    public int? DecisionById { get; set; }
    public virtual ApplicationUser? DecisionBy { get; set; }
    
    public DateTime? DecisionDate { get; set; }
    
    [StringLength(1000)]
    public string? DecisionReason { get; set; }
    
    // Teklif Bilgileri (Eğer teklif verilmişse)
    [Range(0, double.MaxValue)]
    public decimal? OfferedSalary { get; set; }
    
    public DateTime? OfferDate { get; set; }
    
    public DateTime? OfferExpiryDate { get; set; }
    
    [StringLength(1000)]
    public string? OfferDetails { get; set; }
    
    // Red Bilgileri
    [StringLength(500)]
    public string? RejectionReason { get; set; }
    
    public DateTime? RejectionDate { get; set; }
    
    // İstatistik ve Takip
    public int ViewCount { get; set; } = 0;
    
    public DateTime? LastViewedAt { get; set; }
    
    public int? LastViewedById { get; set; }
    public virtual ApplicationUser? LastViewedBy { get; set; }
    
    [StringLength(100)]
    public string? Source { get; set; } // Website, LinkedIn, Referral, etc.
    
    [StringLength(1000)]
    public string? InternalNotes { get; set; }
    
    // Navigation Properties
    public virtual ICollection<InterviewNote> InterviewNotes { get; set; } = new List<InterviewNote>();
    public virtual ICollection<ApplicationDocument> Documents { get; set; } = new List<ApplicationDocument>();
    
    // Computed Properties
    public string StatusText => Status switch
    {
        ApplicationStatus.Applied => "Başvuru Alındı",
        ApplicationStatus.UnderReview => "İnceleniyor",
        ApplicationStatus.Shortlisted => "Ön Seçime Alındı",
        ApplicationStatus.InterviewScheduled => "Mülakat Planlandı",
        ApplicationStatus.InterviewCompleted => "Mülakat Tamamlandı",
        ApplicationStatus.SecondInterview => "İkinci Mülakat",
        ApplicationStatus.Offered => "Teklif Verildi",
        ApplicationStatus.Accepted => "Teklif Kabul Edildi",
        ApplicationStatus.Rejected => "Reddedildi",
        ApplicationStatus.Withdrawn => "Başvuru Çekildi",
        ApplicationStatus.Hired => "İşe Alındı",
        _ => "Bilinmiyor"
    };
    
    public int DaysInProcess => (DateTime.Now - ApplicationDate).Days;
    
    public bool IsApplicationActive => Status != ApplicationStatus.Rejected && 
                                      Status != ApplicationStatus.Withdrawn && 
                                      Status != ApplicationStatus.Hired;
}
