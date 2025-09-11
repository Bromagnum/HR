using AutoMapper;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MVC.Models;

namespace MVC.Controllers;

[Authorize(Roles = "Admin,Manager")] // Sadece Admin ve Manager izin türlerini yönetebilir
public class LeaveTypeController : Controller
{
    private readonly ILeaveTypeService _leaveTypeService;
    private readonly IMapper _mapper;

    public LeaveTypeController(ILeaveTypeService leaveTypeService, IMapper mapper)
    {
        _leaveTypeService = leaveTypeService;
        _mapper = mapper;
    }

    // GET: LeaveType
    public async Task<IActionResult> Index()
    {
        var result = await _leaveTypeService.GetAllAsync();
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return View(new List<LeaveTypeListViewModel>());
        }

        var viewModels = _mapper.Map<List<LeaveTypeListViewModel>>(result.Data);
        return View(viewModels);
    }

    // GET: LeaveType/Active
    public async Task<IActionResult> Active()
    {
        var result = await _leaveTypeService.GetActiveAsync();
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return View("Index", new List<LeaveTypeListViewModel>());
        }

        var viewModels = _mapper.Map<List<LeaveTypeListViewModel>>(result.Data);
        ViewData["Title"] = "Aktif İzin Türleri";
        ViewData["ShowInactiveButton"] = true;
        return View("Index", viewModels);
    }

    // GET: LeaveType/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var result = await _leaveTypeService.GetByIdAsync(id);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<LeaveTypeDetailViewModel>(result.Data);
        return View(viewModel);
    }

    // GET: LeaveType/Create
    public IActionResult Create()
    {
        var viewModel = new LeaveTypeCreateViewModel();
        return View(viewModel);
    }

    // POST: LeaveType/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LeaveTypeCreateViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var dto = _mapper.Map<BLL.DTOs.LeaveTypeCreateDto>(viewModel);
        var result = await _leaveTypeService.CreateAsync(dto);

        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return View(viewModel);
        }

        TempData["Success"] = "İzin türü başarıyla oluşturuldu.";
        return RedirectToAction(nameof(Details), new { id = result.Data.Id });
    }

    // GET: LeaveType/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _leaveTypeService.GetByIdAsync(id);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<LeaveTypeEditViewModel>(result.Data);
        return View(viewModel);
    }

    // POST: LeaveType/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, LeaveTypeEditViewModel viewModel)
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

        var dto = _mapper.Map<BLL.DTOs.LeaveTypeUpdateDto>(viewModel);
        var result = await _leaveTypeService.UpdateAsync(dto);

        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return View(viewModel);
        }

        TempData["Success"] = "İzin türü başarıyla güncellendi.";
        return RedirectToAction(nameof(Details), new { id = viewModel.Id });
    }

    // GET: LeaveType/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _leaveTypeService.GetByIdAsync(id);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<LeaveTypeDetailViewModel>(result.Data);
        return View(viewModel);
    }

    // POST: LeaveType/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _leaveTypeService.DeleteAsync(id);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Delete), new { id });
        }

        TempData["Success"] = "İzin türü başarıyla silindi.";
        return RedirectToAction(nameof(Index));
    }

    // POST: LeaveType/ToggleStatus/5
    [HttpPost]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        var result = await _leaveTypeService.ToggleStatusAsync(id);
        if (!result.IsSuccess)
        {
            return Json(new { success = false, message = result.Message });
        }

        return Json(new { success = true, message = "Durum başarıyla güncellendi." });
    }

    // GET: LeaveType/Statistics/5
    public async Task<IActionResult> Statistics(int id, int year = 0)
    {
        if (year == 0) year = DateTime.Now.Year;

        var result = await _leaveTypeService.GetLeaveTypeStatisticsAsync(id, year);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Details), new { id });
        }

        ViewData["LeaveTypeId"] = id;
        ViewData["Year"] = year;
        return View(result.Data);
    }

    // GET: LeaveType/AllStatistics
    public async Task<IActionResult> AllStatistics(int year = 0)
    {
        if (year == 0) year = DateTime.Now.Year;

        var result = await _leaveTypeService.GetAllLeaveTypesStatisticsAsync(year);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return View(new Dictionary<string, object>());
        }

        ViewData["Year"] = year;
        return View(result.Data);
    }

    // GET: LeaveType/MostUsed
    public async Task<IActionResult> MostUsed(int year = 0, int count = 5)
    {
        if (year == 0) year = DateTime.Now.Year;

        var result = await _leaveTypeService.GetMostUsedLeaveTypesAsync(year, count);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return View(new List<LeaveTypeListViewModel>());
        }

        var viewModels = _mapper.Map<List<LeaveTypeListViewModel>>(result.Data);
        ViewData["Year"] = year;
        ViewData["Count"] = count;
        return View(viewModels);
    }

    // Debug endpoint for troubleshooting
    public async Task<IActionResult> Debug()
    {
        try
        {
            var allResult = await _leaveTypeService.GetAllAsync();
            var activeResult = await _leaveTypeService.GetActiveAsync();

            var debugInfo = new
            {
                AllLeaveTypesCount = allResult.IsSuccess ? allResult.Data.Count() : 0,
                AllLeaveTypesSuccess = allResult.IsSuccess,
                AllLeaveTypesMessage = allResult.Message,
                ActiveLeaveTypesCount = activeResult.IsSuccess ? activeResult.Data.Count() : 0,
                ActiveLeaveTypesSuccess = activeResult.IsSuccess,
                ActiveLeaveTypesMessage = activeResult.Message,
                AllLeaveTypes = allResult.IsSuccess ? allResult.Data.Take(5) : null,
                ActiveLeaveTypes = activeResult.IsSuccess ? activeResult.Data.Take(5) : null
            };

            return Json(debugInfo);
        }
        catch (Exception ex)
        {
            return Json(new { Error = ex.Message, StackTrace = ex.StackTrace });
        }
    }
}
