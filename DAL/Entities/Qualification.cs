using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class Qualification : BaseEntity
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Category { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string IssuingAuthority { get; set; } = string.Empty;

    [StringLength(100)]
    public string? CredentialNumber { get; set; }

    [Required]
    public DateTime IssueDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public bool HasExpiration { get; set; } = false;

    [StringLength(100)]
    public string? Level { get; set; }

    [Range(0, 100)]
    public int? Score { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    [StringLength(500)]
    public string? AttachmentPath { get; set; }

    [StringLength(200)]
    public string? Location { get; set; }

    // Computed Properties
    public bool IsExpired => HasExpiration && ExpirationDate.HasValue && ExpirationDate.Value < DateTime.Now;
    public bool IsExpiringSoon => HasExpiration && ExpirationDate.HasValue && 
                                  ExpirationDate.Value > DateTime.Now && 
                                  ExpirationDate.Value <= DateTime.Now.AddDays(30);

    // Navigation Properties
    [Required]
    public int PersonId { get; set; }
    public virtual Person Person { get; set; } = null!;
}
