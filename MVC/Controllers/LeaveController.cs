using AutoMapper;
using BLL.Services;
using BLL.Services.Export;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using MVC.Models;
using DAL.Entities;

namespace MVC.Controllers;

[Authorize] // Tüm çalışanlar erişebilir
public class LeaveController : Controller
{
    private readonly ILeaveService _leaveService;
    private readonly ILeaveTypeService _leaveTypeService;
    private readonly IPersonService _personService;
    private readonly IDepartmentService _departmentService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPdfExportService _pdfExportService;
    private readonly IMapper _mapper;

    public LeaveController(
        ILeaveService leaveService,
        ILeaveTypeService leaveTypeService,
        IPersonService personService,
        IDepartmentService departmentService,
        ICurrentUserService currentUserService,
        IPdfExportService pdfExportService,
        IMapper mapper)
    {
        _leaveService = leaveService;
        _leaveTypeService = leaveTypeService;
        _personService = personService;
        _departmentService = departmentService;
        _currentUserService = currentUserService;
        _pdfExportService = pdfExportService;
        _mapper = mapper;
    }

    // GET: Leave
    public async Task<IActionResult> Index(LeaveFilterViewModel filter)
    {
        // Employee sadece kendi izinlerini görsün
        if (_currentUserService.IsInRole("Employee"))
        {
            return RedirectToAction("MyLeaves");
        }

        // Prepare filter dropdowns
        await PrepareFilterViewData(filter);

        var filterDto = _mapper.Map<BLL.DTOs.LeaveFilterDto>(filter);
        
        // Apply department filtering for Managers
        if (_currentUserService.IsInRole("Manager"))
        {
            var managerDepartmentId = _currentUserService.DepartmentId;
            System.Diagnostics.Debug.WriteLine($"Manager accessing Leave Index - DepartmentId: {managerDepartmentId}");
            
            if (!managerDepartmentId.HasValue)
            {
                TempData["Error"] = "Departman bilginiz bulunamadı. Lütfen yöneticinize başvurun.";
                return View(new List<LeaveListViewModel>());
            }
            
            // Set department filter for manager
            filterDto.DepartmentId = managerDepartmentId.Value;
            System.Diagnostics.Debug.WriteLine($"Department filter applied: {managerDepartmentId.Value}");
        }

        // For Managers, always use filtered approach (department filtering is applied)
        // For Admins, use GetAllAsync only if no filters are applied
        var hasFilters = !string.IsNullOrEmpty(filter.SearchTerm) || filter.PersonId.HasValue || 
                         filter.LeaveTypeId.HasValue || filter.Status.HasValue || filterDto.DepartmentId.HasValue;
        
        System.Diagnostics.Debug.WriteLine($"LeaveController.Index - hasFilters: {hasFilters}, DepartmentId: {filterDto.DepartmentId}");
        
        var result = hasFilters 
            ? await _leaveService.GetFilteredAsync(filterDto)
            : await _leaveService.GetAllAsync();
            
        System.Diagnostics.Debug.WriteLine($"LeaveController.Index - Result Success: {result.IsSuccess}, Count: {result.Data?.Count() ?? 0}");

        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return View(new List<LeaveListViewModel>());
        }

        var viewModels = _mapper.Map<List<LeaveListViewModel>>(result.Data);
        ViewData["Filter"] = filter;
        return View(viewModels);
    }


    // GET: Leave/MyLeaves - Employee için kendi izinleri
    public async Task<IActionResult> MyLeaves(int? year)
    {
        var currentPersonId = _currentUserService.PersonId;
        if (!currentPersonId.HasValue)
        {
            TempData["Error"] = "Personel kimliği alınamadı. Lütfen yöneticinize başvurun.";
            return RedirectToAction("Index", "Home");
        }
        
        if (year == null) year = DateTime.Now.Year;

        var result = await _leaveService.GetLeavesByPersonAsync(currentPersonId.Value, year);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return View(new List<LeaveListViewModel>());
        }

        var viewModels = _mapper.Map<List<LeaveListViewModel>>(result.Data);
        ViewData["Title"] = $"İzinlerim ({year})";
        ViewData["CurrentYear"] = year;
        ViewData["PersonId"] = currentPersonId.Value;
        ViewData["IsEmployeeView"] = _currentUserService.IsInRole("Employee");
        
        return View(viewModels);
    }

    // GET: Leave/Calendar
    public async Task<IActionResult> Calendar(DateTime? startDate, DateTime? endDate, int? departmentId)
    {
        if (startDate == null) startDate = DateTime.Today.AddMonths(-1);
        if (endDate == null) endDate = DateTime.Today.AddMonths(2);

        var result = await _leaveService.GetCalendarDataAsync(startDate.Value, endDate.Value, departmentId);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return View(new List<LeaveCalendarViewModel>());
        }

        var viewModels = _mapper.Map<List<LeaveCalendarViewModel>>(result.Data);
        
        // Prepare department dropdown
        var departmentsResult = await _departmentService.GetAllAsync();
        if (departmentsResult.IsSuccess)
        {
            ViewBag.Departments = new SelectList(departmentsResult.Data, "Id", "Name", departmentId);
        }

        ViewData["StartDate"] = startDate.Value.ToString("yyyy-MM-dd");
        ViewData["EndDate"] = endDate.Value.ToString("yyyy-MM-dd");
        ViewData["DepartmentId"] = departmentId;

        return View(viewModels);
    }

    // GET: Leave/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var result = await _leaveService.GetByIdAsync(id);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<LeaveDetailViewModel>(result.Data);
        
        // Authorization data for view
        ViewData["CurrentUserPersonId"] = _currentUserService.PersonId;
        
        // Check if current user can approve this leave
        bool canApprove = false;
        if (_currentUserService.IsInRole("Admin"))
        {
            canApprove = true;
        }
        else if (_currentUserService.IsInRole("Manager"))
        {
            // Manager can approve if the leave belongs to someone in their department
            var currentUserDepartmentId = _currentUserService.DepartmentId;
            if (currentUserDepartmentId.HasValue && result.Data?.DepartmentId == currentUserDepartmentId.Value)
            {
                canApprove = true;
            }
        }
        
        ViewData["CanApproveLeave"] = canApprove;
        
        return View(viewModel);
    }

    // GET: Leave/Create
    public async Task<IActionResult> Create()
    {
        // Employee için özelleştirilmiş form
        if (_currentUserService.IsInRole("Employee"))
        {
            var currentPersonId = _currentUserService.PersonId;
            if (!currentPersonId.HasValue)
            {
                TempData["Error"] = "Personel kimliği alınamadı. Lütfen yöneticinize başvurun.";
                return RedirectToAction("MyLeaves");
            }

            await LoadLeaveTypes();
            var viewModel = new LeaveCreateViewModel
            {
                PersonId = currentPersonId.Value,
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(1)
            };
            
            ViewData["Title"] = "Yeni İzin Talebi";
            ViewData["IsEmployeeView"] = true;
            return View("CreateEmployee", viewModel);
        }

        // Admin/Manager için tam form
        var adminViewModel = new LeaveCreateViewModel();
        await PrepareCreateEditViewData(adminViewModel);
        return View(adminViewModel);
    }

    // POST: Leave/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LeaveCreateViewModel viewModel)
    {
        try
        {
            // Debug: Log incoming request
            System.Diagnostics.Debug.WriteLine($"Leave Create POST called with PersonId: {viewModel.PersonId}, LeaveTypeId: {viewModel.LeaveTypeId}");
            
            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("ModelState is invalid");
                foreach (var error in ModelState)
                {
                    System.Diagnostics.Debug.WriteLine($"Field: {error.Key}, Errors: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                await PrepareCreateEditViewData(viewModel);
                return View(viewModel);
            }

            var dto = _mapper.Map<BLL.DTOs.LeaveCreateDto>(viewModel);
            System.Diagnostics.Debug.WriteLine($"Mapped DTO: PersonId={dto.PersonId}, LeaveTypeId={dto.LeaveTypeId}, TotalDays={dto.TotalDays}");
            
            var result = await _leaveService.CreateAsync(dto);
            System.Diagnostics.Debug.WriteLine($"Service result: Success={result.IsSuccess}, Message={result.Message}");

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;
                await PrepareCreateEditViewData(viewModel);
                return View(viewModel);
            }

            TempData["Success"] = "İzin talebi başarıyla oluşturuldu.";
            System.Diagnostics.Debug.WriteLine($"Redirecting to Details with ID: {result.Data?.Id}");
            return RedirectToAction(nameof(Details), new { id = result.Data?.Id });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Exception in Create: {ex.Message}");
            TempData["Error"] = $"Bir hata oluştu: {ex.Message}";
            await PrepareCreateEditViewData(viewModel);
            return View(viewModel);
        }
    }

    // GET: Leave/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _leaveService.GetByIdAsync(id);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        // Only allow editing pending leaves
        if (result.Data?.Status != LeaveStatus.Pending)
        {
            TempData["Error"] = "Sadece bekleyen izinler düzenlenebilir.";
            return RedirectToAction(nameof(Details), new { id });
        }

        // Authorization check
        bool canEdit = false;
        if (_currentUserService.IsInRole("Admin"))
        {
            canEdit = true;
        }
        else if (_currentUserService.IsInRole("Manager"))
        {
            // Manager can edit leaves in their department
            var currentUserDepartmentId = _currentUserService.DepartmentId;
            if (currentUserDepartmentId.HasValue && result.Data?.DepartmentId == currentUserDepartmentId.Value)
            {
                canEdit = true;
            }
        }
        else if (_currentUserService.IsInRole("Employee"))
        {
            // Employee can only edit their own leaves
            var currentUserPersonId = _currentUserService.PersonId;
            if (currentUserPersonId.HasValue && result.Data?.PersonId == currentUserPersonId.Value)
            {
                canEdit = true;
            }
        }

        if (!canEdit)
        {
            TempData["Error"] = "Bu izin talebini düzenleme yetkiniz bulunmamaktadır.";
            return RedirectToAction(nameof(Details), new { id });
        }

        var viewModel = _mapper.Map<LeaveEditViewModel>(result.Data);
        await PrepareCreateEditViewData(viewModel);
        return View(viewModel);
    }

    // POST: Leave/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, LeaveEditViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData["Error"] = "Geçersiz istek.";
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            await PrepareCreateEditViewData(viewModel);
            return View(viewModel);
        }

        var dto = _mapper.Map<BLL.DTOs.LeaveUpdateDto>(viewModel);
        var result = await _leaveService.UpdateAsync(dto);

        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            await PrepareCreateEditViewData(viewModel);
            return View(viewModel);
        }

        TempData["Success"] = "İzin talebi başarıyla güncellendi.";
        return RedirectToAction(nameof(Details), new { id = viewModel.Id });
    }

    // GET: Leave/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _leaveService.GetByIdAsync(id);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var viewModel = _mapper.Map<LeaveDetailViewModel>(result.Data);
        return View(viewModel);
    }

    // POST: Leave/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _leaveService.DeleteAsync(id);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Delete), new { id });
        }

        TempData["Success"] = "İzin talebi başarıyla silindi.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Leave/Approve/5
    public async Task<IActionResult> Approve(int id)
    {
        var result = await _leaveService.GetByIdAsync(id);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        if (result.Data?.Status != LeaveStatus.Pending)
        {
            TempData["Error"] = "Sadece bekleyen izinler onaylanabilir.";
            return RedirectToAction(nameof(Details), new { id });
        }

        // Authorization check - Only Admin and authorized Managers can approve
        bool canApprove = false;
        if (_currentUserService.IsInRole("Admin"))
        {
            canApprove = true;
        }
        else if (_currentUserService.IsInRole("Manager"))
        {
            // Manager can approve if the leave belongs to someone in their department
            var currentUserDepartmentId = _currentUserService.DepartmentId;
            if (currentUserDepartmentId.HasValue && result.Data?.DepartmentId == currentUserDepartmentId.Value)
            {
                canApprove = true;
            }
        }

        if (!canApprove)
        {
            TempData["Error"] = "Bu izin talebini onaylama/reddetme yetkiniz bulunmamaktadır.";
            return RedirectToAction(nameof(Details), new { id });
        }

        var viewModel = new LeaveApprovalViewModel
        {
            Id = result.Data.Id,
            PersonName = result.Data.PersonName,
            LeaveTypeName = result.Data.LeaveTypeName,
            DateRange = $"{result.Data.StartDate:dd.MM.yyyy} - {result.Data.EndDate:dd.MM.yyyy}",
            TotalDays = result.Data.TotalDays,
            Reason = result.Data.Reason,
            ApprovedById = _currentUserService.UserId ?? 1
        };

        return View(viewModel);
    }

    // POST: Leave/Approve/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id, LeaveApprovalViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData["Error"] = "Geçersiz istek.";
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var dto = _mapper.Map<BLL.DTOs.LeaveApprovalDto>(viewModel);
        dto.IsApproved = viewModel.IsApproved;
        dto.ApprovedById = _currentUserService.UserId ?? throw new UnauthorizedAccessException("Kullanıcı kimliği alınamadı.");
        dto.RejectionReason = viewModel.IsApproved ? null : viewModel.ApprovalNotes;

        var result = viewModel.IsApproved 
            ? await _leaveService.ApproveLeaveAsync(dto)
            : await _leaveService.RejectLeaveAsync(dto);

        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return View(viewModel);
        }

        var action = viewModel.IsApproved ? "onaylandı" : "reddedildi";
        TempData["Success"] = $"İzin talebi başarıyla {action}.";
        return RedirectToAction(nameof(Details), new { id = viewModel.Id });
    }

    // POST: Leave/Cancel/5
    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        int currentUserId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("Kullanıcı kimliği alınamadı.");

        var result = await _leaveService.CancelAsync(id, currentUserId);
        if (!result.IsSuccess)
        {
            return Json(new { success = false, message = result.Message });
        }

        return Json(new { success = true, message = "İzin talebi başarıyla iptal edildi." });
    }

    // GET: Leave/Statistics
    [Route("Leave/Statistics")]
    public async Task<IActionResult> Statistics(int? personId, int? departmentId, int? year)
    {
        if (year == null) year = DateTime.Now.Year;

        var result = await _leaveService.GetLeaveStatisticsAsync(personId, departmentId, year);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return View(new Dictionary<string, object>());
        }

        // Prepare dropdowns
        var personsResult = await _personService.GetAllAsync();
        if (personsResult.IsSuccess)
        {
            ViewBag.Persons = new SelectList(personsResult.Data, "Id", "FullName", personId);
        }

        var departmentsResult = await _departmentService.GetAllAsync();
        if (departmentsResult.IsSuccess)
        {
            ViewBag.Departments = new SelectList(departmentsResult.Data, "Id", "Name", departmentId);
        }

        ViewData["PersonId"] = personId;
        ViewData["DepartmentId"] = departmentId;
        ViewData["Year"] = year;

        return View(result.Data);
    }


    // AJAX: Calculate working days
    [HttpPost]
    public async Task<IActionResult> CalculateWorkingDays(DateTime startDate, DateTime endDate)
    {
        var result = await _leaveService.CalculateWorkingDaysAsync(startDate, endDate);
        if (!result.IsSuccess)
        {
            return Json(new { success = false, message = result.Message });
        }

        return Json(new { success = true, workingDays = result.Data });
    }

    // AJAX: Check conflicts
    [HttpPost]
    public async Task<IActionResult> CheckConflicts(int personId, DateTime startDate, DateTime endDate, int? excludeLeaveId)
    {
        var result = await _leaveService.CheckConflictsAsync(personId, startDate, endDate, excludeLeaveId);
        if (!result.IsSuccess)
        {
            return Json(new { success = false, message = result.Message });
        }

        var hasConflicts = result.Data.Any();
        var conflicts = _mapper.Map<List<LeaveListViewModel>>(result.Data);

        return Json(new { 
            success = true, 
            hasConflicts = hasConflicts, 
            conflicts = conflicts 
        });
    }


    // Debug endpoint for forms and services
    public async Task<IActionResult> Debug()
    {
        try
        {
            var allResult = await _leaveService.GetAllAsync();
            var pendingResult = await _leaveService.GetPendingApprovalsAsync();
            var personsResult = await _personService.GetAllAsync();
            var leaveTypesResult = await _leaveTypeService.GetActiveAsync();

            var debugInfo = new
            {
                AllLeavesCount = allResult.IsSuccess ? allResult.Data.Count() : 0,
                AllLeavesSuccess = allResult.IsSuccess,
                AllLeavesMessage = allResult.Message,
                PendingLeavesCount = pendingResult.IsSuccess ? pendingResult.Data.Count() : 0,
                PendingLeavesSuccess = pendingResult.IsSuccess,
                PendingLeavesMessage = pendingResult.Message,
                PersonsCount = personsResult.IsSuccess ? personsResult.Data.Count() : 0,
                PersonsSuccess = personsResult.IsSuccess,
                PersonsMessage = personsResult.Message,
                LeaveTypesCount = leaveTypesResult.IsSuccess ? leaveTypesResult.Data.Count() : 0,
                LeaveTypesSuccess = leaveTypesResult.IsSuccess,
                LeaveTypesMessage = leaveTypesResult.Message,
                AllLeaves = allResult.IsSuccess ? allResult.Data.Take(5) : null,
                PendingLeaves = pendingResult.IsSuccess ? pendingResult.Data.Take(5) : null,
                Persons = personsResult.IsSuccess ? personsResult.Data.Take(5) : null,
                LeaveTypes = leaveTypesResult.IsSuccess ? leaveTypesResult.Data.Take(5) : null,
                CurrentTime = DateTime.Now,
                Environment = "Development"
            };

            return Json(debugInfo);
        }
        catch (Exception ex)
        {
            return Json(new { Error = ex.Message, StackTrace = ex.StackTrace });
        }
    }

    // Test endpoint for leave request process
    [HttpPost]
    public async Task<IActionResult> TestLeaveRequest()
    {
        try
        {
            // Create a test leave request
            var currentPersonId = _currentUserService.PersonId ?? throw new UnauthorizedAccessException("Personel kimliği alınamadı.");
            var testDto = new BLL.DTOs.LeaveCreateDto
            {
                PersonId = currentPersonId,
                LeaveTypeId = 1, // Using first leave type for testing
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(3),
                Reason = "Test izin talebi",
                Notes = "Debug test için oluşturuldu"
            };

            System.Diagnostics.Debug.WriteLine($"Testing leave request: PersonId={testDto.PersonId}, LeaveTypeId={testDto.LeaveTypeId}");

            var result = await _leaveService.CreateAsync(testDto);
            
            return Json(new
            {
                Success = result.IsSuccess,
                Message = result.Message,
                Data = result.Data,
                TestTime = DateTime.Now
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                Success = false,
                Error = ex.Message,
                StackTrace = ex.StackTrace
            });
        }
    }

    #region Private Helper Methods

    private async Task LoadLeaveTypes()
    {
        var leaveTypesResult = await _leaveTypeService.GetActiveAsync();
        ViewBag.LeaveTypes = leaveTypesResult.IsSuccess && leaveTypesResult.Data != null
            ? new SelectList(leaveTypesResult.Data, "Id", "Name")
            : new SelectList(Enumerable.Empty<SelectListItem>());
    }

    private async Task PrepareFilterViewData(LeaveFilterViewModel filter)
    {
        // Persons dropdown
        var personsResult = await _personService.GetAllAsync();
        if (personsResult.IsSuccess)
        {
            filter.Persons = personsResult.Data.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.FirstName} {p.LastName}",
                Selected = p.Id == filter.PersonId
            }).Prepend(new SelectListItem { Value = "", Text = "Tüm Personel" });
        }

        // LeaveTypes dropdown
        var leaveTypesResult = await _leaveTypeService.GetActiveAsync();
        if (leaveTypesResult.IsSuccess)
        {
            filter.LeaveTypes = leaveTypesResult.Data.Select(lt => new SelectListItem
            {
                Value = lt.Id.ToString(),
                Text = lt.Name,
                Selected = lt.Id == filter.LeaveTypeId
            }).Prepend(new SelectListItem { Value = "", Text = "Tüm İzin Türleri" });
        }

        // Departments dropdown
        var departmentsResult = await _departmentService.GetAllAsync();
        if (departmentsResult.IsSuccess)
        {
            filter.Departments = departmentsResult.Data.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name,
                Selected = d.Id == filter.DepartmentId
            }).Prepend(new SelectListItem { Value = "", Text = "Tüm Departmanlar" });
        }

        // Status dropdown
        filter.StatusOptions = Enum.GetValues<LeaveStatus>().Select(s => new SelectListItem
        {
            Value = ((int)s).ToString(),
            Text = GetStatusText(s),
            Selected = s == filter.Status
        }).Prepend(new SelectListItem { Value = "", Text = "Tüm Durumlar" });
    }

    private async Task PrepareCreateEditViewData(dynamic viewModel)
    {
        // Initialize empty lists to prevent null reference errors
        viewModel.Persons = new List<SelectListItem>();
        viewModel.LeaveTypes = new List<SelectListItem>();
        viewModel.HandoverPersons = new List<SelectListItem> 
        { 
            new SelectListItem { Value = "", Text = "Devir Kişisi Seçin" } 
        };

        // Persons dropdown
        var personsResult = await _personService.GetAllAsync();
        if (personsResult.IsSuccess && personsResult.Data != null)
        {
            viewModel.Persons = personsResult.Data.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.FirstName} {p.LastName}"
            }).ToList();

            // Handover persons dropdown (same as persons but with empty option)
            viewModel.HandoverPersons = personsResult.Data.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.FirstName} {p.LastName}"
            }).Prepend(new SelectListItem { Value = "", Text = "Devir Kişisi Seçin" }).ToList();
        }

        // LeaveTypes dropdown
        var leaveTypesResult = await _leaveTypeService.GetActiveAsync();
        if (leaveTypesResult.IsSuccess && leaveTypesResult.Data != null)
        {
            viewModel.LeaveTypes = leaveTypesResult.Data.Select(lt => new SelectListItem
            {
                Value = lt.Id.ToString(),
                Text = lt.Name
            }).ToList();
        }
    }

    private static string GetStatusText(LeaveStatus status)
    {
        return status switch
        {
            LeaveStatus.Pending => "Onay Bekliyor",
            LeaveStatus.Approved => "Onaylandı",
            LeaveStatus.Rejected => "Reddedildi",
            LeaveStatus.Cancelled => "İptal Edildi",
            LeaveStatus.InProgress => "Devam Ediyor",
            LeaveStatus.Completed => "Tamamlandı",
            _ => status.ToString()
        };
    }

    // GET: Leave/Upcoming
    public async Task<IActionResult> Upcoming(int days = 30, int? departmentId = null)
    {
        var result = await _leaveService.GetUpcomingLeavesAsync(days);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return View(new List<LeaveListViewModel>());
        }

        var viewModels = _mapper.Map<List<LeaveListViewModel>>(result.Data);
        
        // Apply department filter if specified (by department name for now)
        if (departmentId.HasValue)
        {
            // Get department name to filter by
            var deptResult = await _departmentService.GetAllAsync();
            if (deptResult.IsSuccess && deptResult.Data != null)
            {
                var selectedDepartment = deptResult.Data.FirstOrDefault(d => d.Id == departmentId.Value);
                if (selectedDepartment != null)
                {
                    viewModels = viewModels.Where(l => l.DepartmentName == selectedDepartment.Name).ToList();
                }
            }
        }
        
        // Prepare statistics for the cards shown in the screenshot
        var upcomingStats = new Dictionary<string, object>();
        upcomingStats["TotalRequests"] = viewModels.Count;
        upcomingStats["PendingApproval"] = viewModels.Count(l => l.Status == DAL.Entities.LeaveStatus.Pending);
        upcomingStats["Approved"] = viewModels.Count(l => l.Status == DAL.Entities.LeaveStatus.Approved);
        upcomingStats["InProgress"] = viewModels.Count(l => l.Status == DAL.Entities.LeaveStatus.InProgress);
        
        ViewBag.Statistics = upcomingStats;
        ViewData["Days"] = days;
        ViewData["DepartmentId"] = departmentId;
        
        // Prepare department dropdown
        var departmentsResult = await _departmentService.GetAllAsync();
        if (departmentsResult.IsSuccess)
        {
            ViewBag.Departments = new SelectList(departmentsResult.Data, "Id", "Name", departmentId);
        }
        
        return View(viewModels);
    }

    // GET: Leave/PendingApprovals
    public async Task<IActionResult> PendingApprovals(int? departmentId = null, int? leaveTypeId = null)
    {
        var result = await _leaveService.GetFilteredAsync(new BLL.DTOs.LeaveFilterDto
        {
            Status = DAL.Entities.LeaveStatus.Pending,
            DepartmentId = departmentId,
            LeaveTypeId = leaveTypeId
        });

        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return View(new List<LeaveListViewModel>());
        }

        var viewModels = _mapper.Map<List<LeaveListViewModel>>(result.Data ?? new List<BLL.DTOs.LeaveListDto>());
        
        // Prepare statistics for pending approvals
        var pendingStats = new Dictionary<string, object>();
        pendingStats["TotalPending"] = viewModels.Count;
        pendingStats["UrgentRequests"] = viewModels.Count(l => l.IsUrgent);
        pendingStats["RequiringDocuments"] = viewModels.Count(l => l.RequiresDocument && !l.HasDocument);
        pendingStats["LongPending"] = viewModels.Count(l => (DateTime.Now - l.RequestDate).Days > 7);
        
        ViewBag.Statistics = pendingStats;
        ViewData["DepartmentId"] = departmentId;
        ViewData["LeaveTypeId"] = leaveTypeId;
        
        // Prepare dropdowns like in Index action
        var departmentsResult = await _departmentService.GetAllAsync();
        if (departmentsResult.IsSuccess)
        {
            ViewBag.Departments = new SelectList(departmentsResult.Data ?? new List<BLL.DTOs.DepartmentListDto>(), "Id", "Name", departmentId);
        }

        var leaveTypesResult = await _leaveTypeService.GetActiveAsync();
        if (leaveTypesResult.IsSuccess)
        {
            ViewBag.LeaveTypes = new SelectList(leaveTypesResult.Data ?? new List<BLL.DTOs.LeaveTypeListDto>(), "Id", "Name", leaveTypeId);
        }
        
        return View(viewModels);
    }

    #endregion
}
