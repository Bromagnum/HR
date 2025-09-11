using AutoMapper;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using MVC.Models;

namespace MVC.Controllers;

// DEPRECATED: Bu modül kaldırıldı, artık kullanılmıyor
// [Authorize(Roles = "Admin,Manager")]
public class LeaveBalanceController : Controller
{
    private readonly ILeaveBalanceService _leaveBalanceService;
    private readonly ILeaveTypeService _leaveTypeService;
    private readonly IPersonService _personService;
    private readonly IDepartmentService _departmentService;
    private readonly IMapper _mapper;

    public LeaveBalanceController(
        ILeaveBalanceService leaveBalanceService,
        ILeaveTypeService leaveTypeService,
        IPersonService personService,
        IDepartmentService departmentService,
        IMapper mapper)
    {
        _leaveBalanceService = leaveBalanceService;
        _leaveTypeService = leaveTypeService;
        _personService = personService;
        _departmentService = departmentService;
        _mapper = mapper;
    }

    // GET: LeaveBalance
    public async Task<IActionResult> Index(int? personId, int? leaveTypeId, int? departmentId, int year = 0)
    {
        if (year == 0) year = DateTime.Now.Year;

        IEnumerable<BLL.DTOs.LeaveBalanceListDto> balances;

        if (personId.HasValue)
        {
            var result = await _leaveBalanceService.GetBalancesByPersonAsync(personId.Value, year);
            balances = result.IsSuccess ? result.Data : new List<BLL.DTOs.LeaveBalanceListDto>();
        }
        else if (leaveTypeId.HasValue)
        {
            var result = await _leaveBalanceService.GetBalancesByLeaveTypeAsync(leaveTypeId.Value, year);
            balances = result.IsSuccess ? result.Data : new List<BLL.DTOs.LeaveBalanceListDto>();
        }
        else if (departmentId.HasValue)
        {
            var result = await _leaveBalanceService.GetDepartmentBalancesAsync(departmentId.Value, year);
            balances = result.IsSuccess ? result.Data : new List<BLL.DTOs.LeaveBalanceListDto>();
        }
        else
        {
            var result = await _leaveBalanceService.GetAllAsync();
            balances = result.IsSuccess ? result.Data.Where(b => b.Year == year) : new List<BLL.DTOs.LeaveBalanceListDto>();
        }

        var viewModels = _mapper.Map<List<LeaveBalanceListViewModel>>(balances);

        // Prepare filter dropdowns
        await PrepareFilterViewData(personId, leaveTypeId, departmentId, year);

        ViewData["PersonId"] = personId;
        ViewData["LeaveTypeId"] = leaveTypeId;
        ViewData["DepartmentId"] = departmentId;
        ViewData["Year"] = year;

        return View(viewModels);
    }

    // GET: LeaveBalance/Summary/5
    public async Task<IActionResult> Summary(int personId, int year = 0)
    {
        if (year == 0) year = DateTime.Now.Year;

        var result = await _leaveBalanceService.GetBalanceSummaryAsync(personId, year);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<LeaveBalanceSummaryViewModel>(result.Data);
        return View(viewModel);
    }

    // GET: LeaveBalance/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var result = await _leaveBalanceService.GetByIdAsync(id);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<LeaveBalanceDetailViewModel>(result.Data);
        return View(viewModel);
    }

    // GET: LeaveBalance/Create
    public async Task<IActionResult> Create()
    {
        var viewModel = new LeaveBalanceCreateViewModel();
        await PrepareCreateEditViewData(viewModel);
        return View(viewModel);
    }

    // POST: LeaveBalance/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LeaveBalanceCreateViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            await PrepareCreateEditViewData(viewModel);
            return View(viewModel);
        }

        var dto = _mapper.Map<BLL.DTOs.LeaveBalanceCreateDto>(viewModel);
        var result = await _leaveBalanceService.CreateAsync(dto);

        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            await PrepareCreateEditViewData(viewModel);
            return View(viewModel);
        }

        TempData["Success"] = "İzin bakiyesi başarıyla oluşturuldu.";
        return RedirectToAction(nameof(Details), new { id = result.Data?.Id });
    }

    // GET: LeaveBalance/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _leaveBalanceService.GetByIdAsync(id);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<LeaveBalanceEditViewModel>(result.Data);
        return View(viewModel);
    }

    // POST: LeaveBalance/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, LeaveBalanceEditViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData["Error"] = "Geçersiz istek.";
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var dto = _mapper.Map<BLL.DTOs.LeaveBalanceUpdateDto>(viewModel);
        var result = await _leaveBalanceService.UpdateAsync(dto);

        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return View(viewModel);
        }

        TempData["Success"] = "İzin bakiyesi başarıyla güncellendi.";
        return RedirectToAction(nameof(Details), new { id = viewModel.Id });
    }

    // GET: LeaveBalance/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _leaveBalanceService.GetByIdAsync(id);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<LeaveBalanceDetailViewModel>(result.Data);
        return View(viewModel);
    }

    // POST: LeaveBalance/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _leaveBalanceService.DeleteAsync(id);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Delete), new { id });
        }

        TempData["Success"] = "İzin bakiyesi başarıyla silindi.";
        return RedirectToAction(nameof(Index));
    }

    // GET: LeaveBalance/Adjust
    public async Task<IActionResult> Adjust(int? personId, int? leaveTypeId, int year = 0)
    {
        if (year == 0) year = DateTime.Now.Year;

        var viewModel = new LeaveBalanceAdjustmentViewModel
        {
            PersonId = personId ?? 0,
            LeaveTypeId = leaveTypeId ?? 0,
            Year = year,
            AdjustedById = 1 // TODO: Get current user ID
        };

        await PrepareAdjustmentViewData(viewModel);

        // If person and leave type are selected, get current balance
        if (personId.HasValue && leaveTypeId.HasValue)
        {
            var balanceResult = await _leaveBalanceService.GetBalanceAsync(personId.Value, leaveTypeId.Value, year);
            if (balanceResult.IsSuccess)
            {
                viewModel.CurrentBalance = balanceResult.Data.RemainingDays;
            }
        }

        return View(viewModel);
    }

    // POST: LeaveBalance/Adjust
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Adjust(LeaveBalanceAdjustmentViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            await PrepareAdjustmentViewData(viewModel);
            return View(viewModel);
        }

        var dto = _mapper.Map<BLL.DTOs.LeaveBalanceAdjustmentDto>(viewModel);
        var result = await _leaveBalanceService.AdjustBalanceAsync(dto);

        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            await PrepareAdjustmentViewData(viewModel);
            return View(viewModel);
        }

        TempData["Success"] = "Bakiye düzeltmesi başarıyla yapıldı.";
        return RedirectToAction(nameof(Index), new { personId = viewModel.PersonId, year = viewModel.Year });
    }

    // GET: LeaveBalance/Statistics
    public async Task<IActionResult> Statistics(int? departmentId, int year = 0)
    {
        if (year == 0) year = DateTime.Now.Year;

        var result = await _leaveBalanceService.GetBalanceStatisticsAsync(departmentId, year);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return View(new Dictionary<string, object>());
        }

        // Prepare department dropdown
        var departmentsResult = await _departmentService.GetAllAsync();
        if (departmentsResult.IsSuccess)
        {
            ViewBag.Departments = new SelectList(departmentsResult.Data, "Id", "Name", departmentId);
        }

        ViewData["DepartmentId"] = departmentId;
        ViewData["Year"] = year;

        return View(result.Data);
    }

    // GET: LeaveBalance/LowBalanceAlerts
    public async Task<IActionResult> LowBalanceAlerts(decimal threshold = 2.0m)
    {
        var result = await _leaveBalanceService.GetLowBalanceAlertsAsync(threshold);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return View("Index", new List<LeaveBalanceListViewModel>());
        }

        var viewModels = _mapper.Map<List<LeaveBalanceListViewModel>>(result.Data);
        ViewData["Title"] = $"Düşük Bakiye Uyarıları (< {threshold} gün)";
        ViewData["Threshold"] = threshold;
        return View("Index", viewModels);
    }

    // GET: LeaveBalance/OverusedBalances
    public async Task<IActionResult> OverusedBalances(int year = 0)
    {
        if (year == 0) year = DateTime.Now.Year;

        var result = await _leaveBalanceService.GetOverusedBalancesAsync(year);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return View("Index", new List<LeaveBalanceListViewModel>());
        }

        var viewModels = _mapper.Map<List<LeaveBalanceListViewModel>>(result.Data);
        ViewData["Title"] = $"Aşırı Kullanım ({year})";
        ViewData["Year"] = year;
        return View("Index", viewModels);
    }

    // GET: LeaveBalance/UnusedBalances
    public async Task<IActionResult> UnusedBalances(int year = 0, decimal threshold = 0.8m)
    {
        if (year == 0) year = DateTime.Now.Year;

        var result = await _leaveBalanceService.GetUnusedBalancesAsync(year, threshold);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return View("Index", new List<LeaveBalanceListViewModel>());
        }

        var viewModels = _mapper.Map<List<LeaveBalanceListViewModel>>(result.Data);
        ViewData["Title"] = $"Kullanılmayan Bakiyeler ({year} - >{threshold * 100}% kullanılmamış)";
        ViewData["Year"] = year;
        ViewData["Threshold"] = threshold;
        return View("Index", viewModels);
    }

    // POST: LeaveBalance/Recalculate
    [HttpPost]
    public async Task<IActionResult> Recalculate(int personId, int year)
    {
        var result = await _leaveBalanceService.RecalculateAllBalancesAsync(personId, year);
        if (!result.IsSuccess)
        {
            return Json(new { success = false, message = result.Message });
        }

        return Json(new { success = true, message = "Bakiyeler başarıyla yeniden hesaplandı." });
    }

    // POST: LeaveBalance/ProcessAccruals
    [HttpPost]
    public async Task<IActionResult> ProcessAccruals(DateTime cutoffDate)
    {
        var result = await _leaveBalanceService.ProcessMonthlyAccrualsAsync(cutoffDate);
        if (!result.IsSuccess)
        {
            return Json(new { success = false, message = result.Message });
        }

        return Json(new { success = true, message = "Aylık tahakkuklar başarıyla işlendi." });
    }

    // POST: LeaveBalance/ProcessCarryOver
    [HttpPost]
    public async Task<IActionResult> ProcessCarryOver(int fromYear, int toYear)
    {
        var result = await _leaveBalanceService.ProcessYearEndCarryOverAsync(fromYear, toYear);
        if (!result.IsSuccess)
        {
            return Json(new { success = false, message = result.Message });
        }

        return Json(new { success = true, message = $"{fromYear} yılından {toYear} yılına devir işlemi başarıyla tamamlandı." });
    }

    // POST: LeaveBalance/InitializeYear
    [HttpPost]
    public async Task<IActionResult> InitializeYear(int year)
    {
        var result = await _leaveBalanceService.InitializeBalancesForYearAsync(year);
        if (!result.IsSuccess)
        {
            return Json(new { success = false, message = result.Message });
        }

        return Json(new { success = true, message = $"{year} yılı bakiyeleri başarıyla başlatıldı." });
    }

    // AJAX: Get balance info
    [HttpPost]
    public async Task<IActionResult> GetBalanceInfo(int personId, int leaveTypeId, int year)
    {
        var result = await _leaveBalanceService.GetBalanceAsync(personId, leaveTypeId, year);
        if (!result.IsSuccess)
        {
            return Json(new { success = false, message = result.Message });
        }

        var balance = result.Data;
        return Json(new
        {
            success = true,
            allocatedDays = balance.AllocatedDays,
            usedDays = balance.UsedDays,
            pendingDays = balance.PendingDays,
            remainingDays = balance.RemainingDays,
            availableDays = balance.AvailableDays
        });
    }

    // Debug endpoint
    public async Task<IActionResult> Debug()
    {
        try
        {
            var allResult = await _leaveBalanceService.GetAllAsync();
            var currentYear = DateTime.Now.Year;

            var debugInfo = new
            {
                AllBalancesCount = allResult.IsSuccess ? allResult.Data.Count() : 0,
                AllBalancesSuccess = allResult.IsSuccess,
                AllBalancesMessage = allResult.Message,
                CurrentYearBalancesCount = allResult.IsSuccess ? allResult.Data.Count(b => b.Year == currentYear) : 0,
                AllBalances = allResult.IsSuccess ? allResult.Data.Take(5) : null
            };

            return Json(debugInfo);
        }
        catch (Exception ex)
        {
            return Json(new { Error = ex.Message, StackTrace = ex.StackTrace });
        }
    }

    #region Private Helper Methods

    private async Task PrepareFilterViewData(int? personId, int? leaveTypeId, int? departmentId, int year)
    {
        // Persons dropdown
        var personsResult = await _personService.GetAllAsync();
        if (personsResult.IsSuccess)
        {
            ViewBag.Persons = new SelectList(personsResult.Data, "Id", "FullName", personId);
        }

        // LeaveTypes dropdown
        var leaveTypesResult = await _leaveTypeService.GetActiveAsync();
        if (leaveTypesResult.IsSuccess)
        {
            ViewBag.LeaveTypes = new SelectList(leaveTypesResult.Data, "Id", "Name", leaveTypeId);
        }

        // Departments dropdown
        var departmentsResult = await _departmentService.GetAllAsync();
        if (departmentsResult.IsSuccess)
        {
            ViewBag.Departments = new SelectList(departmentsResult.Data, "Id", "Name", departmentId);
        }

        // Years dropdown
        var years = Enumerable.Range(2020, 11).Select(y => new SelectListItem
        {
            Value = y.ToString(),
            Text = y.ToString(),
            Selected = y == year
        });
        ViewBag.Years = years;
    }

    private async Task PrepareCreateEditViewData(dynamic viewModel)
    {
        // Persons dropdown
        var personsResult = await _personService.GetAllAsync();
        if (personsResult.IsSuccess)
        {
            viewModel.Persons = personsResult.Data.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.FirstName} {p.LastName}"
            });
        }

        // LeaveTypes dropdown
        var leaveTypesResult = await _leaveTypeService.GetActiveAsync();
        if (leaveTypesResult.IsSuccess)
        {
            viewModel.LeaveTypes = leaveTypesResult.Data.Select(lt => new SelectListItem
            {
                Value = lt.Id.ToString(),
                Text = lt.Name
            });
        }
    }

    private async Task PrepareAdjustmentViewData(LeaveBalanceAdjustmentViewModel viewModel)
    {
        await PrepareCreateEditViewData(viewModel);
    }

    // GET: LeaveBalance/Statistics
    [Route("LeaveBalance/Statistics")]
    public async Task<IActionResult> Statistics(int? personId, int? leaveTypeId, int? departmentId, int year = 0)
    {
        if (year == 0) year = DateTime.Now.Year;

        var statistics = new Dictionary<string, object>();
        
        try
        {
            // Get overall balance statistics
            var allBalancesResult = await _leaveBalanceService.GetAllAsync();
            if (allBalancesResult.IsSuccess && allBalancesResult.Data != null)
            {
                var balances = allBalancesResult.Data.ToList();
                
                statistics["TotalBalances"] = balances.Count;
                statistics["TotalAllocatedDays"] = balances.Sum(b => b.AllocatedDays);
                statistics["TotalUsedDays"] = balances.Sum(b => b.UsedDays);
                statistics["TotalPendingDays"] = balances.Sum(b => b.PendingDays);
                statistics["TotalRemainingDays"] = balances.Sum(b => b.RemainingDays);
                
                // Department-wise statistics
                var departmentStats = balances
                    .Where(b => !string.IsNullOrEmpty(b.DepartmentName))
                    .GroupBy(b => b.DepartmentName)
                    .Select(g => new
                    {
                        Department = g.Key,
                        AllocatedDays = g.Sum(b => b.AllocatedDays),
                        UsedDays = g.Sum(b => b.UsedDays),
                        RemainingDays = g.Sum(b => b.RemainingDays)
                    })
                    .OrderByDescending(s => s.AllocatedDays)
                    .ToList();
                
                statistics["DepartmentStatistics"] = departmentStats;
                
                // Leave type statistics
                var leaveTypeStats = balances
                    .GroupBy(b => b.LeaveTypeName)
                    .Select(g => new
                    {
                        LeaveType = g.Key,
                        AllocatedDays = g.Sum(b => b.AllocatedDays),
                        UsedDays = g.Sum(b => b.UsedDays),
                        RemainingDays = g.Sum(b => b.RemainingDays),
                        UtilizationRate = g.Sum(b => b.AllocatedDays) > 0 ? 
                            Math.Round((g.Sum(b => b.UsedDays) / g.Sum(b => b.AllocatedDays)) * 100, 2) : 0
                    })
                    .OrderByDescending(s => s.AllocatedDays)
                    .ToList();
                
                statistics["LeaveTypeStatistics"] = leaveTypeStats;
                
                // Low balance alerts
                var lowBalanceThreshold = 5; // 5 days or less
                var lowBalances = balances
                    .Where(b => b.RemainingDays <= lowBalanceThreshold)
                    .OrderBy(b => b.RemainingDays)
                    .ToList();
                
                statistics["LowBalanceAlerts"] = lowBalances;
                statistics["LowBalanceCount"] = lowBalances.Count;
                
                // Overused balances (negative remaining days)
                var overusedBalances = balances
                    .Where(b => b.RemainingDays < 0)
                    .OrderBy(b => b.RemainingDays)
                    .ToList();
                
                statistics["OverusedBalances"] = overusedBalances;
                statistics["OverusedCount"] = overusedBalances.Count;
            }
            
            statistics["Year"] = year;
            statistics["PersonId"] = personId ?? 0;
            statistics["LeaveTypeId"] = leaveTypeId ?? 0;
            statistics["DepartmentId"] = departmentId ?? 0;
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"İstatistikler yüklenirken hata oluştu: {ex.Message}";
        }
        
        // Prepare filter dropdowns
        await PrepareFilterViewData(personId, leaveTypeId, departmentId, year);
        
        return View(statistics);
    }

    #endregion
}
