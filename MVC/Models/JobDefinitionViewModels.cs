using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.Entities;

namespace MVC.Models;

// Job Definition ViewModels
public class JobDefinitionListViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int PositionId { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public bool IsApproved { get; set; }
    public string ApprovedByName { get; set; } = string.Empty;
    public DateTime? ApprovedAt { get; set; }
    public int RequiredQualificationCount { get; set; }
    public int MatchingResultsCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Computed properties
    public string StatusText => IsApproved ? "Onaylandı" : "Onay Bekliyor";
    public string StatusClass => IsApproved ? "text-success" : "text-warning";
}

public class JobDefinitionDetailViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int PositionId { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public bool IsApproved { get; set; }
    public int? ApprovedById { get; set; }
    public string ApprovedByName { get; set; } = string.Empty;
    public DateTime? ApprovedAt { get; set; }
    public int? PreviousVersionId { get; set; }
    
    // Requirements
    public int MinRequiredExperience { get; set; }
    public int? PreferredExperience { get; set; }
    public EducationLevel MinEducationLevel { get; set; }
    public EducationLevel? PreferredEducationLevel { get; set; }
    public string? RequiredCertifications { get; set; }
    public string? PreferredCertifications { get; set; }
    public string? RequiredLanguages { get; set; }
    public string? PreferredLanguages { get; set; }
    public bool RequiresDriverLicense { get; set; }
    public int? TravelRequirement { get; set; }
    public EmploymentType EmploymentType { get; set; }
    public bool IsRemoteWorkAllowed { get; set; }
    
    // Additional Info
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public string? Benefits { get; set; }
    public string? WorkingHours { get; set; }
    public string? TeamStructure { get; set; }
    public string? ReportingStructure { get; set; }
    public string? KeyPerformanceIndicators { get; set; }
    public string? CareerDevelopmentPath { get; set; }
    public string? Notes { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Related data
    public List<JobDefinitionQualificationViewModel> RequiredQualifications { get; set; } = new();
    public List<QualificationMatchingResultViewModel> MatchingResults { get; set; } = new();
    
    // Computed properties
    public string StatusText => IsApproved ? "Onaylandı" : "Onay Bekliyor";
    public string StatusClass => IsApproved ? "text-success" : "text-warning";
    public string EducationLevelText => MinEducationLevel.ToString();
    public string EmploymentTypeText => EmploymentType.ToString();
}

public class JobDefinitionCreateViewModel
{
    [Required(ErrorMessage = "Başlık zorunludur")]
    [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Açıklama zorunludur")]
    [StringLength(4000, ErrorMessage = "Açıklama en fazla 4000 karakter olabilir")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Pozisyon seçimi zorunludur")]
    public int PositionId { get; set; }
    
    [Range(0, 50, ErrorMessage = "Minimum deneyim 0-50 yıl arasında olmalıdır")]
    public int MinRequiredExperience { get; set; }
    
    [Range(0, 50, ErrorMessage = "Tercih edilen deneyim 0-50 yıl arasında olmalıdır")]
    public int? PreferredExperience { get; set; }
    
    [Required(ErrorMessage = "Minimum eğitim seviyesi zorunludur")]
    public EducationLevel MinEducationLevel { get; set; }
    
    public EducationLevel? PreferredEducationLevel { get; set; }
    
    [StringLength(1000, ErrorMessage = "Gerekli sertifikalar en fazla 1000 karakter olabilir")]
    public string? RequiredCertifications { get; set; }
    
    [StringLength(1000, ErrorMessage = "Tercih edilen sertifikalar en fazla 1000 karakter olabilir")]
    public string? PreferredCertifications { get; set; }
    
    [StringLength(500, ErrorMessage = "Gerekli diller en fazla 500 karakter olabilir")]
    public string? RequiredLanguages { get; set; }
    
    [StringLength(500, ErrorMessage = "Tercih edilen diller en fazla 500 karakter olabilir")]
    public string? PreferredLanguages { get; set; }
    
    public bool RequiresDriverLicense { get; set; }
    
    [Range(0, 100, ErrorMessage = "Seyahat gereksinimi 0-100 arasında olmalıdır")]
    public int? TravelRequirement { get; set; }
    
    [Required(ErrorMessage = "İstihdam türü zorunludur")]
    public EmploymentType EmploymentType { get; set; }
    
    public bool IsRemoteWorkAllowed { get; set; }
    
    [Range(0, 999999.99, ErrorMessage = "Minimum maaş geçerli bir değer olmalıdır")]
    public decimal? MinSalary { get; set; }
    
    [Range(0, 999999.99, ErrorMessage = "Maksimum maaş geçerli bir değer olmalıdır")]
    public decimal? MaxSalary { get; set; }
    
    [StringLength(2000, ErrorMessage = "Yan haklar en fazla 2000 karakter olabilir")]
    public string? Benefits { get; set; }
    
    [StringLength(500, ErrorMessage = "Çalışma saatleri en fazla 500 karakter olabilir")]
    public string? WorkingHours { get; set; }
    
    [StringLength(1000, ErrorMessage = "Takım yapısı en fazla 1000 karakter olabilir")]
    public string? TeamStructure { get; set; }
    
    [StringLength(1000, ErrorMessage = "Raporlama yapısı en fazla 1000 karakter olabilir")]
    public string? ReportingStructure { get; set; }
    
    [StringLength(2000, ErrorMessage = "KPI'lar en fazla 2000 karakter olabilir")]
    public string? KeyPerformanceIndicators { get; set; }
    
    [StringLength(1000, ErrorMessage = "Kariyer gelişim yolu en fazla 1000 karakter olabilir")]
    public string? CareerDevelopmentPath { get; set; }
    
    [StringLength(2000, ErrorMessage = "Notlar en fazla 2000 karakter olabilir")]
    public string? Notes { get; set; }
    
    // Required qualifications and skills (can be added after creation)
    public List<JobDefinitionQualificationCreateViewModel> RequiredQualifications { get; set; } = new();
    public List<JobRequiredSkillCreateViewModel> RequiredSkills { get; set; } = new();
}

public class JobDefinitionEditViewModel : JobDefinitionCreateViewModel
{
    public int Id { get; set; }
    public string Version { get; set; } = string.Empty;
    public bool IsApproved { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class JobDefinitionFilterViewModel
{
    public string? Title { get; set; }
    public int? PositionId { get; set; }
    public int? DepartmentId { get; set; }
    public bool? IsApproved { get; set; }
    public EducationLevel? MinEducationLevel { get; set; }
    public string? Version { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

// Job Definition Qualification ViewModels
public class JobDefinitionQualificationViewModel
{
    public int Id { get; set; }
    public int JobDefinitionId { get; set; }
    public string Category { get; set; } = string.Empty;
    public string QualificationName { get; set; } = string.Empty;
    public QualificationImportance Importance { get; set; }
    public string ImportanceText => Importance.ToString();
    public int? MinScore { get; set; }
    public int Weight { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class JobDefinitionQualificationCreateViewModel
{
    [Required(ErrorMessage = "Kategori zorunludur")]
    [StringLength(100, ErrorMessage = "Kategori en fazla 100 karakter olabilir")]
    public string Category { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Nitelik adı zorunludur")]
    [StringLength(200, ErrorMessage = "Nitelik adı en fazla 200 karakter olabilir")]
    public string QualificationName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Önem derecesi zorunludur")]
    public QualificationImportance Importance { get; set; }
    
    [Range(0, 100, ErrorMessage = "Minimum puan 0-100 arasında olmalıdır")]
    public int? MinScore { get; set; }
    
    [Range(1, 100, ErrorMessage = "Ağırlık 1-100 arasında olmalıdır")]
    public int Weight { get; set; } = 10;
    
    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    public string? Description { get; set; }
}

// Job Required Skill ViewModels
public class JobRequiredSkillCreateViewModel
{
    [Required(ErrorMessage = "Beceri şablonu seçimi zorunludur")]
    public int SkillTemplateId { get; set; }
    
    [Required(ErrorMessage = "Önem derecesi zorunludur")]
    public QualificationImportance Importance { get; set; }
    
    [Range(1, 10, ErrorMessage = "Minimum seviye 1-10 arasında olmalıdır")]
    public int MinLevel { get; set; } = 1;
    
    [Range(1, 10, ErrorMessage = "Tercih edilen seviye 1-10 arasında olmalıdır")]
    public int? PreferredLevel { get; set; }
    
    [Range(0, 50, ErrorMessage = "Minimum deneyim yılı 0-50 arasında olmalıdır")]
    public int? MinExperienceYears { get; set; }
    
    public bool RequiresCertification { get; set; }
    
    [StringLength(500, ErrorMessage = "Tercih edilen sertifikalar en fazla 500 karakter olabilir")]
    public string? PreferredCertifications { get; set; }
    
    [Range(1, 100, ErrorMessage = "Ağırlık 1-100 arasında olmalıdır")]
    public int Weight { get; set; } = 10;
    
    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    public string? Description { get; set; }
}

// Qualification Matching Result ViewModels
public class QualificationMatchingResultViewModel
{
    public int Id { get; set; }
    public int JobDefinitionId { get; set; }
    public string JobDefinitionTitle { get; set; } = string.Empty;
    public int PersonId { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string PersonEmail { get; set; } = string.Empty;
    public decimal OverallMatchPercentage { get; set; }
    public decimal RequiredSkillsMatch { get; set; }
    public decimal PreferredSkillsMatch { get; set; }
    public decimal ExperienceMatch { get; set; }
    public decimal EducationMatch { get; set; }
    public decimal CertificationMatch { get; set; }
    public MatchingStatus Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public string? MissingRequirements { get; set; }
    public string? Recommendations { get; set; }
    public DateTime CalculatedAt { get; set; }
    public int? ReviewedById { get; set; }
    public string? ReviewedByName { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? ReviewNotes { get; set; }
    
    // Computed properties
    public string MatchPercentageText => $"{OverallMatchPercentage:F1}%";
    public string MatchClass => OverallMatchPercentage >= 80 ? "text-success" : 
                                OverallMatchPercentage >= 50 ? "text-warning" : "text-danger";
    public string StatusClass => Status switch
    {
        MatchingStatus.Matched => "text-success",
        MatchingStatus.PartialMatch => "text-warning",
        MatchingStatus.NoMatch => "text-danger",
        _ => "text-muted"
    };
}

// Job Definition Matching ViewModels
public class JobDefinitionMatchingViewModel
{
    public int JobDefinitionId { get; set; }
    public List<int>? SelectedPersonIds { get; set; }
    public bool RecalculateExisting { get; set; }
    public List<PersonListViewModel> AvailablePersons { get; set; } = new();
}

public class JobDefinitionTopMatchesViewModel
{
    public int JobDefinitionId { get; set; }
    public string JobDefinitionTitle { get; set; } = string.Empty;
    public List<QualificationMatchingResultViewModel> TopMatches { get; set; } = new();
}

// Summary ViewModels
public class JobDefinitionSummaryViewModel
{
    public int TotalDefinitions { get; set; }
    public int ApprovedDefinitions { get; set; }
    public int PendingApprovalDefinitions { get; set; }
    public int DefinitionsWithMatches { get; set; }
    public Dictionary<string, int> DefinitionsByDepartment { get; set; } = new();
    public Dictionary<string, int> DefinitionsByPosition { get; set; } = new();
    
    // Computed properties
    public decimal ApprovalRate => TotalDefinitions > 0 ? Math.Round((decimal)ApprovedDefinitions / TotalDefinitions * 100, 1) : 0;
    public decimal MatchingCompletionRate => TotalDefinitions > 0 ? Math.Round((decimal)DefinitionsWithMatches / TotalDefinitions * 100, 1) : 0;
}

// Helper ViewModels
// PersonListViewModel is already defined in PersonViewModel.cs
