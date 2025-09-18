using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

/// <summary>
/// Beceri şablonları - standart beceri tanımları
/// </summary>
public class SkillTemplate : BaseEntity
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Category { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required]
    public SkillType Type { get; set; } = SkillType.Technical;

    [Range(1, 5)]
    public int MaxLevel { get; set; } = 5;

    [StringLength(500)]
    public string? LevelDescriptions { get; set; } // JSON array of level descriptions

    public bool IsVerifiable { get; set; } = false;

    [StringLength(200)]
    public string? VerificationMethod { get; set; }

    public bool RequiresCertification { get; set; } = false;

    [StringLength(1000)]
    public string? Keywords { get; set; }

    [StringLength(1000)]
    public string? RelatedSkills { get; set; }

    // Usage tracking
    public int UsageCount { get; set; } = 0;
    public DateTime? LastUsedAt { get; set; }

    // Navigation Properties
    public virtual ICollection<PersonSkill> PersonSkills { get; set; } = new List<PersonSkill>();
    public virtual ICollection<JobRequiredSkill> JobRequiredSkills { get; set; } = new List<JobRequiredSkill>();
}

/// <summary>
/// Kişi becerileri
/// </summary>
public class PersonSkill : BaseEntity
{
    [Required]
    public int PersonId { get; set; }
    public virtual Person Person { get; set; } = null!;

    [Required]
    public int SkillTemplateId { get; set; }
    public virtual SkillTemplate SkillTemplate { get; set; } = null!;

    [Range(1, 5)]
    public int Level { get; set; } = 1;

    [Range(0, 50)]
    public int? ExperienceYears { get; set; }

    [Range(0, 12)]
    public int? ExperienceMonths { get; set; }

    public bool IsCertified { get; set; } = false;

    [StringLength(200)]
    public string? CertificationName { get; set; }

    [StringLength(200)]
    public string? CertificationAuthority { get; set; }

    public DateTime? CertificationDate { get; set; }

    public DateTime? CertificationExpiry { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    [StringLength(1000)]
    public string? ProjectExamples { get; set; }

    public DateTime? LastUsed { get; set; }

    public bool IsEndorsed { get; set; } = false;

    public int? EndorsedById { get; set; }
    public virtual Person? EndorsedBy { get; set; }

    public DateTime? EndorsedAt { get; set; }

    [StringLength(500)]
    public string? EndorsementNotes { get; set; }

    // Self-assessment vs manager assessment
    public bool IsSelfAssessed { get; set; } = true;

    public int? AssessedById { get; set; }
    public virtual Person? AssessedBy { get; set; }

    public DateTime? AssessedAt { get; set; }

    // Computed Properties
    public decimal TotalExperience => (ExperienceYears ?? 0) + ((ExperienceMonths ?? 0) / 12.0m);
    public bool IsCertificationExpired => CertificationExpiry.HasValue && CertificationExpiry.Value < DateTime.Now;
    public bool IsCertificationExpiringSoon => CertificationExpiry.HasValue && 
                                               CertificationExpiry.Value > DateTime.Now && 
                                               CertificationExpiry.Value <= DateTime.Now.AddDays(90);

    // Navigation Properties
    public virtual ICollection<SkillAssessment> SkillAssessments { get; set; } = new List<SkillAssessment>();
}

/// <summary>
/// İş için gerekli beceriler
/// </summary>
public class JobRequiredSkill : BaseEntity
{
    [Required]
    public int JobDefinitionId { get; set; }
    public virtual JobDefinition JobDefinition { get; set; } = null!;

    [Required]
    public int SkillTemplateId { get; set; }
    public virtual SkillTemplate SkillTemplate { get; set; } = null!;

    [Required]
    public QualificationImportance Importance { get; set; } = QualificationImportance.Required;

    [Range(1, 5)]
    public int MinLevel { get; set; } = 1;

    [Range(1, 5)]
    public int? PreferredLevel { get; set; }

    [Range(0, 50)]
    public int? MinExperienceYears { get; set; }

    [Range(0, 50)]
    public int? PreferredExperienceYears { get; set; }

    public bool RequiresCertification { get; set; } = false;

    [StringLength(1000)]
    public string? SpecificRequirements { get; set; }

    [Range(1, 100)]
    public int Weight { get; set; } = 10; // For matching algorithm

    [StringLength(500)]
    public string? AssessmentCriteria { get; set; }
}

/// <summary>
/// Beceri değerlendirme sonuçları
/// </summary>
public class SkillAssessment : BaseEntity
{
    [Required]
    public int PersonSkillId { get; set; }
    public virtual PersonSkill PersonSkill { get; set; } = null!;

    [Required]
    public AssessmentType Type { get; set; }

    [Range(1, 5)]
    public int AssessedLevel { get; set; }

    [Range(0, 100)]
    public int? Score { get; set; }

    [StringLength(1000)]
    public string? Feedback { get; set; }

    [StringLength(1000)]
    public string? ImprovementAreas { get; set; }

    [StringLength(1000)]
    public string? Recommendations { get; set; }

    public DateTime AssessmentDate { get; set; } = DateTime.Now;

    [Required]
    public int AssessorId { get; set; }
    public virtual Person Assessor { get; set; } = null!;

    [StringLength(200)]
    public string? AssessmentMethod { get; set; }

    public bool IsValid { get; set; } = true;

    public DateTime? ValidUntil { get; set; }
}

public enum SkillType
{
    Technical = 1,      // Teknik
    Soft = 2,           // Yumuşak beceri
    Language = 3,       // Dil
    Leadership = 4,     // Liderlik
    Management = 5,     // Yönetim
    Communication = 6,  // İletişim
    Creative = 7,       // Yaratıcı
    Analytical = 8      // Analitik
}

public enum AssessmentType
{
    SelfAssessment = 1,     // Öz değerlendirme
    ManagerAssessment = 2,  // Yönetici değerlendirmesi
    PeerAssessment = 3,     // Meslektaş değerlendirmesi
    ExternalAssessment = 4, // Dış değerlendirme
    TestAssessment = 5      // Test değerlendirmesi
}
