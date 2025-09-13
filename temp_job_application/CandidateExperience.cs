using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class CandidateExperience : BaseEntity
{
    [Required]
    public int CandidateId { get; set; }
    public virtual Candidate Candidate { get; set; } = null!;
    
    [Required]
    [StringLength(100)]
    public string CompanyName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string JobTitle { get; set; } = string.Empty;
    
    [Required]
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public bool IsCurrentJob { get; set; } = false;
    
    [StringLength(1000)]
    public string? JobDescription { get; set; }
    
    [StringLength(1000)]
    public string? Achievements { get; set; }
    
    [StringLength(500)]
    public string? TechnologiesUsed { get; set; }
    
    [StringLength(100)]
    public string? Location { get; set; }
    
    [StringLength(50)]
    public string? EmploymentType { get; set; } // Full-time, Part-time, Contract, Freelance
    
    [StringLength(100)]
    public string? Department { get; set; }
    
    [StringLength(100)]
    public string? ReportingTo { get; set; }
    
    [StringLength(100)]
    public string? TeamSize { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal? Salary { get; set; }
    
    [StringLength(500)]
    public string? ReasonForLeaving { get; set; }
    
    // Computed Properties
    public string Duration
    {
        get
        {
            var end = EndDate ?? DateTime.Now;
            var totalMonths = (end.Year - StartDate.Year) * 12 + end.Month - StartDate.Month;
            var years = totalMonths / 12;
            var months = totalMonths % 12;
            
            if (years > 0 && months > 0)
                return $"{years} yıl {months} ay";
            else if (years > 0)
                return $"{years} yıl";
            else if (months > 0)
                return $"{months} ay";
            else
                return "1 aydan az";
        }
    }
    
    public int DurationInMonths
    {
        get
        {
            var end = EndDate ?? DateTime.Now;
            return (end.Year - StartDate.Year) * 12 + end.Month - StartDate.Month;
        }
    }
}
