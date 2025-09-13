using BLL.Services;
using BLL.Services.Export;
using BLL.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using MVC.Models;
using AutoMapper;

namespace MVC.Controllers;

[Authorize(Roles = "Admin,Manager")]
public class OrganizationController : Controller
{
    private readonly IOrganizationService _organizationService;
    private readonly IPersonService _personService;
    private readonly IExcelExportService _excelExportService;
    private readonly IMapper _mapper;

    public OrganizationController(IOrganizationService organizationService, IPersonService personService, 
                                IExcelExportService excelExportService, IMapper mapper)
    {
        _organizationService = organizationService;
        _personService = personService;
        _excelExportService = excelExportService;
        _mapper = mapper;
    }

    // GET: Organization
    public async Task<IActionResult> Index(OrganizationFilterViewModel? filter)
    {
        filter ??= new OrganizationFilterViewModel();
        
        var filterDto = _mapper.Map<OrganizationFilterDto>(filter);
        var result = await _organizationService.GetFilteredAsync(filterDto);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return View(new List<OrganizationListViewModel>());
        }

        var viewModels = _mapper.Map<IEnumerable<OrganizationListViewModel>>(result.Data);
        
        // Load filter data
        await LoadOrganizationSelectListAsync();
        
        ViewData["Filter"] = filter;
        return View(viewModels);
    }

    // GET: Organization/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var result = await _organizationService.GetByIdAsync(id);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<OrganizationDetailViewModel>(result.Data);
        return View(viewModel);
    }

    // GET: Organization/Create
    public async Task<IActionResult> Create()
    {
        await LoadOrganizationSelectListAsync();
        await LoadPersonSelectListAsync();
        return View(new OrganizationCreateViewModel());
    }

    // POST: Organization/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(OrganizationCreateViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var dto = _mapper.Map<OrganizationCreateDto>(viewModel);
            var result = await _organizationService.CreateAsync(dto);
            
            if (result.Success)
            {
                TempData["Success"] = "Organizasyon başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            
            TempData["Error"] = result.Message;
        }

        await LoadOrganizationSelectListAsync();
        await LoadPersonSelectListAsync();
        return View(viewModel);
    }

    // GET: Organization/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _organizationService.GetByIdAsync(id);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<OrganizationEditViewModel>(result.Data);
        
        await LoadOrganizationSelectListAsync();
        await LoadPersonSelectListAsync();
        return View(viewModel);
    }

    // POST: Organization/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, OrganizationEditViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            var dto = _mapper.Map<OrganizationUpdateDto>(viewModel);
            var result = await _organizationService.UpdateAsync(dto);
            
            if (result.Success)
            {
                TempData["Success"] = "Organizasyon başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            
            TempData["Error"] = result.Message;
        }

        await LoadOrganizationSelectListAsync();
        await LoadPersonSelectListAsync();
        return View(viewModel);
    }

    // POST: Organization/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _organizationService.DeleteAsync(id);
        
        if (result.Success)
        {
            TempData["Success"] = "Organizasyon başarıyla silindi.";
        }
        else
        {
            TempData["Error"] = result.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Organization/Tree
    public async Task<IActionResult> Tree()
    {
        var result = await _organizationService.GetOrganizationTreeAsync();
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return View(new List<OrganizationTreeViewModel>());
        }

        var viewModels = await BuildTreeViewModel(result.Data);
        return View(viewModels);
    }

    // Helper Methods
    private async Task LoadOrganizationSelectListAsync()
    {
        var result = await _organizationService.GetAllAsync();
        if (result.Success && result.Data != null)
        {
            ViewBag.OrganizationSelectList = result.Data
                .Where(o => o.IsActive)
                .Select(o => new SelectListItem
                {
                    Value = o.Id.ToString(),
                    Text = $"{o.Code} - {o.Name}"
                })
                .ToList();
        }
        else
        {
            ViewBag.OrganizationSelectList = new List<SelectListItem>();
        }
    }

    private async Task LoadPersonSelectListAsync()
    {
        var result = await _personService.GetAllAsync();
        if (result.Success && result.Data != null)
        {
            ViewBag.PersonSelectList = result.Data
                .Where(p => p.IsActive)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.FirstName} {p.LastName}"
                })
                .ToList();
        }
        else
        {
            ViewBag.PersonSelectList = new List<SelectListItem>();
        }
    }

    private async Task<List<OrganizationTreeViewModel>> BuildTreeViewModel(IEnumerable<OrganizationTreeDto> organizations)
    {
        var result = new List<OrganizationTreeViewModel>();
        
        foreach (var org in organizations)
        {
            var viewModel = _mapper.Map<OrganizationTreeViewModel>(org);
            if (org.Children.Any())
            {
                viewModel.Children = await BuildTreeViewModel(org.Children);
            }
            result.Add(viewModel);
        }
        
        return result;
    }

    // Export Actions
    [HttpPost]
    public async Task<IActionResult> ExportToExcel()
    {
        var result = await _organizationService.GetAllAsync();
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        // Simple CSV export for now
        TempData["Info"] = "Excel export özelliği geliştiriliyor. Şimdilik listede görüntülenen veriler bulunmaktadır.";
        return RedirectToAction(nameof(Index));
    }
}
