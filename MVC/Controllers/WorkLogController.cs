using AutoMapper;
using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Models;

namespace MVC.Controllers
{
    public class WorkLogController : Controller
    {
        private readonly IWorkLogService _workLogService;
        private readonly IPersonService _personService;
        private readonly IMapper _mapper;

        public WorkLogController(IWorkLogService workLogService, IPersonService personService, IMapper mapper)
        {
            _workLogService = workLogService;
            _personService = personService;
            _mapper = mapper;
        }

        // GET: WorkLog
        public async Task<IActionResult> Index(WorkLogFilterViewModel? filter = null)
        {
            try
            {
                await LoadSelectListsAsync(filter);

                IEnumerable<WorkLogListDto> workLogs;

                if (filter?.PersonId.HasValue == true || filter?.StartDate.HasValue == true || filter?.EndDate.HasValue == true)
                {
                    // Apply filters
                    if (filter.PersonId.HasValue && filter.StartDate.HasValue && filter.EndDate.HasValue)
                    {
                        var result = await _workLogService.GetByPersonAndDateRangeAsync(
                            filter.PersonId.Value, 
                            filter.StartDate.Value, 
                            filter.EndDate.Value);
                        workLogs = result.Success ? result.Data : new List<WorkLogListDto>();
                    }
                    else if (filter.StartDate.HasValue && filter.EndDate.HasValue)
                    {
                        var result = await _workLogService.GetByDateRangeAsync(filter.StartDate.Value, filter.EndDate.Value);
                        workLogs = result.Success ? result.Data : new List<WorkLogListDto>();
                    }
                    else if (filter.PersonId.HasValue)
                    {
                        var result = await _workLogService.GetByPersonIdAsync(filter.PersonId.Value);
                        workLogs = result.Success ? result.Data : new List<WorkLogListDto>();
                    }
                    else
                    {
                        var result = await _workLogService.GetAllAsync();
                        workLogs = result.Success ? result.Data : new List<WorkLogListDto>();
                    }
                }
                else
                {
                    var result = await _workLogService.GetAllAsync();
                    workLogs = result.Success ? result.Data : new List<WorkLogListDto>();
                }

                // Apply additional filters
                if (!string.IsNullOrEmpty(filter?.WorkType))
                {
                    workLogs = workLogs.Where(w => w.WorkType == filter.WorkType);
                }

                if (!string.IsNullOrEmpty(filter?.Status))
                {
                    workLogs = workLogs.Where(w => w.Status == filter.Status);
                }

                if (filter?.OnlyOvertime == true)
                {
                    workLogs = workLogs.Where(w => w.IsOvertime);
                }

                if (filter?.OnlyLateArrivals == true)
                {
                    workLogs = workLogs.Where(w => w.IsLateArrival);
                }

                if (filter?.OnlyEarlyDepartures == true)
                {
                    workLogs = workLogs.Where(w => w.IsEarlyDeparture);
                }

                var viewModels = _mapper.Map<IEnumerable<WorkLogListViewModel>>(workLogs);
                ViewBag.Filter = filter;
                return View(viewModels);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Çalışma kayıtları yüklenirken hata oluştu: {ex.Message}";
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

        // GET: WorkLog/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new WorkLogCreateViewModel();
            await LoadCreateSelectListsAsync(viewModel);
            return View(viewModel);
        }

        // POST: WorkLog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: WorkLog/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _workLogService.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            var viewModel = _mapper.Map<WorkLogUpdateViewModel>(result.Data);
            await LoadUpdateSelectListsAsync(viewModel);
            return View(viewModel);
        }

        // POST: WorkLog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(WorkLogUpdateViewModel viewModel)
        {
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

            await LoadUpdateSelectListsAsync(viewModel);
            return View(viewModel);
        }

        // GET: WorkLog/Delete/5
        public async Task<IActionResult> Delete(int id)
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

        // POST: WorkLog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _workLogService.DeleteAsync(id);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        // POST: WorkLog/ToggleStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var result = await _workLogService.ToggleStatusAsync(id);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        // GET: WorkLog/CheckIn
        public async Task<IActionResult> CheckIn()
        {
            var viewModel = new WorkLogCheckInViewModel();
            await LoadCheckInSelectListsAsync(viewModel);
            return View(viewModel);
        }

        // POST: WorkLog/CheckIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn(WorkLogCheckInViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var dto = _mapper.Map<WorkLogCheckInDto>(viewModel);
                var result = await _workLogService.CheckInAsync(dto);

                if (result.Success)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = result.Message;
            }

            await LoadCheckInSelectListsAsync(viewModel);
            return View(viewModel);
        }

        // GET: WorkLog/CheckOut/5
        public async Task<IActionResult> CheckOut(int id)
        {
            var result = await _workLogService.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            var workLog = result.Data;
            if (workLog.EndTime.HasValue)
            {
                TempData["Error"] = "Bu kayıt için çıkış işlemi zaten yapılmış.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new WorkLogCheckOutViewModel
            {
                Id = workLog.Id,
                PersonName = workLog.PersonName,
                Date = workLog.Date,
                StartTime = workLog.StartTime
            };

            return View(viewModel);
        }

        // POST: WorkLog/CheckOut/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(WorkLogCheckOutViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var dto = _mapper.Map<WorkLogCheckOutDto>(viewModel);
                var result = await _workLogService.CheckOutAsync(dto);

                if (result.Success)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = result.Message;
            }

            return View(viewModel);
        }

        // GET: WorkLog/TimeSheet
        public async Task<IActionResult> TimeSheet(int? personId, DateTime? startDate, DateTime? endDate)
        {
            if (!personId.HasValue || !startDate.HasValue || !endDate.HasValue)
            {
                var viewModel = new WorkLogTimeSheetViewModel();
                await LoadTimeSheetSelectListAsync();
                ViewBag.PersonId = personId;
                ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
                ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
                return View(viewModel);
            }

            var result = await _workLogService.GetTimeSheetAsync(personId.Value, startDate.Value, endDate.Value);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                var emptyViewModel = new WorkLogTimeSheetViewModel();
                await LoadTimeSheetSelectListAsync();
                return View(emptyViewModel);
            }

            var timeSheetViewModel = _mapper.Map<WorkLogTimeSheetViewModel>(result.Data);
            await LoadTimeSheetSelectListAsync();
            return View(timeSheetViewModel);
        }

        // GET: WorkLog/Reports/Overtime
        public async Task<IActionResult> OvertimeReport(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                ViewBag.StartDate = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd");
                ViewBag.EndDate = DateTime.Today.ToString("yyyy-MM-dd");
                return View(new List<WorkLogListViewModel>());
            }

            var result = await _workLogService.GetOvertimeReportAsync(startDate.Value, endDate.Value);
            var viewModels = result.Success ? _mapper.Map<IEnumerable<WorkLogListViewModel>>(result.Data) : new List<WorkLogListViewModel>();
            
            ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");
            return View(viewModels);
        }

        // GET: WorkLog/Reports/LateArrivals
        public async Task<IActionResult> LateArrivalReport(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                ViewBag.StartDate = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd");
                ViewBag.EndDate = DateTime.Today.ToString("yyyy-MM-dd");
                return View(new List<WorkLogListViewModel>());
            }

            var result = await _workLogService.GetLateArrivalReportAsync(startDate.Value, endDate.Value);
            var viewModels = result.Success ? _mapper.Map<IEnumerable<WorkLogListViewModel>>(result.Data) : new List<WorkLogListViewModel>();
            
            ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");
            return View(viewModels);
        }

        // GET: WorkLog/Active
        public async Task<IActionResult> Active()
        {
            var result = await _workLogService.GetAllAsync();
            var workLogs = result.Success ? result.Data.Where(w => w.Status == "Active") : new List<WorkLogListDto>();
            var viewModels = _mapper.Map<IEnumerable<WorkLogListViewModel>>(workLogs);
            return View("Index", viewModels);
        }

        // GET: WorkLog/PendingApprovals
        public async Task<IActionResult> PendingApprovals()
        {
            var result = await _workLogService.GetPendingApprovalsAsync();
            var viewModels = result.Success ? _mapper.Map<IEnumerable<WorkLogListViewModel>>(result.Data) : new List<WorkLogListViewModel>();
            return View("Index", viewModels);
        }

        // Helper Methods
        private async Task LoadSelectListsAsync(WorkLogFilterViewModel? filter)
        {
            var personsResult = await _personService.GetAllAsync();
            var persons = personsResult.Success ? personsResult.Data : new List<PersonListDto>();
            
            ViewBag.PersonSelectList = new SelectList(persons, "Id", "FullName", filter?.PersonId);
            ViewBag.WorkTypeSelectList = new SelectList(GetWorkTypes(), "Value", "Text", filter?.WorkType);
            ViewBag.StatusSelectList = new SelectList(GetStatusTypes(), "Value", "Text", filter?.Status);
        }

        private async Task LoadCreateSelectListsAsync(WorkLogCreateViewModel viewModel)
        {
            var personsResult = await _personService.GetAllAsync();
            var persons = personsResult.Success ? personsResult.Data : new List<PersonListDto>();
            
            viewModel.PersonSelectList = new SelectList(persons, "Id", "FullName", viewModel.PersonId);
            viewModel.WorkTypeSelectList = new SelectList(GetWorkTypes(), "Value", "Text", viewModel.WorkType);
        }

        private async Task LoadUpdateSelectListsAsync(WorkLogUpdateViewModel viewModel)
        {
            var personsResult = await _personService.GetAllAsync();
            var persons = personsResult.Success ? personsResult.Data : new List<PersonListDto>();
            
            viewModel.PersonSelectList = new SelectList(persons, "Id", "FullName", viewModel.PersonId);
            viewModel.WorkTypeSelectList = new SelectList(GetWorkTypes(), "Value", "Text", viewModel.WorkType);
            viewModel.StatusSelectList = new SelectList(GetStatusTypes(), "Value", "Text", viewModel.Status);
        }

        private async Task LoadCheckInSelectListsAsync(WorkLogCheckInViewModel viewModel)
        {
            var personsResult = await _personService.GetAllAsync();
            var persons = personsResult.Success ? personsResult.Data : new List<PersonListDto>();
            
            viewModel.PersonSelectList = new SelectList(persons, "Id", "FullName", viewModel.PersonId);
            viewModel.WorkTypeSelectList = new SelectList(GetWorkTypes(), "Value", "Text", viewModel.WorkType);
        }

        private async Task LoadTimeSheetSelectListAsync()
        {
            var personsResult = await _personService.GetAllAsync();
            var persons = personsResult.Success ? personsResult.Data : new List<PersonListDto>();
            
            ViewBag.PersonSelectList = new SelectList(persons, "Id", "FullName");
        }

        private static List<SelectListItem> GetWorkTypes()
        {
            return new List<SelectListItem>
            {
                new() { Value = "", Text = "-- Tümü --" },
                new() { Value = "Office", Text = "Ofis" },
                new() { Value = "Remote", Text = "Uzaktan" },
                new() { Value = "Field", Text = "Saha" },
                new() { Value = "Meeting", Text = "Toplantı" }
            };
        }

        private static List<SelectListItem> GetStatusTypes()
        {
            return new List<SelectListItem>
            {
                new() { Value = "", Text = "-- Tümü --" },
                new() { Value = "Active", Text = "Aktif" },
                new() { Value = "Completed", Text = "Tamamlandı" },
                new() { Value = "Approved", Text = "Onaylandı" },
                new() { Value = "Rejected", Text = "Reddedildi" },
                new() { Value = "Pending", Text = "Onay Bekliyor" }
            };
        }

        // Debug action to check WorkLog data
        public async Task<IActionResult> Debug()
        {
            try
            {
                var allWorkLogs = await _workLogService.GetAllAsync();
                var allCount = allWorkLogs?.Data?.Count() ?? 0;
                
                // Test the specific date range you're using
                var startDate = new DateTime(2025, 9, 1);
                var endDate = new DateTime(2025, 12, 2);
                var dateRangeWorkLogs = await _workLogService.GetByDateRangeAsync(startDate, endDate);
                var dateRangeCount = dateRangeWorkLogs?.Data?.Count() ?? 0;
                
                return Json(new
                {
                    Success = true,
                    Message = $"GetAllAsync found: {allCount}, GetByDateRangeAsync (2025-09-01 to 2025-12-02) found: {dateRangeCount}",
                    AllSuccess = allWorkLogs?.Success ?? false,
                    DateRangeSuccess = dateRangeWorkLogs?.Success ?? false,
                    DateRangeMessage = dateRangeWorkLogs?.Message ?? "No message",
                    SampleData = allWorkLogs?.Data?.Take(3).Select(w => new 
                    {
                        w.Id,
                        w.PersonName,
                        w.Date,
                        w.Status,
                        w.IsActive
                    })
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Details = ex.ToString()
                });
            }
        }
    }
}
