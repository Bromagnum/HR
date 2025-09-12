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

        // GET: WorkLog/Create - Sadece Admin/Manager (Employee yapamaz)
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create()
        {
            await LoadPersonsForFilter();
            
            var viewModel = new WorkLogCreateViewModel
            {
                Date = DateTime.Today,
                StartTime = new TimeSpan(9, 0, 0),
                WorkType = "Office"
            };

            viewModel.PersonSelectList = ViewBag.PersonSelectList as SelectList;
            viewModel.WorkTypeSelectList = new SelectList(new[]
            {
                new { Value = "Office", Text = "Ofis" },
                new { Value = "Remote", Text = "Uzaktan" },
                new { Value = "Field", Text = "Saha" },
                new { Value = "Meeting", Text = "Toplantı" }
            }, "Value", "Text");

            return View(viewModel);
        }

        // POST: WorkLog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create(WorkLogCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadPersonsForFilter();
                model.PersonSelectList = ViewBag.PersonSelectList as SelectList;
                model.WorkTypeSelectList = new SelectList(new[]
                {
                    new { Value = "Office", Text = "Ofis" },
                    new { Value = "Remote", Text = "Uzaktan" },
                    new { Value = "Field", Text = "Saha" },
                    new { Value = "Meeting", Text = "Toplantı" }
                }, "Value", "Text");
                return View(model);
            }

            var createDto = _mapper.Map<WorkLogCreateDto>(model);
            var result = await _workLogService.CreateAsync(createDto);

            if (result.Success)
            {
                TempData["Success"] = "Çalışma kaydı başarıyla oluşturuldu.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = result.Message;
            await LoadPersonsForFilter();
            model.PersonSelectList = ViewBag.PersonSelectList as SelectList;
            model.WorkTypeSelectList = new SelectList(new[]
            {
                new { Value = "Office", Text = "Ofis" },
                new { Value = "Remote", Text = "Uzaktan" },
                new { Value = "Field", Text = "Saha" },
                new { Value = "Meeting", Text = "Toplantı" }
            }, "Value", "Text");
            return View(model);
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

            await LoadPersonsForFilter();
            
            var viewModel = _mapper.Map<WorkLogUpdateViewModel>(result.Data);
            viewModel.PersonSelectList = ViewBag.PersonSelectList as SelectList;
            viewModel.WorkTypeSelectList = new SelectList(new[]
            {
                new { Value = "Office", Text = "Ofis" },
                new { Value = "Remote", Text = "Uzaktan" },
                new { Value = "Field", Text = "Saha" },
                new { Value = "Meeting", Text = "Toplantı" }
            }, "Value", "Text");
            viewModel.StatusSelectList = new SelectList(new[]
            {
                new { Value = "Active", Text = "Aktif" },
                new { Value = "Completed", Text = "Tamamlandı" },
                new { Value = "Approved", Text = "Onaylandı" },
                new { Value = "Rejected", Text = "Reddedildi" }
            }, "Value", "Text");

            return View(viewModel);
        }

        // POST: WorkLog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id, WorkLogUpdateViewModel model)
        {
            if (id != model.Id)
            {
                TempData["Error"] = "Geçersiz işlem.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                await LoadPersonsForFilter();
                model.PersonSelectList = ViewBag.PersonSelectList as SelectList;
                model.WorkTypeSelectList = new SelectList(new[]
                {
                    new { Value = "Office", Text = "Ofis" },
                    new { Value = "Remote", Text = "Uzaktan" },
                    new { Value = "Field", Text = "Saha" },
                    new { Value = "Meeting", Text = "Toplantı" }
                }, "Value", "Text");
                model.StatusSelectList = new SelectList(new[]
                {
                    new { Value = "Active", Text = "Aktif" },
                    new { Value = "Completed", Text = "Tamamlandı" },
                    new { Value = "Approved", Text = "Onaylandı" },
                    new { Value = "Rejected", Text = "Reddedildi" }
                }, "Value", "Text");
                return View(model);
            }

            var updateDto = _mapper.Map<WorkLogUpdateDto>(model);
            var result = await _workLogService.UpdateAsync(updateDto);

            if (result.Success)
            {
                TempData["Success"] = "Çalışma kaydı başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = result.Message;
            await LoadPersonsForFilter();
            model.PersonSelectList = ViewBag.PersonSelectList as SelectList;
            model.WorkTypeSelectList = new SelectList(new[]
            {
                new { Value = "Office", Text = "Ofis" },
                new { Value = "Remote", Text = "Uzaktan" },
                new { Value = "Field", Text = "Saha" },
                new { Value = "Meeting", Text = "Toplantı" }
            }, "Value", "Text");
            model.StatusSelectList = new SelectList(new[]
            {
                new { Value = "Active", Text = "Aktif" },
                new { Value = "Completed", Text = "Tamamlandı" },
                new { Value = "Approved", Text = "Onaylandı" },
                new { Value = "Rejected", Text = "Reddedildi" }
            }, "Value", "Text");
            return View(model);
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

        // GET: WorkLog/TimeSheet - Çalışma saati tablosu
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> TimeSheet(DateTime? startDate, DateTime? endDate, int? personId)
        {
            if (!startDate.HasValue) startDate = DateTime.Today.AddDays(-30);
            if (!endDate.HasValue) endDate = DateTime.Today;

            // Load persons for filter
            await LoadPersonsForFilter();
            
            ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");
            ViewBag.PersonId = personId;

            // Eğer personId seçilmemişse boş model döndür
            if (!personId.HasValue)
            {
                return View(new WorkLogTimeSheetViewModel 
                { 
                    StartDate = startDate.Value, 
                    EndDate = endDate.Value 
                });
            }

            // Person bilgisini al
            var personResult = await _personService.GetByIdAsync(personId.Value);
            if (!personResult.Success)
            {
                TempData["Error"] = "Personel bulunamadı.";
                return View(new WorkLogTimeSheetViewModel 
                { 
                    StartDate = startDate.Value, 
                    EndDate = endDate.Value 
                });
            }

            var workLogResult = await _workLogService.GetByPersonAndDateRangeAsync(personId.Value, startDate.Value, endDate.Value);
            
            var workLogs = workLogResult.Success ? _mapper.Map<List<WorkLogListViewModel>>(workLogResult.Data) : new List<WorkLogListViewModel>();

            var viewModel = new WorkLogTimeSheetViewModel
            {
                PersonId = personId.Value,
                PersonName = personResult.Data.FullName,
                DepartmentName = personResult.Data.DepartmentName,
                StartDate = startDate.Value,
                EndDate = endDate.Value,
                WorkLogs = workLogs,
                TotalHours = workLogs.Sum(w => w.TotalHours),
                TotalRegularHours = workLogs.Sum(w => w.RegularHours),
                TotalOvertimeHours = workLogs.Sum(w => w.OvertimeHours),
                TotalWorkDays = workLogs.Count,
                LateArrivals = workLogs.Count(w => w.IsLateArrival),
                EarlyDepartures = workLogs.Count(w => w.IsEarlyDeparture)
            };
            
            return View(viewModel);
        }

        // GET: WorkLog/CheckIn - Giriş çıkış işlemleri
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> CheckIn()
        {
            // Load persons for dropdown
            await LoadPersonsForFilter();
            
            var viewModel = new WorkLogCheckInViewModel
            {
                StartTime = DateTime.Now.TimeOfDay,
                WorkType = "Office"
            };

            // Set PersonSelectList from ViewBag
            viewModel.PersonSelectList = ViewBag.PersonSelectList as SelectList;

            // Load work types for dropdown
            viewModel.WorkTypeSelectList = new SelectList(new[]
            {
                new { Value = "Office", Text = "Ofis" },
                new { Value = "Remote", Text = "Uzaktan" },
                new { Value = "Field", Text = "Saha" },
                new { Value = "Meeting", Text = "Toplantı" }
            }, "Value", "Text");

            ViewBag.Date = DateTime.Today.ToString("dd.MM.yyyy");
            
            return View(viewModel);
        }

        // POST: WorkLog/CheckIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> CheckIn(WorkLogCheckInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadPersonsForFilter();
                model.PersonSelectList = ViewBag.PersonSelectList as SelectList;
                model.WorkTypeSelectList = new SelectList(new[]
                {
                    new { Value = "Office", Text = "Ofis" },
                    new { Value = "Remote", Text = "Uzaktan" },
                    new { Value = "Field", Text = "Saha" },
                    new { Value = "Meeting", Text = "Toplantı" }
                }, "Value", "Text");
                return View(model);
            }

            var checkInDto = new WorkLogCheckInDto
            {
                PersonId = model.PersonId,
                StartTime = model.StartTime,
                WorkType = model.WorkType,
                Location = model.Location,
                Notes = model.Notes
            };

            var result = await _workLogService.CheckInAsync(checkInDto);

            if (result.Success)
            {
                TempData["Success"] = "Giriş başarıyla kaydedildi.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = result.Message;
            await LoadPersonsForFilter();
            model.PersonSelectList = ViewBag.PersonSelectList as SelectList;
            model.WorkTypeSelectList = new SelectList(new[]
            {
                new { Value = "Office", Text = "Ofis" },
                new { Value = "Remote", Text = "Uzaktan" },
                new { Value = "Field", Text = "Saha" },
                new { Value = "Meeting", Text = "Toplantı" }
            }, "Value", "Text");
            return View(model);
        }

        // GET: WorkLog/OvertimeReport - Mesai raporu
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> OvertimeReport(DateTime? startDate, DateTime? endDate, int? departmentId)
        {
            if (!startDate.HasValue) startDate = DateTime.Today.AddMonths(-1);
            if (!endDate.HasValue) endDate = DateTime.Today;

            var result = await _workLogService.GetOvertimeReportAsync(startDate.Value, endDate.Value);
            
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(new List<WorkLogListViewModel>());
            }

            var viewModel = _mapper.Map<List<WorkLogListViewModel>>(result.Data);
            
            ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");
            ViewBag.DepartmentId = departmentId;
            
            return View(viewModel);
        }

        // GET: WorkLog/Active - Aktif çalışma kayıtları
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Active()
        {
            var result = await _workLogService.GetAllAsync();
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(new List<WorkLogListViewModel>());
            }

            // Sadece aktif kayıtları filtrele (EndTime null olanlar)
            var activeWorkLogs = result.Data.Where(w => w.Status == "Active" && !w.EndTime.HasValue).ToList();
            var viewModel = _mapper.Map<List<WorkLogListViewModel>>(activeWorkLogs);
            
            ViewData["Title"] = "Aktif Çalışma Kayıtları";
            return View("Index", viewModel);
        }

        // GET: WorkLog/PendingApprovals - Onay bekleyen kayıtlar
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> PendingApprovals()
        {
            var result = await _workLogService.GetPendingApprovalsAsync();
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(new List<WorkLogListViewModel>());
            }

            var viewModel = _mapper.Map<List<WorkLogListViewModel>>(result.Data);
            ViewData["Title"] = "Onay Bekleyen Çalışma Kayıtları";
            return View("Index", viewModel);
        }

        // GET: WorkLog/LateArrivalReport - Geç gelme raporu
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> LateArrivalReport(DateTime? startDate, DateTime? endDate, int? departmentId)
        {
            if (!startDate.HasValue) startDate = DateTime.Today.AddMonths(-1);
            if (!endDate.HasValue) endDate = DateTime.Today;

            var result = await _workLogService.GetLateArrivalReportAsync(startDate.Value, endDate.Value);
            
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(new List<WorkLogListViewModel>());
            }

            var viewModel = _mapper.Map<List<WorkLogListViewModel>>(result.Data);
            
            ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");
            ViewBag.DepartmentId = departmentId;
            
            return View(viewModel);
        }

        // Helper method to load persons for filter dropdowns
        private async Task LoadPersonsForFilter()
        {
            var personsResult = await _personService.GetAllAsync();
            if (personsResult.Success && personsResult.Data != null)
            {
                var personsList = personsResult.Data
                    .Where(p => p.IsActive)
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = $"{p.FirstName} {p.LastName} - {p.DepartmentName ?? "Departmansız"}"
                    })
                    .OrderBy(x => x.Text)
                    .ToList();

                ViewBag.Persons = personsList;
                ViewBag.PersonSelectList = new SelectList(personsList, "Value", "Text");
            }
            else
            {
                ViewBag.Persons = new List<SelectListItem>();
                ViewBag.PersonSelectList = new SelectList(new List<SelectListItem>(), "Value", "Text");
            }
        }

    }
}
