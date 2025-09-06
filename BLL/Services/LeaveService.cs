using AutoMapper;
using BLL.DTOs;
using BLL.Utilities;
using DAL.Entities;
using DAL.Repositories;

namespace BLL.Services;

public class LeaveService : ILeaveService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LeaveService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<LeaveListDto>>> GetAllAsync()
    {
        try
        {
            var leaves = await _unitOfWork.Leaves.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<LeaveListDto>>(leaves);
            return Result<IEnumerable<LeaveListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveListDto>>.Fail($"İzin kayıtları getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<LeaveListDto>>> GetFilteredAsync(LeaveFilterDto filter)
    {
        try
        {
            var leaves = await _unitOfWork.Leaves.GetFilteredLeavesAsync(
                filter.PersonId, filter.LeaveTypeId, filter.DepartmentId, 
                filter.Status, filter.StartDate, filter.EndDate, filter.Year);
            
            var dtos = _mapper.Map<IEnumerable<LeaveListDto>>(leaves);
            return Result<IEnumerable<LeaveListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveListDto>>.Fail($"Filtrelenmiş izin kayıtları getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<LeaveDetailDto>> GetByIdAsync(int id)
    {
        try
        {
            var leave = await _unitOfWork.Leaves.GetByIdAsync(id);
            if (leave == null)
            {
                return Result<LeaveDetailDto>.Fail("İzin kaydı bulunamadı.");
            }

            var dto = _mapper.Map<LeaveDetailDto>(leave);
            
            // Calculate remaining balance
            var balanceResult = await GetRemainingBalanceAsync(leave.PersonId, leave.LeaveTypeId, leave.StartDate.Year);
            if (balanceResult.IsSuccess)
            {
                dto.RemainingBalance = balanceResult.Data;
            }

            return Result<LeaveDetailDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            return Result<LeaveDetailDto>.Fail($"İzin kaydı getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<LeaveDetailDto>> CreateAsync(LeaveCreateDto dto)
    {
        try
        {
            var validationResult = await ValidateLeaveRequestAsync(dto);
            if (!validationResult.IsSuccess)
            {
                return Result<LeaveDetailDto>.Fail(validationResult.Message);
            }

            // Calculate total days
            var workingDaysResult = await CalculateWorkingDaysAsync(dto.StartDate, dto.EndDate);
            if (!workingDaysResult.IsSuccess)
            {
                return Result<LeaveDetailDto>.Fail(workingDaysResult.Message);
            }
            dto.TotalDays = workingDaysResult.Data;

            var leave = _mapper.Map<Leave>(dto);
            leave.CreatedAt = DateTime.Now;

            await _unitOfWork.Leaves.AddAsync(leave);
            await _unitOfWork.SaveChangesAsync();

            // Update balance
            await UpdateBalanceAfterLeaveCreation(leave);

            var detailDto = _mapper.Map<LeaveDetailDto>(leave);
            return Result<LeaveDetailDto>.Ok(detailDto);
        }
        catch (Exception ex)
        {
            return Result<LeaveDetailDto>.Fail($"İzin kaydı oluşturulurken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<LeaveDetailDto>> UpdateAsync(LeaveUpdateDto dto)
    {
        try
        {
            var validationResult = await ValidateLeaveUpdateAsync(dto);
            if (!validationResult.IsSuccess)
            {
                return Result<LeaveDetailDto>.Fail(validationResult.Message);
            }

            var leave = await _unitOfWork.Leaves.GetByIdAsync(dto.Id);
            if (leave == null)
            {
                return Result<LeaveDetailDto>.Fail("İzin kaydı bulunamadı.");
            }

            // Only allow updates for pending leaves
            if (leave.Status != LeaveStatus.Pending)
            {
                return Result<LeaveDetailDto>.Fail("Sadece bekleyen izinler düzenlenebilir.");
            }

            // Calculate new total days
            var workingDaysResult = await CalculateWorkingDaysAsync(dto.StartDate, dto.EndDate);
            if (!workingDaysResult.IsSuccess)
            {
                return Result<LeaveDetailDto>.Fail(workingDaysResult.Message);
            }
            dto.TotalDays = workingDaysResult.Data;

            _mapper.Map(dto, leave);
            leave.UpdatedAt = DateTime.Now;

            _unitOfWork.Leaves.Update(leave);
            await _unitOfWork.SaveChangesAsync();

            var detailDto = _mapper.Map<LeaveDetailDto>(leave);
            return Result<LeaveDetailDto>.Ok(detailDto);
        }
        catch (Exception ex)
        {
            return Result<LeaveDetailDto>.Fail($"İzin kaydı güncellenirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        try
        {
            var leave = await _unitOfWork.Leaves.GetByIdAsync(id);
            if (leave == null)
            {
                return Result<bool>.Fail("İzin kaydı bulunamadı.");
            }

            if (leave.Status == LeaveStatus.InProgress)
            {
                return Result<bool>.Fail("Devam eden izin silinemez.");
            }

            _unitOfWork.Leaves.Remove(leave);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"İzin kaydı silinirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> CancelAsync(int id, int userId)
    {
        try
        {
            var leave = await _unitOfWork.Leaves.GetByIdAsync(id);
            if (leave == null)
            {
                return Result<bool>.Fail("İzin kaydı bulunamadı.");
            }

            if (leave.Status == LeaveStatus.Completed)
            {
                return Result<bool>.Fail("Tamamlanmış izin iptal edilemez.");
            }

            leave.Status = LeaveStatus.Cancelled;
            leave.UpdatedAt = DateTime.Now;

            _unitOfWork.Leaves.Update(leave);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"İzin iptal edilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<LeaveListDto>>> GetLeavesByPersonAsync(int personId, int? year = null)
    {
        try
        {
            var leaves = year.HasValue 
                ? await _unitOfWork.Leaves.GetLeavesByPersonAsync(personId, year.Value)
                : await _unitOfWork.Leaves.GetLeavesByPersonAsync(personId);
            var dtos = _mapper.Map<IEnumerable<LeaveListDto>>(leaves);
            return Result<IEnumerable<LeaveListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveListDto>>.Fail($"Personel izinleri getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<LeaveListDto>>> GetLeavesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var leaves = await _unitOfWork.Leaves.GetLeavesByDateRangeAsync(startDate, endDate);
            var dtos = _mapper.Map<IEnumerable<LeaveListDto>>(leaves);
            return Result<IEnumerable<LeaveListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveListDto>>.Fail($"Tarih aralığındaki izinler getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<LeaveListDto>>> GetPendingApprovalsAsync()
    {
        try
        {
            var leaves = await _unitOfWork.Leaves.GetPendingApprovalsAsync();
            var dtos = _mapper.Map<IEnumerable<LeaveListDto>>(leaves);
            return Result<IEnumerable<LeaveListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveListDto>>.Fail($"Onay bekleyen izinler getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<LeaveListDto>>> GetPendingApprovalsByManagerAsync(int managerId)
    {
        try
        {
            var leaves = await _unitOfWork.Leaves.GetPendingApprovalsByManagerAsync(managerId);
            var dtos = _mapper.Map<IEnumerable<LeaveListDto>>(leaves);
            return Result<IEnumerable<LeaveListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveListDto>>.Fail($"Yönetici onay bekleyen izinleri getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<LeaveListDto>>> GetActiveLeavesByDateAsync(DateTime date)
    {
        try
        {
            var leaves = await _unitOfWork.Leaves.GetActiveLeavesByDateAsync(date);
            var dtos = _mapper.Map<IEnumerable<LeaveListDto>>(leaves);
            return Result<IEnumerable<LeaveListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveListDto>>.Fail($"Aktif izinler getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<LeaveListDto>>> GetTeamLeavesAsync(int departmentId, DateTime startDate, DateTime endDate)
    {
        try
        {
            var leaves = await _unitOfWork.Leaves.GetTeamLeavesAsync(departmentId, startDate, endDate);
            var dtos = _mapper.Map<IEnumerable<LeaveListDto>>(leaves);
            return Result<IEnumerable<LeaveListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveListDto>>.Fail($"Takım izinleri getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> ApproveLeaveAsync(LeaveApprovalDto approval)
    {
        try
        {
            var leave = await _unitOfWork.Leaves.GetByIdAsync(approval.Id);
            if (leave == null)
            {
                return Result<bool>.Fail("İzin kaydı bulunamadı.");
            }

            if (leave.Status != LeaveStatus.Pending)
            {
                return Result<bool>.Fail("Sadece bekleyen izinler onaylanabilir.");
            }

            leave.Status = LeaveStatus.Approved;
            leave.ApprovedById = approval.ApprovedById;
            leave.ApprovedAt = DateTime.Now;
            leave.ApprovalNotes = approval.ApprovalNotes;
            leave.UpdatedAt = DateTime.Now;

            _unitOfWork.Leaves.Update(leave);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"İzin onaylanırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> RejectLeaveAsync(LeaveApprovalDto rejection)
    {
        try
        {
            var leave = await _unitOfWork.Leaves.GetByIdAsync(rejection.Id);
            if (leave == null)
            {
                return Result<bool>.Fail("İzin kaydı bulunamadı.");
            }

            if (leave.Status != LeaveStatus.Pending)
            {
                return Result<bool>.Fail("Sadece bekleyen izinler reddedilebilir.");
            }

            leave.Status = LeaveStatus.Rejected;
            leave.ApprovedById = rejection.ApprovedById;
            leave.ApprovedAt = DateTime.Now;
            leave.RejectionReason = rejection.RejectionReason;
            leave.UpdatedAt = DateTime.Now;

            _unitOfWork.Leaves.Update(leave);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"İzin reddedilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> CanUserApproveLeaveAsync(int leaveId, int userId)
    {
        try
        {
            var leave = await _unitOfWork.Leaves.GetByIdAsync(leaveId);
            if (leave == null)
            {
                return Result<bool>.Fail("İzin kaydı bulunamadı.");
            }

            // Simple check - user cannot approve their own leave
            if (leave.PersonId == userId)
            {
                return Result<bool>.Ok(false);
            }

            // Additional checks can be added here (role-based, department-based, etc.)
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Onay yetkisi kontrolü sırasında hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<LeaveListDto>>> GetLeavesRequiringMyApprovalAsync(int managerId)
    {
        try
        {
            var leaves = await _unitOfWork.Leaves.GetPendingApprovalsByManagerAsync(managerId);
            var dtos = _mapper.Map<IEnumerable<LeaveListDto>>(leaves);
            return Result<IEnumerable<LeaveListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveListDto>>.Fail($"Onay gereken izinler getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> ValidateLeaveRequestAsync(LeaveCreateDto dto)
    {
        try
        {
            if (dto.StartDate < DateTime.Today)
            {
                return Result<bool>.Fail("İzin başlangıç tarihi bugünden önce olamaz.");
            }

            if (dto.EndDate < dto.StartDate)
            {
                return Result<bool>.Fail("İzin bitiş tarihi başlangıç tarihinden önce olamaz.");
            }

            var workingDaysResult = await CalculateWorkingDaysAsync(dto.StartDate, dto.EndDate);
            if (!workingDaysResult.IsSuccess)
            {
                return Result<bool>.Fail(workingDaysResult.Message);
            }

            if (workingDaysResult.Data <= 0)
            {
                return Result<bool>.Fail("İzin süresi en az 1 iş günü olmalıdır.");
            }

            // Check sufficient balance
            var balanceResult = await HasSufficientBalanceAsync(dto.PersonId, dto.LeaveTypeId, workingDaysResult.Data, dto.StartDate.Year);
            if (!balanceResult.IsSuccess)
            {
                return Result<bool>.Fail(balanceResult.Message);
            }

            if (!balanceResult.Data)
            {
                return Result<bool>.Fail("Yetersiz izin bakiyesi.");
            }

            // Check conflicts
            var conflictsResult = await CheckConflictsAsync(dto.PersonId, dto.StartDate, dto.EndDate);
            if (!conflictsResult.IsSuccess)
            {
                return Result<bool>.Fail(conflictsResult.Message);
            }

            if (conflictsResult.Data?.Any() == true)
            {
                return Result<bool>.Fail("Belirtilen tarih aralığında çakışan izin bulunuyor.");
            }

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"İzin doğrulama sırasında hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> ValidateLeaveUpdateAsync(LeaveUpdateDto dto)
    {
        try
        {
            if (dto.StartDate < DateTime.Today)
            {
                return Result<bool>.Fail("İzin başlangıç tarihi bugünden önce olamaz.");
            }

            if (dto.EndDate < dto.StartDate)
            {
                return Result<bool>.Fail("İzin bitiş tarihi başlangıç tarihinden önce olamaz.");
            }

            var workingDaysResult = await CalculateWorkingDaysAsync(dto.StartDate, dto.EndDate);
            if (!workingDaysResult.IsSuccess)
            {
                return Result<bool>.Fail(workingDaysResult.Message);
            }

            if (workingDaysResult.Data <= 0)
            {
                return Result<bool>.Fail("İzin süresi en az 1 iş günü olmalıdır.");
            }

            // Check conflicts (excluding current leave)
            var conflictsResult = await CheckConflictsAsync(dto.PersonId, dto.StartDate, dto.EndDate, dto.Id);
            if (!conflictsResult.IsSuccess)
            {
                return Result<bool>.Fail(conflictsResult.Message);
            }

            if (conflictsResult.Data?.Any() == true)
            {
                return Result<bool>.Fail("Belirtilen tarih aralığında çakışan izin bulunuyor.");
            }

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"İzin güncelleme doğrulama sırasında hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<LeaveListDto>>> CheckConflictsAsync(int personId, DateTime startDate, DateTime endDate, int? excludeLeaveId = null)
    {
        try
        {
            var conflicts = await _unitOfWork.Leaves.GetConflictingLeavesAsync(personId, startDate, endDate, excludeLeaveId);
            var dtos = _mapper.Map<IEnumerable<LeaveListDto>>(conflicts);
            return Result<IEnumerable<LeaveListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveListDto>>.Fail($"Çakışma kontrolü sırasında hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> HasSufficientBalanceAsync(int personId, int leaveTypeId, int requestedDays, int year)
    {
        try
        {
            var remainingBalance = await GetRemainingBalanceAsync(personId, leaveTypeId, year);
            if (!remainingBalance.IsSuccess)
            {
                return Result<bool>.Fail(remainingBalance.Message);
            }

            return Result<bool>.Ok(remainingBalance.Data >= requestedDays);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Bakiye kontrolü sırasında hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<decimal>> GetRemainingBalanceAsync(int personId, int leaveTypeId, int year)
    {
        try
        {
            var balance = await _unitOfWork.LeaveBalances.GetBalanceAsync(personId, leaveTypeId, year);
            if (balance == null)
            {
                return Result<decimal>.Ok(0);
            }

            var remaining = balance.AllocatedDays + balance.CarriedOverDays + balance.ManualAdjustment - balance.UsedDays - balance.PendingDays;
            return Result<decimal>.Ok(remaining);
        }
        catch (Exception ex)
        {
            return Result<decimal>.Fail($"Kalan bakiye hesaplanırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<LeaveCalendarDto>>> GetCalendarDataAsync(DateTime startDate, DateTime endDate, int? departmentId = null)
    {
        try
        {
            var leaves = await _unitOfWork.Leaves.GetCalendarDataAsync(startDate, endDate, departmentId);
            var dtos = _mapper.Map<IEnumerable<LeaveCalendarDto>>(leaves);
            return Result<IEnumerable<LeaveCalendarDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveCalendarDto>>.Fail($"Takvim verileri getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<Dictionary<string, object>>> GetLeaveStatisticsAsync(int? personId = null, int? departmentId = null, int? year = null)
    {
        try
        {
            if (year == null) year = DateTime.Now.Year;

            var statistics = new Dictionary<string, object>();
            
            if (personId.HasValue)
            {
                var personLeaves = await _unitOfWork.Leaves.GetLeavesByPersonAsync(personId.Value, year.Value);
                statistics["PersonId"] = personId.Value;
                statistics["Year"] = year;
                statistics["TotalLeaves"] = personLeaves.Count();
                statistics["ApprovedLeaves"] = personLeaves.Count(l => l.Status == LeaveStatus.Approved);
                statistics["PendingLeaves"] = personLeaves.Count(l => l.Status == LeaveStatus.Pending);
                statistics["TotalDays"] = personLeaves.Where(l => l.Status == LeaveStatus.Approved).Sum(l => l.TotalDays);
            }
            else
            {
                var allLeaves = await _unitOfWork.Leaves.GetAllAsync();
                var yearLeaves = allLeaves.Where(l => l.StartDate.Year == year);
                
                if (departmentId.HasValue)
                {
                    yearLeaves = yearLeaves.Where(l => l.Person.DepartmentId == departmentId);
                    statistics["DepartmentId"] = departmentId.Value;
                }

                statistics["Year"] = year;
                statistics["TotalLeaves"] = yearLeaves.Count();
                statistics["ApprovedLeaves"] = yearLeaves.Count(l => l.Status == LeaveStatus.Approved);
                statistics["PendingLeaves"] = yearLeaves.Count(l => l.Status == LeaveStatus.Pending);
                statistics["TotalDays"] = yearLeaves.Where(l => l.Status == LeaveStatus.Approved).Sum(l => l.TotalDays);
            }

            return Result<Dictionary<string, object>>.Ok(statistics);
        }
        catch (Exception ex)
        {
            return Result<Dictionary<string, object>>.Fail($"İstatistikler hesaplanırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<LeaveListDto>>> GetUpcomingLeavesAsync(int days = 30)
    {
        try
        {
            var leaves = await _unitOfWork.Leaves.GetUpcomingLeavesAsync(days);
            var dtos = _mapper.Map<IEnumerable<LeaveListDto>>(leaves);
            return Result<IEnumerable<LeaveListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveListDto>>.Fail($"Yaklaşan izinler getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<LeaveListDto>>> GetLeavesRequiringAttentionAsync()
    {
        try
        {
            var urgentLeaves = await _unitOfWork.Leaves.GetUrgentLeavesAsync();
            var dtos = _mapper.Map<IEnumerable<LeaveListDto>>(urgentLeaves);
            return Result<IEnumerable<LeaveListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveListDto>>.Fail($"Acil izinler getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<int>> CalculateWorkingDaysAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            int workingDays = 0;
            var currentDate = startDate;

            while (currentDate <= endDate)
            {
                var isWorkingDayResult = await IsWorkingDayAsync(currentDate);
                if (isWorkingDayResult.IsSuccess && isWorkingDayResult.Data)
                {
                    workingDays++;
                }
                currentDate = currentDate.AddDays(1);
            }

            return Result<int>.Ok(workingDays);
        }
        catch (Exception ex)
        {
            return Result<int>.Fail($"İş günü hesaplanırken hata oluştu: {ex.Message}");
        }
    }

    public Task<Result<bool>> IsWorkingDayAsync(DateTime date)
    {
        try
        {
            // Simple implementation - exclude weekends
            var isWorkingDay = date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
            
            // TODO: Add holiday checking logic here
            
            return Task.FromResult(Result<bool>.Ok(isWorkingDay));
        }
        catch (Exception ex)
        {
            return Task.FromResult(Result<bool>.Fail($"İş günü kontrolü sırasında hata oluştu: {ex.Message}"));
        }
    }

    public async Task<Result<DateTime>> GetNextWorkingDayAsync(DateTime date)
    {
        try
        {
            var nextDate = date.AddDays(1);
            
            while (true)
            {
                var isWorkingDayResult = await IsWorkingDayAsync(nextDate);
                if (isWorkingDayResult.IsSuccess && isWorkingDayResult.Data)
                {
                    return Result<DateTime>.Ok(nextDate);
                }
                nextDate = nextDate.AddDays(1);
                
                // Prevent infinite loop
                if (nextDate > date.AddDays(10))
                {
                    return Result<DateTime>.Fail("Sonraki iş günü bulunamadı.");
                }
            }
        }
        catch (Exception ex)
        {
            return Result<DateTime>.Fail($"Sonraki iş günü hesaplanırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> UpdateLeaveStatusBasedOnDatesAsync()
    {
        try
        {
            var today = DateTime.Today;
            var allLeaves = await _unitOfWork.Leaves.GetAllAsync();
            
            foreach (var leave in allLeaves)
            {
                if (leave.Status == LeaveStatus.Approved)
                {
                    if (today >= leave.StartDate && today <= leave.EndDate)
                    {
                        leave.Status = LeaveStatus.InProgress;
                    }
                    else if (today > leave.EndDate)
                    {
                        leave.Status = LeaveStatus.Completed;
                    }
                    
                    _unitOfWork.Leaves.Update(leave);
                }
            }
            
            await _unitOfWork.SaveChangesAsync();
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"İzin durumları güncellenirken hata oluştu: {ex.Message}");
        }
    }

    private async Task UpdateBalanceAfterLeaveCreation(Leave leave)
    {
        try
        {
            var balance = await _unitOfWork.LeaveBalances.GetBalanceAsync(leave.PersonId, leave.LeaveTypeId, leave.StartDate.Year);
            if (balance != null)
            {
                balance.PendingDays += leave.TotalDays;
                _unitOfWork.LeaveBalances.Update(balance);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            // Log error but don't fail the leave creation
            Console.WriteLine($"Bakiye güncellenirken hata: {ex.Message}");
        }
    }
}
