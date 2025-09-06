using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.Entities;

namespace MVC.Models;

public class LeaveStatisticsViewModel
{
    [Display(Name = "Personel")]
    public int? PersonId { get; set; }
    
    [Display(Name = "Departman")]
    public int? DepartmentId { get; set; }
    
    [Display(Name = "Yıl")]
    public int Year { get; set; } = DateTime.Now.Year;
    
    // Summary statistics
    [Display(Name = "Toplam İzin")]
    public int TotalLeaves { get; set; }
    
    [Display(Name = "Onay Bekleyen")]
    public int PendingLeaves { get; set; }
    
    [Display(Name = "Onaylanan")]
    public int ApprovedLeaves { get; set; }
    
    [Display(Name = "Devam Eden")]
    public int InProgressLeaves { get; set; }
    
    [Display(Name = "Tamamlanan")]
    public int CompletedLeaves { get; set; }
    
    [Display(Name = "Reddedilen")]
    public int RejectedLeaves { get; set; }
    
    [Display(Name = "İptal Edilen")]
    public int CancelledLeaves { get; set; }
    
    [Display(Name = "Toplam İzin Günü")]
    public int TotalLeaveDays { get; set; }
    
    [Display(Name = "Ortalama İzin Süresi")]
    public decimal AverageLeaveDuration { get; set; }
    
    [Display(Name = "Ortalama Onay Süresi")]
    public decimal AverageApprovalTime { get; set; }
    
    // For dropdown lists
    public IEnumerable<SelectListItem> Persons { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> Departments { get; set; } = new List<SelectListItem>();
    
    // Chart and analysis data
    public List<LeaveTypeDistribution> LeaveTypeDistribution { get; set; } = new List<LeaveTypeDistribution>();
    public List<MonthlyTrend> MonthlyTrends { get; set; } = new List<MonthlyTrend>();
    public List<DepartmentAnalysis> DepartmentAnalysis { get; set; } = new List<DepartmentAnalysis>();
    public List<TopEmployee> TopLeaveUsers { get; set; } = new List<TopEmployee>();
    public List<UpcomingLeave> UpcomingLeaves { get; set; } = new List<UpcomingLeave>();
}

public class LeaveTypeDistribution
{
    public string LeaveTypeName { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int Count { get; set; }
    public int TotalDays { get; set; }
    public decimal Percentage { get; set; }
    public decimal AverageDuration { get; set; }
}

public class MonthlyTrend
{
    public int Month { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public int LeaveCount { get; set; }
    public int TotalDays { get; set; }
    public decimal AverageApprovalTime { get; set; }
}

public class DepartmentAnalysis
{
    public string DepartmentName { get; set; } = string.Empty;
    public int EmployeeCount { get; set; }
    public int LeaveCount { get; set; }
    public int TotalDays { get; set; }
    public decimal AveragePerEmployee { get; set; }
    public decimal UsageRate { get; set; }
}

public class TopEmployee
{
    public int Rank { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public int LeaveCount { get; set; }
    public int TotalDays { get; set; }
    public decimal UsagePercentage { get; set; }
}

public class UpcomingLeave
{
    public int Id { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string LeaveTypeName { get; set; } = string.Empty;
    public string LeaveTypeColor { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalDays { get; set; }
    public LeaveStatus Status { get; set; }
    public int DaysUntilStart { get; set; }
}
