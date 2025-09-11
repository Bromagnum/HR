using BLL.Services;
using BLL.DTOs;
using BLL.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using MVC.Models;
using AutoMapper;

namespace MVC.Controllers;

[Authorize(Roles = "Admin,Manager,Employee")]
public class PersonController : Controller
{
    private readonly IPersonService _personService;
    private readonly IDepartmentService _departmentService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public PersonController(IPersonService personService, IDepartmentService departmentService, 
        ICurrentUserService currentUserService, IMapper mapper)
    {
        _personService = personService;
        _departmentService = departmentService;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    // GET: Person
    public async Task<IActionResult> Index()
    {
        // Admin can see all employees, Manager can only see their department
        if (_currentUserService.IsInRole("Admin"))
        {
            var result = await _personService.GetAllAsync();
            
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(new List<PersonListViewModel>());
            }

            var viewModel = _mapper.Map<IEnumerable<PersonListViewModel>>(result.Data);
            return View(viewModel);
        }
        else if (_currentUserService.IsInRole("Manager"))
        {
            var managerDepartmentId = _currentUserService.DepartmentId;
            if (!managerDepartmentId.HasValue)
            {
                TempData["Error"] = "Departman bilginiz bulunamadı. Lütfen yöneticinize başvurun.";
                return View(new List<PersonListViewModel>());
            }

            var result = await _personService.GetByDepartmentIdAsync(managerDepartmentId.Value);
            
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(new List<PersonListViewModel>());
            }

            var viewModel = _mapper.Map<IEnumerable<PersonListViewModel>>(result.Data);
            return View(viewModel);
        }
        else
        {
            TempData["Error"] = "Bu sayfaya erişim yetkiniz bulunmamaktadır.";
            return RedirectToAction("Index", "Home");
        }
    }

    // GET: Person/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var result = await _personService.GetByIdAsync(id);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        // Check if Manager can access this employee (department-based access control)
        if (_currentUserService.IsInRole("Manager"))
        {
            var managerDepartmentId = _currentUserService.DepartmentId;
            if (!managerDepartmentId.HasValue || result.Data!.DepartmentId != managerDepartmentId.Value)
            {
                TempData["Error"] = "Bu personelin bilgilerine erişim yetkiniz bulunmamaktadır.";
                return RedirectToAction(nameof(Index));
            }
        }

        var viewModel = _mapper.Map<PersonDetailViewModel>(result.Data);
        return View(viewModel);
    }

    // GET: Person/MyProfile - Employee için kendi profili
    public async Task<IActionResult> MyProfile()
    {
        if (_currentUserService.IsInRole("Employee"))
        {
            var currentPersonId = _currentUserService.PersonId;
            if (!currentPersonId.HasValue)
            {
                TempData["Error"] = "Profil bilginiz bulunamadı. Lütfen yöneticinize başvurun.";
                return RedirectToAction("Index", "Home");
            }

            var result = await _personService.GetByIdAsync(currentPersonId.Value);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction("Index", "Home");
            }

            var viewModel = _mapper.Map<PersonDetailViewModel>(result.Data);
            
            // KVKK uyumluluğu için hassas bilgileri maskele
            if (!string.IsNullOrEmpty(viewModel.TcKimlikNo))
            {
                viewModel.TcKimlikNo = DataMaskingHelper.MaskTcKimlikNo(viewModel.TcKimlikNo);
            }
            
            if (!string.IsNullOrEmpty(viewModel.SskNumber))
            {
                viewModel.SskNumber = DataMaskingHelper.MaskSskNo(viewModel.SskNumber);
            }
            
            if (!string.IsNullOrEmpty(viewModel.Email))
            {
                viewModel.Email = DataMaskingHelper.MaskEmail(viewModel.Email);
            }
            
            if (!string.IsNullOrEmpty(viewModel.Phone))
            {
                viewModel.Phone = DataMaskingHelper.MaskPhoneNumber(viewModel.Phone);
            }
            
            ViewData["Title"] = "Profilim";
            ViewData["IsEmployeeView"] = true;
            return View("MyProfile", viewModel);
        }
        
        // Admin/Manager için normal Index'e yönlendir
        return RedirectToAction("Index");
    }

    // GET: Person/Create - Sadece Admin ve Manager
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create()
    {
        await LoadDepartments();
        return View();
    }

    // POST: Person/Create - Sadece Admin ve Manager
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create(PersonCreateViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var dto = _mapper.Map<PersonCreateDto>(viewModel);
            var result = await _personService.CreateAsync(dto);
            
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

        await LoadDepartments();
        return View(viewModel);
    }

    // GET: Person/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _personService.GetByIdAsync(id);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<PersonEditViewModel>(result.Data);
        await LoadDepartments();
        return View(viewModel);
    }

    // POST: Person/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PersonEditViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var dto = _mapper.Map<PersonUpdateDto>(viewModel);
            var result = await _personService.UpdateAsync(dto);
            
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

        await LoadDepartments();
        return View(viewModel);
    }

    // GET: Person/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _personService.GetByIdAsync(id);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<PersonDetailViewModel>(result.Data);
        return View(viewModel);
    }

    // POST: Person/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _personService.DeleteAsync(id);
        
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

    // POST: Person/ToggleStatus/5
    [HttpPost]
    public async Task<IActionResult> ToggleStatus(int id, bool isActive)
    {
        var result = await _personService.SetActiveStatusAsync(id, isActive);
        
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

    private async Task LoadDepartments()
    {
        var departmentsResult = await _departmentService.GetAllAsync();
        
        if (departmentsResult.Success && departmentsResult.Data != null)
        {
            ViewBag.Departments = new SelectList(
                departmentsResult.Data.Where(d => d.IsActive),
                "Id", 
                "Name"
            );
        }
        else
        {
            ViewBag.Departments = new SelectList(Enumerable.Empty<SelectListItem>());
        }
    }
}
