using AutoMapper;
using BLL.DTOs;
using BLL.Services;
using BLL.Services.Export;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Models;
using MVC.Services;

namespace MVC.Controllers;

/// <summary>
/// Bordro (Payroll) Controller
/// Bordro yönetimi işlemlerini gerçekleştirir
/// </summary>
[Authorize(Roles = "Admin,Manager")] // Sadece Admin ve Manager erişebilir
public class PayrollController : Controller
{
    private readonly IPayrollService _payrollService;
    private readonly IPersonService _personService;
    private readonly IDepartmentService _departmentService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IExcelExportService _excelExportService;
    private readonly IMapper _mapper;
    private readonly ILogger<PayrollController> _logger;

    public PayrollController(
        IPayrollService payrollService,
        IPersonService personService,
        IDepartmentService departmentService,
        ICurrentUserService currentUserService,
        IExcelExportService excelExportService,
        IMapper mapper,
        ILogger<PayrollController> logger)
    {
        _payrollService = payrollService;
        _personService = personService;
        _departmentService = departmentService;
        _currentUserService = currentUserService;
        _excelExportService = excelExportService;
        _mapper = mapper;
        _logger = logger;
    }

    #region Index & List Actions

    /// <summary>
    /// Bordro listesi sayfası
    /// Admin: Tüm bordroları görebilir
    /// Manager: Sadece kendi departmanının bordrolarını görebilir
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(PayrollFilterViewModel? filter)
    {
        try
        {
            filter ??= new PayrollFilterViewModel();

            // Manager rolü için departman filtresi uygula
            if (_currentUserService.IsInRole("Manager") && !_currentUserService.IsInRole("Admin"))
            {
                var managerDepartmentId = _currentUserService.DepartmentId;
                if (managerDepartmentId.HasValue)
                {
                    filter.DepartmentId = managerDepartmentId.Value;
                }
                else
                {
                    TempData["Warning"] = "Departman bilginiz bulunamadı. Lütfen yöneticinize başvurun.";
                    return View(new List<PayrollListViewModel>());
                }
            }

            // Filtre DTO'ya dönüştür
            var filterDto = _mapper.Map<PayrollFilterDto>(filter);

            // Bordrolar getir
            var result = filterDto.PersonId.HasValue || filterDto.DepartmentId.HasValue || 
                        filterDto.Year.HasValue || filterDto.Month.HasValue || 
                        filterDto.MinNetSalary.HasValue || filterDto.MaxNetSalary.HasValue
                ? await _payrollService.SearchAsync(filterDto)
                : await _payrollService.GetAllAsync();

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return View(new List<PayrollListViewModel>());
            }

            // ViewModel'e dönüştür
            var payrollViewModels = _mapper.Map<IEnumerable<PayrollListViewModel>>(result.Data);

            // Filtreleme dropdown'ları yükle
            await LoadFilterDropdowns(filter);

            return View(payrollViewModels);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Bordro listesi yüklenirken hata oluştu");
            TempData["Error"] = "Bordro listesi yüklenirken bir hata oluştu.";
            return View(new List<PayrollListViewModel>());
        }
    }

    #endregion

    #region Details Action

    /// <summary>
    /// Bordro detay sayfası
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var result = await _payrollService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            // Manager için departman kontrolü
            if (_currentUserService.IsInRole("Manager") && !_currentUserService.IsInRole("Admin"))
            {
                var managerDepartmentId = _currentUserService.DepartmentId;
                var personResult = await _personService.GetByIdAsync(result.Data.PersonId);
                
                if (!personResult.Success || 
                    !managerDepartmentId.HasValue || 
                    personResult.Data.DepartmentId != managerDepartmentId.Value)
                {
                    TempData["Error"] = "Bu bordroyu görüntüleme yetkiniz bulunmamaktadır.";
                    return RedirectToAction(nameof(Index));
                }
            }

            var viewModel = _mapper.Map<PayrollDetailViewModel>(result.Data);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Bordro detayı yüklenirken hata oluştu. ID: {PayrollId}", id);
            TempData["Error"] = "Bordro detayı yüklenirken bir hata oluştu.";
            return RedirectToAction(nameof(Index));
        }
    }

    #endregion

    #region Create Actions

    /// <summary>
    /// Yeni bordro oluşturma formu
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            var viewModel = new PayrollCreateViewModel();
            await LoadCreateDropdowns(viewModel);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Bordro oluşturma formu yüklenirken hata oluştu");
            TempData["Error"] = "Form yüklenirken bir hata oluştu.";
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Yeni bordro oluşturma işlemi
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PayrollCreateViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await LoadCreateDropdowns(model);
                return View(model);
            }

            // Manager için kısıtlama - sadece kendi departmanındaki personele bordro oluşturabilir
            if (_currentUserService.IsInRole("Manager") && !_currentUserService.IsInRole("Admin"))
            {
                var managerDepartmentId = _currentUserService.DepartmentId;
                var personResult = await _personService.GetByIdAsync(model.PersonId);
                
                if (!personResult.Success || 
                    !managerDepartmentId.HasValue || 
                    personResult.Data.DepartmentId != managerDepartmentId.Value)
                {
                    ModelState.AddModelError("PersonId", "Sadece kendi departmanınızdaki personeller için bordro oluşturabilirsiniz.");
                    await LoadCreateDropdowns(model);
                    return View(model);
                }
            }

            // Bordroyu hazırlayan kişi bilgisini ekle
            model.PreparedById = _currentUserService.PersonId;

            // DTO'ya dönüştür ve oluştur
            var createDto = _mapper.Map<PayrollCreateDto>(model);
            var result = await _payrollService.CreateAsync(createDto);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                await LoadCreateDropdowns(model);
                return View(model);
            }

            TempData["Success"] = $"{model.Year}/{model.Month} dönemi bordrosu başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Details), new { id = result.Data.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Bordro oluşturulurken hata oluştu");
            ModelState.AddModelError(string.Empty, "Bordro oluşturulurken bir hata oluştu.");
            await LoadCreateDropdowns(model);
            return View(model);
        }
    }

    #endregion

    #region Edit Actions

    /// <summary>
    /// Bordro düzenleme formu
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var result = await _payrollService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            // Manager için departman kontrolü
            if (_currentUserService.IsInRole("Manager") && !_currentUserService.IsInRole("Admin"))
            {
                var managerDepartmentId = _currentUserService.DepartmentId;
                var personResult = await _personService.GetByIdAsync(result.Data.PersonId);
                
                if (!personResult.Success || 
                    !managerDepartmentId.HasValue || 
                    personResult.Data.DepartmentId != managerDepartmentId.Value)
                {
                    TempData["Error"] = "Bu bordroyu düzenleme yetkiniz bulunmamaktadır.";
                    return RedirectToAction(nameof(Index));
                }
            }

            var viewModel = _mapper.Map<PayrollEditViewModel>(result.Data);
            await LoadEditDropdowns(viewModel);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Bordro düzenleme formu yüklenirken hata oluştu. ID: {PayrollId}", id);
            TempData["Error"] = "Form yüklenirken bir hata oluştu.";
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Bordro düzenleme işlemi
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PayrollEditViewModel model)
    {
        try
        {
            if (id != model.Id)
            {
                TempData["Error"] = "Geçersiz bordro ID.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                await LoadEditDropdowns(model);
                return View(model);
            }

            // Manager için departman kontrolü
            if (_currentUserService.IsInRole("Manager") && !_currentUserService.IsInRole("Admin"))
            {
                var managerDepartmentId = _currentUserService.DepartmentId;
                var personResult = await _personService.GetByIdAsync(model.PersonId);
                
                if (!personResult.Success || 
                    !managerDepartmentId.HasValue || 
                    personResult.Data.DepartmentId != managerDepartmentId.Value)
                {
                    TempData["Error"] = "Bu bordroyu düzenleme yetkiniz bulunmamaktadır.";
                    return RedirectToAction(nameof(Index));
                }
            }

            // DTO'ya dönüştür ve güncelle
            var updateDto = _mapper.Map<PayrollUpdateDto>(model);
            var result = await _payrollService.UpdateAsync(updateDto);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                await LoadEditDropdowns(model);
                return View(model);
            }

            TempData["Success"] = "Bordro başarıyla güncellendi.";
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Bordro güncellenirken hata oluştu. ID: {PayrollId}", id);
            ModelState.AddModelError(string.Empty, "Bordro güncellenirken bir hata oluştu.");
            await LoadEditDropdowns(model);
            return View(model);
        }
    }

    #endregion

    #region Delete Action

    /// <summary>
    /// Bordro silme işlemi
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            // Önce bordroyu kontrol et
            var payrollResult = await _payrollService.GetByIdAsync(id);
            if (!payrollResult.IsSuccess)
            {
                return Json(new { success = false, message = payrollResult.Message });
            }

            // Manager için departman kontrolü
            if (_currentUserService.IsInRole("Manager") && !_currentUserService.IsInRole("Admin"))
            {
                var managerDepartmentId = _currentUserService.DepartmentId;
                var personResult = await _personService.GetByIdAsync(payrollResult.Data.PersonId);
                
                if (!personResult.Success || 
                    !managerDepartmentId.HasValue || 
                    personResult.Data.DepartmentId != managerDepartmentId.Value)
                {
                    return Json(new { success = false, message = "Bu bordroyu silme yetkiniz bulunmamaktadır." });
                }
            }

            var result = await _payrollService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = true, message = "Bordro başarıyla silindi." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Bordro silinirken hata oluştu. ID: {PayrollId}", id);
            return Json(new { success = false, message = "Bordro silinirken bir hata oluştu." });
        }
    }

    #endregion

    #region Reports Actions

    /// <summary>
    /// Dönem bordro özet raporu
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> PeriodSummary(int? year, int? month, int? departmentId)
    {
        try
        {
            year ??= DateTime.Now.Year;
            month ??= DateTime.Now.Month;

            // ViewBag'leri ayarla
            ViewBag.SelectedYear = year;
            ViewBag.SelectedMonth = month;
            ViewBag.SelectedDepartmentId = departmentId;

            // Departman dropdown'ını yükle
            await LoadDepartmentDropdown(departmentId);

            var result = await _payrollService.GetPeriodSummaryAsync(year.Value, month.Value);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return View(new PayrollSummaryViewModel());
            }

            var viewModel = _mapper.Map<PayrollSummaryViewModel>(result.Data);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Dönem özet raporu yüklenirken hata oluştu");
            TempData["Error"] = "Rapor yüklenirken bir hata oluştu.";
            return View(new PayrollSummaryViewModel());
        }
    }

    /// <summary>
    /// Personel yıllık bordro özeti
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> PersonYearlySummary(int? personId, int? year, int? departmentId)
    {
        try
        {
            year ??= DateTime.Now.Year;

            // ViewBag'leri ayarla
            ViewBag.SelectedPersonId = personId;
            ViewBag.SelectedYear = year;
            ViewBag.SelectedDepartmentId = departmentId;

            // Dropdown'ları yükle
            await LoadPersonDropdown(personId);
            await LoadDepartmentDropdown(departmentId);

            // Eğer personId yoksa sadece formu göster
            if (!personId.HasValue)
            {
                return View(null);
            }

            var result = await _payrollService.GetPersonYearlySummaryAsync(personId.Value, year.Value);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return View(null);
            }

            var viewModel = _mapper.Map<PersonYearlyPayrollSummaryViewModel>(result.Data);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Personel yıllık özet raporu yüklenirken hata oluştu");
            TempData["Error"] = "Rapor yüklenirken bir hata oluştu.";
            return View(null);
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Personel dropdown'ını yükler
    /// </summary>
    private async Task LoadPersonDropdown(int? selectedPersonId)
    {
        try
        {
            var personResult = await _personService.GetAllAsync();
            if (personResult.Success)
            {
                var persons = personResult.Data;

                // Manager için departman filtresi
                if (_currentUserService.IsInRole("Manager") && !_currentUserService.IsInRole("Admin"))
                {
                    var managerDepartmentId = _currentUserService.DepartmentId;
                    if (managerDepartmentId.HasValue)
                    {
                        persons = persons.Where(p => p.DepartmentId == managerDepartmentId.Value);
                    }
                }

                var selectList = persons.Select(p => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.FirstName} {p.LastName} ({p.EmployeeNumber}) - {p.DepartmentName}",
                    Selected = selectedPersonId.HasValue && p.Id == selectedPersonId.Value
                }).ToList();

                ViewBag.PersonSelectList = selectList;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Personel dropdown'ı yüklenirken hata oluştu");
        }
    }

    /// <summary>
    /// Departman dropdown'ını yükler
    /// </summary>
    private async Task LoadDepartmentDropdown(int? selectedDepartmentId)
    {
        try
        {
            var departmentResult = await _departmentService.GetAllAsync();
            if (departmentResult.Success)
            {
                var departments = departmentResult.Data;

                // Manager için departman filtresi
                if (_currentUserService.IsInRole("Manager") && !_currentUserService.IsInRole("Admin"))
                {
                    var managerDepartmentId = _currentUserService.DepartmentId;
                    if (managerDepartmentId.HasValue)
                    {
                        departments = departments.Where(d => d.Id == managerDepartmentId.Value);
                    }
                }

                var selectList = departments.Select(d => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name,
                    Selected = selectedDepartmentId.HasValue && d.Id == selectedDepartmentId.Value
                }).ToList();

                ViewBag.DepartmentSelectList = selectList;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Departman dropdown'ı yüklenirken hata oluştu");
        }
    }

    /// <summary>
    /// Filtreleme dropdown'larını yükler
    /// </summary>
    private async Task LoadFilterDropdowns(PayrollFilterViewModel filter)
    {
        try
        {
            // Personel listesi (Manager için sadece kendi departmanı)
            var personResult = await _personService.GetAllAsync();
            if (personResult.Success)
            {
                var persons = personResult.Data;

                // Manager için departman filtresi
                if (_currentUserService.IsInRole("Manager") && !_currentUserService.IsInRole("Admin"))
                {
                    var managerDepartmentId = _currentUserService.DepartmentId;
                    if (managerDepartmentId.HasValue)
                    {
                        persons = persons.Where(p => p.DepartmentId == managerDepartmentId.Value);
                    }
                }

                ViewBag.PersonList = new SelectList(
                    persons.Select(p => new { Id = p.Id, FullName = $"{p.FirstName} {p.LastName} ({p.EmployeeNumber})" }),
                    "Id", "FullName", filter.PersonId);
            }

            // Departman listesi (Manager için sadece kendi departmanı)
            var departmentResult = await _departmentService.GetAllAsync();
            if (departmentResult.Success)
            {
                var departments = departmentResult.Data;

                // Manager için departman filtresi
                if (_currentUserService.IsInRole("Manager") && !_currentUserService.IsInRole("Admin"))
                {
                    var managerDepartmentId = _currentUserService.DepartmentId;
                    if (managerDepartmentId.HasValue)
                    {
                        departments = departments.Where(d => d.Id == managerDepartmentId.Value);
                    }
                }

                ViewBag.DepartmentList = new SelectList(departments, "Id", "Name", filter.DepartmentId);
            }

            // Yıl listesi
            var years = Enumerable.Range(2020, 11).Select(y => new { Value = y, Text = y.ToString() });
            ViewBag.YearList = new SelectList(years, "Value", "Text", filter.Year);

            // Ay listesi
            var months = new[]
            {
                new { Value = 1, Text = "Ocak" }, new { Value = 2, Text = "Şubat" },
                new { Value = 3, Text = "Mart" }, new { Value = 4, Text = "Nisan" },
                new { Value = 5, Text = "Mayıs" }, new { Value = 6, Text = "Haziran" },
                new { Value = 7, Text = "Temmuz" }, new { Value = 8, Text = "Ağustos" },
                new { Value = 9, Text = "Eylül" }, new { Value = 10, Text = "Ekim" },
                new { Value = 11, Text = "Kasım" }, new { Value = 12, Text = "Aralık" }
            };
            ViewBag.MonthList = new SelectList(months, "Value", "Text", filter.Month);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Filtre dropdown'ları yüklenirken hata oluştu");
        }
    }

    /// <summary>
    /// Bordro oluşturma dropdown'larını yükler
    /// </summary>
    private async Task LoadCreateDropdowns(PayrollCreateViewModel model)
    {
        try
        {
            // Personel listesi (Manager için sadece kendi departmanı)
            var personResult = await _personService.GetAllAsync();
            if (personResult.Success)
            {
                var persons = personResult.Data;

                // Manager için departman filtresi
                if (_currentUserService.IsInRole("Manager") && !_currentUserService.IsInRole("Admin"))
                {
                    var managerDepartmentId = _currentUserService.DepartmentId;
                    if (managerDepartmentId.HasValue)
                    {
                        persons = persons.Where(p => p.DepartmentId == managerDepartmentId.Value);
                    }
                }

                ViewBag.PersonList = new SelectList(
                    persons.Select(p => new { Id = p.Id, FullName = $"{p.FirstName} {p.LastName} ({p.EmployeeNumber}) - {p.DepartmentName}" }),
                    "Id", "FullName", model.PersonId);

                // Bordroyu hazırlayan kişi listesi (Admin ve Manager'lar)
                ViewBag.PreparedByList = new SelectList(
                    persons.Where(p => p.DepartmentName?.Contains("İnsan Kaynakları") == true || 
                                      p.PositionName?.Contains("Yönetici") == true)
                           .Select(p => new { Id = p.Id, FullName = $"{p.FirstName} {p.LastName}" }),
                    "Id", "FullName", model.PreparedById);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Oluşturma dropdown'ları yüklenirken hata oluştu");
        }
    }

    /// <summary>
    /// Bordro düzenleme dropdown'larını yükler
    /// </summary>
    private async Task LoadEditDropdowns(PayrollEditViewModel model)
    {
        try
        {
            // Mevcut personel bilgilerini getir
            var personResult = await _personService.GetByIdAsync(model.PersonId);
            if (personResult.Success)
            {
                model.PersonFullName = $"{personResult.Data.FirstName} {personResult.Data.LastName}";
                model.DepartmentName = personResult.Data.DepartmentName ?? "";
            }

            // Personel listesi (Manager için sadece kendi departmanı)
            var allPersonsResult = await _personService.GetAllAsync();
            if (allPersonsResult.Success)
            {
                var persons = allPersonsResult.Data;

                // Manager için departman filtresi
                if (_currentUserService.IsInRole("Manager") && !_currentUserService.IsInRole("Admin"))
                {
                    var managerDepartmentId = _currentUserService.DepartmentId;
                    if (managerDepartmentId.HasValue)
                    {
                        persons = persons.Where(p => p.DepartmentId == managerDepartmentId.Value);
                    }
                }

                ViewBag.PersonList = new SelectList(
                    persons.Select(p => new { Id = p.Id, FullName = $"{p.FirstName} {p.LastName} ({p.EmployeeNumber}) - {p.DepartmentName}" }),
                    "Id", "FullName", model.PersonId);

                // Bordroyu hazırlayan kişi listesi
                ViewBag.PreparedByList = new SelectList(
                    persons.Where(p => p.DepartmentName?.Contains("İnsan Kaynakları") == true || 
                                      p.PositionName?.Contains("Yönetici") == true)
                           .Select(p => new { Id = p.Id, FullName = $"{p.FirstName} {p.LastName}" }),
                    "Id", "FullName", model.PreparedById);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Düzenleme dropdown'ları yüklenirken hata oluştu");
        }
    }

    #endregion

    #region Export Actions

    /// <summary>
    /// Dönem bordro özetini Excel'e aktar
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> ExportPeriodSummary(int year, int month, int? departmentId)
    {
        try
        {
            var result = await _payrollService.GetPeriodSummaryAsync(year, month);
            
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(PeriodSummary), new { year, month, departmentId });
            }

            var excelData = await _excelExportService.ExportAsync(new[] { result.Data! }, $"Bordro_Dönem_Özeti_{year}_{month:D2}");
            
            var fileName = $"Bordro_Dönem_Özeti_{year}_{month:D2}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            return File(excelData, "text/csv", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Dönem özeti Excel export hatası: {@Year}, {@Month}", year, month);
            TempData["Error"] = $"Excel export hatası: {ex.Message}";
            return RedirectToAction(nameof(PeriodSummary), new { year, month, departmentId });
        }
    }

    /// <summary>
    /// Personel yıllık bordro özetini Excel'e aktar
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> ExportPersonYearlySummary(int? personId, int year, int? departmentId)
    {
        try
        {
            if (!personId.HasValue)
            {
                TempData["Warning"] = "Lütfen bir personel seçin.";
                return RedirectToAction(nameof(PersonYearlySummary), new { personId, year, departmentId });
            }

            var result = await _payrollService.GetPersonYearlySummaryAsync(personId.Value, year);
            
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(PersonYearlySummary), new { personId, year, departmentId });
            }

            var excelData = await _excelExportService.ExportAsync(new[] { result.Data! }, $"Personel_Yıllık_Bordro_Özeti_{year}");
            
            var fileName = $"Personel_Yıllık_Bordro_Özeti_{year}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            return File(excelData, "text/csv", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Personel yıllık özet Excel export hatası: {@PersonId}, {@Year}", personId, year);
            TempData["Error"] = $"Excel export hatası: {ex.Message}";
            return RedirectToAction(nameof(PersonYearlySummary), new { personId, year, departmentId });
        }
    }

    #endregion
}
