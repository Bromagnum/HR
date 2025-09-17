using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MVC.Models;
using BLL.Services;
using BLL.DTOs;
using AutoMapper;

namespace MVC.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPersonService _personService;
    private readonly IDepartmentService _departmentService;
    private readonly IPositionService _positionService;
    private readonly ILeaveService _leaveService;
    private readonly IPayrollService _payrollService;
    private readonly IMaterialService _materialService;
    private readonly IMapper _mapper;

    public HomeController(
        ILogger<HomeController> logger,
        IPersonService personService,
        IDepartmentService departmentService,
        IPositionService positionService,
        ILeaveService leaveService,
        IPayrollService payrollService,
        IMaterialService materialService,
        IMapper mapper)
    {
        _logger = logger;
        _personService = personService;
        _departmentService = departmentService;
        _positionService = positionService;
        _leaveService = leaveService;
        _payrollService = payrollService;
        _materialService = materialService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            // Dashboard istatistiklerini topla
            await LoadDashboardStatistics();
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Dashboard yüklenirken hata oluştu");
            return View();
        }
    }

    private async Task LoadDashboardStatistics()
    {
        try
        {
            // Personel istatistikleri
            var personsResult = await _personService.GetAllAsync();
            if (personsResult.IsSuccess)
            {
                var persons = personsResult.Data?.ToList() ?? new List<PersonListDto>();
                ViewBag.TotalEmployees = persons.Count;
                ViewBag.ActiveEmployees = persons.Count(p => p.IsActive);
            }

            // Departman istatistikleri
            var departmentsResult = await _departmentService.GetAllAsync();
            if (departmentsResult.IsSuccess)
            {
                var departments = departmentsResult.Data?.ToList() ?? new List<DepartmentListDto>();
                ViewBag.TotalDepartments = departments.Count;
                ViewBag.ActiveDepartments = departments.Count(d => d.IsActive);
            }

            // Pozisyon istatistikleri
            var positionsResult = await _positionService.GetAllAsync();
            if (positionsResult.IsSuccess)
            {
                var positions = positionsResult.Data?.ToList() ?? new List<PositionListDto>();
                ViewBag.TotalPositions = positions.Count;
                ViewBag.OpenPositions = positions.Count(p => p.IsActive && p.IsAvailable);
            }

            // İzin istatistikleri
            var leavesResult = await _leaveService.GetAllAsync();
            if (leavesResult.IsSuccess)
            {
                var leaves = leavesResult.Data?.ToList() ?? new List<LeaveListDto>();
                ViewBag.TotalLeaveRequests = leaves.Count;
                ViewBag.PendingLeaveRequests = leaves.Count(l => l.Status == DAL.Entities.LeaveStatus.Pending);
                ViewBag.ApprovedLeaveRequests = leaves.Count(l => l.Status == DAL.Entities.LeaveStatus.Approved);
            }

            // Malzeme istatistikleri
            var materialStatsResult = await _materialService.GetStockSummaryAsync();
            if (materialStatsResult.Success && materialStatsResult.Data != null)
            {
                ViewBag.TotalMaterials = materialStatsResult.Data.TotalMaterials;
                ViewBag.LowStockMaterials = materialStatsResult.Data.LowStockCount;
                ViewBag.TotalMaterialValue = materialStatsResult.Data.TotalStockValue;
            }

            // Bordro istatistikleri (bu ay)
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;
            var payrollSummaryResult = await _payrollService.GetPeriodSummaryAsync(currentYear, currentMonth);
            if (payrollSummaryResult.IsSuccess && payrollSummaryResult.Data != null)
            {
                ViewBag.MonthlyPayrollTotal = payrollSummaryResult.Data.TotalNetSalary;
                ViewBag.AverageEmployeeSalary = payrollSummaryResult.Data.AverageNetSalary;
            }

            // Chart data hazırla
            await LoadChartData();
            
            ViewBag.LastUpdated = DateTime.Now;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Dashboard istatistikleri yüklenirken hata oluştu");
            // Silent fail - dashboard will show default values
            
            // Chart data default değerler
            ViewBag.ChartDepartmentLabels = "[]";
            ViewBag.ChartDepartmentData = "[]";
            ViewBag.ChartLeaveStatusLabels = "[]";
            ViewBag.ChartLeaveStatusData = "[]";
            ViewBag.ChartHiringTrendLabels = "[]";
            ViewBag.ChartHiringTrendData = "[]";
            ViewBag.ChartStockStatusLabels = "[]";
            ViewBag.ChartStockStatusData = "[]";
        }
    }

    private async Task LoadChartData()
    {
        try
        {
            _logger.LogInformation("Starting chart data loading...");
            
            // 1. Departman bazında personel dağılımı
            var personsResult = await _personService.GetAllAsync();
            var departmentsResult = await _departmentService.GetAllAsync();

            if (personsResult.IsSuccess && departmentsResult.IsSuccess)
            {
                var persons = personsResult.Data?.ToList() ?? new List<PersonListDto>();
                var departments = departmentsResult.Data?.ToList() ?? new List<DepartmentListDto>();

                var departmentStats = departments
                    .Where(d => d.IsActive)
                    .Select(d => new
                    {
                        Name = d.Name,
                        Count = persons.Count(p => p.DepartmentId == d.Id && p.IsActive)
                    })
                    .Where(d => d.Count > 0)
                    .ToList();

                ViewBag.ChartDepartmentLabels = System.Text.Json.JsonSerializer.Serialize(departmentStats.Select(d => d.Name));
                ViewBag.ChartDepartmentData = System.Text.Json.JsonSerializer.Serialize(departmentStats.Select(d => d.Count));
                
                _logger.LogInformation("Department chart data loaded: {Count} departments", departmentStats.Count);
            }
            else
            {
                _logger.LogWarning("Failed to load department/person data. Persons: {PersonSuccess}, Departments: {DeptSuccess}", 
                    personsResult.IsSuccess, departmentsResult.IsSuccess);
                ViewBag.ChartDepartmentLabels = "[]";
                ViewBag.ChartDepartmentData = "[]";
            }

            // 2. İzin talebi durumları
            var leavesResult = await _leaveService.GetAllAsync();
            if (leavesResult.IsSuccess)
            {
                var leaves = leavesResult.Data?.ToList() ?? new List<LeaveListDto>();
                var leaveStats = new[]
                {
                    new { Label = "Beklemede", Count = leaves.Count(l => l.Status == DAL.Entities.LeaveStatus.Pending) },
                    new { Label = "Onaylandı", Count = leaves.Count(l => l.Status == DAL.Entities.LeaveStatus.Approved) },
                    new { Label = "Reddedildi", Count = leaves.Count(l => l.Status == DAL.Entities.LeaveStatus.Rejected) },
                    new { Label = "İptal", Count = leaves.Count(l => l.Status == DAL.Entities.LeaveStatus.Cancelled) }
                }.ToList(); // Remove the filter to show even zero counts

                ViewBag.ChartLeaveStatusLabels = System.Text.Json.JsonSerializer.Serialize(leaveStats.Select(l => l.Label));
                ViewBag.ChartLeaveStatusData = System.Text.Json.JsonSerializer.Serialize(leaveStats.Select(l => l.Count));
                
                _logger.LogInformation("Leave status chart data loaded: {TotalLeaves} leaves, {StatsCount} status types", 
                    leaves.Count, leaveStats.Count);
            }
            else
            {
                _logger.LogWarning("Failed to load leave data: {ErrorMessage}", leavesResult.Message);
                ViewBag.ChartLeaveStatusLabels = "[]";
                ViewBag.ChartLeaveStatusData = "[]";
            }

            // 3. Son 6 ay işe alım trendi
            if (personsResult.IsSuccess)
            {
                var persons = personsResult.Data?.ToList() ?? new List<PersonListDto>();
                var hiringTrend = new List<object>();
                
                for (int i = 5; i >= 0; i--)
                {
                    var targetMonth = DateTime.Now.AddMonths(-i);
                    var monthStart = new DateTime(targetMonth.Year, targetMonth.Month, 1);
                    var monthEnd = monthStart.AddMonths(1).AddDays(-1);
                    
                    var hiringCount = persons.Count(p => 
                        p.HireDate.HasValue && 
                        p.HireDate.Value >= monthStart && 
                        p.HireDate.Value <= monthEnd);
                    
                    hiringTrend.Add(new
                    {
                        Month = targetMonth.ToString("MMM yyyy"),
                        Count = hiringCount
                    });
                }

                ViewBag.ChartHiringTrendLabels = System.Text.Json.JsonSerializer.Serialize(hiringTrend.Select(h => ((dynamic)h).Month));
                ViewBag.ChartHiringTrendData = System.Text.Json.JsonSerializer.Serialize(hiringTrend.Select(h => ((dynamic)h).Count));
                
                _logger.LogInformation("Hiring trend chart data loaded: {TotalPersons} persons, {MonthsCount} months", 
                    persons.Count, hiringTrend.Count);
            }
            else
            {
                _logger.LogWarning("Cannot load hiring trend - person data not available");
                ViewBag.ChartHiringTrendLabels = "[]";
                ViewBag.ChartHiringTrendData = "[]";
            }

            // 4. Stok durumu
            var materialStatsResult = await _materialService.GetStockSummaryAsync();
            if (materialStatsResult.Success && materialStatsResult.Data != null)
            {
                var normalStockCount = materialStatsResult.Data.TotalMaterials - materialStatsResult.Data.LowStockCount - materialStatsResult.Data.OverStockCount;
                var stockStats = new[]
                {
                    new { Label = "Normal Stok", Count = normalStockCount },
                    new { Label = "Düşük Stok", Count = materialStatsResult.Data.LowStockCount },
                    new { Label = "Fazla Stok", Count = materialStatsResult.Data.OverStockCount }
                }.ToList(); // Show even zero counts

                ViewBag.ChartStockStatusLabels = System.Text.Json.JsonSerializer.Serialize(stockStats.Select(s => s.Label));
                ViewBag.ChartStockStatusData = System.Text.Json.JsonSerializer.Serialize(stockStats.Select(s => s.Count));
                
                _logger.LogInformation("Stock status chart data loaded: Total={TotalMaterials}, Normal={Normal}, Low={Low}, Over={Over}", 
                    materialStatsResult.Data.TotalMaterials, normalStockCount, 
                    materialStatsResult.Data.LowStockCount, materialStatsResult.Data.OverStockCount);
            }
            else
            {
                _logger.LogWarning("Failed to load material stock data: Success={Success}, Error={Error}", 
                    materialStatsResult.Success, materialStatsResult.Message);
                ViewBag.ChartStockStatusLabels = "[]";
                ViewBag.ChartStockStatusData = "[]";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Chart verileri yüklenirken hata oluştu");
            
            // Default empty values
            ViewBag.ChartDepartmentLabels = "[]";
            ViewBag.ChartDepartmentData = "[]";
            ViewBag.ChartLeaveStatusLabels = "[]";
            ViewBag.ChartLeaveStatusData = "[]";
            ViewBag.ChartHiringTrendLabels = "[]";
            ViewBag.ChartHiringTrendData = "[]";
            ViewBag.ChartStockStatusLabels = "[]";
            ViewBag.ChartStockStatusData = "[]";
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
