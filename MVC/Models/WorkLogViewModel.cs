using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Models
{
    public class WorkLogListViewModel
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

    public class WorkLogDetailViewModel
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

    public class WorkLogCreateViewModel
    {
        [Required(ErrorMessage = "Personel seçimi gereklidir")]
        [Display(Name = "Personel")]
        public int PersonId { get; set; }

        [Required(ErrorMessage = "Tarih gereklidir")]
        [Display(Name = "Tarih")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Başlangıç saati gereklidir")]
        [Display(Name = "Başlangıç Saati")]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; } = new TimeSpan(9, 0, 0);

        [Display(Name = "Bitiş Saati")]
        [DataType(DataType.Time)]
        public TimeSpan? EndTime { get; set; }

        [Display(Name = "Mola Başlangıç")]
        [DataType(DataType.Time)]
        public TimeSpan? BreakStartTime { get; set; }

        [Display(Name = "Mola Bitiş")]
        [DataType(DataType.Time)]
        public TimeSpan? BreakEndTime { get; set; }

        [Range(0, 480, ErrorMessage = "Mola süresi 0-480 dakika arasında olmalıdır")]
        [Display(Name = "Mola Süresi (Dakika)")]
        public decimal BreakDurationMinutes { get; set; } = 60;

        [Required(ErrorMessage = "Çalışma türü gereklidir")]
        [Display(Name = "Çalışma Türü")]
        public string WorkType { get; set; } = "Office";

        [Display(Name = "Notlar")]
        [StringLength(500, ErrorMessage = "Notlar en fazla 500 karakter olabilir")]
        [DataType(DataType.MultilineText)]
        public string? Notes { get; set; }

        [Display(Name = "Tamamlanan Görevler")]
        [StringLength(1000, ErrorMessage = "Tamamlanan görevler en fazla 1000 karakter olabilir")]
        [DataType(DataType.MultilineText)]
        public string? TasksCompleted { get; set; }

        [Display(Name = "Lokasyon")]
        [StringLength(200, ErrorMessage = "Lokasyon en fazla 200 karakter olabilir")]
        public string? Location { get; set; }

        [Display(Name = "Hafta Sonu")]
        public bool IsWeekend { get; set; } = false;

        [Display(Name = "Tatil Günü")]
        public bool IsHoliday { get; set; } = false;

        // For dropdown lists
        public SelectList? PersonSelectList { get; set; }
        public SelectList? WorkTypeSelectList { get; set; }
    }

    public class WorkLogUpdateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Personel seçimi gereklidir")]
        [Display(Name = "Personel")]
        public int PersonId { get; set; }

        [Required(ErrorMessage = "Tarih gereklidir")]
        [Display(Name = "Tarih")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Başlangıç saati gereklidir")]
        [Display(Name = "Başlangıç Saati")]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Display(Name = "Bitiş Saati")]
        [DataType(DataType.Time)]
        public TimeSpan? EndTime { get; set; }

        [Display(Name = "Mola Başlangıç")]
        [DataType(DataType.Time)]
        public TimeSpan? BreakStartTime { get; set; }

        [Display(Name = "Mola Bitiş")]
        [DataType(DataType.Time)]
        public TimeSpan? BreakEndTime { get; set; }

        [Range(0, 480, ErrorMessage = "Mola süresi 0-480 dakika arasında olmalıdır")]
        [Display(Name = "Mola Süresi (Dakika)")]
        public decimal BreakDurationMinutes { get; set; }

        [Required(ErrorMessage = "Çalışma türü gereklidir")]
        [Display(Name = "Çalışma Türü")]
        public string WorkType { get; set; } = "Office";

        [Display(Name = "Notlar")]
        [StringLength(500, ErrorMessage = "Notlar en fazla 500 karakter olabilir")]
        [DataType(DataType.MultilineText)]
        public string? Notes { get; set; }

        [Display(Name = "Tamamlanan Görevler")]
        [StringLength(1000, ErrorMessage = "Tamamlanan görevler en fazla 1000 karakter olabilir")]
        [DataType(DataType.MultilineText)]
        public string? TasksCompleted { get; set; }

        [Display(Name = "Lokasyon")]
        [StringLength(200, ErrorMessage = "Lokasyon en fazla 200 karakter olabilir")]
        public string? Location { get; set; }

        [Display(Name = "Durum")]
        [StringLength(20, ErrorMessage = "Durum en fazla 20 karakter olabilir")]
        public string Status { get; set; } = "Active";

        [Display(Name = "Hafta Sonu")]
        public bool IsWeekend { get; set; } = false;

        [Display(Name = "Tatil Günü")]
        public bool IsHoliday { get; set; } = false;

        // For dropdown lists
        public SelectList? PersonSelectList { get; set; }
        public SelectList? WorkTypeSelectList { get; set; }
        public SelectList? StatusSelectList { get; set; }
    }

    public class WorkLogTimeSheetViewModel
    {
        public int PersonId { get; set; }
        public string PersonName { get; set; } = string.Empty;
        public string? DepartmentName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<WorkLogListViewModel> WorkLogs { get; set; } = new();
        public decimal TotalHours { get; set; }
        public decimal TotalRegularHours { get; set; }
        public decimal TotalOvertimeHours { get; set; }
        public int TotalWorkDays { get; set; }
        public int LateArrivals { get; set; }
        public int EarlyDepartures { get; set; }
    }

    public class WorkLogCheckInViewModel
    {
        [Required(ErrorMessage = "Personel seçimi gereklidir")]
        [Display(Name = "Personel")]
        public int PersonId { get; set; }

        [Required(ErrorMessage = "Başlangıç saati gereklidir")]
        [Display(Name = "Başlangıç Saati")]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; } = DateTime.Now.TimeOfDay;

        [Required(ErrorMessage = "Çalışma türü gereklidir")]
        [Display(Name = "Çalışma Türü")]
        public string WorkType { get; set; } = "Office";

        [Display(Name = "Lokasyon")]
        [StringLength(200, ErrorMessage = "Lokasyon en fazla 200 karakter olabilir")]
        public string? Location { get; set; }

        [Display(Name = "Notlar")]
        [StringLength(500, ErrorMessage = "Notlar en fazla 500 karakter olabilir")]
        [DataType(DataType.MultilineText)]
        public string? Notes { get; set; }

        // For dropdown lists
        public SelectList? PersonSelectList { get; set; }
        public SelectList? WorkTypeSelectList { get; set; }
    }

    public class WorkLogCheckOutViewModel
    {
        public int Id { get; set; }
        public string PersonName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Bitiş saati gereklidir")]
        [Display(Name = "Bitiş Saati")]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; } = DateTime.Now.TimeOfDay;

        [Display(Name = "Tamamlanan Görevler")]
        [StringLength(1000, ErrorMessage = "Tamamlanan görevler en fazla 1000 karakter olabilir")]
        [DataType(DataType.MultilineText)]
        public string? TasksCompleted { get; set; }

        [Display(Name = "Notlar")]
        [StringLength(500, ErrorMessage = "Notlar en fazla 500 karakter olabilir")]
        [DataType(DataType.MultilineText)]
        public string? Notes { get; set; }
    }

    public class WorkLogApprovalViewModel
    {
        public int Id { get; set; }
        public string PersonName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public decimal TotalHours { get; set; }
        public string WorkType { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public string? TasksCompleted { get; set; }

        [Required(ErrorMessage = "Onay durumu gereklidir")]
        [Display(Name = "Onay Durumu")]
        public string Status { get; set; } = string.Empty; // Approved, Rejected

        [Display(Name = "Onay Notları")]
        [StringLength(500, ErrorMessage = "Onay notları en fazla 500 karakter olabilir")]
        [DataType(DataType.MultilineText)]
        public string? ApprovalNotes { get; set; }

        public int ApprovedById { get; set; }

        // For dropdown lists
        public SelectList? StatusSelectList { get; set; }
    }

    public class WorkLogFilterViewModel
    {
        [Display(Name = "Personel")]
        public int? PersonId { get; set; }

        [Display(Name = "Başlangıç Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Bitiş Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Çalışma Türü")]
        public string? WorkType { get; set; }

        [Display(Name = "Durum")]
        public string? Status { get; set; }

        [Display(Name = "Sadece Mesai Kayıtları")]
        public bool OnlyOvertime { get; set; } = false;

        [Display(Name = "Sadece Geç Gelenler")]
        public bool OnlyLateArrivals { get; set; } = false;

        [Display(Name = "Sadece Erken Gidenler")]
        public bool OnlyEarlyDepartures { get; set; } = false;

        // For dropdown lists
        public SelectList? PersonSelectList { get; set; }
        public SelectList? WorkTypeSelectList { get; set; }
        public SelectList? StatusSelectList { get; set; }
    }
}
