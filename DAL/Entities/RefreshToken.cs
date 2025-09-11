namespace DAL.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
    public bool IsRevoked { get; set; }
    public string? ReplacedByToken { get; set; }
    public string? ReasonRevoked { get; set; }
    public string CreatedByIp { get; set; } = string.Empty;
    public DateTime? RevokedAt { get; set; }
    public string? RevokedByIp { get; set; }
    
    // Foreign Keys
    public int UserId { get; set; }
    
    // Navigation Properties
    public ApplicationUser User { get; set; } = null!;
    
    // Computed Properties
    public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
    public new bool IsActive => !IsRevoked && !IsExpired;
}
