using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class WorkLogListDto
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string PersonName { get; set; } = string.Empty;
        public string? DepartmentName { get; set; }
        public string? EmployeeNumber { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public decimal TotalHours { get; set; }
        public decimal RegularHours { get; set; }
        public decimal OvertimeHours { get; set; }
        public string Status { get; set; } = string.Empty;
        public string WorkType { get; set; } = string.Empty;
        public bool IsLateArrival { get; set; }
        public bool IsEarlyDeparture { get; set; }
        public bool IsOvertime { get; set; }
        public bool IsActive { get; set; }
    }

    public class WorkLogDetailDto
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string PersonName { get; set; } = string.Empty;
        public string? DepartmentName { get; set; }
        public string? EmployeeNumber { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public TimeSpan? BreakStartTime { get; set; }
        public TimeSpan? BreakEndTime { get; set; }
        public decimal BreakDurationMinutes { get; set; }
        public decimal TotalHours { get; set; }
        public decimal RegularHours { get; set; }
        public decimal OvertimeHours { get; set; }
        public string Status { get; set; } = string.Empty;
        public string WorkType { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public string? TasksCompleted { get; set; }
        public string? Location { get; set; }
        public int? ApprovedById { get; set; }
        public string? ApprovedByName { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string? ApprovalNotes { get; set; }
        public bool IsLateArrival { get; set; }
        public bool IsEarlyDeparture { get; set; }
        public bool IsOvertime { get; set; }
        public bool IsWeekend { get; set; }
        public bool IsHoliday { get; set; }
        public string? CheckInIP { get; set; }
        public string? CheckOutIP { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class WorkLogCreateDto
    {
        [Required(ErrorMessage = "Personel seçimi gereklidir")]
        public int PersonId { get; set; }

        [Required(ErrorMessage = "Tarih gereklidir")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Başlangıç saati gereklidir")]
        public TimeSpan StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public TimeSpan? BreakStartTime { get; set; }

        public TimeSpan? BreakEndTime { get; set; }

        [Range(0, 480, ErrorMessage = "Mola süresi 0-480 dakika arasında olmalıdır")]
        public decimal BreakDurationMinutes { get; set; } = 0;

        [Required(ErrorMessage = "Çalışma türü gereklidir")]
        [StringLength(20, ErrorMessage = "Çalışma türü en fazla 20 karakter olabilir")]
        public string WorkType { get; set; } = "Office";

        [StringLength(500, ErrorMessage = "Notlar en fazla 500 karakter olabilir")]
        public string? Notes { get; set; }

        [StringLength(1000, ErrorMessage = "Tamamlanan görevler en fazla 1000 karakter olabilir")]
        public string? TasksCompleted { get; set; }

        [StringLength(200, ErrorMessage = "Lokasyon en fazla 200 karakter olabilir")]
        public string? Location { get; set; }

        public bool IsWeekend { get; set; } = false;
        public bool IsHoliday { get; set; } = false;
    }

    public class WorkLogUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Personel seçimi gereklidir")]
        public int PersonId { get; set; }

        [Required(ErrorMessage = "Tarih gereklidir")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Başlangıç saati gereklidir")]
        public TimeSpan StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public TimeSpan? BreakStartTime { get; set; }

        public TimeSpan? BreakEndTime { get; set; }

        [Range(0, 480, ErrorMessage = "Mola süresi 0-480 dakika arasında olmalıdır")]
        public decimal BreakDurationMinutes { get; set; } = 0;

        [Required(ErrorMessage = "Çalışma türü gereklidir")]
        [StringLength(20, ErrorMessage = "Çalışma türü en fazla 20 karakter olabilir")]
        public string WorkType { get; set; } = "Office";

        [StringLength(500, ErrorMessage = "Notlar en fazla 500 karakter olabilir")]
        public string? Notes { get; set; }

        [StringLength(1000, ErrorMessage = "Tamamlanan görevler en fazla 1000 karakter olabilir")]
        public string? TasksCompleted { get; set; }

        [StringLength(200, ErrorMessage = "Lokasyon en fazla 200 karakter olabilir")]
        public string? Location { get; set; }

        [StringLength(20, ErrorMessage = "Durum en fazla 20 karakter olabilir")]
        public string Status { get; set; } = "Active";

        public bool IsWeekend { get; set; } = false;
        public bool IsHoliday { get; set; } = false;
    }

    public class WorkLogTimeSheetDto
    {
        public int PersonId { get; set; }
        public string PersonName { get; set; } = string.Empty;
        public string? DepartmentName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<WorkLogListDto> WorkLogs { get; set; } = new();
        public decimal TotalHours { get; set; }
        public decimal TotalRegularHours { get; set; }
        public decimal TotalOvertimeHours { get; set; }
        public int TotalWorkDays { get; set; }
        public int LateArrivals { get; set; }
        public int EarlyDepartures { get; set; }
    }

    public class WorkLogCheckInDto
    {
        [Required(ErrorMessage = "Personel seçimi gereklidir")]
        public int PersonId { get; set; }

        [Required(ErrorMessage = "Başlangıç saati gereklidir")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Çalışma türü gereklidir")]
        public string WorkType { get; set; } = "Office";

        [StringLength(200, ErrorMessage = "Lokasyon en fazla 200 karakter olabilir")]
        public string? Location { get; set; }

        [StringLength(500, ErrorMessage = "Notlar en fazla 500 karakter olabilir")]
        public string? Notes { get; set; }
    }

    public class WorkLogCheckOutDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bitiş saati gereklidir")]
        public TimeSpan EndTime { get; set; }

        [StringLength(1000, ErrorMessage = "Tamamlanan görevler en fazla 1000 karakter olabilir")]
        public string? TasksCompleted { get; set; }

        [StringLength(500, ErrorMessage = "Notlar en fazla 500 karakter olabilir")]
        public string? Notes { get; set; }
    }

    public class WorkLogApprovalDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Onay durumu gereklidir")]
        public string Status { get; set; } = string.Empty; // Approved, Rejected

        [StringLength(500, ErrorMessage = "Onay notları en fazla 500 karakter olabilir")]
        public string? ApprovalNotes { get; set; }

        public int ApprovedById { get; set; }
    }
}
