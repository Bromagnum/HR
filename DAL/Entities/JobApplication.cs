using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class JobApplication : BaseEntity
{
    // Başvuru Sahibi Bilgileri
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Phone { get; set; } = string.Empty;

    [StringLength(11)]
    public string? NationalId { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    [Range(18, 70)]
    public int? Age { get; set; }

    [StringLength(20)]
    public string? Gender { get; set; }

    // Başvuru Bilgileri
    [Required]
    public int PositionId { get; set; }

    [Required]
    public JobApplicationStatus Status { get; set; } = JobApplicationStatus.Submitted;

    public DateTime ApplicationDate { get; set; } = DateTime.Now;

    [StringLength(2000)]
    public string? CoverLetter { get; set; }

    [Range(0, 50)]
    public int? ExperienceYears { get; set; }

    [StringLength(200)]
    public string? CurrentCompany { get; set; }

    [StringLength(100)]
    public string? CurrentPosition { get; set; }

    [Range(0, 999999)]
    public decimal? ExpectedSalary { get; set; }

    [StringLength(100)]
    public string? EducationLevel { get; set; }

    [StringLength(200)]
    public string? University { get; set; }

    [StringLength(100)]
    public string? Department { get; set; }

    public int? GraduationYear { get; set; }

    [StringLength(1000)]
    public string? Skills { get; set; }

    [StringLength(500)]
    public string? Languages { get; set; }

    public bool IsAvailableImmediately { get; set; } = true;

    public DateTime? AvailableStartDate { get; set; }

    // Admin Değerlendirmesi
    public int? ReviewedById { get; set; }

    public DateTime? ReviewedAt { get; set; }

    [StringLength(2000)]
    public string? ReviewNotes { get; set; }

    [Range(1, 10)]
    public int? Rating { get; set; }

    public DateTime? InterviewDate { get; set; }

    [StringLength(1000)]
    public string? InterviewNotes { get; set; }

    // Navigation Properties
    public virtual Position Position { get; set; } = null!;
    public virtual Person? ReviewedBy { get; set; }
    public virtual ICollection<ApplicationDocument> Documents { get; set; } = new List<ApplicationDocument>();

    // Computed Properties
    public string FullName => $"{FirstName} {LastName}";

    public string StatusText => Status switch
    {
        JobApplicationStatus.Draft => "Taslak",
        JobApplicationStatus.Submitted => "Gönderildi",
        JobApplicationStatus.UnderReview => "İnceleme Aşamasında",
        JobApplicationStatus.Interviewed => "Mülakat Yapıldı",
        JobApplicationStatus.Approved => "Onaylandı",
        JobApplicationStatus.Rejected => "Reddedildi",
        JobApplicationStatus.Withdrawn => "Geri Çekildi",
        _ => "Bilinmiyor"
    };

    public string StatusClass => Status switch
    {
        JobApplicationStatus.Draft => "secondary",
        JobApplicationStatus.Submitted => "warning",
        JobApplicationStatus.UnderReview => "info",
        JobApplicationStatus.Interviewed => "primary",
        JobApplicationStatus.Approved => "success",
        JobApplicationStatus.Rejected => "danger",
        JobApplicationStatus.Withdrawn => "dark",
        _ => "light"
    };

    public bool HasCV => Documents.Any(d => d.DocumentType == DocumentType.CV);
    public bool HasCoverLetter => !string.IsNullOrEmpty(CoverLetter);
    public int DocumentCount => Documents.Count;
    public string ApplicationPeriod => $"{ApplicationDate:dd.MM.yyyy} - {(ReviewedAt?.ToString("dd.MM.yyyy") ?? "Devam ediyor")}";
}
