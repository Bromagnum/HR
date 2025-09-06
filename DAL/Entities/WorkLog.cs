using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class WorkLog : BaseEntity
    {
        [Required]
        public int PersonId { get; set; }
        public virtual Person Person { get; set; } = null!;

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public TimeSpan? BreakStartTime { get; set; }

        public TimeSpan? BreakEndTime { get; set; }

        [Range(0, 24)]
        public decimal BreakDurationMinutes { get; set; } = 0;

        [Range(0, 24)]
        public decimal TotalHours { get; set; } = 0;

        [Range(0, 24)]
        public decimal RegularHours { get; set; } = 0;

        [Range(0, 24)]
        public decimal OvertimeHours { get; set; } = 0;

        [StringLength(20)]
        public string Status { get; set; } = "Active"; // Active, Completed, Approved, Rejected

        [StringLength(20)]
        public string WorkType { get; set; } = "Office"; // Office, Remote, Field, Meeting

        [StringLength(500)]
        public string? Notes { get; set; }

        [StringLength(1000)]
        public string? TasksCompleted { get; set; }

        // Location tracking (optional)
        [StringLength(200)]
        public string? Location { get; set; }

        // Approval workflow
        public int? ApprovedById { get; set; }
        public virtual Person? ApprovedBy { get; set; }

        public DateTime? ApprovedAt { get; set; }

        [StringLength(500)]
        public string? ApprovalNotes { get; set; }

        // Auto-calculated properties
        public bool IsLateArrival { get; set; } = false;
        public bool IsEarlyDeparture { get; set; } = false;
        public bool IsOvertime { get; set; } = false;

        // Weekend/Holiday flags
        public bool IsWeekend { get; set; } = false;
        public bool IsHoliday { get; set; } = false;

        // IP tracking for security
        [StringLength(50)]
        public string? CheckInIP { get; set; }

        [StringLength(50)]
        public string? CheckOutIP { get; set; }
    }
}
