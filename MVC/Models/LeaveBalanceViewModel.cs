using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Models;

public class LeaveBalanceListViewModel
{
    public int Id { get; set; }
    
    public int PersonId { get; set; }
    
    [Display(Name = "Personel")]
    public string PersonName { get; set; } = string.Empty;
    
    [Display(Name = "Sicil No")]
    public string EmployeeNumber { get; set; } = string.Empty;
    
    [Display(Name = "Departman")]
    public string DepartmentName { get; set; } = string.Empty;
    
    [Display(Name = "İzin Türü")]
    public string LeaveTypeName { get; set; } = string.Empty;
    
    public string LeaveTypeColor { get; set; } = string.Empty;
    
    [Display(Name = "Yıl")]
    public int Year { get; set; }
    
    [Display(Name = "Tahsisli")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = false)]
    public decimal AllocatedDays { get; set; }
    
    [Display(Name = "Kullanılan")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = false)]
    public decimal UsedDays { get; set; }
    
    [Display(Name = "Bekleyen")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = false)]
    public decimal PendingDays { get; set; }
    
    [Display(Name = "Devir")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = false)]
    public decimal CarriedOverDays { get; set; }
    
    [Display(Name = "Kullanılabilir")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = false)]
    public decimal AvailableDays { get; set; }
    
    [Display(Name = "Kalan")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = false)]
    public decimal RemainingDays { get; set; }
    
    [Display(Name = "Tahakkuk")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = false)]
    public decimal AccruedToDate { get; set; }
    
    [Display(Name = "Manuel Düzeltme")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = false)]
    public decimal ManualAdjustment { get; set; }
    
    [Display(Name = "Düzeltme Gerekçesi")]
    public string? AdjustmentReason { get; set; }
    
    [Display(Name = "Aktif")]
    public bool IsActive { get; set; }
    
    // Calculated properties
    [Display(Name = "Kullanım %")]
    [DisplayFormat(DataFormatString = "{0:N1}%", ApplyFormatInEditMode = false)]
    public decimal UsagePercentage { get; set; }
    
    public bool IsOverused { get; set; }
    public bool NeedsAttention { get; set; }
    
    // For UI styling
    public string UsageBarClass => UsagePercentage switch
    {
        <= 25 => "bg-success",
        <= 50 => "bg-info",
        <= 75 => "bg-warning",
        <= 100 => "bg-danger",
        _ => "bg-dark"
    };
    
    public string StatusBadgeClass => NeedsAttention ? "bg-danger" : (IsOverused ? "bg-warning" : "bg-success");
}

public class LeaveBalanceDetailViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "Personel")]
    public string PersonName { get; set; } = string.Empty;
    
    [Display(Name = "Sicil No")]
    public string EmployeeNumber { get; set; } = string.Empty;
    
    [Display(Name = "Departman")]
    public string DepartmentName { get; set; } = string.Empty;
    
    [Display(Name = "İzin Türü")]
    public string LeaveTypeName { get; set; } = string.Empty;
    
    public string LeaveTypeColor { get; set; } = string.Empty;
    
    public bool LeaveTypeRequiresDocument { get; set; }
    
    [Display(Name = "Yıl")]
    public int Year { get; set; }
    
    // Balance Information
    [Display(Name = "Tahsisli Gün")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = false)]
    public decimal AllocatedDays { get; set; }
    
    [Display(Name = "Kullanılan Gün")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = false)]
    public decimal UsedDays { get; set; }
    
    [Display(Name = "Bekleyen Gün")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = false)]
    public decimal PendingDays { get; set; }
    
    [Display(Name = "Devir Gün")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = false)]
    public decimal CarriedOverDays { get; set; }
    
    [Display(Name = "Kullanılabilir Gün")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = false)]
    public decimal AvailableDays { get; set; }
    
    [Display(Name = "Kalan Gün")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = false)]
    public decimal RemainingDays { get; set; }
    
    // Accrual Information
    [Display(Name = "Aylık Tahakkuk")]
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
    public decimal MonthlyAccrual { get; set; }
    
    [Display(Name = "Son Tahakkuk Tarihi")]
    [DataType(DataType.Date)]
    public DateTime LastAccrualDate { get; set; }
    
    [Display(Name = "Bugüne Tahakkuk")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = false)]
    public decimal AccruedToDate { get; set; }
    
    // Adjustment Information
    [Display(Name = "Manuel Düzeltme")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = false)]
    public decimal ManualAdjustment { get; set; }
    
    [Display(Name = "Düzeltme Gerekçesi")]
    public string? AdjustmentReason { get; set; }
    
    [Display(Name = "Düzeltme Tarihi")]
    [DataType(DataType.DateTime)]
    public DateTime? AdjustmentDate { get; set; }
    
    [Display(Name = "Düzelten Kişi")]
    public string? AdjustedByName { get; set; }
    
    // Timestamps
    [Display(Name = "Oluşturulma")]
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }
    
    [Display(Name = "Güncellenme")]
    [DataType(DataType.DateTime)]
    public DateTime? UpdatedAt { get; set; }
    
    [Display(Name = "Son Hesaplama")]
    [DataType(DataType.DateTime)]
    public DateTime? LastCalculated { get; set; }
    
    [Display(Name = "Aktif")]
    public bool IsActive { get; set; }
}

public class LeaveBalanceCreateViewModel
{
    [Required(ErrorMessage = "Personel seçimi zorunludur.")]
    [Display(Name = "Personel*")]
    public int PersonId { get; set; }
    
    [Required(ErrorMessage = "İzin türü seçimi zorunludur.")]
    [Display(Name = "İzin Türü*")]
    public int LeaveTypeId { get; set; }
    
    [Required(ErrorMessage = "Yıl zorunludur.")]
    [Range(2020, 2030, ErrorMessage = "Yıl 2020-2030 arasında olmalıdır.")]
    [Display(Name = "Yıl*")]
    public int Year { get; set; } = DateTime.Now.Year;
    
    [Required(ErrorMessage = "Tahsisli gün sayısı zorunludur.")]
    [Range(0, 365, ErrorMessage = "Tahsisli gün sayısı 0-365 arasında olmalıdır.")]
    [Display(Name = "Tahsisli Gün*")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = true)]
    public decimal AllocatedDays { get; set; }
    
    [Range(0, 365, ErrorMessage = "Devir gün sayısı 0-365 arasında olmalıdır.")]
    [Display(Name = "Devir Gün")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = true)]
    public decimal CarriedOverDays { get; set; } = 0;
    
    [Range(0, 50, ErrorMessage = "Aylık tahakkuk 0-50 arasında olmalıdır.")]
    [Display(Name = "Aylık Tahakkuk")]
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
    public decimal MonthlyAccrual { get; set; } = 0;
    
    [StringLength(500, ErrorMessage = "Düzeltme gerekçesi en fazla 500 karakter olabilir.")]
    [Display(Name = "Düzeltme Gerekçesi")]
    public string? AdjustmentReason { get; set; }
    
    // For dropdown lists
    public IEnumerable<SelectListItem> Persons { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> LeaveTypes { get; set; } = new List<SelectListItem>();
}

public class LeaveBalanceEditViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "Personel")]
    public string PersonName { get; set; } = string.Empty;
    
    [Display(Name = "İzin Türü")]
    public string LeaveTypeName { get; set; } = string.Empty;
    
    [Display(Name = "Yıl")]
    public int Year { get; set; }
    
    [Required(ErrorMessage = "Tahsisli gün sayısı zorunludur.")]
    [Range(0, 365, ErrorMessage = "Tahsisli gün sayısı 0-365 arasında olmalıdır.")]
    [Display(Name = "Tahsisli Gün*")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = true)]
    public decimal AllocatedDays { get; set; }
    
    [Range(0, 365, ErrorMessage = "Devir gün sayısı 0-365 arasında olmalıdır.")]
    [Display(Name = "Devir Gün")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = true)]
    public decimal CarriedOverDays { get; set; }
    
    [Range(0, 50, ErrorMessage = "Aylık tahakkuk 0-50 arasında olmalıdır.")]
    [Display(Name = "Aylık Tahakkuk")]
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
    public decimal MonthlyAccrual { get; set; }
    
    [Range(-365, 365, ErrorMessage = "Manuel düzeltme -365 ile 365 arasında olmalıdır.")]
    [Display(Name = "Manuel Düzeltme")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = true)]
    public decimal ManualAdjustment { get; set; }
    
    [StringLength(500, ErrorMessage = "Düzeltme gerekçesi en fazla 500 karakter olabilir.")]
    [Display(Name = "Düzeltme Gerekçesi")]
    public string? AdjustmentReason { get; set; }
    
    // Read-only fields for context
    [Display(Name = "Kullanılan Gün")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = false)]
    public decimal UsedDays { get; set; }
    
    [Display(Name = "Bekleyen Gün")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = false)]
    public decimal PendingDays { get; set; }
}

public class LeaveBalanceAdjustmentViewModel
{
    [Required(ErrorMessage = "Personel seçimi zorunludur.")]
    [Display(Name = "Personel*")]
    public int PersonId { get; set; }
    
    [Required(ErrorMessage = "İzin türü seçimi zorunludur.")]
    [Display(Name = "İzin Türü*")]
    public int LeaveTypeId { get; set; }
    
    [Required(ErrorMessage = "Yıl zorunludur.")]
    [Range(2020, 2030, ErrorMessage = "Yıl 2020-2030 arasında olmalıdır.")]
    [Display(Name = "Yıl*")]
    public int Year { get; set; } = DateTime.Now.Year;
    
    [Required(ErrorMessage = "Düzeltme miktarı zorunludur.")]
    [Range(-365, 365, ErrorMessage = "Düzeltme miktarı -365 ile 365 arasında olmalıdır.")]
    [Display(Name = "Düzeltme Miktarı (Gün)*")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = true)]
    public decimal AdjustmentDays { get; set; }
    
    [Required(ErrorMessage = "Düzeltme türü zorunludur.")]
    [Display(Name = "Düzeltme Türü*")]
    public string AdjustmentType { get; set; } = "Manual";
    
    [Required(ErrorMessage = "Gerekçe zorunludur.")]
    [StringLength(500, ErrorMessage = "Gerekçe en fazla 500 karakter olabilir.")]
    [Display(Name = "Gerekçe*")]
    public string Reason { get; set; } = string.Empty;
    
    public int AdjustedById { get; set; }
    
    // For dropdown lists
    public IEnumerable<SelectListItem> Persons { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> LeaveTypes { get; set; } = new List<SelectListItem>();
    
    // For context display
    [Display(Name = "Mevcut Bakiye")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = false)]
    public decimal CurrentBalance { get; set; }
    
    [Display(Name = "Yeni Bakiye")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = false)]
    public decimal NewBalance => CurrentBalance + AdjustmentDays;
}

public class LeaveBalanceSummaryViewModel
{
    [Display(Name = "Personel")]
    public string PersonName { get; set; } = string.Empty;
    
    [Display(Name = "Departman")]
    public string DepartmentName { get; set; } = string.Empty;
    
    [Display(Name = "Yıl")]
    public int Year { get; set; }
    
    public List<LeaveBalanceListViewModel> Balances { get; set; } = new();
    
    [Display(Name = "Toplam Tahsisli")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = false)]
    public decimal TotalAllocated { get; set; }
    
    [Display(Name = "Toplam Kullanılan")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = false)]
    public decimal TotalUsed { get; set; }
    
    [Display(Name = "Toplam Bekleyen")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = false)]
    public decimal TotalPending { get; set; }
    
    [Display(Name = "Toplam Kullanılabilir")]
    [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = false)]
    public decimal TotalAvailable { get; set; }
    
    [Display(Name = "Genel Kullanım %")]
    [DisplayFormat(DataFormatString = "{0:N1}%", ApplyFormatInEditMode = false)]
    public decimal OverallUsagePercentage { get; set; }
}
