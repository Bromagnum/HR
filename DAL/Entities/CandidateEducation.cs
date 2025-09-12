using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class CandidateEducation : BaseEntity
{
    [Required]
    public int CandidateId { get; set; }
    public virtual Candidate Candidate { get; set; } = null!;
    
    [Required]
    [StringLength(100)]
    public string SchoolName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string Degree { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string FieldOfStudy { get; set; } = string.Empty;
    
    [Required]
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public bool IsOngoing { get; set; } = false;
    
    [Range(0, 4)]
    public decimal? GPA { get; set; }
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    [StringLength(100)]
    public string? Location { get; set; }
    
    [StringLength(100)]
    public string? Honors { get; set; }
    
    [StringLength(1000)]
    public string? RelevantCoursework { get; set; }
    
    // Computed Properties
    public string Duration
    {
        get
        {
            var end = EndDate ?? DateTime.Now;
            var years = end.Year - StartDate.Year;
            return years > 0 ? $"{years} yıl" : "1 yıldan az";
        }
    }
}
