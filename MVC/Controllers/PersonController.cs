using BLL.Services;
using BLL.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Models;
using AutoMapper;

namespace MVC.Controllers;

public class PersonController : Controller
{
    private readonly IPersonService _personService;
    private readonly IDepartmentService _departmentService;
    private readonly IMapper _mapper;

    public PersonController(IPersonService personService, IDepartmentService departmentService, IMapper mapper)
    {
        _personService = personService;
        _departmentService = departmentService;
        _mapper = mapper;
    }

    // GET: Person
    public async Task<IActionResult> Index()
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

    // GET: Person/Details/5
    public async Task<IActionResult> Details(int id)
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

    // GET: Person/Create
    public async Task<IActionResult> Create()
    {
        await LoadDepartments();
        return View();
    }

    // POST: Person/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
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
