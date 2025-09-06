namespace DAL.Entities;

public class LeaveType : BaseEntity
{
    public string Name { get; set; } = string.Empty; // Annual, Sick, Maternity, etc.
    public string Description { get; set; } = string.Empty;
    public int MaxDaysPerYear { get; set; } = 0; // 0 = unlimited
    public bool RequiresApproval { get; set; } = true;
    public bool RequiresDocument { get; set; } = false; // For sick leave, medical reports
    public bool IsPaid { get; set; } = true;
    public bool CanCarryOver { get; set; } = false; // Can unused days carry to next year
    public int MaxCarryOverDays { get; set; } = 0;
    public string Color { get; set; } = "#007bff"; // For calendar display
    public int NotificationDays { get; set; } = 2; // Days before to notify manager
    
    // Navigation Properties
    public virtual ICollection<Leave> Leaves { get; set; } = new List<Leave>();
    public virtual ICollection<LeaveBalance> LeaveBalances { get; set; } = new List<LeaveBalance>();
}
