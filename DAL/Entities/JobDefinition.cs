using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

/// <summary>
/// İş tanımı şablonu - pozisyonlar için detaylı gereksinimler
/// </summary>
public class JobDefinition : BaseEntity
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public int PositionId { get; set; }
    public virtual Position Position { get; set; } = null!;

    [Required]
    [StringLength(5000)]
    public string DetailedDescription { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? MainResponsibilities { get; set; }

    [StringLength(2000)]
    public string? SecondaryResponsibilities { get; set; }

    [StringLength(2000)]
    public string? RequiredSkills { get; set; }

    [StringLength(2000)]
    public string? PreferredSkills { get; set; }

    [Range(0, 50)]
    public int MinRequiredExperience { get; set; }

    [Range(0, 50)]
    public int? PreferredExperience { get; set; }

    [Required]
    public EducationLevel MinEducationLevel { get; set; } = EducationLevel.HighSchool;

    public EducationLevel? PreferredEducationLevel { get; set; }

    [StringLength(1000)]
    public string? RequiredCertifications { get; set; }

    [StringLength(1000)]
    public string? PreferredCertifications { get; set; }

    [StringLength(1000)]
    public string? TechnicalSkills { get; set; }

    [StringLength(1000)]
    public string? SoftSkills { get; set; }

    [StringLength(500)]
    public string? Languages { get; set; }

    [Range(0, 100)]
    public int? TravelRequirement { get; set; } // Percentage

    public bool RemoteWorkAllowed { get; set; } = false;

    [StringLength(1000)]
    public string? PhysicalRequirements { get; set; }

    [StringLength(1000)]
    public string? WorkingConditions { get; set; }

    [StringLength(1000)]
    public string? CareerPath { get; set; }

    [StringLength(1000)]
    public string? PerformanceMetrics { get; set; }

    // Approval workflow
    public bool IsApproved { get; set; } = false;
    public int? ApprovedById { get; set; }
    public virtual Person? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }

    // Version control
    [StringLength(50)]
    public string Version { get; set; } = "1.0";
    public int? PreviousVersionId { get; set; }
    public virtual JobDefinition? PreviousVersion { get; set; }

    // Navigation Properties
    public virtual ICollection<JobDefinitionQualification> RequiredQualifications { get; set; } = new List<JobDefinitionQualification>();
    public virtual ICollection<QualificationMatchingResult> MatchingResults { get; set; } = new List<QualificationMatchingResult>();
}

/// <summary>
/// İş tanımı için gerekli nitelikler
/// </summary>
public class JobDefinitionQualification : BaseEntity
{
    [Required]
    public int JobDefinitionId { get; set; }
    public virtual JobDefinition JobDefinition { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string QualificationName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Category { get; set; } = string.Empty;

    [Required]
    public QualificationImportance Importance { get; set; } = QualificationImportance.Required;

    [Range(0, 100)]
    public int? MinScore { get; set; }

    [Range(0, 50)]
    public int? MinExperience { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [Range(1, 100)]
    public int Weight { get; set; } = 10; // For matching algorithm
}

/// <summary>
/// Nitelik eşleştirme sonuçları
/// </summary>
public class QualificationMatchingResult : BaseEntity
{
    [Required]
    public int JobDefinitionId { get; set; }
    public virtual JobDefinition JobDefinition { get; set; } = null!;

    [Required]
    public int PersonId { get; set; }
    public virtual Person Person { get; set; } = null!;

    [Range(0, 100)]
    public decimal OverallMatchPercentage { get; set; }

    [Range(0, 100)]
    public decimal RequiredSkillsMatch { get; set; }

    [Range(0, 100)]
    public decimal PreferredSkillsMatch { get; set; }

    [Range(0, 100)]
    public decimal ExperienceMatch { get; set; }

    [Range(0, 100)]
    public decimal EducationMatch { get; set; }

    [Range(0, 100)]
    public decimal CertificationMatch { get; set; }

    public MatchingStatus Status { get; set; } = MatchingStatus.Pending;

    [StringLength(2000)]
    public string? MatchingDetails { get; set; }

    [StringLength(1000)]
    public string? MissingRequirements { get; set; }

    [StringLength(1000)]
    public string? Recommendations { get; set; }

    public DateTime CalculatedAt { get; set; } = DateTime.Now;

    public int? ReviewedById { get; set; }
    public virtual Person? ReviewedBy { get; set; }
    public DateTime? ReviewedAt { get; set; }

    [StringLength(1000)]
    public string? ReviewNotes { get; set; }
}

public enum QualificationImportance
{
    Required = 1,       // Zorunlu
    Preferred = 2,      // Tercih edilen
    Nice = 3           // Olması güzel
}

public enum MatchingStatus
{
    Pending = 1,        // Beklemede
    Matched = 2,        // Eşleşti
    PartialMatch = 3,   // Kısmi eşleşme
    NoMatch = 4,        // Eşleşmiyor
    Reviewed = 5        // İncelendi
}
