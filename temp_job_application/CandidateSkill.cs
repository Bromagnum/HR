using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public enum SkillLevel
{
    Beginner = 1,
    Intermediate = 2,
    Advanced = 3,
    Expert = 4
}

public enum SkillCategory
{
    Technical = 1,
    Software = 2,
    Language = 3,
    SoftSkill = 4,
    Management = 5,
    Design = 6,
    Other = 7
}

public class CandidateSkill : BaseEntity
{
    [Required]
    public int CandidateId { get; set; }
    public virtual Candidate Candidate { get; set; } = null!;
    
    [Required]
    [StringLength(100)]
    public string SkillName { get; set; } = string.Empty;
    
    [Required]
    public SkillLevel Level { get; set; } = SkillLevel.Intermediate;
    
    [Required]
    public SkillCategory Category { get; set; } = SkillCategory.Technical;
    
    [Range(0, 20)]
    public int? YearsOfExperience { get; set; }
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    [StringLength(100)]
    public string? CertificationName { get; set; }
    
    public DateTime? CertificationDate { get; set; }
    
    public DateTime? CertificationExpiry { get; set; }
    
    [StringLength(200)]
    public string? CertificationUrl { get; set; }
    
    public bool IsVerified { get; set; } = false;
    
    public DateTime? LastUsed { get; set; }
    
    // Computed Properties
    public string LevelText => Level switch
    {
        SkillLevel.Beginner => "Başlangıç",
        SkillLevel.Intermediate => "Orta",
        SkillLevel.Advanced => "İleri",
        SkillLevel.Expert => "Uzman",
        _ => "Bilinmiyor"
    };
    
    public string CategoryText => Category switch
    {
        SkillCategory.Technical => "Teknik",
        SkillCategory.Software => "Yazılım",
        SkillCategory.Language => "Dil",
        SkillCategory.SoftSkill => "Kişisel Beceri",
        SkillCategory.Management => "Yönetim",
        SkillCategory.Design => "Tasarım",
        SkillCategory.Other => "Diğer",
        _ => "Bilinmiyor"
    };
    
    public bool IsCertificationValid => CertificationExpiry == null || CertificationExpiry > DateTime.Now;
}
