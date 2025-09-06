namespace DAL.Entities;

public class LeaveBalance : BaseEntity
{
    public int PersonId { get; set; }
    public int LeaveTypeId { get; set; }
    public int Year { get; set; } = DateTime.Now.Year;
    
    // Balance Tracking
    public decimal AllocatedDays { get; set; } = 0; // Total days allocated for the year
    public decimal UsedDays { get; set; } = 0; // Days already taken
    public decimal PendingDays { get; set; } = 0; // Days in pending requests
    public decimal CarriedOverDays { get; set; } = 0; // Days carried from previous year
    
    // Calculated Properties (can be computed or set manually)
    public decimal AvailableDays { get; set; }
    public decimal RemainingDays { get; set; }
    
    // Accrual Information (for monthly accrual systems)
    public decimal MonthlyAccrual { get; set; } = 0; // Days accrued per month
    public DateTime LastAccrualDate { get; set; } = DateTime.Now;
    public decimal AccruedToDate { get; set; } = 0; // Total accrued so far this year
    
    // Adjustment tracking
    public decimal ManualAdjustment { get; set; } = 0; // HR adjustments
    public string? AdjustmentReason { get; set; }
    public DateTime? AdjustmentDate { get; set; }
    // Temporarily commented out to avoid FK constraint issues
    // public int? AdjustedById { get; set; } // HR person who made adjustment
    
    // Navigation Properties
    public virtual Person Person { get; set; } = null!;
    public virtual LeaveType LeaveType { get; set; } = null!;
    // public virtual Person? AdjustedBy { get; set; }
}
