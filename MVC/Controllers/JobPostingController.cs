using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;
using BLL.Services;
using BLL.DTOs;
using MVC.Models;
using BLL.Services.Export;
using DAL.Entities;

namespace MVC.Controllers;

public class JobPostingController : Controller
{
    private readonly IJobPostingService _jobPostingService;
    private readonly IPositionService _positionService;
    private readonly IDepartmentService _departmentService;
    private readonly IMapper _mapper;
    private readonly IExcelExportService _excelExportService;

    public JobPostingController(
        IJobPostingService jobPostingService,
        IPositionService positionService,
        IDepartmentService departmentService,
        IMapper mapper,
        IExcelExportService excelExportService)
    {
        _jobPostingService = jobPostingService;
        _positionService = positionService;
        _departmentService = departmentService;
        _mapper = mapper;
        _excelExportService = excelExportService;
    }

    // GET: JobPosting
    public async Task<IActionResult> Index(JobPostingFilterDto? filter)
    {
        try
        {
            var result = await _jobPostingService.GetFilteredAsync(filter);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return View(new List<JobPostingListViewModel>());
            }

            var viewModels = _mapper.Map<List<JobPostingListViewModel>>(result.Data);
            
            // Load filter data
            await LoadFilterDropdowns(filter);
            
            return View(viewModels);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"İş ilanları yüklenirken hata oluştu: {ex.Message}";
            return View(new List<JobPostingListViewModel>());
        }
    }

    // GET: JobPosting/Details/5
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var result = await _jobPostingService.GetByIdAsync(id);
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["Error"] = result.Message ?? "İş ilanı bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = _mapper.Map<JobPostingDetailViewModel>(result.Data);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"İş ilanı detayları yüklenirken hata oluştu: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: JobPosting/Create
    public async Task<IActionResult> Create()
    {
        try
        {
            await LoadCreateDropdowns();
            return View(new JobPostingCreateViewModel());
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Sayfa yüklenirken hata oluştu: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: JobPosting/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(JobPostingCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadCreateDropdowns();
            return View(model);
        }

        try
        {
            var dto = _mapper.Map<JobPostingCreateDto>(model);
            var result = await _jobPostingService.CreateAsync(dto);

            if (result.IsSuccess)
            {
                TempData["Success"] = "İş ilanı başarıyla oluşturuldu.";
                return RedirectToAction(nameof(Details), new { id = result.Data!.Id });
            }

            ModelState.AddModelError("", result.Message!);
            await LoadCreateDropdowns();
            return View(model);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"İş ilanı oluşturulurken hata oluştu: {ex.Message}";
            await LoadCreateDropdowns();
            return View(model);
        }
    }

    // GET: JobPosting/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var result = await _jobPostingService.GetByIdAsync(id);
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["Error"] = result.Message ?? "İş ilanı bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = _mapper.Map<JobPostingEditViewModel>(result.Data);
            await LoadEditDropdowns(viewModel);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"İş ilanı düzenleme sayfası yüklenirken hata oluştu: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: JobPosting/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, JobPostingEditViewModel model)
    {
        if (id != model.Id)
        {
            TempData["Error"] = "Geçersiz iş ilanı ID'si.";
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            await LoadEditDropdowns(model);
            return View(model);
        }

        try
        {
            var dto = _mapper.Map<JobPostingUpdateDto>(model);
            var result = await _jobPostingService.UpdateAsync(dto);

            if (result.IsSuccess)
            {
                TempData["Success"] = "İş ilanı başarıyla güncellendi.";
                return RedirectToAction(nameof(Details), new { id });
            }

            ModelState.AddModelError("", result.Message!);
            await LoadEditDropdowns(model);
            return View(model);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"İş ilanı güncellenirken hata oluştu: {ex.Message}";
            await LoadEditDropdowns(model);
            return View(model);
        }
    }

    // POST: JobPosting/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _jobPostingService.DeleteAsync(id);
            
            if (result.IsSuccess)
            {
                TempData["Success"] = "İş ilanı başarıyla silindi.";
            }
            else
            {
                TempData["Error"] = result.Message!;
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"İş ilanı silinirken hata oluştu: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: JobPosting/Publish/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Publish(int id)
    {
        try
        {
            // For now, use a placeholder user ID (1). In a real app, get this from authentication
            var result = await _jobPostingService.PublishAsync(id, 1);
            
            if (result.IsSuccess)
            {
                TempData["Success"] = "İş ilanı başarıyla yayınlandı.";
            }
            else
            {
                TempData["Error"] = result.Message!;
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"İş ilanı yayınlanırken hata oluştu: {ex.Message}";
        }

        return RedirectToAction(nameof(Details), new { id });
    }

    // POST: JobPosting/Suspend/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Suspend(int id)
    {
        try
        {
            // For now, use a placeholder user ID (1). In a real app, get this from authentication
            var result = await _jobPostingService.SuspendAsync(id, 1);
            
            if (result.IsSuccess)
            {
                TempData["Success"] = "İş ilanı başarıyla durduruldu.";
            }
            else
            {
                TempData["Error"] = result.Message!;
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"İş ilanı durdurulurken hata oluştu: {ex.Message}";
        }

        return RedirectToAction(nameof(Details), new { id });
    }

    // POST: JobPosting/Close/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Close(int id)
    {
        try
        {
            // For now, use a placeholder user ID (1). In a real app, get this from authentication
            var result = await _jobPostingService.CloseAsync(id, 1);
            
            if (result.IsSuccess)
            {
                TempData["Success"] = "İş ilanı başarıyla kapatıldı.";
            }
            else
            {
                TempData["Error"] = result.Message!;
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"İş ilanı kapatılırken hata oluştu: {ex.Message}";
        }

        return RedirectToAction(nameof(Details), new { id });
    }

    // GET: JobPosting/Statistics
    public async Task<IActionResult> Statistics()
    {
        try
        {
            var summaryResult = await _jobPostingService.GetSummaryAsync();
            var monthlyResult = await _jobPostingService.GetMonthlyPostingSummaryAsync(DateTime.Now.Year);
            var departmentResult = await _jobPostingService.GetDepartmentPostingSummaryAsync();

            if (!summaryResult.IsSuccess || !monthlyResult.IsSuccess || !departmentResult.IsSuccess)
            {
                TempData["Error"] = "İstatistikler yüklenirken hata oluştu.";
                return View();
            }

            ViewBag.Summary = summaryResult.Data;
            ViewBag.MonthlyData = monthlyResult.Data;
            ViewBag.DepartmentData = departmentResult.Data;

            return View();
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"İstatistikler yüklenirken hata oluştu: {ex.Message}";
            return View();
        }
    }

    // POST: JobPosting/ExportToExcel
    [HttpPost]
    public async Task<IActionResult> ExportToExcel(JobPostingFilterDto? filter)
    {
        try
        {
            var result = await _jobPostingService.GetFilteredAsync(filter);
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["Error"] = "Export edilecek veri bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var exportData = await _excelExportService.ExportAsync(result.Data, "İş İlanları");
            return File(exportData, "text/csv", "Is_Ilanlari.csv");
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Export işlemi sırasında hata oluştu: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    #region Public Job Posting Actions (for candidates)

    // GET: JobPosting/Public
    public async Task<IActionResult> Public(PublicJobPostingFilterDto? filter)
    {
        try
        {
            var result = await _jobPostingService.GetPublicListingsAsync(filter);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return View(new List<PublicJobPostingListViewModel>());
            }

            var viewModels = _mapper.Map<List<PublicJobPostingListViewModel>>(result.Data);
            
            // Load filter data
            await LoadPublicFilterDropdowns(filter);
            
            return View(viewModels);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"İş ilanları yüklenirken hata oluştu: {ex.Message}";
            return View(new List<PublicJobPostingListViewModel>());
        }
    }

    // GET: JobPosting/PublicDetails/5
    public async Task<IActionResult> PublicDetails(int id)
    {
        try
        {
            var result = await _jobPostingService.GetPublicDetailsAsync(id);
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["Error"] = result.Message ?? "İş ilanı bulunamadı.";
                return RedirectToAction(nameof(Public));
            }

            var viewModel = _mapper.Map<PublicJobPostingDetailViewModel>(result.Data);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"İş ilanı detayları yüklenirken hata oluştu: {ex.Message}";
            return RedirectToAction(nameof(Public));
        }
    }

    #endregion

    #region Private Methods

    private async Task LoadFilterDropdowns(JobPostingFilterDto? filter)
    {
        // Positions
        var positionsResult = await _positionService.GetAllAsync();
        var positions = positionsResult.IsSuccess ? positionsResult.Data : new List<PositionListDto>();
        ViewBag.PositionSelectList = new SelectList(positions, "Id", "Name", filter?.PositionId);

        // Departments
        var departmentsResult = await _departmentService.GetAllAsync();
        var departments = departmentsResult.IsSuccess ? departmentsResult.Data : new List<DepartmentListDto>();
        ViewBag.DepartmentSelectList = new SelectList(departments, "Id", "Name", filter?.DepartmentId);

        // Status options
        var statusOptions = Enum.GetValues<JobPostingStatus>()
            .Select(s => new { Value = (int)s, Text = GetStatusText(s) })
            .ToList();
        ViewBag.StatusSelectList = new SelectList(statusOptions, "Value", "Text", (int?)filter?.Status);

        // Employment types
        var employmentTypes = Enum.GetValues<EmploymentType>()
            .Select(e => new { Value = (int)e, Text = GetEmploymentTypeText(e) })
            .ToList();
        ViewBag.EmploymentTypeSelectList = new SelectList(employmentTypes, "Value", "Text", (int?)filter?.EmploymentType);

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

        // Departments
        var departmentsResult = await _departmentService.GetAllAsync();
        var departments = departmentsResult.IsSuccess ? departmentsResult.Data!.Where(d => d.IsActive) : new List<DepartmentListDto>();
        ViewBag.DepartmentSelectList = new SelectList(departments, "Id", "Name");

        // Employment types
        var employmentTypes = Enum.GetValues<EmploymentType>()
            .Select(e => new { Value = (int)e, Text = GetEmploymentTypeText(e) })
            .ToList();
        ViewBag.EmploymentTypeSelectList = new SelectList(employmentTypes, "Value", "Text");
    }

    private async Task LoadEditDropdowns(JobPostingEditViewModel model)
    {
        // Positions
        var positionsResult = await _positionService.GetAllAsync();
        var positions = positionsResult.IsSuccess ? positionsResult.Data! : new List<PositionListDto>();
        ViewBag.PositionSelectList = new SelectList(positions, "Id", "Name", model.PositionId);

        // Departments
        var departmentsResult = await _departmentService.GetAllAsync();
        var departments = departmentsResult.IsSuccess ? departmentsResult.Data! : new List<DepartmentListDto>();
        ViewBag.DepartmentSelectList = new SelectList(departments, "Id", "Name", model.DepartmentId);

        // Status options
        var statusOptions = Enum.GetValues<JobPostingStatus>()
            .Select(s => new { Value = (int)s, Text = GetStatusText(s) })
            .ToList();
        ViewBag.StatusSelectList = new SelectList(statusOptions, "Value", "Text", (int)model.Status);

        // Employment types
        var employmentTypes = Enum.GetValues<EmploymentType>()
            .Select(e => new { Value = (int)e, Text = GetEmploymentTypeText(e) })
            .ToList();
        ViewBag.EmploymentTypeSelectList = new SelectList(employmentTypes, "Value", "Text", (int)model.EmploymentType);
    }

    private async Task LoadPublicFilterDropdowns(PublicJobPostingFilterDto? filter)
    {
        // Departments
        var departmentsResult = await _departmentService.GetAllAsync();
        var departments = departmentsResult.IsSuccess ? departmentsResult.Data!.Where(d => d.IsActive) : new List<DepartmentListDto>();
        ViewBag.DepartmentSelectList = new SelectList(departments, "Id", "Name", filter?.DepartmentId);

        // Employment types
        var employmentTypes = Enum.GetValues<EmploymentType>()
            .Select(e => new { Value = (int)e, Text = GetEmploymentTypeText(e) })
            .ToList();
        ViewBag.EmploymentTypeSelectList = new SelectList(employmentTypes, "Value", "Text", (int?)filter?.EmploymentType);

        ViewBag.SearchTerm = filter?.SearchTerm;
        ViewBag.Location = filter?.Location;
    }

    private static string GetStatusText(JobPostingStatus status)
    {
        return status switch
        {
            JobPostingStatus.Draft => "Taslak",
            JobPostingStatus.Active => "Aktif",
            JobPostingStatus.Suspended => "Durduruldu",
            JobPostingStatus.Closed => "Kapatıldı",
            JobPostingStatus.Expired => "Süresi Doldu",
            _ => "Bilinmiyor"
        };
    }

    private static string GetEmploymentTypeText(EmploymentType type)
    {
        return type switch
        {
            EmploymentType.FullTime => "Tam Zamanlı",
            EmploymentType.PartTime => "Yarı Zamanlı",
            EmploymentType.Contract => "Sözleşmeli",
            EmploymentType.Internship => "Staj",
            EmploymentType.Freelance => "Serbest Çalışan",
            _ => "Bilinmiyor"
        };
    }

    #endregion
}
