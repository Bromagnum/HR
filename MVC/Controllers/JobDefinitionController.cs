using AutoMapper;
using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Models;

namespace MVC.Controllers;

public class JobDefinitionController : Controller
{
    private readonly IJobDefinitionService _jobDefinitionService;
    private readonly IPositionService _positionService;
    private readonly IDepartmentService _departmentService;
    private readonly IPersonService _personService;
    private readonly ISkillManagementService _skillManagementService;
    private readonly IMapper _mapper;
    private readonly ILogger<JobDefinitionController> _logger;

    public JobDefinitionController(
        IJobDefinitionService jobDefinitionService,
        IPositionService positionService,
        IDepartmentService departmentService,
        IPersonService personService,
        ISkillManagementService skillManagementService,
        IMapper mapper,
        ILogger<JobDefinitionController> logger)
    {
        _jobDefinitionService = jobDefinitionService;
        _positionService = positionService;
        _departmentService = departmentService;
        _personService = personService;
        _skillManagementService = skillManagementService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IActionResult> Index(JobDefinitionFilterViewModel? filter = null)
    {
        try
        {
            var filterDto = _mapper.Map<JobDefinitionFilterDto>(filter ?? new JobDefinitionFilterViewModel());
            var result = await _jobDefinitionService.GetFilteredAsync(filterDto);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return View(new List<JobDefinitionListViewModel>());
            }

            var viewModels = _mapper.Map<IEnumerable<JobDefinitionListViewModel>>(result.Data);
            await LoadSelectListsAsync();
            
            ViewBag.Filter = filter ?? new JobDefinitionFilterViewModel();
            return View(viewModels);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in JobDefinition Index");
            TempData["Error"] = "İş tanımları yüklenirken hata oluştu.";
            return View(new List<JobDefinitionListViewModel>());
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var result = await _jobDefinitionService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            var viewModel = _mapper.Map<JobDefinitionDetailViewModel>(result.Data);

            // Load matching results
            var matchingResult = await _jobDefinitionService.GetMatchingResultsAsync(id);
            if (matchingResult.IsSuccess)
            {
                viewModel.MatchingResults = _mapper.Map<List<QualificationMatchingResultViewModel>>(matchingResult.Data);
            }

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting job definition details for id: {Id}", id);
            TempData["Error"] = "İş tanımı detayları alınırken hata oluştu.";
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<IActionResult> Create()
    {
        try
        {
            await LoadSelectListsAsync();
            await LoadSkillSelectListsAsync();
            return View(new JobDefinitionCreateViewModel());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading create view");
            TempData["Error"] = "Sayfa yüklenirken hata oluştu.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(JobDefinitionCreateViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync();
                await LoadSkillSelectListsAsync();
                return View(model);
            }

            var dto = _mapper.Map<JobDefinitionCreateDto>(model);
            var result = await _jobDefinitionService.CreateAsync(dto);

            if (result.IsSuccess)
            {
                TempData["Success"] = "İş tanımı başarıyla oluşturuldu.";
                return RedirectToAction(nameof(Details), new { id = result.Data!.Id });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                await LoadSelectListsAsync();
                await LoadSkillSelectListsAsync();
                return View(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating job definition");
            ModelState.AddModelError("", "İş tanımı oluşturulurken hata oluştu.");
            await LoadSelectListsAsync();
            await LoadSkillSelectListsAsync();
            return View(model);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var result = await _jobDefinitionService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            if (result.Data!.IsApproved)
            {
                TempData["Warning"] = "Onaylanmış iş tanımları düzenlenemez. Yeni versiyon oluşturabilirsiniz.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var viewModel = _mapper.Map<JobDefinitionEditViewModel>(result.Data);
            await LoadSelectListsAsync();
            await LoadSkillSelectListsAsync();
            
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading edit view for id: {Id}", id);
            TempData["Error"] = "İş tanımı düzenleme sayfası yüklenirken hata oluştu.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(JobDefinitionEditViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync();
                await LoadSkillSelectListsAsync();
                return View(model);
            }

            var dto = _mapper.Map<JobDefinitionUpdateDto>(model);
            var result = await _jobDefinitionService.UpdateAsync(dto);

            if (result.IsSuccess)
            {
                TempData["Success"] = "İş tanımı başarıyla güncellendi.";
                return RedirectToAction(nameof(Details), new { id = model.Id });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                await LoadSelectListsAsync();
                await LoadSkillSelectListsAsync();
                return View(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating job definition: {Id}", model.Id);
            ModelState.AddModelError("", "İş tanımı güncellenirken hata oluştu.");
            await LoadSelectListsAsync();
            await LoadSkillSelectListsAsync();
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _jobDefinitionService.DeleteAsync(id);
            
            if (result.IsSuccess)
            {
                TempData["Success"] = "İş tanımı başarıyla silindi.";
            }
            else
            {
                TempData["Error"] = result.Message;
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting job definition: {Id}", id);
            TempData["Error"] = "İş tanımı silinirken hata oluştu.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id)
    {
        try
        {
            // TODO: Get current user ID from authentication
            var currentUserId = 1; // Placeholder
            
            var result = await _jobDefinitionService.ApproveAsync(id, currentUserId);
            
            if (result.IsSuccess)
            {
                TempData["Success"] = "İş tanımı başarıyla onaylandı.";
            }
            else
            {
                TempData["Error"] = result.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving job definition: {Id}", id);
            TempData["Error"] = "İş tanımı onaylanırken hata oluştu.";
            return RedirectToAction(nameof(Details), new { id });
        }
    }

    public async Task<IActionResult> CalculateMatches(int id)
    {
        try
        {
            var personsResult = await _personService.GetAllAsync();
            if (!personsResult.IsSuccess)
            {
                TempData["Error"] = "Personel listesi alınamadı.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var viewModel = new JobDefinitionMatchingViewModel
            {
                JobDefinitionId = id,
                AvailablePersons = _mapper.Map<List<PersonListViewModel>>(personsResult.Data)
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading calculate matches view for job: {Id}", id);
            TempData["Error"] = "Eşleşme hesaplama sayfası yüklenirken hata oluştu.";
            return RedirectToAction(nameof(Details), new { id });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CalculateMatches(JobDefinitionMatchingViewModel model)
    {
        try
        {
            if (model.SelectedPersonIds?.Any() == true)
            {
                var successCount = 0;
                foreach (var personId in model.SelectedPersonIds)
                {
                    var result = await _jobDefinitionService.CalculateMatchAsync(model.JobDefinitionId, personId);
                    if (result.IsSuccess)
                        successCount++;
                }

                TempData["Success"] = $"{successCount} personel için eşleşme hesaplama tamamlandı.";
            }
            else
            {
                // Calculate for all persons
                var request = new JobDefinitionMatchingRequestDto
                {
                    JobDefinitionId = model.JobDefinitionId,
                    RecalculateExisting = model.RecalculateExisting
                };

                var result = await _jobDefinitionService.CalculateAllMatchesAsync(request);
                if (result.IsSuccess)
                {
                    TempData["Success"] = $"Tüm personel için eşleşme hesaplama tamamlandı. {result.Data?.Count()} sonuç bulundu.";
                }
                else
                {
                    TempData["Error"] = result.Message;
                }
            }

            return RedirectToAction(nameof(Details), new { id = model.JobDefinitionId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating matches for job: {Id}", model.JobDefinitionId);
            TempData["Error"] = "Eşleşme hesaplama sırasında hata oluştu.";
            return RedirectToAction(nameof(Details), new { id = model.JobDefinitionId });
        }
    }

    public async Task<IActionResult> TopMatches(int id, int count = 10)
    {
        try
        {
            var result = await _jobDefinitionService.GetTopMatchesAsync(id, count);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(Details), new { id });
            }

            var viewModel = new JobDefinitionTopMatchesViewModel
            {
                JobDefinitionId = id,
                TopMatches = _mapper.Map<List<QualificationMatchingResultViewModel>>(result.Data)
            };

            // Get job definition details
            var jobResult = await _jobDefinitionService.GetByIdAsync(id);
            if (jobResult.IsSuccess)
            {
                viewModel.JobDefinitionTitle = jobResult.Data!.Title;
            }

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting top matches for job: {Id}", id);
            TempData["Error"] = "En iyi eşleşmeler alınırken hata oluştu.";
            return RedirectToAction(nameof(Details), new { id });
        }
    }

    [HttpPost]
    public async Task<IActionResult> ExportDefinition(int id)
    {
        try
        {
            var result = await _jobDefinitionService.ExportDefinitionAsync(id);
            if (result.IsSuccess && result.Data != null)
            {
                return File(result.Data, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", 
                           $"is_tanimi_{id}.docx");
            }
            else
            {
                TempData["Error"] = result.Message ?? "Export sırasında hata oluştu.";
                return RedirectToAction(nameof(Details), new { id });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting job definition: {Id}", id);
            TempData["Error"] = "İş tanımı export edilirken hata oluştu.";
            return RedirectToAction(nameof(Details), new { id });
        }
    }

    [HttpPost]
    public async Task<IActionResult> ExportMatchingResults(int id)
    {
        try
        {
            var result = await _jobDefinitionService.ExportMatchingResultsAsync(id);
            if (result.IsSuccess && result.Data != null)
            {
                return File(result.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                           $"esleme_sonuclari_{id}.xlsx");
            }
            else
            {
                TempData["Error"] = result.Message ?? "Export sırasında hata oluştu.";
                return RedirectToAction(nameof(Details), new { id });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting matching results: {Id}", id);
            TempData["Error"] = "Eşleşme sonuçları export edilirken hata oluştu.";
            return RedirectToAction(nameof(Details), new { id });
        }
    }

    public async Task<IActionResult> Summary()
    {
        try
        {
            var result = await _jobDefinitionService.GetSummaryAsync();
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return View(new JobDefinitionSummaryViewModel());
            }

            var viewModel = _mapper.Map<JobDefinitionSummaryViewModel>(result.Data);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting job definition summary");
            TempData["Error"] = "Özet bilgiler alınırken hata oluştu.";
            return View(new JobDefinitionSummaryViewModel());
        }
    }

    private async Task LoadSelectListsAsync()
    {
        var positionsResult = await _positionService.GetAllAsync();
        var departmentsResult = await _departmentService.GetAllAsync();

        ViewBag.PositionSelectList = positionsResult.IsSuccess
            ? new SelectList(positionsResult.Data, "Id", "Name")
            : new SelectList(Enumerable.Empty<SelectListItem>());

        ViewBag.DepartmentSelectList = departmentsResult.IsSuccess
            ? new SelectList(departmentsResult.Data, "Id", "Name")
            : new SelectList(Enumerable.Empty<SelectListItem>());

        // Add education level select list
        ViewBag.EducationLevelSelectList = new SelectList(
            Enum.GetValues<DAL.Entities.EducationLevel>()
                .Select(e => new { Value = (int)e, Text = e.ToString() }),
            "Value", "Text");

        // Add employment type select list
        ViewBag.EmploymentTypeSelectList = new SelectList(
            Enum.GetValues<DAL.Entities.EmploymentType>()
                .Select(e => new { Value = (int)e, Text = e.ToString() }),
            "Value", "Text");
    }

    private async Task LoadSkillSelectListsAsync()
    {
        var skillTemplatesResult = await _skillManagementService.GetAllSkillTemplatesAsync();
        
        ViewBag.SkillTemplateSelectList = skillTemplatesResult.IsSuccess
            ? new SelectList(skillTemplatesResult.Data, "Id", "Name")
            : new SelectList(Enumerable.Empty<SelectListItem>());

        // Add qualification importance select list
        ViewBag.QualificationImportanceSelectList = new SelectList(
            Enum.GetValues<DAL.Entities.QualificationImportance>()
                .Select(e => new { Value = (int)e, Text = e.ToString() }),
            "Value", "Text");

        // Add skill categories
        var categoriesResult = await _skillManagementService.GetSkillCategoriesAsync();
        ViewBag.SkillCategorySelectList = categoriesResult.IsSuccess
            ? new SelectList(categoriesResult.Data)
            : new SelectList(Enumerable.Empty<string>());
    }
}
