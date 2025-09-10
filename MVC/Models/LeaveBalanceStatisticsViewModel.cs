using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Models;

public class LeaveBalanceStatisticsViewModel
{
    [Display(Name = "Departman")]
    public int? DepartmentId { get; set; }
    
    [Display(Name = "Yıl")]
    public int Year { get; set; } = DateTime.Now.Year;
    
    [Display(Name = "Toplam Çalışan")]
    public int TotalEmployees { get; set; }
    
    [Display(Name = "Toplam Personel")]
    public int TotalPersonnel { get; set; }
    
    [Display(Name = "Toplam Aktif Bakiyeler")]
    public int TotalActiveBalances { get; set; }
    
    [Display(Name = "Ortalama Kalan Günler")]
    public decimal AverageRemainingDays { get; set; }
    
    [Display(Name = "Toplam Tahsis")]
    public decimal TotalAllocated { get; set; }
    
    [Display(Name = "Toplam Kullanılan")]
    public decimal TotalUsed { get; set; }
    
    [Display(Name = "Toplam Kalan")]
    public decimal TotalRemaining { get; set; }
    
    [Display(Name = "Ortalama Kullanım Oranı")]
    public decimal AverageUsagePercentage { get; set; }
    
    [Display(Name = "Düşük Bakiye Sayısı")]
    public int LowBalanceCount { get; set; }
    
    [Display(Name = "Yüksek Bakiye Sayısı")]
    public int HighBalanceCount { get; set; }
    
    // For dropdown lists
    public IEnumerable<SelectListItem> Departments { get; set; } = new List<SelectListItem>();
    
    // Chart data
    public List<LeaveTypeStatistic> LeaveTypeStatistics { get; set; } = new List<LeaveTypeStatistic>();
    public List<DepartmentStatistic> DepartmentStatistics { get; set; } = new List<DepartmentStatistic>();
    public List<BalanceAlert> LowBalanceAlerts { get; set; } = new List<BalanceAlert>();
    public List<BalanceAlert> HighBalanceAlerts { get; set; } = new List<BalanceAlert>();
    
    // Recent balances for summary view
    public List<RecentBalanceItem> RecentBalances { get; set; } = new List<RecentBalanceItem>();
}

public class LeaveTypeStatistic
{
    public string LeaveTypeName { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public decimal TotalAllocated { get; set; }
    public decimal TotalUsed { get; set; }
    public decimal TotalRemaining { get; set; }
    public decimal UsagePercentage { get; set; }
    public int EmployeeCount { get; set; }
}

public class DepartmentStatistic
{
    public string DepartmentName { get; set; } = string.Empty;
    public decimal TotalAllocated { get; set; }
    public decimal TotalUsed { get; set; }
    public decimal TotalRemaining { get; set; }
    public decimal UsagePercentage { get; set; }
    public int EmployeeCount { get; set; }
}

public class BalanceAlert
{
    public int Id { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public string LeaveTypeName { get; set; } = string.Empty;
    public string LeaveTypeColor { get; set; } = string.Empty;
    public decimal RemainingDays { get; set; }
    public decimal AllocatedDays { get; set; }
    public string AlertLevel { get; set; } = string.Empty; // Critical, Warning, High
    public string DepartmentName { get; set; } = string.Empty;
}

public class RecentBalanceItem
{
    public int Id { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string LeaveTypeName { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal TotalDays { get; set; }
    public decimal UsedDays { get; set; }
    public decimal RemainingDays { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
