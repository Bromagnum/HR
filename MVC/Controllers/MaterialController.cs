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
public class MaterialController : Controller
{
    private readonly IMaterialService _materialService;
    private readonly IOrganizationService _organizationService;
    private readonly IExcelExportService _excelExportService;
    private readonly IMapper _mapper;

    public MaterialController(IMaterialService materialService, IOrganizationService organizationService,
                            IExcelExportService excelExportService, IMapper mapper)
    {
        _materialService = materialService;
        _organizationService = organizationService;
        _excelExportService = excelExportService;
        _mapper = mapper;
    }

    // GET: Material
    public async Task<IActionResult> Index(MaterialFilterViewModel? filter)
    {
        filter ??= new MaterialFilterViewModel();
        
        var filterDto = _mapper.Map<MaterialFilterDto>(filter);
        var result = await _materialService.GetFilteredAsync(filterDto);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return View(new List<MaterialListViewModel>());
        }

        var viewModels = _mapper.Map<IEnumerable<MaterialListViewModel>>(result.Data);
        
        // Load filter data
        await LoadOrganizationSelectListAsync();
        await LoadCategoriesAsync();
        
        ViewData["Filter"] = filter;
        return View(viewModels);
    }

    // GET: Material/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var result = await _materialService.GetByIdAsync(id);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<MaterialDetailViewModel>(result.Data);
        return View(viewModel);
    }

    // GET: Material/Create
    public async Task<IActionResult> Create()
    {
        await LoadOrganizationSelectListAsync();
        await LoadCategoriesAsync();
        return View(new MaterialCreateViewModel());
    }

    // POST: Material/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MaterialCreateViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var dto = _mapper.Map<MaterialCreateDto>(viewModel);
            var result = await _materialService.CreateAsync(dto);
            
            if (result.Success)
            {
                TempData["Success"] = "Malzeme başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            
            TempData["Error"] = result.Message;
        }

        await LoadOrganizationSelectListAsync();
        await LoadCategoriesAsync();
        return View(viewModel);
    }

    // GET: Material/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _materialService.GetByIdAsync(id);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<MaterialEditViewModel>(result.Data);
        
        await LoadOrganizationSelectListAsync();
        await LoadCategoriesAsync();
        return View(viewModel);
    }

    // POST: Material/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MaterialEditViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            var dto = _mapper.Map<MaterialUpdateDto>(viewModel);
            var result = await _materialService.UpdateAsync(dto);
            
            if (result.Success)
            {
                TempData["Success"] = "Malzeme başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            
            TempData["Error"] = result.Message;
        }

        await LoadOrganizationSelectListAsync();
        await LoadCategoriesAsync();
        return View(viewModel);
    }

    // POST: Material/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _materialService.DeleteAsync(id);
        
        if (result.Success)
        {
            TempData["Success"] = "Malzeme başarıyla silindi.";
        }
        else
        {
            TempData["Error"] = result.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Material/LowStock
    public async Task<IActionResult> LowStock()
    {
        var result = await _materialService.GetLowStockMaterialsAsync();
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return View(new List<MaterialListViewModel>());
        }

        var viewModels = _mapper.Map<IEnumerable<MaterialListViewModel>>(result.Data);
        return View(viewModels);
    }

    // GET: Material/StockSummary
    public async Task<IActionResult> StockSummary()
    {
        var result = await _materialService.GetStockSummaryAsync();
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return View(new MaterialStockSummaryViewModel());
        }

        var viewModel = _mapper.Map<MaterialStockSummaryViewModel>(result.Data);
        return View(viewModel);
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

    private async Task LoadCategoriesAsync()
    {
        var result = await _materialService.GetCategoriesAsync();
        if (result.Success && result.Data != null)
        {
            ViewBag.CategorySelectList = result.Data
                .Select(c => new SelectListItem
                {
                    Value = c,
                    Text = c
                })
                .ToList();
        }
        else
        {
            ViewBag.CategorySelectList = new List<SelectListItem>();
        }
    }

    // Export Actions
    [HttpPost]
    public async Task<IActionResult> ExportToExcel()
    {
        var result = await _materialService.GetAllAsync();
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        // Simple CSV export for now
        TempData["Info"] = "Excel export özelliği geliştiriliyor. Şimdilik listede görüntülenen veriler bulunmaktadır.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> ExportLowStockToExcel()
    {
        var result = await _materialService.GetLowStockMaterialsAsync();
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(LowStock));
        }

        // Simple CSV export for now
        TempData["Info"] = "Excel export özelliği geliştiriliyor. Şimdilik listede görüntülenen veriler bulunmaktadır.";
        return RedirectToAction(nameof(LowStock));
    }
}
