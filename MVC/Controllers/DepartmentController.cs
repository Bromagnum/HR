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
public class DepartmentController : Controller
{
    private readonly IDepartmentService _departmentService;
    private readonly IPersonService _personService;
    private readonly IExcelExportService _excelExportService;
    private readonly IMapper _mapper;

    public DepartmentController(IDepartmentService departmentService, IPersonService personService, 
                              IExcelExportService excelExportService, IMapper mapper)
    {
        _departmentService = departmentService;
        _personService = personService;
        _excelExportService = excelExportService;
        _mapper = mapper;
    }

    // GET: Department
    public async Task<IActionResult> Index(DepartmentFilterViewModel? filter)
    {
        // Eğer filter null ise default oluştur
        filter ??= new DepartmentFilterViewModel();
        
        var filterDto = _mapper.Map<DepartmentFilterDto>(filter);
        var result = await _departmentService.SearchAsync(filterDto);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            var emptyResult = new DepartmentSearchResultViewModel
            {
                Filter = filter
            };
            return View("IndexWithSearch", emptyResult);
        }

        var searchResultViewModel = _mapper.Map<DepartmentSearchResultViewModel>(result.Data);
        
        // Hiyerarşik yapı için level hesaplama
        var hierarchicalList = BuildHierarchicalList(searchResultViewModel.Departments.ToList());
        searchResultViewModel.Departments = hierarchicalList;
        
        // Filtreleme için parent departmanları yükle
        await LoadParentDepartmentsForFilter();
        
        return View("IndexWithSearch", searchResultViewModel);
    }

    // GET: Department/Search - AJAX için
    [HttpGet]
    public async Task<IActionResult> Search(string? searchTerm, bool? isActive, int page = 1)
    {
        var filter = new DepartmentFilterDto
        {
            SearchTerm = searchTerm,
            IsActive = isActive,
            Page = page,
            PageSize = 20
        };
        
        var result = await _departmentService.SearchAsync(filter);
        
        if (!result.Success)
        {
            return Json(new { success = false, message = result.Message });
        }

        var viewModel = _mapper.Map<DepartmentSearchResultViewModel>(result.Data);
        var hierarchicalList = BuildHierarchicalList(viewModel.Departments.ToList());
        
        return PartialView("_DepartmentSearchResults", hierarchicalList);
    }

    // GET: Department/Filter - Gelişmiş filtreleme için
    [HttpPost]
    public async Task<IActionResult> Filter(DepartmentFilterViewModel filter)
    {
        return await Index(filter);
    }

    // GET: Department/Tree
    public async Task<IActionResult> Tree()
    {
        var result = await _departmentService.GetRootDepartmentsAsync();
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return View(new List<DepartmentTreeViewModel>());
        }

        var treeViewModel = await BuildTreeViewModel(result.Data);
        return View(treeViewModel);
    }

    // GET: Department/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var result = await _departmentService.GetByIdAsync(id);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<DepartmentDetailViewModel>(result.Data);
        return View(viewModel);
    }

    // GET: Department/Create
    public async Task<IActionResult> Create()
    {
        await LoadParentDepartments();
        return View();
    }

    // POST: Department/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DepartmentCreateViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var dto = _mapper.Map<DepartmentCreateDto>(viewModel);
            var result = await _departmentService.CreateAsync(dto);
            
            if (result.Success)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Error"] = result.Message;
                if (result.Errors.Any())
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
        }

        await LoadParentDepartments();
        return View(viewModel);
    }

    // GET: Department/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _departmentService.GetByIdAsync(id);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<DepartmentEditViewModel>(result.Data);
        await LoadParentDepartments(id);
        return View(viewModel);
    }

    // POST: Department/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DepartmentEditViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var dto = _mapper.Map<DepartmentUpdateDto>(viewModel);
            var result = await _departmentService.UpdateAsync(dto);
            
            if (result.Success)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Error"] = result.Message;
                if (result.Errors.Any())
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
        }

        await LoadParentDepartments(id);
        return View(viewModel);
    }

    // GET: Department/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _departmentService.GetByIdAsync(id);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<DepartmentDetailViewModel>(result.Data);
        return View(viewModel);
    }

    // POST: Department/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _departmentService.DeleteAsync(id);
        
        if (result.Success)
        {
            TempData["Success"] = result.Message;
        }
        else
        {
            TempData["Error"] = result.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: Department/ToggleStatus/5
    [HttpPost]
    public async Task<IActionResult> ToggleStatus(int id, bool isActive)
    {
        var result = await _departmentService.SetActiveStatusAsync(id, isActive);
        
        if (result.Success)
        {
            TempData["Success"] = result.Message;
        }
        else
        {
            TempData["Error"] = result.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Department/Employees/5
    public async Task<IActionResult> Employees(int id)
    {
        var departmentResult = await _departmentService.GetByIdAsync(id);
        if (!departmentResult.Success)
        {
            TempData["Error"] = departmentResult.Message;
            return RedirectToAction(nameof(Index));
        }

        var employeesResult = await _personService.GetByDepartmentIdAsync(id);
        if (!employeesResult.Success)
        {
            TempData["Error"] = employeesResult.Message;
            return RedirectToAction(nameof(Index));
        }

        ViewBag.DepartmentName = departmentResult.Data!.Name;
        ViewBag.DepartmentId = id;
        
        var viewModel = _mapper.Map<IEnumerable<PersonListViewModel>>(employeesResult.Data);
        return View(viewModel);
    }

    private async Task LoadParentDepartments(int? excludeId = null)
    {
        var departmentsResult = await _departmentService.GetAllAsync();
        
        if (departmentsResult.Success && departmentsResult.Data != null)
        {
            var departments = departmentsResult.Data
                .Where(d => d.IsActive && (!excludeId.HasValue || d.Id != excludeId.Value))
                .OrderBy(d => d.Name);
                
            ViewBag.ParentDepartments = new SelectList(departments, "Id", "Name");
        }
        else
        {
            ViewBag.ParentDepartments = new SelectList(Enumerable.Empty<SelectListItem>());
        }
    }

    private async Task LoadParentDepartmentsForFilter()
    {
        var departmentsResult = await _departmentService.GetAllAsync();
        
        if (departmentsResult.Success && departmentsResult.Data != null)
        {
            var departments = departmentsResult.Data
                .Where(d => d.IsActive)
                .OrderBy(d => d.Name);
                
            ViewBag.FilterParentDepartments = new SelectList(departments, "Id", "Name");
        }
        else
        {
            ViewBag.FilterParentDepartments = new SelectList(Enumerable.Empty<SelectListItem>());
        }
    }

    private List<DepartmentListViewModel> BuildHierarchicalList(List<DepartmentListViewModel> departments)
    {
        var result = new List<DepartmentListViewModel>();
        var lookup = departments.ToLookup(d => d.ParentDepartmentName);
        
        // Ana departmanları bul (parent'ı null olanlar)
        var rootDepartments = departments.Where(d => string.IsNullOrEmpty(d.ParentDepartmentName)).ToList();
        
        foreach (var rootDept in rootDepartments)
        {
            AddDepartmentWithChildren(rootDept, departments, result, 0);
        }
        
        return result;
    }

    private void AddDepartmentWithChildren(DepartmentListViewModel department, List<DepartmentListViewModel> allDepartments, List<DepartmentListViewModel> result, int level)
    {
        department.Level = level;
        department.HasChildren = allDepartments.Any(d => d.ParentDepartmentName == department.Name);
        result.Add(department);
        
        var children = allDepartments.Where(d => d.ParentDepartmentName == department.Name).OrderBy(d => d.Name);
        foreach (var child in children)
        {
            AddDepartmentWithChildren(child, allDepartments, result, level + 1);
        }
    }

    private async Task<List<DepartmentTreeViewModel>> BuildTreeViewModel(IEnumerable<DepartmentListDto> departments)
    {
        var result = new List<DepartmentTreeViewModel>();
        
        foreach (var dept in departments)
        {
            var treeNode = new DepartmentTreeViewModel
            {
                Id = dept.Id,
                Name = dept.Name,
                Description = dept.Description,
                EmployeeCount = dept.EmployeeCount,
                IsActive = dept.IsActive,
                Level = 0
            };
            
            // Alt departmanları getir
            var subDepartmentsResult = await _departmentService.GetSubDepartmentsAsync(dept.Id);
            if (subDepartmentsResult.Success && subDepartmentsResult.Data != null)
            {
                treeNode.Children = await BuildTreeViewModelRecursive(subDepartmentsResult.Data, 1);
            }
            
            result.Add(treeNode);
        }
        
        return result;
    }

    private async Task<List<DepartmentTreeViewModel>> BuildTreeViewModelRecursive(IEnumerable<DepartmentListDto> departments, int level)
    {
        var result = new List<DepartmentTreeViewModel>();
        
        foreach (var dept in departments)
        {
            var treeNode = new DepartmentTreeViewModel
            {
                Id = dept.Id,
                Name = dept.Name,
                Description = dept.Description,
                EmployeeCount = dept.EmployeeCount,
                IsActive = dept.IsActive,
                Level = level
            };
            
            // Alt departmanları getir
            var subDepartmentsResult = await _departmentService.GetSubDepartmentsAsync(dept.Id);
            if (subDepartmentsResult.Success && subDepartmentsResult.Data != null && subDepartmentsResult.Data.Any())
            {
                treeNode.Children = await BuildTreeViewModelRecursive(subDepartmentsResult.Data, level + 1);
            }
            
            result.Add(treeNode);
        }
        
        return result;
    }

    // EXPORT ACTIONS

    // GET: Department/ExportList
    public async Task<IActionResult> ExportList(DepartmentFilterViewModel? filter)
    {
        try
        {
            // Filtrelenmiş departmanları al
            filter ??= new DepartmentFilterViewModel { PageSize = 1000 }; // Tüm sonuçları al
            var filterDto = _mapper.Map<DepartmentFilterDto>(filter);
            var result = await _departmentService.SearchAsync(filterDto);
            
            if (!result.Success || result.Data == null)
            {
                TempData["Error"] = "Departman listesi alınırken hata oluştu: " + result.Message;
                return RedirectToAction(nameof(Index));
            }

            // Excel dosyası oluştur
            var excelData = await _excelExportService.ExportDepartmentListAsync(result.Data.Departments, true);
            
            var fileName = $"Departman_Listesi_{DateTime.Now:yyyyMMdd_HHmmss}.html";
            return File(excelData, "text/html", fileName);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Excel export sırasında hata oluştu: " + ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: Department/ExportOrganizationChart
    public async Task<IActionResult> ExportOrganizationChart()
    {
        try
        {
            var result = await _departmentService.GetAllAsync();
            
            if (!result.Success || result.Data == null)
            {
                TempData["Error"] = "Departman listesi alınırken hata oluştu: " + result.Message;
                return RedirectToAction(nameof(Index));
            }

            var excelData = await _excelExportService.ExportOrganizationChartAsync(result.Data);
            
            var fileName = $"Organizasyon_Semasi_{DateTime.Now:yyyyMMdd_HHmmss}.html";
            return File(excelData, "text/html", fileName);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Organizasyon şeması export sırasında hata oluştu: " + ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: Department/ExportDetail/{id}
    public async Task<IActionResult> ExportDetail(int id)
    {
        try
        {
            var result = await _departmentService.GetByIdAsync(id);
            
            if (!result.Success || result.Data == null)
            {
                TempData["Error"] = "Departman detayları alınırken hata oluştu: " + result.Message;
                return RedirectToAction(nameof(Index));
            }

            var excelData = await _excelExportService.ExportDepartmentDetailAsync(id, result.Data);
            
            var fileName = $"Departman_Detay_{result.Data.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.html";
            // Dosya adından geçersiz karakterleri temizle
            fileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
            
            return File(excelData, "text/html", fileName);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Departman detay export sırasında hata oluştu: " + ex.Message;
            return RedirectToAction(nameof(Details), new { id });
        }
    }

    // POST: Department/ExportSearchResults
    [HttpPost]
    public async Task<IActionResult> ExportSearchResults(DepartmentFilterViewModel filter)
    {
        try
        {
            var filterDto = _mapper.Map<DepartmentFilterDto>(filter);
            filterDto.PageSize = 1000; // Tüm sonuçları al
            
            var result = await _departmentService.SearchAsync(filterDto);
            
            if (!result.Success || result.Data == null)
            {
                TempData["Error"] = "Arama sonuçları alınırken hata oluştu: " + result.Message;
                return RedirectToAction(nameof(Index), filter);
            }

            var excelData = await _excelExportService.ExportSearchResultsAsync(result.Data);
            
            var searchTerm = !string.IsNullOrEmpty(filter.SearchTerm) ? $"_{filter.SearchTerm}" : "";
            var fileName = $"Departman_Arama_Sonuclari{searchTerm}_{DateTime.Now:yyyyMMdd_HHmmss}.html";
            // Dosya adından geçersiz karakterleri temizle
            fileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
            
            return File(excelData, "text/html", fileName);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Arama sonuçları export sırasında hata oluştu: " + ex.Message;
            return RedirectToAction(nameof(Index), filter);
        }
    }

    // GET: Department/ExportCurrentView
    public async Task<IActionResult> ExportCurrentView(string? searchTerm, bool? isActive, bool? hasParent, 
                                                      string? sortBy, bool sortDescending = false)
    {
        var filter = new DepartmentFilterViewModel
        {
            SearchTerm = searchTerm,
            IsActive = isActive,
            HasParent = hasParent,
            SortBy = sortBy ?? "Name",
            SortDescending = sortDescending,
            PageSize = 1000
        };
        
        return await ExportSearchResults(filter);
    }
}
