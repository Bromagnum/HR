namespace DAL.Entities;

public class UserLoginLog : BaseEntity
{
    public int UserId { get; set; }
    public DateTime LoginTime { get; set; } = DateTime.UtcNow;
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string? DeviceInfo { get; set; }
    public string? Location { get; set; }
    public bool IsSuccessful { get; set; } = true;
    public string? FailureReason { get; set; }
    public DateTime? LogoutTime { get; set; }
    
    // Navigation Properties
    public ApplicationUser User { get; set; } = null!;
}
