using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class Candidate : BaseEntity
{
    // Kimlik Bilgileri
    [Required]
    [StringLength(11)]
    public string TcKimlikNo { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string? FatherName { get; set; }
    
    [StringLength(50)]
    public string? MotherName { get; set; }
    
    [StringLength(100)]
    public string? BirthPlace { get; set; }
    
    public DateTime? BirthDate { get; set; }
    
    // İletişim Bilgileri
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [Phone]
    [StringLength(20)]
    public string Phone { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Address { get; set; }
    
    [StringLength(100)]
    public string? City { get; set; }
    
    [StringLength(100)]
    public string? Country { get; set; } = "Türkiye";
    
    // Kişisel Bilgiler
    [StringLength(20)]
    public string? Gender { get; set; }
    
    [StringLength(20)]
    public string? MaritalStatus { get; set; }
    
    [StringLength(20)]
    public string? MilitaryStatus { get; set; }
    
    [StringLength(10)]
    public string? DriverLicenseClass { get; set; }
    
    // Profesyonel Bilgiler
    [Range(0, 50)]
    public int? ExperienceYears { get; set; }
    
    [StringLength(100)]
    public string? CurrentCompany { get; set; }
    
    [StringLength(100)]
    public string? CurrentPosition { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal? ExpectedSalary { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal? CurrentSalary { get; set; }
    
    public DateTime? AvailableStartDate { get; set; }
    
    [StringLength(20)]
    public string? PreferredWorkType { get; set; } // Remote, Office, Hybrid
    
    // CV Dosyası
    [StringLength(500)]
    public string? CvFilePath { get; set; }
    
    [StringLength(100)]
    public string? CvFileName { get; set; }
    
    public DateTime? CvUploadDate { get; set; }
    
    // Sosyal Medya
    [StringLength(200)]
    public string? LinkedInUrl { get; set; }
    
    [StringLength(200)]
    public string? GitHubUrl { get; set; }
    
    [StringLength(200)]
    public string? PersonalWebsite { get; set; }
    
    // Durum Bilgileri
    [StringLength(20)]
    public string Status { get; set; } = "Active"; // Active, Inactive, Blacklisted
    
    [StringLength(500)]
    public string? Notes { get; set; }
    
    public DateTime? LastContactDate { get; set; }
    
    [StringLength(100)]
    public string? Source { get; set; } // Website, LinkedIn, Referral, etc.
    
    // Navigation Properties
    public virtual ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
    public virtual ICollection<CandidateEducation> Educations { get; set; } = new List<CandidateEducation>();
    public virtual ICollection<CandidateExperience> Experiences { get; set; } = new List<CandidateExperience>();
    public virtual ICollection<CandidateSkill> Skills { get; set; } = new List<CandidateSkill>();
    public virtual ICollection<InterviewNote> InterviewNotes { get; set; } = new List<InterviewNote>();
    
    // Computed Properties
    public string FullName => $"{FirstName} {LastName}".Trim();
    public int Age => BirthDate.HasValue ? DateTime.Now.Year - BirthDate.Value.Year : 0;
}
