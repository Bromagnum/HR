namespace DAL.Entities;

public class Leave : BaseEntity
{
    public int PersonId { get; set; }
    public int LeaveTypeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalDays { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string? DocumentPath { get; set; } // For medical certificates, etc.
    
    // Status Management
    public LeaveStatus Status { get; set; } = LeaveStatus.Pending;
    public DateTime RequestDate { get; set; } = DateTime.Now;
    public bool IsUrgent { get; set; } = false;
    
    // Approval Workflow
    public int? ApprovedById { get; set; } // Manager who approved/rejected
    public DateTime? ApprovedAt { get; set; }
    public string? ApprovalNotes { get; set; }
    public string? RejectionReason { get; set; }
    
    // Emergency Contact (for longer leaves)
    public string? EmergencyContact { get; set; }
    public string? EmergencyPhone { get; set; }
    
    // Handover Information
    public string? HandoverNotes { get; set; }
    public int? HandoverToPersonId { get; set; } // Who will handle responsibilities
    
    // Navigation Properties
    public virtual Person Person { get; set; } = null!;
    public virtual LeaveType LeaveType { get; set; } = null!;
    public virtual Person? ApprovedBy { get; set; }
    public virtual Person? HandoverToPerson { get; set; }
}

public enum LeaveStatus
{
    Pending = 0,      // Waiting for approval
    Approved = 1,     // Approved by manager
    Rejected = 2,     // Rejected by manager
    Cancelled = 3,    // Cancelled by employee
    InProgress = 4,   // Currently on leave
    Completed = 5     // Leave finished
}
