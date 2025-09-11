using AutoMapper;
using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using MVC.Models;
using MVC.Services;

namespace MVC.Controllers
{
    [Authorize] // Tüm çalışanlar çalışma saatlerini görüntüleyebilir
    public class WorkLogController : Controller
    {
        private readonly IWorkLogService _workLogService;
        private readonly IPersonService _personService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public WorkLogController(IWorkLogService workLogService, IPersonService personService, ICurrentUserService currentUserService, IMapper mapper)
        {
            _workLogService = workLogService;
            _personService = personService;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        // GET: WorkLog
        public async Task<IActionResult> Index(WorkLogFilterViewModel? filter = null)
        {
            try
            {
                // Employee sadece kendi kayıtlarını görebilir
                if (_currentUserService.IsInRole("Employee"))
                {
                    var currentPersonId = _currentUserService.PersonId;
                    if (!currentPersonId.HasValue)
                    {
                        TempData["Error"] = "Personel kimliği alınamadı. Lütfen yöneticinize başvurun.";
                        return RedirectToAction("Index", "Home");
                    }

                    // Employee için sadece kendi kayıtları
                    var result = await _workLogService.GetByPersonIdAsync(currentPersonId.Value);
                    var workLogData = result.Success ? result.Data : new List<WorkLogListDto>();
                    var employeeViewModels = _mapper.Map<List<WorkLogListViewModel>>(workLogData);
                    
                    ViewData["Title"] = "Çalışma Kayıtlarım";
                    ViewData["IsEmployeeView"] = true;
                    ViewData["PersonId"] = currentPersonId.Value;
                    
                    return View("MyWorkLogs", employeeViewModels);
                }

                // Admin/Manager için normal işlem
                return await ProcessAdminManagerView(filter);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Bir hata oluştu: " + ex.Message;
                return View(new List<WorkLogListViewModel>());
            }
        }

        private async Task<IActionResult> ProcessAdminManagerView(WorkLogFilterViewModel? filter)
        {
            try
            {
                var result = await _workLogService.GetAllAsync();
                if (!result.Success)
                {
                    TempData["Error"] = result.Message;
                    return View(new List<WorkLogListViewModel>());
                }

                var viewModels = _mapper.Map<List<WorkLogListViewModel>>(result.Data);
                ViewData["Filter"] = filter;
                return View(viewModels);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Bir hata oluştu: " + ex.Message;
                return View(new List<WorkLogListViewModel>());
            }
        }

        // GET: WorkLog/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var result = await _workLogService.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            var viewModel = _mapper.Map<WorkLogDetailViewModel>(result.Data);
            return View(viewModel);
        }

        // GET: WorkLog/Create - Sadece Admin/Manager
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create()
        {
            var viewModel = new WorkLogCreateViewModel();
            await LoadCreateSelectListsAsync(viewModel);
            return View(viewModel);
        }

        // POST: WorkLog/Create - Sadece Admin/Manager
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create(WorkLogCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var dto = _mapper.Map<WorkLogCreateDto>(viewModel);
                var result = await _workLogService.CreateAsync(dto);

                if (result.Success)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = result.Message;
            }

            await LoadCreateSelectListsAsync(viewModel);
            return View(viewModel);
        }

        // GET: WorkLog/Edit/5 - Sadece Admin/Manager
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _workLogService.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            var viewModel = _mapper.Map<WorkLogEditViewModel>(result.Data);
            await LoadEditSelectListsAsync(viewModel);
            return View(viewModel);
        }

        // POST: WorkLog/Edit/5 - Sadece Admin/Manager
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id, WorkLogEditViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var dto = _mapper.Map<WorkLogUpdateDto>(viewModel);
                var result = await _workLogService.UpdateAsync(dto);

                if (result.Success)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = result.Message;
            }

            await LoadEditSelectListsAsync(viewModel);
            return View(viewModel);
        }

        // POST: WorkLog/Delete/5 - Sadece Admin/Manager
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _workLogService.DeleteAsync(id);
            
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

        private async Task LoadCreateSelectListsAsync(WorkLogCreateViewModel viewModel)
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

        private async Task LoadEditSelectListsAsync(WorkLogEditViewModel viewModel)
        {
            await LoadCreateSelectListsAsync(new WorkLogCreateViewModel());
        }
    }
}
