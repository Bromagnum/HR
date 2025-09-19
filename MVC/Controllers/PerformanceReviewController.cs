using AutoMapper;
using BLL.DTOs;
using BLL.Services;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Models;

namespace MVC.Controllers;

[Authorize]
public class PerformanceReviewController : Controller
{
    private readonly IPerformanceReviewService _performanceReviewService;
    private readonly IPersonService _personService;
    private readonly IDepartmentService _departmentService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public PerformanceReviewController(
        IPerformanceReviewService performanceReviewService,
        IPersonService personService,
        IDepartmentService departmentService,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _performanceReviewService = performanceReviewService;
        _personService = personService;
        _departmentService = departmentService;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    #region Performance Review Management

    // GET: PerformanceReview
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Index(int? departmentId = null, ReviewStatus? status = null)
    {
        try
        {
            var reviewsResult = await _performanceReviewService.GetAllAsync();
            if (!reviewsResult.IsSuccess)
            {
                TempData["Error"] = reviewsResult.Message;
                return View(new List<PerformanceReviewListDto>());
            }

            var reviews = reviewsResult.Data!.ToList();

            // Filter by department if specified
            if (departmentId.HasValue)
            {
                var departmentReviewsResult = await _performanceReviewService.GetByDepartmentIdAsync(departmentId.Value);
                if (departmentReviewsResult.IsSuccess)
                {
                    reviews = departmentReviewsResult.Data!.ToList();
                }
            }

            // Filter by status if specified
            if (status.HasValue)
            {
                reviews = reviews.Where(r => r.Status == status.Value).ToList();
            }

            // Populate filter dropdowns
            var departmentsResult = await _departmentService.GetAllAsync();
            if (departmentsResult.IsSuccess)
            {
                ViewBag.Departments = new SelectList(departmentsResult.Data!, "Id", "Name", departmentId);
            }

            ViewBag.Statuses = new SelectList(Enum.GetValues<ReviewStatus>()
                .Select(s => new { Value = (int)s, Text = GetStatusText(s) }), "Value", "Text", (int?)status);

            ViewBag.SelectedDepartmentId = departmentId;
            ViewBag.SelectedStatus = status;

            return View(reviews);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Performans değerlendirmeleri yüklenirken hata oluştu";
            return View(new List<PerformanceReviewListDto>());
        }
    }

    // GET: PerformanceReview/Details/5
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var result = await _performanceReviewService.GetByIdAsync(id);
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["Error"] = result.Message ?? "Performans değerlendirmesi bulunamadı";
                return RedirectToAction(nameof(Index));
            }

            var currentUserId = _currentUserService.GetCurrentUserId();
            var canView = await CanViewReview(result.Data, currentUserId);
            
            if (!canView)
            {
                TempData["Error"] = "Bu değerlendirmeyi görüntüleme yetkiniz yok";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Performans değerlendirmesi detayları yüklenirken hata oluştu";
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: PerformanceReview/Create
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create()
    {
        try
        {
            await PopulateCreateDropdowns();
            return View(new PerformanceReviewCreateViewModel());
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Sayfa yüklenirken hata oluştu";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: PerformanceReview/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create(PerformanceReviewCreateViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await PopulateCreateDropdowns();
                return View(model);
            }

            var dto = _mapper.Map<PerformanceReviewCreateDto>(model);
            var result = await _performanceReviewService.CreateAsync(dto);
            
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                await PopulateCreateDropdowns();
                return View(model);
            }

            TempData["Success"] = "Performans değerlendirmesi başarıyla oluşturuldu";
            return RedirectToAction(nameof(Details), new { id = result.Data!.Id });
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Performans değerlendirmesi oluşturulurken hata oluştu";
            await PopulateCreateDropdowns();
            return View(model);
        }
    }

    // GET: PerformanceReview/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var result = await _performanceReviewService.GetByIdAsync(id);
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["Error"] = result.Message ?? "Performans değerlendirmesi bulunamadı";
                return RedirectToAction(nameof(Index));
            }

            var currentUserId = _currentUserService.GetCurrentUserId();
            var canEditResult = await _performanceReviewService.CanEditReviewAsync(id, currentUserId);
            
            if (!canEditResult.IsSuccess || !canEditResult.Data)
            {
                TempData["Error"] = "Bu değerlendirmeyi düzenleme yetkiniz yok";
                return RedirectToAction(nameof(Details), new { id });
            }

            var model = _mapper.Map<PerformanceReviewEditViewModel>(result.Data);
            return View(model);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Performans değerlendirmesi düzenleme sayfası yüklenirken hata oluştu";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: PerformanceReview/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PerformanceReviewEditViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentUserId = _currentUserService.GetCurrentUserId();
            var canEditResult = await _performanceReviewService.CanEditReviewAsync(model.Id, currentUserId);
            
            if (!canEditResult.IsSuccess || !canEditResult.Data)
            {
                TempData["Error"] = "Bu değerlendirmeyi düzenleme yetkiniz yok";
                return RedirectToAction(nameof(Details), new { id = model.Id });
            }

            var dto = _mapper.Map<PerformanceReviewUpdateDto>(model);
            var result = await _performanceReviewService.UpdateAsync(dto);
            
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return View(model);
            }

            TempData["Success"] = "Performans değerlendirmesi başarıyla güncellendi";
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Performans değerlendirmesi güncellenirken hata oluştu";
            return View(model);
        }
    }

    #endregion

    #region Review Status Management

    // POST: PerformanceReview/Submit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(int id)
    {
        try
        {
            var result = await _performanceReviewService.SubmitReviewAsync(id);
            
            if (result.IsSuccess)
            {
                TempData["Success"] = "Performans değerlendirmesi başarıyla gönderildi";
            }
            else
            {
                TempData["Error"] = result.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Performans değerlendirmesi gönderilirken hata oluştu";
            return RedirectToAction(nameof(Details), new { id });
        }
    }

    // POST: PerformanceReview/Approve/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Approve(int id)
    {
        try
        {
            var currentUserId = _currentUserService.GetCurrentUserId();
            var result = await _performanceReviewService.ApproveReviewAsync(id, currentUserId);
            
            if (result.IsSuccess)
            {
                TempData["Success"] = "Performans değerlendirmesi başarıyla onaylandı";
            }
            else
            {
                TempData["Error"] = result.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Performans değerlendirmesi onaylanırken hata oluştu";
            return RedirectToAction(nameof(Details), new { id });
        }
    }

    // POST: PerformanceReview/Reject
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Reject(int id, string reason)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                TempData["Error"] = "Red nedeni belirtilmelidir";
                return RedirectToAction(nameof(Details), new { id });
            }

            var result = await _performanceReviewService.RejectReviewAsync(id, reason);
            
            if (result.IsSuccess)
            {
                TempData["Success"] = "Performans değerlendirmesi reddedildi";
            }
            else
            {
                TempData["Error"] = result.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Performans değerlendirmesi reddedilirken hata oluştu";
            return RedirectToAction(nameof(Details), new { id });
        }
    }

    #endregion

    #region Self Assessment

    // GET: PerformanceReview/SelfAssessment
    public async Task<IActionResult> SelfAssessment()
    {
        try
        {
            var currentUserId = _currentUserService.GetCurrentUserId();
            var reviewsResult = await _performanceReviewService.GetByPersonIdAsync(currentUserId);
            
            if (!reviewsResult.IsSuccess)
            {
                TempData["Error"] = reviewsResult.Message;
                return View(new List<PerformanceReviewListDto>());
            }

            var reviews = reviewsResult.Data!
                .Where(r => r.Status == ReviewStatus.EmployeeReview || !r.IsSelfAssessmentCompleted)
                .ToList();

            return View(reviews);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Öz değerlendirme sayfası yüklenirken hata oluştu";
            return View(new List<PerformanceReviewListDto>());
        }
    }

    // GET: PerformanceReview/CompleteSelfAssessment/5
    public async Task<IActionResult> CompleteSelfAssessment(int id)
    {
        try
        {
            var result = await _performanceReviewService.GetByIdAsync(id);
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["Error"] = result.Message ?? "Performans değerlendirmesi bulunamadı";
                return RedirectToAction(nameof(SelfAssessment));
            }

            var currentUserId = _currentUserService.GetCurrentUserId();
            if (result.Data.PersonId != currentUserId)
            {
                TempData["Error"] = "Bu öz değerlendirmeyi tamamlama yetkiniz yok";
                return RedirectToAction(nameof(SelfAssessment));
            }

            var model = new SelfAssessmentViewModel
            {
                PerformanceReviewId = result.Data.Id,
                PersonName = result.Data.PersonName,
                ReviewPeriodName = result.Data.ReviewPeriodName,
                SelfOverallScore = result.Data.SelfOverallScore ?? 3,
                SelfAssessmentComments = result.Data.SelfAssessmentComments,
                EmployeeComments = result.Data.EmployeeComments
            };

            return View(model);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Öz değerlendirme sayfası yüklenirken hata oluştu";
            return RedirectToAction(nameof(SelfAssessment));
        }
    }

    // POST: PerformanceReview/CompleteSelfAssessment
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CompleteSelfAssessment(SelfAssessmentViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var dto = _mapper.Map<SelfAssessmentDto>(model);
            var result = await _performanceReviewService.CompleteSelfAssessmentAsync(dto);
            
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return View(model);
            }

            TempData["Success"] = "Öz değerlendirme başarıyla tamamlandı";
            return RedirectToAction(nameof(SelfAssessment));
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Öz değerlendirme tamamlanırken hata oluştu";
            return View(model);
        }
    }

    #endregion

    #region Review Periods

    // GET: PerformanceReview/Periods
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Periods()
    {
        try
        {
            var result = await _performanceReviewService.GetAllPeriodsAsync();
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return View(new List<ReviewPeriodListDto>());
            }

            return View(result.Data!);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Değerlendirme dönemleri yüklenirken hata oluştu";
            return View(new List<ReviewPeriodListDto>());
        }
    }

    // GET: PerformanceReview/CreatePeriod
    [Authorize(Roles = "Admin")]
    public IActionResult CreatePeriod()
    {
        var model = new ReviewPeriodCreateViewModel();
        ViewBag.PeriodTypes = new SelectList(Enum.GetValues<ReviewPeriodType>()
            .Select(t => new { Value = (int)t, Text = GetPeriodTypeText(t) }), "Value", "Text");
        return View(model);
    }

    // POST: PerformanceReview/CreatePeriod
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreatePeriod(ReviewPeriodCreateViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PeriodTypes = new SelectList(Enum.GetValues<ReviewPeriodType>()
                    .Select(t => new { Value = (int)t, Text = GetPeriodTypeText(t) }), "Value", "Text", model.Type);
                return View(model);
            }

            var dto = _mapper.Map<ReviewPeriodCreateDto>(model);
            var result = await _performanceReviewService.CreatePeriodAsync(dto);
            
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                ViewBag.PeriodTypes = new SelectList(Enum.GetValues<ReviewPeriodType>()
                    .Select(t => new { Value = (int)t, Text = GetPeriodTypeText(t) }), "Value", "Text", model.Type);
                return View(model);
            }

            TempData["Success"] = "Değerlendirme dönemi başarıyla oluşturuldu";
            return RedirectToAction(nameof(Periods));
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Değerlendirme dönemi oluşturulurken hata oluştu";
            ViewBag.PeriodTypes = new SelectList(Enum.GetValues<ReviewPeriodType>()
                .Select(t => new { Value = (int)t, Text = GetPeriodTypeText(t) }), "Value", "Text", model.Type);
            return View(model);
        }
    }

    #endregion

    #region Analytics

    // GET: PerformanceReview/Analytics
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Analytics(int? departmentId = null, int? year = null)
    {
        try
        {
            var analyticsResult = await _performanceReviewService.GetPerformanceAnalyticsAsync(departmentId, year);
            if (!analyticsResult.IsSuccess)
            {
                TempData["Error"] = analyticsResult.Message;
                return View(new PerformanceAnalyticsDto());
            }

            // Populate filter dropdowns
            var departmentsResult = await _departmentService.GetAllAsync();
            if (departmentsResult.IsSuccess)
            {
                ViewBag.Departments = new SelectList(departmentsResult.Data!, "Id", "Name", departmentId);
            }

            var years = Enumerable.Range(DateTime.Now.Year - 5, 6).Reverse()
                .Select(y => new { Value = y, Text = y.ToString() });
            ViewBag.Years = new SelectList(years, "Value", "Text", year);

            ViewBag.SelectedDepartmentId = departmentId;
            ViewBag.SelectedYear = year;

            return View(analyticsResult.Data!);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Performans analitiği yüklenirken hata oluştu";
            return View(new PerformanceAnalyticsDto());
        }
    }

    #endregion

    #region Helper Methods

    private async Task PopulateCreateDropdowns()
    {
        // Get active employees
        var personsResult = await _personService.GetAllAsync();
        if (personsResult.IsSuccess)
        {
            var activePersons = personsResult.Data!.Where(p => p.IsActive).ToList();
            ViewBag.Persons = new SelectList(activePersons, "Id", "FullName");
            ViewBag.Reviewers = new SelectList(activePersons, "Id", "FullName");
        }

        // Get active review periods
        var periodsResult = await _performanceReviewService.GetActivePeriodsAsync();
        if (periodsResult.IsSuccess)
        {
            ViewBag.ReviewPeriods = new SelectList(periodsResult.Data!, "Id", "Name");
        }
    }

    private async Task<bool> CanViewReview(PerformanceReviewDetailDto review, int currentUserId)
    {
        // User can view if they are:
        // 1. The person being reviewed
        // 2. The reviewer
        // 3. Admin or Manager (handled by authorization)
        return review.PersonId == currentUserId || 
               review.ReviewerId == currentUserId ||
               User.IsInRole("Admin") || 
               User.IsInRole("Manager");
    }

    private static string GetStatusText(ReviewStatus status)
    {
        return status switch
        {
            ReviewStatus.Draft => "Taslak",
            ReviewStatus.InProgress => "Devam Ediyor",
            ReviewStatus.EmployeeReview => "Çalışan Değerlendirmesi",
            ReviewStatus.ManagerReview => "Yönetici Değerlendirmesi",
            ReviewStatus.Completed => "Tamamlandı",
            ReviewStatus.Approved => "Onaylandı",
            _ => "Bilinmiyor"
        };
    }

    private static string GetPeriodTypeText(ReviewPeriodType type)
    {
        return type switch
        {
            ReviewPeriodType.Monthly => "Aylık",
            ReviewPeriodType.Quarterly => "Üç Aylık",
            ReviewPeriodType.SemiAnnual => "Altı Aylık",
            ReviewPeriodType.Annual => "Yıllık",
            ReviewPeriodType.Custom => "Özel",
            _ => "Bilinmiyor"
        };
    }

    #endregion
}
