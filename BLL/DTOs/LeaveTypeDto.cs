namespace BLL.DTOs;

public class LeaveTypeListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int MaxDaysPerYear { get; set; }
    public bool RequiresApproval { get; set; }
    public bool RequiresDocument { get; set; }
    public bool IsPaid { get; set; }
    public bool CanCarryOver { get; set; }
    public int MaxCarryOverDays { get; set; }
    public string Color { get; set; } = string.Empty;
    public int NotificationDays { get; set; }
    public bool IsActive { get; set; }
    public int TotalLeaves { get; set; } // Total leaves of this type
    public int ActiveLeaves { get; set; } // Currently active leaves
}

public class LeaveTypeDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int MaxDaysPerYear { get; set; }
    public bool RequiresApproval { get; set; }
    public bool RequiresDocument { get; set; }
    public bool IsPaid { get; set; }
    public bool CanCarryOver { get; set; }
    public int MaxCarryOverDays { get; set; }
    public string Color { get; set; } = string.Empty;
    public int NotificationDays { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Statistics
    public int TotalLeaves { get; set; }
    public int PendingLeaves { get; set; }
    public int ApprovedLeaves { get; set; }
    public decimal TotalDaysUsed { get; set; }
    
    // Related Data
    public List<LeaveListDto> RecentLeaves { get; set; } = new();
    public List<LeaveBalanceListDto> Balances { get; set; } = new();
}

public class LeaveTypeCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int MaxDaysPerYear { get; set; } = 0;
    public bool RequiresApproval { get; set; } = true;
    public bool RequiresDocument { get; set; } = false;
    public bool IsPaid { get; set; } = true;
    public bool CanCarryOver { get; set; } = false;
    public int MaxCarryOverDays { get; set; } = 0;
    public string Color { get; set; } = "#007bff";
    public int NotificationDays { get; set; } = 2;
}

public class LeaveTypeUpdateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int MaxDaysPerYear { get; set; }
    public bool RequiresApproval { get; set; }
    public bool RequiresDocument { get; set; }
    public bool IsPaid { get; set; }
    public bool CanCarryOver { get; set; }
    public int MaxCarryOverDays { get; set; }
    public string Color { get; set; } = string.Empty;
    public int NotificationDays { get; set; }
    public bool IsActive { get; set; } = true;
}
