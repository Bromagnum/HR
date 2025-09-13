using System.ComponentModel.DataAnnotations;
using DAL.Entities;

namespace BLL.DTOs;

public class CandidateSkillDto
{
    public int Id { get; set; }
    public int CandidateId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public SkillLevel Level { get; set; }
    public string LevelText { get; set; } = string.Empty;
    public SkillCategory Category { get; set; }
    public string CategoryText { get; set; } = string.Empty;
    public int? YearsOfExperience { get; set; }
    public string? Description { get; set; }
    public string? CertificationName { get; set; }
    public DateTime? CertificationDate { get; set; }
    public DateTime? CertificationExpiry { get; set; }
    public string? CertificationUrl { get; set; }
    public bool IsVerified { get; set; }
    public DateTime? LastUsed { get; set; }
    public bool IsCertificationValid { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}

public class CandidateSkillCreateDto
{
    public int CandidateId { get; set; }
    
    [Required(ErrorMessage = "Beceri adı zorunludur")]
    [StringLength(100, ErrorMessage = "Beceri adı en fazla 100 karakter olabilir")]
    public string SkillName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Seviye seçilmelidir")]
    public SkillLevel Level { get; set; } = SkillLevel.Intermediate;
    
    [Required(ErrorMessage = "Kategori seçilmelidir")]
    public SkillCategory Category { get; set; } = SkillCategory.Technical;
    
    [Range(0, 20, ErrorMessage = "Deneyim yılı 0-20 arasında olmalıdır")]
    public int? YearsOfExperience { get; set; }
    
    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    public string? Description { get; set; }
    
    [StringLength(100)]
    public string? CertificationName { get; set; }
    
    public DateTime? CertificationDate { get; set; }
    
    public DateTime? CertificationExpiry { get; set; }
    
    [StringLength(200)]
    [Url(ErrorMessage = "Geçerli bir URL giriniz")]
    public string? CertificationUrl { get; set; }
    
    public bool IsVerified { get; set; }
    
    public DateTime? LastUsed { get; set; }
}

public class CandidateSkillUpdateDto : CandidateSkillCreateDto
{
    public int Id { get; set; }
}

public class SkillStatisticsDto
{
    public string SkillName { get; set; } = string.Empty;
    public int CandidateCount { get; set; }
    public Dictionary<string, int> LevelDistribution { get; set; } = new();
    public Dictionary<string, int> CategoryDistribution { get; set; } = new();
    public decimal AverageExperience { get; set; }
}
