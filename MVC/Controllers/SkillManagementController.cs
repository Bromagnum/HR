using AutoMapper;
using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MVC.Models;

namespace MVC.Controllers;

[Authorize(Roles = "Admin,Manager")]
public class SkillManagementController : Controller
{
    private readonly ISkillManagementService _skillManagementService;
    private readonly IMapper _mapper;
    private readonly ILogger<SkillManagementController> _logger;

    public SkillManagementController(
        ISkillManagementService skillManagementService,
        IMapper mapper,
        ILogger<SkillManagementController> logger)
    {
        _skillManagementService = skillManagementService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: SkillManagement/Analytics
    public async Task<IActionResult> Analytics()
    {
        try
        {
            // Simple analytics using existing methods
            var templatesResult = await _skillManagementService.GetAllSkillTemplatesAsync();
            var categoriesResult = await _skillManagementService.GetSkillCategoriesAsync();
            
            var viewModel = new SkillAnalyticsViewModel
            {
                TotalSkillTemplates = templatesResult.IsSuccess ? templatesResult.Data?.Count() ?? 0 : 0,
                TotalPersonSkills = 0, // Would need to implement this
                ActiveSkillCategories = categoriesResult.IsSuccess ? categoriesResult.Data?.Count() ?? 0 : 0
            };
            
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading skill analytics");
            TempData["Error"] = "Beceri analizi yüklenirken hata oluştu.";
            return View(new SkillAnalyticsViewModel());
        }
    }

    // GET: SkillManagement/SkillTemplates
    public async Task<IActionResult> SkillTemplates(SkillTemplateFilterViewModel? filter = null)
    {
        try
        {
            filter ??= new SkillTemplateFilterViewModel();
            
            var filterDto = _mapper.Map<SkillTemplateFilterDto>(filter);
            var result = await _skillManagementService.GetFilteredSkillTemplatesAsync(filterDto);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return View(new List<SkillTemplateListViewModel>());
            }

            var viewModels = _mapper.Map<IEnumerable<SkillTemplateListViewModel>>(result.Data);
            ViewBag.Filter = filter;
            
            return View(viewModels);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading skill templates");
            TempData["Error"] = "Beceri şablonları yüklenirken hata oluştu.";
            return View(new List<SkillTemplateListViewModel>());
        }
    }

    // GET: SkillManagement/PersonSkills
    public async Task<IActionResult> PersonSkills(PersonSkillFilterViewModel? filter = null)
    {
        try
        {
            filter ??= new PersonSkillFilterViewModel();
            
            var filterDto = _mapper.Map<PersonSkillFilterDto>(filter);
            var result = await _skillManagementService.GetFilteredPersonSkillsAsync(filterDto);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return View(new List<PersonSkillListViewModel>());
            }

            var viewModels = _mapper.Map<IEnumerable<PersonSkillListViewModel>>(result.Data);
            ViewBag.Filter = filter;
            
            return View(viewModels);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading person skills");
            TempData["Error"] = "Personel becerileri yüklenirken hata oluştu.";
            return View(new List<PersonSkillListViewModel>());
        }
    }

    // GET: SkillManagement/SkillTemplates/Details/5
    public async Task<IActionResult> SkillTemplateDetails(int id)
    {
        try
        {
            var result = await _skillManagementService.GetSkillTemplateByIdAsync(id);
            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(SkillTemplates));
            }

            var viewModel = _mapper.Map<SkillTemplateDetailViewModel>(result.Data);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting skill template details for id: {Id}", id);
            TempData["Error"] = "Beceri şablonu detayları alınırken hata oluştu.";
            return RedirectToAction(nameof(SkillTemplates));
        }
    }

    // GET: SkillManagement/SkillTemplates/Create
    public IActionResult CreateSkillTemplate()
    {
        return View(new SkillTemplateCreateViewModel());
    }

    // POST: SkillManagement/SkillTemplates/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateSkillTemplate(SkillTemplateCreateViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var dto = _mapper.Map<SkillTemplateCreateDto>(model);
            var result = await _skillManagementService.CreateSkillTemplateAsync(dto);

            if (result.IsSuccess)
            {
                TempData["Success"] = "Beceri şablonu başarıyla oluşturuldu.";
                return RedirectToAction(nameof(SkillTemplateDetails), new { id = result.Data!.Id });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating skill template");
            ModelState.AddModelError("", "Beceri şablonu oluşturulurken hata oluştu.");
            return View(model);
        }
    }

    // API-like action for getting skill categories
    [HttpGet]
    public async Task<IActionResult> GetSkillCategories()
    {
        try
        {
            var result = await _skillManagementService.GetSkillCategoriesAsync();
            if (result.IsSuccess)
            {
                return Json(result.Data);
            }
            return Json(new List<string>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting skill categories");
            return Json(new List<string>());
        }
    }

    // API-like action for getting skill types
    [HttpGet]
    public IActionResult GetSkillTypes()
    {
        try
        {
            // Get enum values for SkillType
            var skillTypes = Enum.GetValues(typeof(DAL.Entities.SkillType))
                                .Cast<DAL.Entities.SkillType>()
                                .Select(e => e.ToString())
                                .ToList();
            
            return Json(skillTypes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting skill types");
            return Json(new List<string>());
        }
    }
}
