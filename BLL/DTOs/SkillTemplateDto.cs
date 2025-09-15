using DAL.Entities;

namespace BLL.DTOs;

public class SkillTemplateListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public SkillType Type { get; set; }
    public string TypeText { get; set; } = string.Empty;
    public int MaxLevel { get; set; }
    public bool IsVerifiable { get; set; }
    public bool RequiresCertification { get; set; }
    public int UsageCount { get; set; }
    public DateTime? LastUsedAt { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class SkillTemplateDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Description { get; set; }
    public SkillType Type { get; set; }
    public string TypeText { get; set; } = string.Empty;
    public int MaxLevel { get; set; }
    public string? LevelDescriptions { get; set; }
    public bool IsVerifiable { get; set; }
    public string? VerificationMethod { get; set; }
    public bool RequiresCertification { get; set; }
    public string? Keywords { get; set; }
    public string? RelatedSkills { get; set; }
    public int UsageCount { get; set; }
    public DateTime? LastUsedAt { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class SkillTemplateCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Description { get; set; }
    public SkillType Type { get; set; }
    public int MaxLevel { get; set; } = 5;
    public string? LevelDescriptions { get; set; }
    public bool IsVerifiable { get; set; }
    public string? VerificationMethod { get; set; }
    public bool RequiresCertification { get; set; }
    public string? Keywords { get; set; }
    public string? RelatedSkills { get; set; }
}

public class SkillTemplateUpdateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Description { get; set; }
    public SkillType Type { get; set; }
    public int MaxLevel { get; set; }
    public string? LevelDescriptions { get; set; }
    public bool IsVerifiable { get; set; }
    public string? VerificationMethod { get; set; }
    public bool RequiresCertification { get; set; }
    public string? Keywords { get; set; }
    public string? RelatedSkills { get; set; }
}

public class SkillTemplateFilterDto
{
    public string? Name { get; set; }
    public string? Category { get; set; }
    public SkillType? Type { get; set; }
    public bool? IsVerifiable { get; set; }
    public bool? RequiresCertification { get; set; }
    public bool? IsActive { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
