using System.ComponentModel.DataAnnotations;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Models;

public class LeaveListViewModel
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
    
    [Display(Name = "Başlangıç")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    
    [Display(Name = "Bitiş")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
    
    [Display(Name = "Gün Sayısı")]
    public int TotalDays { get; set; }
    
    [Display(Name = "Gerekçe")]
    public string Reason { get; set; } = string.Empty;
    
    [Display(Name = "Durum")]
    public LeaveStatus Status { get; set; }
    
    public string StatusText { get; set; } = string.Empty;
    
    [Display(Name = "Talep Tarihi")]
    [DataType(DataType.Date)]
    public DateTime RequestDate { get; set; }
    
    [Display(Name = "Onaylayan")]
    public string? ApprovedByName { get; set; }
    
    [Display(Name = "Onay Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? ApprovedAt { get; set; }
    
    public bool RequiresDocument { get; set; }
    public bool HasDocument { get; set; }
    public int DaysUntilStart { get; set; }
    public bool IsUrgent { get; set; }
    
    // Calculated properties for display
    public string StatusBadgeClass => Status switch
    {
        LeaveStatus.Pending => "bg-warning text-dark",
        LeaveStatus.Approved => "bg-success",
        LeaveStatus.Rejected => "bg-danger",
        LeaveStatus.Cancelled => "bg-secondary",
        LeaveStatus.InProgress => "bg-info",
        LeaveStatus.Completed => "bg-primary",
        _ => "bg-secondary"
    };
    
    public string StatusIcon => Status switch
    {
        LeaveStatus.Pending => "fas fa-clock",
        LeaveStatus.Approved => "fas fa-check",
        LeaveStatus.Rejected => "fas fa-times",
        LeaveStatus.Cancelled => "fas fa-ban",
        LeaveStatus.InProgress => "fas fa-play",
        LeaveStatus.Completed => "fas fa-check-double",
        _ => "fas fa-question"
    };
}

public class LeaveDetailViewModel
{
    public int Id { get; set; }
    
    public int PersonId { get; set; }
    
    [Display(Name = "Personel")]
    public string PersonName { get; set; } = string.Empty;
    
    [Display(Name = "Sicil No")]
    public string EmployeeNumber { get; set; } = string.Empty;
    
    [Display(Name = "Departman")]
    public string DepartmentName { get; set; } = string.Empty;
    
    public int DepartmentId { get; set; }
    
    [Display(Name = "İzin Türü")]
    public string LeaveTypeName { get; set; } = string.Empty;
    
    public string LeaveTypeColor { get; set; } = string.Empty;
    public bool LeaveTypeRequiresDocument { get; set; }
    
    [Display(Name = "Başlangıç Tarihi")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    
    [Display(Name = "Bitiş Tarihi")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
    
    [Display(Name = "Gün Sayısı")]
    public int TotalDays { get; set; }
    
    [Display(Name = "Gerekçe")]
    public string Reason { get; set; } = string.Empty;
    
    [Display(Name = "Notlar")]
    public string? Notes { get; set; }
    
    [Display(Name = "Belge")]
    public string? DocumentPath { get; set; }
    
    [Display(Name = "Durum")]
    public LeaveStatus Status { get; set; }
    
    public string StatusText { get; set; } = string.Empty;
    
    [Display(Name = "Talep Tarihi")]
    [DataType(DataType.DateTime)]
    public DateTime RequestDate { get; set; }
    
    [Display(Name = "Onaylayan")]
    public string? ApprovedByName { get; set; }
    
    [Display(Name = "Onay Tarihi")]
    [DataType(DataType.DateTime)]
    public DateTime? ApprovedAt { get; set; }
    
    [Display(Name = "Onay Notları")]
    public string? ApprovalNotes { get; set; }
    
    [Display(Name = "Red Gerekçesi")]
    public string? RejectionReason { get; set; }
    
    [Display(Name = "Acil Durum İletişim")]
    public string? EmergencyContact { get; set; }
    
    [Display(Name = "Acil Durum Telefon")]
    public string? EmergencyPhone { get; set; }
    
    [Display(Name = "Devir Notları")]
    public string? HandoverNotes { get; set; }
    
    [Display(Name = "Devir Kişisi")]
    public string? HandoverToPersonName { get; set; }
    
    [Display(Name = "Oluşturulma")]
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }
    
    [Display(Name = "Güncellenme")]
    [DataType(DataType.DateTime)]
    public DateTime? UpdatedAt { get; set; }
    
}

public class LeaveCreateViewModel
{
    [Required(ErrorMessage = "Personel seçimi zorunludur.")]
    [Display(Name = "Personel*")]
    public int PersonId { get; set; }
    
    [Required(ErrorMessage = "İzin türü seçimi zorunludur.")]
    [Display(Name = "İzin Türü*")]
    public int LeaveTypeId { get; set; }
    
    [Required(ErrorMessage = "Başlangıç tarihi zorunludur.")]
    [Display(Name = "Başlangıç Tarihi*")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; } = DateTime.Today.AddDays(1);
    
    [Required(ErrorMessage = "Bitiş tarihi zorunludur.")]
    [Display(Name = "Bitiş Tarihi*")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; } = DateTime.Today.AddDays(1);
    
    [Required(ErrorMessage = "Gerekçe zorunludur.")]
    [StringLength(500, ErrorMessage = "Gerekçe en fazla 500 karakter olabilir.")]
    [Display(Name = "Gerekçe*")]
    public string Reason { get; set; } = string.Empty;
    
    [StringLength(1000, ErrorMessage = "Notlar en fazla 1000 karakter olabilir.")]
    [Display(Name = "Notlar")]
    public string? Notes { get; set; }
    
    [StringLength(200, ErrorMessage = "Acil durum iletişim en fazla 200 karakter olabilir.")]
    [Display(Name = "Acil Durum İletişim")]
    public string? EmergencyContact { get; set; }
    
    [StringLength(15, ErrorMessage = "Telefon numarası en fazla 15 karakter olabilir.")]
    [Display(Name = "Acil Durum Telefon")]
    public string? EmergencyPhone { get; set; }
    
    [StringLength(1000, ErrorMessage = "Devir notları en fazla 1000 karakter olabilir.")]
    [Display(Name = "Devir Notları")]
    public string? HandoverNotes { get; set; }
    
    [Display(Name = "Devir Kişisi")]
    public int? HandoverToPersonId { get; set; }
    
    // Calculated field
    [Display(Name = "Gün Sayısı")]
    public int TotalDays { get; set; }
    
    // For dropdown lists
    public IEnumerable<SelectListItem> Persons { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> LeaveTypes { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> HandoverPersons { get; set; } = new List<SelectListItem>();
}

public class LeaveEditViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Personel seçimi zorunludur.")]
    [Display(Name = "Personel*")]
    public int PersonId { get; set; }
    
    [Required(ErrorMessage = "İzin türü seçimi zorunludur.")]
    [Display(Name = "İzin Türü*")]
    public int LeaveTypeId { get; set; }
    
    [Required(ErrorMessage = "Başlangıç tarihi zorunludur.")]
    [Display(Name = "Başlangıç Tarihi*")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    
    [Required(ErrorMessage = "Bitiş tarihi zorunludur.")]
    [Display(Name = "Bitiş Tarihi*")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
    
    [Required(ErrorMessage = "Gerekçe zorunludur.")]
    [StringLength(500, ErrorMessage = "Gerekçe en fazla 500 karakter olabilir.")]
    [Display(Name = "Gerekçe*")]
    public string Reason { get; set; } = string.Empty;
    
    [StringLength(1000, ErrorMessage = "Notlar en fazla 1000 karakter olabilir.")]
    [Display(Name = "Notlar")]
    public string? Notes { get; set; }
    
    [StringLength(200, ErrorMessage = "Acil durum iletişim en fazla 200 karakter olabilir.")]
    [Display(Name = "Acil Durum İletişim")]
    public string? EmergencyContact { get; set; }
    
    [StringLength(15, ErrorMessage = "Telefon numarası en fazla 15 karakter olabilir.")]
    [Display(Name = "Acil Durum Telefon")]
    public string? EmergencyPhone { get; set; }
    
    [StringLength(1000, ErrorMessage = "Devir notları en fazla 1000 karakter olabilir.")]
    [Display(Name = "Devir Notları")]
    public string? HandoverNotes { get; set; }
    
    [Display(Name = "Devir Kişisi")]
    public int? HandoverToPersonId { get; set; }
    
    [Display(Name = "Gün Sayısı")]
    public int TotalDays { get; set; }
    
    // Current status (read-only)
    [Display(Name = "Mevcut Durum")]
    public LeaveStatus CurrentStatus { get; set; }
    
    // For dropdown lists
    public IEnumerable<SelectListItem> Persons { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> LeaveTypes { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> HandoverPersons { get; set; } = new List<SelectListItem>();
}

public class LeaveApprovalViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "Personel")]
    public string PersonName { get; set; } = string.Empty;
    
    [Display(Name = "İzin Türü")]
    public string LeaveTypeName { get; set; } = string.Empty;
    
    [Display(Name = "Tarih Aralığı")]
    public string DateRange { get; set; } = string.Empty;
    
    [Display(Name = "Gün Sayısı")]
    public int TotalDays { get; set; }
    
    [Display(Name = "Gerekçe")]
    public string Reason { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Onay kararı seçilmelidir.")]
    [Display(Name = "Karar*")]
    public bool IsApproved { get; set; }
    
    [StringLength(500, ErrorMessage = "Onay notları en fazla 500 karakter olabilir.")]
    [Display(Name = "Onay Notları")]
    public string? ApprovalNotes { get; set; }
    
    [StringLength(500, ErrorMessage = "Red gerekçesi en fazla 500 karakter olabilir.")]
    [Display(Name = "Red Gerekçesi")]
    public string? RejectionReason { get; set; }
    
    public int ApprovedById { get; set; }
}

public class LeaveFilterViewModel
{
    [Display(Name = "Personel")]
    public int? PersonId { get; set; }
    
    [Display(Name = "İzin Türü")]
    public int? LeaveTypeId { get; set; }
    
    [Display(Name = "Departman")]
    public int? DepartmentId { get; set; }
    
    [Display(Name = "Durum")]
    public LeaveStatus? Status { get; set; }
    
    [Display(Name = "Başlangıç Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? StartDate { get; set; }
    
    [Display(Name = "Bitiş Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }
    
    [Display(Name = "Yıl")]
    public int Year { get; set; } = DateTime.Now.Year;
    
    [Display(Name = "Onay Bekleyen")]
    public bool? RequiresApproval { get; set; }
    
    [Display(Name = "Belgeli")]
    public bool? HasDocument { get; set; }
    
    [Display(Name = "Arama")]
    public string? SearchTerm { get; set; }
    
    // For dropdown lists
    public IEnumerable<SelectListItem> Persons { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> LeaveTypes { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> Departments { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> StatusOptions { get; set; } = new List<SelectListItem>();
}

public class LeaveCalendarViewModel
{
    public int Id { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string LeaveTypeName { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalDays { get; set; }
    public LeaveStatus Status { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Tooltip { get; set; } = string.Empty;
}
