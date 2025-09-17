namespace BLL.DTOs;

public class DashboardSummaryDto
{
    public int TotalPersonnel { get; set; }
    public int ActiveDepartments { get; set; }
    public int TotalPositions { get; set; }
    public int PendingLeaveRequests { get; set; }
    public int TotalMaterials { get; set; }
    public int LowStockMaterials { get; set; }
    public decimal TotalMaterialValue { get; set; }
    public decimal MonthlyPayrollTotal { get; set; }
    public decimal AverageEmployeeSalary { get; set; }
    public DateTime GeneratedAt { get; set; }
    
    // Chart Data for PDF
    public List<ChartDataDto> DepartmentDistribution { get; set; } = new();
    public List<ChartDataDto> LeaveStatusDistribution { get; set; } = new();
    public List<ChartDataDto> HiringTrend { get; set; } = new();
    public List<ChartDataDto> StockStatus { get; set; } = new();
}

public class ChartDataDto
{
    public string Label { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string Color { get; set; } = "#007bff";
    public string Unit { get; set; } = string.Empty;
}
