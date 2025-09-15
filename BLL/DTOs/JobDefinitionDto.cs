using DAL.Entities;

namespace BLL.DTOs;

public class JobDefinitionListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int PositionId { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public EducationLevel MinEducationLevel { get; set; }
    public int MinRequiredExperience { get; set; }
    public bool IsApproved { get; set; }
    public string Version { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? ApprovedByName { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public int RequiredQualificationCount { get; set; }
    public int MatchingResultsCount { get; set; }
}

public class JobDefinitionDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int PositionId { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string DetailedDescription { get; set; } = string.Empty;
    public string? MainResponsibilities { get; set; }
    public string? SecondaryResponsibilities { get; set; }
    public int MinRequiredExperience { get; set; }
    public int? PreferredExperience { get; set; }
    public EducationLevel MinEducationLevel { get; set; }
    public EducationLevel? PreferredEducationLevel { get; set; }
    public string? RequiredCertifications { get; set; }
    public string? PreferredCertifications { get; set; }
    public string? TechnicalSkills { get; set; }
    public string? SoftSkills { get; set; }
    public string? Languages { get; set; }
    public int? TravelRequirement { get; set; }
    public bool RemoteWorkAllowed { get; set; }
    public string? PhysicalRequirements { get; set; }
    public string? WorkingConditions { get; set; }
    public string? CareerPath { get; set; }
    public string? PerformanceMetrics { get; set; }
    public bool IsApproved { get; set; }
    public string? ApprovedByName { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string Version { get; set; } = string.Empty;
    public int? PreviousVersionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<JobDefinitionQualificationDto> RequiredQualifications { get; set; } = new();
    public List<JobRequiredSkillDto> RequiredSkills { get; set; } = new();
}

public class JobDefinitionCreateDto
{
    public string Title { get; set; } = string.Empty;
    public int PositionId { get; set; }
    public string DetailedDescription { get; set; } = string.Empty;
    public string? MainResponsibilities { get; set; }
    public string? SecondaryResponsibilities { get; set; }
    public int MinRequiredExperience { get; set; }
    public int? PreferredExperience { get; set; }
    public EducationLevel MinEducationLevel { get; set; }
    public EducationLevel? PreferredEducationLevel { get; set; }
    public string? RequiredCertifications { get; set; }
    public string? PreferredCertifications { get; set; }
    public string? TechnicalSkills { get; set; }
    public string? SoftSkills { get; set; }
    public string? Languages { get; set; }
    public int? TravelRequirement { get; set; }
    public bool RemoteWorkAllowed { get; set; }
    public string? PhysicalRequirements { get; set; }
    public string? WorkingConditions { get; set; }
    public string? CareerPath { get; set; }
    public string? PerformanceMetrics { get; set; }
    public List<JobDefinitionQualificationCreateDto> RequiredQualifications { get; set; } = new();
    public List<JobRequiredSkillCreateDto> RequiredSkills { get; set; } = new();
}

public class JobDefinitionUpdateDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string DetailedDescription { get; set; } = string.Empty;
    public string? MainResponsibilities { get; set; }
    public string? SecondaryResponsibilities { get; set; }
    public int MinRequiredExperience { get; set; }
    public int? PreferredExperience { get; set; }
    public EducationLevel MinEducationLevel { get; set; }
    public EducationLevel? PreferredEducationLevel { get; set; }
    public string? RequiredCertifications { get; set; }
    public string? PreferredCertifications { get; set; }
    public string? TechnicalSkills { get; set; }
    public string? SoftSkills { get; set; }
    public string? Languages { get; set; }
    public int? TravelRequirement { get; set; }
    public bool RemoteWorkAllowed { get; set; }
    public string? PhysicalRequirements { get; set; }
    public string? WorkingConditions { get; set; }
    public string? CareerPath { get; set; }
    public string? PerformanceMetrics { get; set; }
    public List<JobDefinitionQualificationCreateDto> RequiredQualifications { get; set; } = new();
    public List<JobRequiredSkillCreateDto> RequiredSkills { get; set; } = new();
}

public class JobDefinitionQualificationDto
{
    public int Id { get; set; }
    public string QualificationName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public QualificationImportance Importance { get; set; }
    public int? MinScore { get; set; }
    public int? MinExperience { get; set; }
    public string? Description { get; set; }
    public int Weight { get; set; }
}

public class JobDefinitionQualificationCreateDto
{
    public string QualificationName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public QualificationImportance Importance { get; set; }
    public int? MinScore { get; set; }
    public int? MinExperience { get; set; }
    public string? Description { get; set; }
    public int Weight { get; set; } = 10;
}

public class JobDefinitionFilterDto
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
    public int PageSize { get; set; } = 10;
}
