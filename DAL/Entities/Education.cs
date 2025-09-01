using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class Education : BaseEntity
{
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

    // Navigation Properties
    [Required]
    public int PersonId { get; set; }
    public virtual Person Person { get; set; } = null!;
}

