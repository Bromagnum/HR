using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs;

public class CandidateExperienceDto
{
    public int Id { get; set; }
    public int CandidateId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrentJob { get; set; }
    public string? JobDescription { get; set; }
    public string? Achievements { get; set; }
    public string? TechnologiesUsed { get; set; }
    public string? Location { get; set; }
    public string? EmploymentType { get; set; }
    public string? Department { get; set; }
    public string? ReportingTo { get; set; }
    public string? TeamSize { get; set; }
    public decimal? Salary { get; set; }
    public string? ReasonForLeaving { get; set; }
    public string Duration { get; set; } = string.Empty;
    public int DurationInMonths { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}

public class CandidateExperienceCreateDto
{
    public int CandidateId { get; set; }
    
    [Required(ErrorMessage = "Şirket adı zorunludur")]
    [StringLength(100, ErrorMessage = "Şirket adı en fazla 100 karakter olabilir")]
    public string CompanyName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Pozisyon zorunludur")]
    [StringLength(100, ErrorMessage = "Pozisyon en fazla 100 karakter olabilir")]
    public string JobTitle { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Başlangıç tarihi zorunludur")]
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public bool IsCurrentJob { get; set; }
    
    [StringLength(1000, ErrorMessage = "İş tanımı en fazla 1000 karakter olabilir")]
    public string? JobDescription { get; set; }
    
    [StringLength(1000, ErrorMessage = "Başarılar en fazla 1000 karakter olabilir")]
    public string? Achievements { get; set; }
    
    [StringLength(500)]
    public string? TechnologiesUsed { get; set; }
    
    [StringLength(100)]
    public string? Location { get; set; }
    
    [StringLength(50)]
    public string? EmploymentType { get; set; }
    
    [StringLength(100)]
    public string? Department { get; set; }
    
    [StringLength(100)]
    public string? ReportingTo { get; set; }
    
    [StringLength(100)]
    public string? TeamSize { get; set; }
    
    [Range(0, double.MaxValue, ErrorMessage = "Maaş pozitif olmalıdır")]
    public decimal? Salary { get; set; }
    
    [StringLength(500)]
    public string? ReasonForLeaving { get; set; }
}

public class CandidateExperienceUpdateDto : CandidateExperienceCreateDto
{
    public int Id { get; set; }
}
