namespace BLL.DTOs;

public class LeaveBalanceListDto
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public int LeaveTypeId { get; set; }
    public string LeaveTypeName { get; set; } = string.Empty;
    public string LeaveTypeColor { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal AllocatedDays { get; set; }
    public decimal UsedDays { get; set; }
    public decimal PendingDays { get; set; }
    public decimal CarriedOverDays { get; set; }
    public decimal AvailableDays { get; set; }
    public decimal RemainingDays { get; set; }
    public decimal AccruedToDate { get; set; }
    public decimal ManualAdjustment { get; set; }
    public string? AdjustmentReason { get; set; }
    public bool IsActive { get; set; }
    
    // Calculated fields
    public decimal UsagePercentage => AllocatedDays > 0 ? (UsedDays / AllocatedDays) * 100 : 0;
    public bool IsOverused => UsedDays > AllocatedDays + CarriedOverDays;
    public bool NeedsAttention => AvailableDays < 0 || IsOverused;
}

public class LeaveBalanceDetailDto
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public int LeaveTypeId { get; set; }
    public string LeaveTypeName { get; set; } = string.Empty;
    public string LeaveTypeColor { get; set; } = string.Empty;
    public int Year { get; set; }
    
    // Balance Information
    public decimal AllocatedDays { get; set; }
    public decimal UsedDays { get; set; }
    public decimal PendingDays { get; set; }
    public decimal CarriedOverDays { get; set; }
    public decimal AvailableDays { get; set; }
    public decimal RemainingDays { get; set; }
    
    // Accrual Information
    public decimal MonthlyAccrual { get; set; }
    public DateTime LastAccrualDate { get; set; }
    public decimal AccruedToDate { get; set; }
    
    // Adjustment Information
    public decimal ManualAdjustment { get; set; }
    public string? AdjustmentReason { get; set; }
    public DateTime? AdjustmentDate { get; set; }
    public string? AdjustedByName { get; set; }
    
    // Timestamps
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
    
    // Related Data
    public List<LeaveListDto> Leaves { get; set; } = new();
    public List<LeaveBalanceHistoryDto> History { get; set; } = new();
}

public class LeaveBalanceCreateDto
{
    public int PersonId { get; set; }
    public int LeaveTypeId { get; set; }
    public int Year { get; set; } = DateTime.Now.Year;
    public decimal AllocatedDays { get; set; }
    public decimal CarriedOverDays { get; set; } = 0;
    public decimal MonthlyAccrual { get; set; } = 0;
    public string? AdjustmentReason { get; set; }
}

public class LeaveBalanceUpdateDto
{
    public int Id { get; set; }
    public decimal AllocatedDays { get; set; }
    public decimal CarriedOverDays { get; set; }
    public decimal MonthlyAccrual { get; set; }
    public decimal ManualAdjustment { get; set; }
    public string? AdjustmentReason { get; set; }
    public int? AdjustedById { get; set; }
}

public class LeaveBalanceAdjustmentDto
{
    public int PersonId { get; set; }
    public int LeaveTypeId { get; set; }
    public int Year { get; set; }
    public decimal AdjustmentDays { get; set; }
    public string Reason { get; set; } = string.Empty;
    public int AdjustedById { get; set; }
}

public class LeaveBalanceHistoryDto
{
    public DateTime Date { get; set; }
    public string Action { get; set; } = string.Empty;
    public decimal PreviousValue { get; set; }
    public decimal NewValue { get; set; }
    public decimal ChangeDays { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? AdjustedByName { get; set; }
}

public class LeaveBalanceSummaryDto
{
    public int PersonId { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public int Year { get; set; }
    public List<LeaveBalanceListDto> Balances { get; set; } = new();
    public decimal TotalAllocated { get; set; }
    public decimal TotalUsed { get; set; }
    public decimal TotalPending { get; set; }
    public decimal TotalAvailable { get; set; }
    public decimal OverallUsagePercentage => TotalAllocated > 0 ? (TotalUsed / TotalAllocated) * 100 : 0;
}
