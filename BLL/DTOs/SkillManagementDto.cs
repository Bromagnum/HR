using DAL.Entities;

namespace BLL.DTOs;

// Person Skill DTOs
public class PersonSkillDto
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string PersonEmail { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public int SkillTemplateId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public string SkillCategory { get; set; } = string.Empty;
    public SkillType SkillType { get; set; }
    public int Level { get; set; }
    public string LevelText { get; set; } = string.Empty;
    public int ExperienceYears { get; set; }
    public string? TotalExperience { get; set; }
    public bool IsCertified { get; set; }
    public DateTime? CertificationDate { get; set; }
    public DateTime? CertificationExpiryDate { get; set; }
    public bool IsCertificationExpired { get; set; }
    public bool IsCertificationExpiringSoon { get; set; }
    public string? CertificationAuthority { get; set; }
    public string? CertificationNumber { get; set; }
    public bool IsEndorsed { get; set; }
    public int? EndorsedById { get; set; }
    public string? EndorsedByName { get; set; }
    public DateTime? EndorsedAt { get; set; }
    public string? EndorsementNotes { get; set; }
    public bool IsSelfAssessed { get; set; }
    public int? AssessedById { get; set; }
    public string? AssessedByName { get; set; }
    public DateTime? AssessedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class PersonSkillCreateDto
{
    public int PersonId { get; set; }
    public int SkillTemplateId { get; set; }
    public int Level { get; set; }
    public int ExperienceYears { get; set; }
    public bool IsCertified { get; set; }
    public DateTime? CertificationDate { get; set; }
    public DateTime? CertificationExpiryDate { get; set; }
    public string? CertificationAuthority { get; set; }
    public string? CertificationNumber { get; set; }
    public bool IsSelfAssessed { get; set; }
    public string? Notes { get; set; }
}

public class PersonSkillUpdateDto
{
    public int Id { get; set; }
    public int Level { get; set; }
    public int ExperienceYears { get; set; }
    public bool IsCertified { get; set; }
    public DateTime? CertificationDate { get; set; }
    public DateTime? CertificationExpiryDate { get; set; }
    public string? CertificationAuthority { get; set; }
    public string? CertificationNumber { get; set; }
    public string? Notes { get; set; }
}

public class PersonSkillFilterDto
{
    public int PersonId { get; set; }
    public SkillType? SkillType { get; set; }
    public string? Category { get; set; }
    public int? MinLevel { get; set; }
    public int? MaxLevel { get; set; }
    public bool? IsCertified { get; set; }
    public bool? IsEndorsed { get; set; }
    public int? MinExperienceYears { get; set; }
    public int? MaxExperienceYears { get; set; }
}

// Job Required Skill DTOs
public class JobRequiredSkillDto
{
    public int Id { get; set; }
    public int JobDefinitionId { get; set; }
    public int SkillTemplateId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public string SkillCategory { get; set; } = string.Empty;
    public SkillType SkillType { get; set; }
    public QualificationImportance Importance { get; set; }
    public string ImportanceText { get; set; } = string.Empty;
    public int MinLevel { get; set; }
    public int? PreferredLevel { get; set; }
    public int? MinExperienceYears { get; set; }
    public bool RequiresCertification { get; set; }
    public string? PreferredCertifications { get; set; }
    public int Weight { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class JobRequiredSkillCreateDto
{
    public int? Id { get; set; }
    public int SkillTemplateId { get; set; }
    public QualificationImportance Importance { get; set; }
    public int MinLevel { get; set; }
    public int? PreferredLevel { get; set; }
    public int? MinExperienceYears { get; set; }
    public bool RequiresCertification { get; set; }
    public string? PreferredCertifications { get; set; }
    public int Weight { get; set; } = 10;
    public string? Description { get; set; }
}

// Skill Assessment DTOs
public class SkillAssessmentDto
{
    public int Id { get; set; }
    public int PersonSkillId { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string SkillName { get; set; } = string.Empty;
    public AssessmentType Type { get; set; }
    public string TypeText { get; set; } = string.Empty;
    public int? Score { get; set; }
    public int? MaxScore { get; set; } = 10;
    public string? Notes { get; set; }
    public string? Feedback { get; set; }
    public DateTime AssessmentDate { get; set; }
    public int AssessorId { get; set; }
    public string AssessorName { get; set; } = string.Empty;
    public bool IsValid { get; set; }
    public DateTime? ValidUntil { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class SkillAssessmentCreateDto
{
    public int PersonSkillId { get; set; }
    public AssessmentType Type { get; set; }
    public int? Score { get; set; }
    public int MaxScore { get; set; } = 10;
    public string? Notes { get; set; }
    public string? Feedback { get; set; }
    public int AssessorId { get; set; }
    public DateTime? ValidUntil { get; set; }
}

// Analytics DTOs
public class SkillAnalyticsDto
{
    public int TotalSkillTemplates { get; set; }
    public int TotalPersonSkills { get; set; }
    public int TotalAssessments { get; set; }
    public decimal AverageSkillLevel { get; set; }
    public int CertifiedSkillsCount { get; set; }
    public int EndorsedSkillsCount { get; set; }
    public Dictionary<string, int> SkillsByType { get; set; } = new();
    public Dictionary<string, int> TopSkillCategories { get; set; } = new();
    public Dictionary<string, int> AssessmentsByType { get; set; } = new();
}

public class PersonSkillSummaryDto
{
    public int PersonId { get; set; }
    public int TotalSkillCount { get; set; }
    public decimal AverageSkillLevel { get; set; }
    public int CertifiedSkillCount { get; set; }
    public int EndorsedSkillCount { get; set; }
    public int TotalAssessmentCount { get; set; }
    public DateTime? LatestAssessmentDate { get; set; }
    public Dictionary<string, int> SkillsByType { get; set; } = new();
    public Dictionary<string, int> SkillsByLevel { get; set; } = new();
    public List<PersonSkillSummaryItemDto> TopSkills { get; set; } = new();
}

public class PersonSkillSummaryItemDto
{
    public string SkillName { get; set; } = string.Empty;
    public int Level { get; set; }
    public int ExperienceYears { get; set; }
    public bool IsCertified { get; set; }
    public bool IsEndorsed { get; set; }
}

// Additional DTOs for matching and statistics
public class QualificationMatchingResultDto
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
}

public class QualificationMatchingResultCreateDto
{
    public int JobDefinitionId { get; set; }
    public int PersonId { get; set; }
    public int? ReviewedById { get; set; }
    public string? ReviewNotes { get; set; }
}

// Job Definition related DTOs for matching
public class JobDefinitionMatchingRequestDto
{
    public int JobDefinitionId { get; set; }
    public List<int>? PersonIds { get; set; } // If null, calculate for all persons
    public bool RecalculateExisting { get; set; } = false;
}

public class JobDefinitionSummaryDto
{
    public int TotalDefinitions { get; set; }
    public int ApprovedDefinitions { get; set; }
    public int PendingApprovalDefinitions { get; set; }
    public int DefinitionsWithMatches { get; set; }
    public Dictionary<string, int> DefinitionsByDepartment { get; set; } = new();
    public Dictionary<string, int> DefinitionsByPosition { get; set; } = new();
}

public class DepartmentJobDefinitionSummaryDto
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public int TotalDefinitions { get; set; }
    public int ApprovedDefinitions { get; set; }
    public int PendingDefinitions { get; set; }
    public int TotalMatches { get; set; }
    public int HighQualityMatches { get; set; } // >= 80%
    public decimal AverageMatchPercentage { get; set; }
    public List<JobDefinitionListDto> TopDefinitions { get; set; } = new();
}

public class SkillSummaryDto
{
    public int TotalSkillTemplates { get; set; }
    public int ActiveSkillTemplates { get; set; }
    public int TotalPersonSkills { get; set; }
    public int CertifiedSkills { get; set; }
    public int EndorsedSkills { get; set; }
    public int ExpiringCertifications { get; set; }
    public List<SkillCategorySummaryDto> CategorySummary { get; set; } = new();
    public List<SkillTypeSummaryDto> TypeSummary { get; set; } = new();
}

public class SkillCategorySummaryDto
{
    public string Category { get; set; } = string.Empty;
    public int TemplateCount { get; set; }
    public int PersonSkillCount { get; set; }
    public int CertifiedCount { get; set; }
    public decimal AverageLevel { get; set; }
}

public class SkillTypeSummaryDto
{
    public SkillType Type { get; set; }
    public string TypeText { get; set; } = string.Empty;
    public int TemplateCount { get; set; }
    public int PersonSkillCount { get; set; }
    public int CertifiedCount { get; set; }
    public decimal AverageLevel { get; set; }
}

