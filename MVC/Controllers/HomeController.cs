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
                ViewBag.OpenPositions = positions.Count(p => p.IsActive && p.MaxPositions > 0);
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

            ViewBag.LastUpdated = DateTime.Now;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Dashboard istatistikleri yüklenirken hata oluştu");
            ViewBag.DashboardError = ex.Message;
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
