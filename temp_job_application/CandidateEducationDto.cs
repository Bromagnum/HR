using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs;

public class CandidateEducationDto
{
    public int Id { get; set; }
    public int CandidateId { get; set; }
    public string SchoolName { get; set; } = string.Empty;
    public string Degree { get; set; } = string.Empty;
    public string FieldOfStudy { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsOngoing { get; set; }
    public decimal? GPA { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string? Honors { get; set; }
    public string? RelevantCoursework { get; set; }
    public string Duration { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}

public class CandidateEducationCreateDto
{
    public int CandidateId { get; set; }
    
    [Required(ErrorMessage = "Okul adı zorunludur")]
    [StringLength(100, ErrorMessage = "Okul adı en fazla 100 karakter olabilir")]
    public string SchoolName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Derece zorunludur")]
    [StringLength(100, ErrorMessage = "Derece en fazla 100 karakter olabilir")]
    public string Degree { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Bölüm zorunludur")]
    [StringLength(100, ErrorMessage = "Bölüm en fazla 100 karakter olabilir")]
    public string FieldOfStudy { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Başlangıç tarihi zorunludur")]
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public bool IsOngoing { get; set; }
    
    [Range(0, 4, ErrorMessage = "GPA 0-4 arasında olmalıdır")]
    public decimal? GPA { get; set; }
    
    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    public string? Description { get; set; }
    
    [StringLength(100)]
    public string? Location { get; set; }
    
    [StringLength(100)]
    public string? Honors { get; set; }
    
    [StringLength(1000)]
    public string? RelevantCoursework { get; set; }
}

public class CandidateEducationUpdateDto : CandidateEducationCreateDto
{
    public int Id { get; set; }
}
