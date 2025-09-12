using AutoMapper;
using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using MVC.Models;
using MVC.Services;

namespace MVC.Controllers;

[Authorize] // Tüm çalışanlar yeterlilik bilgilerini görüntüleyebilir
public class QualificationController : Controller
{
    private readonly IQualificationService _qualificationService;
    private readonly IPersonService _personService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public QualificationController(IQualificationService qualificationService, IPersonService personService, ICurrentUserService currentUserService, IMapper mapper)
    {
        _qualificationService = qualificationService;
        _personService = personService;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    // GET: Qualification
    public async Task<IActionResult> Index(string searchTerm, string selectedCategory, bool showExpiredOnly = false, bool showExpiringSoon = false)
    {
        // Employee sadece kendi yeterliliklerini görebilir
        if (_currentUserService.IsInRole("Employee"))
        {
            var currentPersonId = _currentUserService.PersonId;
            if (!currentPersonId.HasValue)
            {
                TempData["Error"] = "Personel kimliği alınamadı. Lütfen yöneticinize başvurun.";
                return RedirectToAction("Index", "Home");
            }

            var result = await _qualificationService.GetByPersonIdAsync(currentPersonId.Value);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View("MyQualifications", new List<QualificationListViewModel>());
            }

            var viewModels = _mapper.Map<List<QualificationListViewModel>>(result.Data);
            ViewData["Title"] = "Yeterliliklerim";
            ViewData["IsEmployeeView"] = true;
            ViewData["PersonId"] = currentPersonId.Value;
            return View("MyQualifications", viewModels);
        }

        // Admin/Manager için tüm yeterlilikler
        var allResult = await _qualificationService.GetAllAsync();
        
        if (!allResult.Success)
        {
            TempData["Error"] = allResult.Message;
            return View(new QualificationIndexViewModel());
        }

        var qualificationViewModels = _mapper.Map<IEnumerable<QualificationListViewModel>>(allResult.Data);

        // Apply filters
        if (!string.IsNullOrEmpty(searchTerm))
        {
            qualificationViewModels = qualificationViewModels.Where(q => 
                q.Name.ToLower().Contains(searchTerm.ToLower()) ||
                q.Category.ToLower().Contains(searchTerm.ToLower()) ||
                q.IssuingAuthority.ToLower().Contains(searchTerm.ToLower()) ||
                q.PersonName.ToLower().Contains(searchTerm.ToLower()));
        }

        if (!string.IsNullOrEmpty(selectedCategory))
        {
            qualificationViewModels = qualificationViewModels.Where(q => q.Category == selectedCategory);
        }

        if (showExpiredOnly)
        {
            qualificationViewModels = qualificationViewModels.Where(q => q.IsExpired);
        }
        else if (showExpiringSoon)
        {
            qualificationViewModels = qualificationViewModels.Where(q => q.IsExpiringSoon);
        }

        var viewModel = new QualificationIndexViewModel
        {
            Qualifications = qualificationViewModels,
            SearchTerm = searchTerm,
            SelectedCategory = selectedCategory,
            ShowExpiredOnly = showExpiredOnly,
            ShowExpiringSoon = showExpiringSoon
        };

        // Get categories for filter dropdown
        ViewBag.Categories = qualificationViewModels
            .Select(q => q.Category)
            .Distinct()
            .OrderBy(c => c)
            .Select(c => new SelectListItem { Value = c, Text = c })
            .ToList();

        return View(viewModel);
    }

    // GET: Qualification/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var result = await _qualificationService.GetByIdAsync(id);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<QualificationDetailViewModel>(result.Data);
        return View(viewModel);
    }

    // GET: Qualification/Create
    public async Task<IActionResult> Create()
    {
        // Employee sadece kendisi için yeterlilik ekleyebilir
        if (_currentUserService.IsInRole("Employee"))
        {
            var currentPersonId = _currentUserService.PersonId;
            if (!currentPersonId.HasValue)
            {
                TempData["Error"] = "Personel kimliği alınamadı. Lütfen yöneticinize başvurun.";
                return RedirectToAction("Index", "Home");
            }

            await LoadPersonSelectListForEmployeeAsync(currentPersonId.Value);
            var viewModel = new QualificationCreateViewModel
            {
                PersonId = currentPersonId.Value
            };
            ViewData["IsEmployeeView"] = true;
            return View(viewModel);
        }

        await LoadPersonSelectListAsync();
        return View(new QualificationCreateViewModel());
    }

    // POST: Qualification/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(QualificationCreateViewModel viewModel)
    {
        // Employee sadece kendisi için yeterlilik ekleyebilir
        if (_currentUserService.IsInRole("Employee"))
        {
            var currentPersonId = _currentUserService.PersonId;
            if (!currentPersonId.HasValue)
            {
                TempData["Error"] = "Personel kimliği alınamadı. Lütfen yöneticinize başvurun.";
                return RedirectToAction("Index", "Home");
            }

            // PersonId'yi zorla set et
            viewModel.PersonId = currentPersonId.Value;
        }

        if (ModelState.IsValid)
        {
            var dto = _mapper.Map<QualificationCreateDto>(viewModel);
            var result = await _qualificationService.CreateAsync(dto);
            
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

        // Employee için özel dropdown
        if (_currentUserService.IsInRole("Employee"))
        {
            await LoadPersonSelectListForEmployeeAsync(viewModel.PersonId);
            ViewData["IsEmployeeView"] = true;
        }
        else
        {
            await LoadPersonSelectListAsync();
        }
        
        return View(viewModel);
    }

    // GET: Qualification/Edit/5
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _qualificationService.GetByIdAsync(id);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<QualificationEditViewModel>(result.Data);
        await LoadPersonSelectListAsync();
        return View(viewModel);
    }

    // POST: Qualification/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Edit(int id, QualificationEditViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData["Error"] = "Geçersiz yeterlilik ID'si.";
            return RedirectToAction(nameof(Index));
        }

        if (ModelState.IsValid)
        {
            var dto = _mapper.Map<QualificationUpdateDto>(viewModel);
            var result = await _qualificationService.UpdateAsync(dto);
            
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

        await LoadPersonSelectListAsync();
        return View(viewModel);
    }

    // GET: Qualification/Delete/5
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _qualificationService.GetByIdAsync(id);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<QualificationDetailViewModel>(result.Data);
        return View(viewModel);
    }

    // POST: Qualification/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _qualificationService.DeleteAsync(id);
        
        if (result.Success)
            TempData["Success"] = result.Message;
        else
            TempData["Error"] = result.Message;

        return RedirectToAction(nameof(Index));
    }

    // POST: Qualification/ChangeStatus/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> ChangeStatus(int id, bool isActive)
    {
        var result = await _qualificationService.ChangeStatusAsync(id, isActive);
        
        if (result.Success)
            TempData["Success"] = result.Message;
        else
            TempData["Error"] = result.Message;

        return RedirectToAction(nameof(Index));
    }

    // GET: Qualification/ExpiringSoon
    public async Task<IActionResult> ExpiringSoon(int days = 30)
    {
        var result = await _qualificationService.GetExpiringSoonAsync(days);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return View(new List<QualificationListViewModel>());
        }

        var viewModels = _mapper.Map<IEnumerable<QualificationListViewModel>>(result.Data);
        ViewBag.Days = days;
        return View(viewModels);
    }

    // GET: Qualification/Expired
    public async Task<IActionResult> Expired()
    {
        var result = await _qualificationService.GetExpiredAsync();
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return View(new List<QualificationListViewModel>());
        }

        var viewModels = _mapper.Map<IEnumerable<QualificationListViewModel>>(result.Data);
        return View(viewModels);
    }

    // GET: Qualification/Debug - for testing database connection
    public async Task<IActionResult> Debug()
    {
        var debugInfo = new System.Text.StringBuilder();
        
        try
        {
            debugInfo.AppendLine("=== QUALIFICATION MODULE DEBUG ===\n");
            
            // Test 1: Direct repository access
            debugInfo.AppendLine("1. Testing UnitOfWork and Repository:");
            using (var scope = HttpContext.RequestServices.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<DAL.Repositories.IUnitOfWork>();
                var directQualifications = await unitOfWork.Qualifications.GetAllAsync();
                debugInfo.AppendLine($"   Direct Repository: Found {directQualifications.Count()} qualifications");
                
                foreach (var qual in directQualifications)
                {
                    debugInfo.AppendLine($"   - ID: {qual.Id}, Name: {qual.Name}, PersonId: {qual.PersonId}, IsActive: {qual.IsActive}");
                }
            }
            
            // Test 2: Service layer
            debugInfo.AppendLine("\n2. Testing Qualification Service:");
            var result = await _qualificationService.GetAllAsync();
            debugInfo.AppendLine($"   Service Success: {result.Success}");
            debugInfo.AppendLine($"   Service Message: {result.Message ?? "N/A"}");
            debugInfo.AppendLine($"   Service Data Count: {result.Data?.Count() ?? 0}");
            
            if (result.Success && result.Data != null)
            {
                foreach (var qual in result.Data)
                {
                    debugInfo.AppendLine($"   - DTO: ID: {qual.Id}, Name: {qual.Name}, Person: {qual.PersonName}, Active: {qual.IsActive}");
                }
            }
            
            // Test 3: Person service
            debugInfo.AppendLine("\n3. Testing Person Service:");
            var personsResult = await _personService.GetAllAsync();
            debugInfo.AppendLine($"   Persons Success: {personsResult.Success}");
            debugInfo.AppendLine($"   Persons Count: {personsResult.Data?.Count() ?? 0}");
            
            // Test 4: AutoMapper
            debugInfo.AppendLine("\n4. Testing AutoMapper:");
            using (var scope = HttpContext.RequestServices.CreateScope())
            {
                var mapper = scope.ServiceProvider.GetRequiredService<AutoMapper.IMapper>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<DAL.Repositories.IUnitOfWork>();
                var testQualification = (await unitOfWork.Qualifications.GetAllAsync()).FirstOrDefault();
                
                if (testQualification != null)
                {
                    try
                    {
                        var mappedDto = mapper.Map<BLL.DTOs.QualificationListDto>(testQualification);
                        debugInfo.AppendLine($"   Mapping Success: {mappedDto.Name}");
                    }
                    catch (Exception mapEx)
                    {
                        debugInfo.AppendLine($"   Mapping Error: {mapEx.Message}");
                    }
                }
                else
                {
                    debugInfo.AppendLine("   No qualification to test mapping");
                }
            }
            
            return Content(debugInfo.ToString());
        }
        catch (Exception ex)
        {
            debugInfo.AppendLine($"\nFATAL ERROR: {ex.Message}");
            debugInfo.AppendLine($"Inner: {ex.InnerException?.Message ?? "N/A"}");
            debugInfo.AppendLine($"Stack: {ex.StackTrace}");
            return Content(debugInfo.ToString());
        }
    }

    private async Task LoadPersonSelectListAsync()
    {
        var personsResult = await _personService.GetAllAsync();
        
        if (personsResult.Success && personsResult.Data != null)
        {
            ViewBag.PersonSelectList = personsResult.Data
                .Where(p => p.IsActive)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.FirstName} {p.LastName} - {p.DepartmentName ?? "Departmansız"}"
                })
                .OrderBy(x => x.Text)
                .ToList();
        }
        else
        {
            ViewBag.PersonSelectList = new List<SelectListItem>();
        }
    }

    private async Task LoadPersonSelectListForEmployeeAsync(int personId)
    {
        var personResult = await _personService.GetByIdAsync(personId);
        
        if (personResult.Success && personResult.Data != null)
        {
            var person = personResult.Data;
            ViewBag.PersonSelectList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = person.Id.ToString(),
                    Text = $"{person.FirstName} {person.LastName}",
                    Selected = true
                }
            };
        }
        else
        {
            ViewBag.PersonSelectList = new List<SelectListItem>();
        }
    }
}
