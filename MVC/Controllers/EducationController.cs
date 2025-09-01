using AutoMapper;
using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Models;

namespace MVC.Controllers;

public class EducationController : Controller
{
    private readonly IEducationService _educationService;
    private readonly IPersonService _personService;
    private readonly IMapper _mapper;

    public EducationController(IEducationService educationService, IPersonService personService, IMapper mapper)
    {
        _educationService = educationService;
        _personService = personService;
        _mapper = mapper;
    }

    // GET: Education
    public async Task<IActionResult> Index()
    {
        var result = await _educationService.GetAllAsync();
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return View(new List<EducationListViewModel>());
        }

        var viewModel = _mapper.Map<List<EducationListViewModel>>(result.Data);
        return View(viewModel);
    }

    // GET: Education/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var result = await _educationService.GetByIdAsync(id);
        if (!result.Success || result.Data == null)
        {
            TempData["Error"] = result.Message ?? "Eğitim bilgisi bulunamadı.";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<EducationDetailViewModel>(result.Data);
        return View(viewModel);
    }

    // GET: Education/Create
    public async Task<IActionResult> Create()
    {
        await LoadPersonSelectListAsync();
        return View(new EducationCreateViewModel());
    }

    // POST: Education/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EducationCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadPersonSelectListAsync();
            return View(model);
        }

        var createDto = _mapper.Map<EducationCreateDto>(model);
        var result = await _educationService.CreateAsync(createDto);

        if (result.Success)
        {
            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        TempData["Error"] = result.Message;
        await LoadPersonSelectListAsync();
        return View(model);
    }

    // GET: Education/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _educationService.GetByIdAsync(id);
        if (!result.Success || result.Data == null)
        {
            TempData["Error"] = result.Message ?? "Eğitim bilgisi bulunamadı.";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<EducationEditViewModel>(result.Data);
        await LoadPersonSelectListAsync();
        return View(viewModel);
    }

    // POST: Education/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EducationEditViewModel model)
    {
        if (id != model.Id)
        {
            TempData["Error"] = "Geçersiz işlem.";
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            await LoadPersonSelectListAsync();
            return View(model);
        }

        var updateDto = _mapper.Map<EducationUpdateDto>(model);
        var result = await _educationService.UpdateAsync(updateDto);

        if (result.Success)
        {
            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        TempData["Error"] = result.Message;
        await LoadPersonSelectListAsync();
        return View(model);
    }

    // GET: Education/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _educationService.GetByIdAsync(id);
        if (!result.Success || result.Data == null)
        {
            TempData["Error"] = result.Message ?? "Eğitim bilgisi bulunamadı.";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<EducationDetailViewModel>(result.Data);
        return View(viewModel);
    }

    // POST: Education/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _educationService.DeleteAsync(id);
        
        if (result.Success)
            TempData["Success"] = result.Message;
        else
            TempData["Error"] = result.Message;

        return RedirectToAction(nameof(Index));
    }

    // POST: Education/ChangeStatus/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(int id, bool isActive)
    {
        var result = await _educationService.ChangeStatusAsync(id, isActive);
        
        if (result.Success)
            TempData["Success"] = result.Message;
        else
            TempData["Error"] = result.Message;

        return RedirectToAction(nameof(Index));
    }

    // GET: Education/ByPerson/5
    public async Task<IActionResult> ByPerson(int personId)
    {
        var result = await _educationService.GetByPersonIdAsync(personId);
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return View(new List<EducationListViewModel>());
        }

        var viewModel = _mapper.Map<List<EducationListViewModel>>(result.Data);
        
        // Get person info for the page title
        var personResult = await _personService.GetByIdAsync(personId);
        if (personResult.Success && personResult.Data != null)
        {
            ViewBag.PersonName = $"{personResult.Data.FirstName} {personResult.Data.LastName}";
            ViewBag.PersonId = personId;
        }

        return View("Index", viewModel);
    }

    // GET: Education/Ongoing
    public async Task<IActionResult> Ongoing()
    {
        var result = await _educationService.GetOngoingEducationsAsync();
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return View("Index", new List<EducationListViewModel>());
        }

        var viewModel = _mapper.Map<List<EducationListViewModel>>(result.Data);
        ViewBag.Title = "Devam Eden Eğitimler";
        return View("Index", viewModel);
    }

    // GET: Education/Completed
    public async Task<IActionResult> Completed()
    {
        var result = await _educationService.GetCompletedEducationsAsync();
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return View("Index", new List<EducationListViewModel>());
        }

        var viewModel = _mapper.Map<List<EducationListViewModel>>(result.Data);
        ViewBag.Title = "Tamamlanan Eğitimler";
        return View("Index", viewModel);
    }

    // GET: Education/ByDegree
    public async Task<IActionResult> ByDegree(string degree)
    {
        if (string.IsNullOrWhiteSpace(degree))
        {
            TempData["Error"] = "Derece bilgisi gereklidir.";
            return RedirectToAction(nameof(Index));
        }

        var result = await _educationService.GetEducationsByDegreeAsync(degree);
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return View("Index", new List<EducationListViewModel>());
        }

        var viewModel = _mapper.Map<List<EducationListViewModel>>(result.Data);
        ViewBag.Title = $"{degree} Dereceli Eğitimler";
        return View("Index", viewModel);
    }

    // GET: Education/Debug - for testing database connection
    public async Task<IActionResult> Debug()
    {
        var debugInfo = new System.Text.StringBuilder();
        
        try
        {
            debugInfo.AppendLine("=== EDUCATION MODULE DEBUG ===\n");
            
            // Test 1: Direct repository access
            debugInfo.AppendLine("1. Testing UnitOfWork and Repository:");
            using (var scope = HttpContext.RequestServices.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<DAL.Repositories.IUnitOfWork>();
                var directEducations = await unitOfWork.Educations.GetAllAsync();
                debugInfo.AppendLine($"   Direct Repository: Found {directEducations.Count()} educations");
                
                foreach (var edu in directEducations)
                {
                    debugInfo.AppendLine($"   - ID: {edu.Id}, School: {edu.SchoolName}, PersonId: {edu.PersonId}, IsActive: {edu.IsActive}");
                }
            }
            
            // Test 2: Service layer
            debugInfo.AppendLine("\n2. Testing Education Service:");
            var result = await _educationService.GetAllAsync();
            debugInfo.AppendLine($"   Service Success: {result.Success}");
            debugInfo.AppendLine($"   Service Message: {result.Message ?? "N/A"}");
            debugInfo.AppendLine($"   Service Data Count: {result.Data?.Count() ?? 0}");
            
            if (result.Success && result.Data != null)
            {
                foreach (var edu in result.Data)
                {
                    debugInfo.AppendLine($"   - DTO: ID: {edu.Id}, School: {edu.SchoolName}, Person: {edu.PersonName}, Active: {edu.IsActive}");
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
                var testEducation = (await unitOfWork.Educations.GetAllAsync()).FirstOrDefault();
                
                if (testEducation != null)
                {
                    try
                    {
                        var mappedDto = mapper.Map<BLL.DTOs.EducationListDto>(testEducation);
                        debugInfo.AppendLine($"   Mapping Success: {mappedDto.SchoolName}");
                    }
                    catch (Exception mapEx)
                    {
                        debugInfo.AppendLine($"   Mapping Error: {mapEx.Message}");
                    }
                }
                else
                {
                    debugInfo.AppendLine("   No education to test mapping");
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
        if (personsResult.Success)
        {
            ViewBag.PersonSelectList = personsResult.Data
                .Where(p => p.IsActive)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.FirstName} {p.LastName} ({p.EmployeeNumber})"
                })
                .OrderBy(x => x.Text)
                .ToList();
        }
        else
        {
            ViewBag.PersonSelectList = new List<SelectListItem>();
        }
    }
}
