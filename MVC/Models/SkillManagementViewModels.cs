using System.ComponentModel.DataAnnotations;
using DAL.Entities;

namespace MVC.Models;

// Skill Analytics ViewModels
public class SkillAnalyticsViewModel
{
    public int TotalSkillTemplates { get; set; }
    public int TotalPersonSkills { get; set; }
    public int ActiveSkillCategories { get; set; }
    public Dictionary<string, int> SkillsByCategory { get; set; } = new();
    public Dictionary<string, int> SkillsByType { get; set; } = new();
    public List<TopSkillViewModel> TopSkills { get; set; } = new();
    public List<SkillGapViewModel> SkillGaps { get; set; } = new();
    public DateTime GeneratedAt { get; set; } = DateTime.Now;
}

public class TopSkillViewModel
{
    public string SkillName { get; set; } = string.Empty;
    public int PersonCount { get; set; }
    public decimal AverageLevel { get; set; }
    public string Category { get; set; } = string.Empty;
}

public class SkillGapViewModel
{
    public string SkillName { get; set; } = string.Empty;
    public int RequiredCount { get; set; }
    public int AvailableCount { get; set; }
    public int GapCount => RequiredCount - AvailableCount;
    public string Category { get; set; } = string.Empty;
}

// Skill Template ViewModels
public class SkillTemplateListViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public SkillType Type { get; set; }
    public string TypeText => Type.ToString();
    public int PersonCount { get; set; }
    public decimal AverageLevel { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SkillTemplateDetailViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public SkillType Type { get; set; }
    public string TypeText => Type.ToString();
    public int MaxLevel { get; set; }
    public bool RequiresCertification { get; set; }
    public string? CertificationBody { get; set; }
    public string? AssessmentCriteria { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Related data
    public List<PersonSkillListViewModel> PersonSkills { get; set; } = new();
    public List<JobRequiredSkillViewModel> RequiredByJobs { get; set; } = new();
    
    // Statistics
    public int TotalPersons { get; set; }
    public decimal AverageLevel { get; set; }
    public int ExpertCount { get; set; }
    public int BeginnerCount { get; set; }
}

public class SkillTemplateCreateViewModel
{
    [Required(ErrorMessage = "Beceri adı zorunludur")]
    [StringLength(100, ErrorMessage = "Beceri adı en fazla 100 karakter olabilir")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Açıklama zorunludur")]
    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Kategori zorunludur")]
    [StringLength(50, ErrorMessage = "Kategori en fazla 50 karakter olabilir")]
    public string Category { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Beceri türü zorunludur")]
    public SkillType Type { get; set; }
    
    [Range(1, 10, ErrorMessage = "Maksimum seviye 1-10 arasında olmalıdır")]
    public int MaxLevel { get; set; } = 5;
    
    public bool RequiresCertification { get; set; }
    
    [StringLength(100, ErrorMessage = "Sertifika kurumu en fazla 100 karakter olabilir")]
    public string? CertificationBody { get; set; }
    
    [StringLength(1000, ErrorMessage = "Değerlendirme kriterleri en fazla 1000 karakter olabilir")]
    public string? AssessmentCriteria { get; set; }
    
    public bool IsActive { get; set; } = true;
}

public class SkillTemplateFilterViewModel
{
    public string? Name { get; set; }
    public string? Category { get; set; }
    public SkillType? Type { get; set; }
    public bool? IsActive { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

// Person Skill ViewModels
public class PersonSkillListViewModel
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string PersonEmail { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public int SkillTemplateId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public string SkillCategory { get; set; } = string.Empty;
    public int Level { get; set; }
    public string LevelText { get; set; } = string.Empty;
    public int? YearsOfExperience { get; set; }
    public bool HasCertification { get; set; }
    public string? CertificationDetails { get; set; }
    public DateTime? LastAssessmentDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PersonSkillFilterViewModel
{
    public int? PersonId { get; set; }
    public string? PersonName { get; set; }
    public int? SkillTemplateId { get; set; }
    public string? SkillCategory { get; set; }
    public int? MinLevel { get; set; }
    public int? MaxLevel { get; set; }
    public bool? HasCertification { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

// Job Required Skill ViewModels
public class JobRequiredSkillViewModel
{
    public int Id { get; set; }
    public int JobDefinitionId { get; set; }
    public string JobDefinitionTitle { get; set; } = string.Empty;
    public int SkillTemplateId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public QualificationImportance Importance { get; set; }
    public string ImportanceText => Importance.ToString();
    public int MinLevel { get; set; }
    public int? PreferredLevel { get; set; }
    public int? MinExperienceYears { get; set; }
    public bool RequiresCertification { get; set; }
    public string? PreferredCertifications { get; set; }
    public int Weight { get; set; }
    public string? Description { get; set; }
}

// Additional ViewModels for AutoMapper compatibility
public class PersonSkillCreateViewModel
{
    [Required(ErrorMessage = "Personel seçimi zorunludur")]
    public int PersonId { get; set; }
    
    [Required(ErrorMessage = "Beceri seçimi zorunludur")]
    public int SkillTemplateId { get; set; }
    
    [Range(1, 10, ErrorMessage = "Seviye 1-10 arasında olmalıdır")]
    public int Level { get; set; }
    
    [Range(0, 50, ErrorMessage = "Deneyim yılı 0-50 arasında olmalıdır")]
    public int ExperienceYears { get; set; }
    
    public bool IsCertified { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime? CertificationDate { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime? CertificationExpiryDate { get; set; }
    
    public string? CertificationAuthority { get; set; }
    
    public string? CertificationNumber { get; set; }
    
    public bool IsSelfAssessed { get; set; }
    
    public string? Notes { get; set; }
}

public class PersonSkillUpdateViewModel
{
    public int Id { get; set; }
    
    [Range(1, 10, ErrorMessage = "Seviye 1-10 arasında olmalıdır")]
    public int Level { get; set; }
    
    [Range(0, 50, ErrorMessage = "Deneyim yılı 0-50 arasında olmalıdır")]
    public int ExperienceYears { get; set; }
    
    public bool IsCertified { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime? CertificationDate { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime? CertificationExpiryDate { get; set; }
    
    public string? CertificationAuthority { get; set; }
    
    public string? CertificationNumber { get; set; }
    
    public string? Notes { get; set; }
}
