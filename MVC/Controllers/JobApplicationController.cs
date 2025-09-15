using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;
using BLL.Services;
using BLL.DTOs;
using MVC.Models;
using BLL.Services.Export;
using DAL.Entities;

namespace MVC.Controllers;

public class JobApplicationController : Controller
{
    private readonly IJobApplicationService _jobApplicationService;
    private readonly IJobPostingService _jobPostingService;
    private readonly IPositionService _positionService;
    private readonly IDepartmentService _departmentService;
    private readonly IMapper _mapper;
    private readonly IExcelExportService _excelExportService;

    public JobApplicationController(
        IJobApplicationService jobApplicationService,
        IJobPostingService jobPostingService,
        IPositionService positionService,
        IDepartmentService departmentService,
        IMapper mapper,
        IExcelExportService excelExportService)
    {
        _jobApplicationService = jobApplicationService;
        _jobPostingService = jobPostingService;
        _positionService = positionService;
        _departmentService = departmentService;
        _mapper = mapper;
        _excelExportService = excelExportService;
    }

    // GET: JobApplication
    public async Task<IActionResult> Index(JobApplicationFilterDto? filter)
    {
        try
        {
            var result = await _jobApplicationService.GetFilteredAsync(filter);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return View(new List<JobApplicationListViewModel>());
            }

            var viewModels = _mapper.Map<List<JobApplicationListViewModel>>(result.Data);
            
            // Load filter data
            await LoadFilterDropdowns(filter);
            
            return View(viewModels);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Başvurular yüklenirken hata oluştu: {ex.Message}";
            return View(new List<JobApplicationListViewModel>());
        }
    }

    // GET: JobApplication/Details/5
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var result = await _jobApplicationService.GetByIdAsync(id);
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["Error"] = result.Message ?? "Başvuru bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = _mapper.Map<JobApplicationDetailViewModel>(result.Data);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Başvuru detayları yüklenirken hata oluştu: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: JobApplication/Create
    public async Task<IActionResult> Create()
    {
        try
        {
            await LoadCreateDropdowns();
            return View(new JobApplicationCreateViewModel());
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Sayfa yüklenirken hata oluştu: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: JobApplication/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(JobApplicationCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadCreateDropdowns();
            return View(model);
        }

        try
        {
            var dto = _mapper.Map<JobApplicationCreateDto>(model);
            var result = await _jobApplicationService.CreateAsync(dto);

            if (result.IsSuccess)
            {
                TempData["Success"] = "Başvuru başarıyla oluşturuldu.";
                return RedirectToAction(nameof(Details), new { id = result.Data!.Id });
            }

            ModelState.AddModelError("", result.Message!);
            await LoadCreateDropdowns();
            return View(model);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Başvuru oluşturulurken hata oluştu: {ex.Message}";
            await LoadCreateDropdowns();
            return View(model);
        }
    }

    // GET: JobApplication/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var result = await _jobApplicationService.GetByIdAsync(id);
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["Error"] = result.Message ?? "Başvuru bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = _mapper.Map<JobApplicationEditViewModel>(result.Data);
            await LoadEditDropdowns(viewModel);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Başvuru düzenleme sayfası yüklenirken hata oluştu: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: JobApplication/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, JobApplicationEditViewModel model)
    {
        if (id != model.Id)
        {
            TempData["Error"] = "Geçersiz başvuru ID'si.";
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            await LoadEditDropdowns(model);
            return View(model);
        }

        try
        {
            var dto = _mapper.Map<JobApplicationUpdateDto>(model);
            var result = await _jobApplicationService.UpdateAsync(dto);

            if (result.IsSuccess)
            {
                TempData["Success"] = "Başvuru başarıyla güncellendi.";
                return RedirectToAction(nameof(Details), new { id });
            }

            ModelState.AddModelError("", result.Message!);
            await LoadEditDropdowns(model);
            return View(model);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Başvuru güncellenirken hata oluştu: {ex.Message}";
            await LoadEditDropdowns(model);
            return View(model);
        }
    }

    // POST: JobApplication/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _jobApplicationService.DeleteAsync(id);
            
            if (result.IsSuccess)
            {
                TempData["Success"] = "Başvuru başarıyla silindi.";
            }
            else
            {
                TempData["Error"] = result.Message!;
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Başvuru silinirken hata oluştu: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: JobApplication/ChangeStatus
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(int id, JobApplicationStatus newStatus)
    {
        try
        {
            // For now, use a placeholder user ID (1). In a real app, get this from authentication
            var result = await _jobApplicationService.ChangeStatusAsync(id, newStatus, 1, $"Status changed to {newStatus}");
            
            if (result.IsSuccess)
            {
                TempData["Success"] = "Başvuru durumu başarıyla güncellendi.";
            }
            else
            {
                TempData["Error"] = result.Message!;
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Başvuru durumu güncellenirken hata oluştu: {ex.Message}";
        }

        return RedirectToAction(nameof(Details), new { id });
    }

    // GET: JobApplication/Statistics
    public async Task<IActionResult> Statistics()
    {
        try
        {
            var summaryResult = await _jobApplicationService.GetSummaryAsync();
            var monthlyResult = await _jobApplicationService.GetMonthlyApplicationSummaryAsync(DateTime.Now.Year);
            var positionResult = await _jobApplicationService.GetPositionApplicationSummaryAsync();

            if (!summaryResult.IsSuccess || !monthlyResult.IsSuccess || !positionResult.IsSuccess)
            {
                TempData["Error"] = "İstatistikler yüklenirken hata oluştu.";
                return View();
            }

            ViewBag.Summary = summaryResult.Data;
            ViewBag.MonthlyData = monthlyResult.Data;
            ViewBag.PositionData = positionResult.Data;

            return View();
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"İstatistikler yüklenirken hata oluştu: {ex.Message}";
            return View();
        }
    }

    // POST: JobApplication/ExportToExcel
    [HttpPost]
    public async Task<IActionResult> ExportToExcel(JobApplicationFilterDto? filter)
    {
        try
        {
            var result = await _jobApplicationService.GetFilteredAsync(filter);
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["Error"] = "Export edilecek veri bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var exportData = await _excelExportService.ExportAsync(result.Data, "Başvurular");
            return File(exportData, "text/csv", "Basvurular.csv");
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Export işlemi sırasında hata oluştu: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    #region Private Methods

    private async Task LoadFilterDropdowns(JobApplicationFilterDto? filter)
    {
        // Positions
        var positionsResult = await _positionService.GetAllAsync();
        var positions = positionsResult.IsSuccess ? positionsResult.Data : new List<PositionListDto>();
        ViewBag.PositionSelectList = new SelectList(positions, "Id", "Name", filter?.PositionId);

        // Status options
        var statusOptions = Enum.GetValues<JobApplicationStatus>()
            .Select(s => new { Value = (int)s, Text = GetStatusText(s) })
            .ToList();
        ViewBag.StatusSelectList = new SelectList(statusOptions, "Value", "Text", (int?)filter?.Status);

        // Date ranges
        ViewBag.SelectedStartDate = filter?.StartDate?.ToString("yyyy-MM-dd");
        ViewBag.SelectedEndDate = filter?.EndDate?.ToString("yyyy-MM-dd");
        ViewBag.SearchTerm = filter?.SearchTerm;
    }

    private async Task LoadCreateDropdowns()
    {
        // Positions
        var positionsResult = await _positionService.GetAllAsync();
        var positions = positionsResult.IsSuccess ? positionsResult.Data!.Where(p => p.IsActive) : new List<PositionListDto>();
        ViewBag.PositionSelectList = new SelectList(positions, "Id", "Name");

        // Education levels
        var educationLevels = Enum.GetValues<EducationLevel>()
            .Select(e => new { Value = (int)e, Text = GetEducationLevelText(e) })
            .ToList();
        ViewBag.EducationLevelSelectList = new SelectList(educationLevels, "Value", "Text");
    }

    private async Task LoadEditDropdowns(JobApplicationEditViewModel model)
    {
        // Positions
        var positionsResult = await _positionService.GetAllAsync();
        var positions = positionsResult.IsSuccess ? positionsResult.Data! : new List<PositionListDto>();
        ViewBag.PositionSelectList = new SelectList(positions, "Id", "Name", model.PositionId);

        // Status options
        var statusOptions = Enum.GetValues<JobApplicationStatus>()
            .Select(s => new { Value = (int)s, Text = GetStatusText(s) })
            .ToList();
        ViewBag.StatusSelectList = new SelectList(statusOptions, "Value", "Text", (int)model.Status);

        // Education levels
        var educationLevels = Enum.GetValues<EducationLevel>()
            .Select(e => new { Value = (int)e, Text = GetEducationLevelText(e) })
            .ToList();
        ViewBag.EducationLevelSelectList = new SelectList(educationLevels, "Value", "Text", model.EducationLevel);
    }

    private static string GetStatusText(JobApplicationStatus status)
    {
        return status switch
        {
            JobApplicationStatus.Draft => "Taslak",
            JobApplicationStatus.Submitted => "Gönderildi",
            JobApplicationStatus.UnderReview => "İnceleme Aşamasında",
            JobApplicationStatus.Interviewed => "Mülakat Yapıldı",
            JobApplicationStatus.Approved => "Onaylandı",
            JobApplicationStatus.Rejected => "Reddedildi",
            JobApplicationStatus.Withdrawn => "Geri Çekildi",
            _ => "Bilinmiyor"
        };
    }

    private static string GetEducationLevelText(EducationLevel level)
    {
        return level switch
        {
            EducationLevel.None => "Yok",
            EducationLevel.Primary => "İlkokul",
            EducationLevel.Secondary => "Ortaokul",
            EducationLevel.HighSchool => "Lise",
            EducationLevel.AssociateDegree => "Ön Lisans",
            EducationLevel.BachelorsDegree => "Lisans",
            EducationLevel.MastersDegree => "Yüksek Lisans",
            EducationLevel.Doctorate => "Doktora",
            _ => "Bilinmiyor"
        };
    }

    #endregion
}
