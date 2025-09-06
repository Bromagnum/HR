using System.ComponentModel.DataAnnotations;

namespace MVC.Models;

public class LeaveTypeListViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "İzin Türü")]
    public string Name { get; set; } = string.Empty;
    
    [Display(Name = "Açıklama")]
    public string Description { get; set; } = string.Empty;
    
    [Display(Name = "Maksimum Gün/Yıl")]
    public int MaxDaysPerYear { get; set; }
    
    [Display(Name = "Onay Gerekli")]
    public bool RequiresApproval { get; set; }
    
    [Display(Name = "Belge Gerekli")]
    public bool RequiresDocument { get; set; }
    
    [Display(Name = "Ücretli")]
    public bool IsPaid { get; set; }
    
    [Display(Name = "Devir Edilebilir")]
    public bool CanCarryOver { get; set; }
    
    [Display(Name = "Maks. Devir Gün")]
    public int MaxCarryOverDays { get; set; }
    
    [Display(Name = "Renk")]
    public string Color { get; set; } = string.Empty;
    
    [Display(Name = "Bildirim Gün")]
    public int NotificationDays { get; set; }
    
    [Display(Name = "Aktif")]
    public bool IsActive { get; set; }
    
    [Display(Name = "Toplam İzin")]
    public int TotalLeaves { get; set; }
    
    [Display(Name = "Aktif İzin")]
    public int ActiveLeaves { get; set; }
}

public class LeaveTypeDetailViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "İzin Türü")]
    public string Name { get; set; } = string.Empty;
    
    [Display(Name = "Açıklama")]
    public string Description { get; set; } = string.Empty;
    
    [Display(Name = "Maksimum Gün/Yıl")]
    public int MaxDaysPerYear { get; set; }
    
    [Display(Name = "Onay Gerekli")]
    public bool RequiresApproval { get; set; }
    
    [Display(Name = "Belge Gerekli")]
    public bool RequiresDocument { get; set; }
    
    [Display(Name = "Ücretli")]
    public bool IsPaid { get; set; }
    
    [Display(Name = "Devir Edilebilir")]
    public bool CanCarryOver { get; set; }
    
    [Display(Name = "Maksimum Devir Gün")]
    public int MaxCarryOverDays { get; set; }
    
    [Display(Name = "Renk")]
    public string Color { get; set; } = string.Empty;
    
    [Display(Name = "Bildirim Gün")]
    public int NotificationDays { get; set; }
    
    [Display(Name = "Aktif")]
    public bool IsActive { get; set; }
    
    [Display(Name = "Oluşturulma Tarihi")]
    public DateTime CreatedAt { get; set; }
    
    [Display(Name = "Güncellenme Tarihi")]
    public DateTime? UpdatedAt { get; set; }
    
    // Statistics
    [Display(Name = "Toplam İzin Sayısı")]
    public int TotalLeaves { get; set; }
    
    [Display(Name = "Bekleyen İzin")]
    public int PendingLeaves { get; set; }
    
    [Display(Name = "Onaylanan İzin")]
    public int ApprovedLeaves { get; set; }
    
    [Display(Name = "Toplam Kullanılan Gün")]
    public decimal TotalDaysUsed { get; set; }
    
    // Recent Leaves
    public List<RecentLeaveViewModel> RecentLeaves { get; set; } = new();
}

public class RecentLeaveViewModel
{
    public int Id { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalDays { get; set; }
    public DAL.Entities.LeaveStatus Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
}

public class LeaveTypeCreateViewModel
{
    [Required(ErrorMessage = "İzin türü adı zorunludur.")]
    [StringLength(100, ErrorMessage = "İzin türü adı en fazla 100 karakter olabilir.")]
    [Display(Name = "İzin Türü*")]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
    [Display(Name = "Açıklama")]
    public string Description { get; set; } = string.Empty;
    
    [Range(0, 365, ErrorMessage = "Maksimum gün sayısı 0-365 arasında olmalıdır.")]
    [Display(Name = "Maksimum Gün/Yıl")]
    public int MaxDaysPerYear { get; set; } = 0;
    
    [Display(Name = "Onay Gerekli")]
    public bool RequiresApproval { get; set; } = true;
    
    [Display(Name = "Belge Gerekli")]
    public bool RequiresDocument { get; set; } = false;
    
    [Display(Name = "Ücretli")]
    public bool IsPaid { get; set; } = true;
    
    [Display(Name = "Devir Edilebilir")]
    public bool CanCarryOver { get; set; } = false;
    
    [Range(0, 365, ErrorMessage = "Maksimum devir gün sayısı 0-365 arasında olmalıdır.")]
    [Display(Name = "Maksimum Devir Gün")]
    public int MaxCarryOverDays { get; set; } = 0;
    
    [Required(ErrorMessage = "Renk seçimi zorunludur.")]
    [Display(Name = "Renk*")]
    public string Color { get; set; } = "#007bff";
    
    [Range(0, 30, ErrorMessage = "Bildirim gün sayısı 0-30 arasında olmalıdır.")]
    [Display(Name = "Bildirim Gün")]
    public int NotificationDays { get; set; } = 2;
}

public class LeaveTypeEditViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "İzin türü adı zorunludur.")]
    [StringLength(100, ErrorMessage = "İzin türü adı en fazla 100 karakter olabilir.")]
    [Display(Name = "İzin Türü*")]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
    [Display(Name = "Açıklama")]
    public string Description { get; set; } = string.Empty;
    
    [Range(0, 365, ErrorMessage = "Maksimum gün sayısı 0-365 arasında olmalıdır.")]
    [Display(Name = "Maksimum Gün/Yıl")]
    public int MaxDaysPerYear { get; set; }
    
    [Display(Name = "Onay Gerekli")]
    public bool RequiresApproval { get; set; }
    
    [Display(Name = "Belge Gerekli")]
    public bool RequiresDocument { get; set; }
    
    [Display(Name = "Ücretli")]
    public bool IsPaid { get; set; }
    
    [Display(Name = "Devir Edilebilir")]
    public bool CanCarryOver { get; set; }
    
    [Range(0, 365, ErrorMessage = "Maksimum devir gün sayısı 0-365 arasında olmalıdır.")]
    [Display(Name = "Maksimum Devir Gün")]
    public int MaxCarryOverDays { get; set; }
    
    [Required(ErrorMessage = "Renk seçimi zorunludur.")]
    [Display(Name = "Renk*")]
    public string Color { get; set; } = string.Empty;
    
    [Range(0, 30, ErrorMessage = "Bildirim gün sayısı 0-30 arasında olmalıdır.")]
    [Display(Name = "Bildirim Gün")]
    public int NotificationDays { get; set; }
    
    [Display(Name = "Aktif")]
    public bool IsActive { get; set; } = true;
}
