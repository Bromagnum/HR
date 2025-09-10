using AutoMapper;
using BLL.DTOs;
using BLL.Utilities;
using DAL.Entities;
using DAL.Repositories;

namespace BLL.Services;

public class LeaveBalanceService : ILeaveBalanceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LeaveBalanceService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<LeaveBalanceListDto>>> GetAllAsync()
    {
        try
        {
            var balances = await _unitOfWork.LeaveBalances.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<LeaveBalanceListDto>>(balances);
            return Result<IEnumerable<LeaveBalanceListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveBalanceListDto>>.Fail($"İzin bakiyeleri getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<LeaveBalanceDetailDto>> GetByIdAsync(int id)
    {
        try
        {
            var balance = await _unitOfWork.LeaveBalances.GetByIdAsync(id);
            if (balance == null)
            {
                return Result<LeaveBalanceDetailDto>.Fail("İzin bakiyesi bulunamadı.");
            }

            var dto = _mapper.Map<LeaveBalanceDetailDto>(balance);
            return Result<LeaveBalanceDetailDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            return Result<LeaveBalanceDetailDto>.Fail($"İzin bakiyesi getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<LeaveBalanceDetailDto>> CreateAsync(LeaveBalanceCreateDto dto)
    {
        try
        {
            var validationResult = await ValidateBalanceAsync(dto);
            if (!validationResult.IsSuccess)
            {
                return Result<LeaveBalanceDetailDto>.Fail(validationResult.Message);
            }

            // Check if balance already exists
            var existingBalance = await _unitOfWork.LeaveBalances.GetBalanceAsync(dto.PersonId, dto.LeaveTypeId, dto.Year);
            if (existingBalance != null)
            {
                return Result<LeaveBalanceDetailDto>.Fail("Bu personel ve izin türü için bu yılda zaten bakiye kaydı mevcut.");
            }

            var balance = _mapper.Map<LeaveBalance>(dto);
            balance.CreatedAt = DateTime.Now;
            balance.IsActive = true;

            // Calculate initial values
            balance.AvailableDays = balance.AllocatedDays + balance.CarriedOverDays + balance.ManualAdjustment;
            balance.RemainingDays = balance.AvailableDays - balance.UsedDays - balance.PendingDays;

            await _unitOfWork.LeaveBalances.AddAsync(balance);
            await _unitOfWork.SaveChangesAsync();

            var detailDto = _mapper.Map<LeaveBalanceDetailDto>(balance);
            return Result<LeaveBalanceDetailDto>.Ok(detailDto);
        }
        catch (Exception ex)
        {
            return Result<LeaveBalanceDetailDto>.Fail($"İzin bakiyesi oluşturulurken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<LeaveBalanceDetailDto>> UpdateAsync(LeaveBalanceUpdateDto dto)
    {
        try
        {
            var validationResult = await ValidateBalanceAsync(dto);
            if (!validationResult.IsSuccess)
            {
                return Result<LeaveBalanceDetailDto>.Fail(validationResult.Message);
            }

            var balance = await _unitOfWork.LeaveBalances.GetByIdAsync(dto.Id);
            if (balance == null)
            {
                return Result<LeaveBalanceDetailDto>.Fail("İzin bakiyesi bulunamadı.");
            }

            _mapper.Map(dto, balance);
            balance.UpdatedAt = DateTime.Now;

            // Recalculate values
            balance.AvailableDays = balance.AllocatedDays + balance.CarriedOverDays + balance.ManualAdjustment;
            balance.RemainingDays = balance.AvailableDays - balance.UsedDays - balance.PendingDays;

            _unitOfWork.LeaveBalances.Update(balance);
            await _unitOfWork.SaveChangesAsync();

            var detailDto = _mapper.Map<LeaveBalanceDetailDto>(balance);
            return Result<LeaveBalanceDetailDto>.Ok(detailDto);
        }
        catch (Exception ex)
        {
            return Result<LeaveBalanceDetailDto>.Fail($"İzin bakiyesi güncellenirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        try
        {
            var canDeleteResult = await CanDeleteBalanceAsync(id);
            if (!canDeleteResult.IsSuccess)
            {
                return Result<bool>.Fail(canDeleteResult.Message);
            }

            var balance = await _unitOfWork.LeaveBalances.GetByIdAsync(id);
            if (balance == null)
            {
                return Result<bool>.Fail("İzin bakiyesi bulunamadı.");
            }

            _unitOfWork.LeaveBalances.Remove(balance);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"İzin bakiyesi silinirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<LeaveBalanceListDto>>> GetBalancesByPersonAsync(int personId, int year)
    {
        try
        {
            var balances = await _unitOfWork.LeaveBalances.GetBalancesByPersonIdAsync(personId, year);
            var dtos = _mapper.Map<IEnumerable<LeaveBalanceListDto>>(balances);
            return Result<IEnumerable<LeaveBalanceListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveBalanceListDto>>.Fail($"Personel bakiyeleri getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<LeaveBalanceSummaryDto>> GetBalanceSummaryAsync(int personId, int year)
    {
        try
        {
            var balances = await _unitOfWork.LeaveBalances.GetBalancesByPersonIdAsync(personId, year);
            var person = await _unitOfWork.Persons.GetByIdAsync(personId);
            
            if (person == null)
            {
                return Result<LeaveBalanceSummaryDto>.Fail("Personel bulunamadı.");
            }

            var balanceDtos = _mapper.Map<List<LeaveBalanceListDto>>(balances);
            
            var summary = new LeaveBalanceSummaryDto
            {
                PersonId = personId,
                PersonName = $"{person.FirstName} {person.LastName}",
                DepartmentName = person.Department?.Name ?? "",
                Year = year,
                Balances = balanceDtos,
                TotalAllocated = balanceDtos.Sum(b => b.AllocatedDays),
                TotalUsed = balanceDtos.Sum(b => b.UsedDays),
                TotalPending = balanceDtos.Sum(b => b.PendingDays),
                TotalAvailable = balanceDtos.Sum(b => b.AvailableDays)
            };

            return Result<LeaveBalanceSummaryDto>.Ok(summary);
        }
        catch (Exception ex)
        {
            return Result<LeaveBalanceSummaryDto>.Fail($"Bakiye özeti oluşturulurken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<LeaveBalanceDetailDto>> GetBalanceAsync(int personId, int leaveTypeId, int year)
    {
        try
        {
            var balance = await _unitOfWork.LeaveBalances.GetBalanceAsync(personId, leaveTypeId, year);
            if (balance == null)
            {
                return Result<LeaveBalanceDetailDto>.Fail("İzin bakiyesi bulunamadı.");
            }

            var dto = _mapper.Map<LeaveBalanceDetailDto>(balance);
            return Result<LeaveBalanceDetailDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            return Result<LeaveBalanceDetailDto>.Fail($"İzin bakiyesi getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<LeaveBalanceListDto>>> GetDepartmentBalancesAsync(int departmentId, int year)
    {
        try
        {
            var balances = await _unitOfWork.LeaveBalances.GetDepartmentBalancesAsync(departmentId, year);
            var dtos = _mapper.Map<IEnumerable<LeaveBalanceListDto>>(balances);
            return Result<IEnumerable<LeaveBalanceListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveBalanceListDto>>.Fail($"Departman bakiyeleri getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<LeaveBalanceListDto>>> GetBalancesByLeaveTypeAsync(int leaveTypeId, int year)
    {
        try
        {
            var balances = await _unitOfWork.LeaveBalances.GetBalancesByLeaveTypeAsync(leaveTypeId, year);
            var dtos = _mapper.Map<IEnumerable<LeaveBalanceListDto>>(balances);
            return Result<IEnumerable<LeaveBalanceListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveBalanceListDto>>.Fail($"İzin türü bakiyeleri getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> UpdateBalanceAfterLeaveAsync(int personId, int leaveTypeId, int year)
    {
        try
        {
            var recalculateResult = await RecalculateBalanceAsync(personId, leaveTypeId, year);
            return recalculateResult;
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"İzin sonrası bakiye güncellenirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> RecalculateAllBalancesAsync(int personId, int year)
    {
        try
        {
            var balances = await _unitOfWork.LeaveBalances.GetBalancesByPersonIdAsync(personId, year);
            
            foreach (var balance in balances)
            {
                await RecalculateBalanceAsync(balance.PersonId, balance.LeaveTypeId, balance.Year);
            }

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Tüm bakiyeler yeniden hesaplanırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> RecalculateBalanceAsync(int personId, int leaveTypeId, int year)
    {
        try
        {
            var balance = await _unitOfWork.LeaveBalances.GetBalanceAsync(personId, leaveTypeId, year);
            if (balance == null)
            {
                return Result<bool>.Fail("İzin bakiyesi bulunamadı.");
            }

            // Recalculate used days
            var usedDaysResult = await CalculateUsedDaysAsync(personId, leaveTypeId, year);
            if (usedDaysResult.IsSuccess)
            {
                balance.UsedDays = usedDaysResult.Data;
            }

            // Recalculate pending days
            var pendingDaysResult = await CalculatePendingDaysAsync(personId, leaveTypeId, year);
            if (pendingDaysResult.IsSuccess)
            {
                balance.PendingDays = pendingDaysResult.Data;
            }

            // Recalculate available and remaining days
            balance.AvailableDays = balance.AllocatedDays + balance.CarriedOverDays + balance.ManualAdjustment;
            balance.RemainingDays = balance.AvailableDays - balance.UsedDays - balance.PendingDays;

            balance.UpdatedAt = DateTime.Now;
            _unitOfWork.LeaveBalances.Update(balance);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Bakiye yeniden hesaplanırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<decimal>> CalculateUsedDaysAsync(int personId, int leaveTypeId, int year)
    {
        try
        {
            var approvedLeaves = await _unitOfWork.Leaves.GetApprovedLeavesByPersonAndTypeAsync(personId, leaveTypeId, year);
            var usedDays = approvedLeaves.Sum(l => l.TotalDays);
            return Result<decimal>.Ok(usedDays);
        }
        catch (Exception ex)
        {
            return Result<decimal>.Fail($"Kullanılan günler hesaplanırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<decimal>> CalculatePendingDaysAsync(int personId, int leaveTypeId, int year)
    {
        try
        {
            var pendingLeaves = await _unitOfWork.Leaves.GetPendingLeavesByPersonAndTypeAsync(personId, leaveTypeId, year);
            var pendingDays = pendingLeaves.Sum(l => l.TotalDays);
            return Result<decimal>.Ok(pendingDays);
        }
        catch (Exception ex)
        {
            return Result<decimal>.Fail($"Bekleyen günler hesaplanırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<decimal>> CalculateAvailableDaysAsync(int personId, int leaveTypeId, int year)
    {
        try
        {
            var balance = await _unitOfWork.LeaveBalances.GetBalanceAsync(personId, leaveTypeId, year);
            if (balance == null)
            {
                return Result<decimal>.Ok(0);
            }

            var availableDays = balance.AllocatedDays + balance.CarriedOverDays + balance.ManualAdjustment - balance.UsedDays - balance.PendingDays;
            return Result<decimal>.Ok(availableDays);
        }
        catch (Exception ex)
        {
            return Result<decimal>.Fail($"Kullanılabilir günler hesaplanırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> ProcessMonthlyAccrualsAsync(DateTime cutoffDate)
    {
        try
        {
            var activeBalances = await _unitOfWork.LeaveBalances.GetActiveBalancesAsync();
            
            foreach (var balance in activeBalances.Where(b => b.MonthlyAccrual > 0))
            {
                await ProcessAccrualForPersonAsync(balance.PersonId, cutoffDate);
            }

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Aylık tahakkuklar işlenirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> ProcessAccrualForPersonAsync(int personId, DateTime cutoffDate)
    {
        try
        {
            var year = cutoffDate.Year;
            var balances = await _unitOfWork.LeaveBalances.GetBalancesByPersonIdAsync(personId, year);
            
            foreach (var balance in balances.Where(b => b.MonthlyAccrual > 0))
            {
                var monthsToAccrue = (cutoffDate.Year - balance.LastAccrualDate.Year) * 12 + 
                                   cutoffDate.Month - balance.LastAccrualDate.Month;

                if (monthsToAccrue > 0)
                {
                    var accruedDays = monthsToAccrue * balance.MonthlyAccrual;
                    balance.AccruedToDate += accruedDays;
                    balance.AvailableDays += accruedDays;
                    balance.RemainingDays += accruedDays;
                    balance.LastAccrualDate = cutoffDate;
                    balance.UpdatedAt = DateTime.Now;

                    _unitOfWork.LeaveBalances.Update(balance);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Personel tahakkuku işlenirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<decimal>> CalculateAccruedDaysAsync(int personId, int leaveTypeId, DateTime asOfDate)
    {
        try
        {
            var balance = await _unitOfWork.LeaveBalances.GetBalanceAsync(personId, leaveTypeId, asOfDate.Year);
            if (balance == null || balance.MonthlyAccrual <= 0)
            {
                return Result<decimal>.Ok(0);
            }

            var monthsToAccrue = (asOfDate.Year - balance.LastAccrualDate.Year) * 12 + 
                               asOfDate.Month - balance.LastAccrualDate.Month;

            var accruedDays = Math.Max(0, monthsToAccrue * balance.MonthlyAccrual);
            return Result<decimal>.Ok(accruedDays);
        }
        catch (Exception ex)
        {
            return Result<decimal>.Fail($"Tahakkuk günleri hesaplanırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> UpdateAccrualAsync(int personId, int leaveTypeId, int year, decimal accruedDays)
    {
        try
        {
            var balance = await _unitOfWork.LeaveBalances.GetBalanceAsync(personId, leaveTypeId, year);
            if (balance == null)
            {
                return Result<bool>.Fail("İzin bakiyesi bulunamadı.");
            }

            balance.AccruedToDate = accruedDays;
            balance.LastAccrualDate = DateTime.Now;
            balance.UpdatedAt = DateTime.Now;

            _unitOfWork.LeaveBalances.Update(balance);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Tahakkuk güncellenirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> ProcessYearEndCarryOverAsync(int fromYear, int toYear)
    {
        try
        {
            var activePersons = await _unitOfWork.Persons.GetActivePersonsAsync();
            
            foreach (var person in activePersons)
            {
                await ProcessCarryOverForPersonAsync(person.Id, fromYear, toYear);
            }

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Yıl sonu devir işlemi sırasında hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> ProcessCarryOverForPersonAsync(int personId, int fromYear, int toYear)
    {
        try
        {
            var previousYearBalances = await _unitOfWork.LeaveBalances.GetBalancesByPersonIdAsync(personId, fromYear);
            
            foreach (var prevBalance in previousYearBalances)
            {
                var leaveType = await _unitOfWork.LeaveTypes.GetByIdAsync(prevBalance.LeaveTypeId);
                if (leaveType == null || !leaveType.CanCarryOver) continue;

                var carryOverDays = await CalculateCarryOverDaysAsync(personId, prevBalance.LeaveTypeId, fromYear);
                if (!carryOverDays.IsSuccess || carryOverDays.Data <= 0) continue;

                var actualCarryOver = Math.Min(carryOverDays.Data, leaveType.MaxCarryOverDays);

                // Check if balance exists for new year
                var newYearBalance = await _unitOfWork.LeaveBalances.GetBalanceAsync(personId, prevBalance.LeaveTypeId, toYear);
                if (newYearBalance == null)
                {
                    // Create new balance for the year
                    await EnsureBalanceExistsAsync(personId, prevBalance.LeaveTypeId, toYear);
                    newYearBalance = await _unitOfWork.LeaveBalances.GetBalanceAsync(personId, prevBalance.LeaveTypeId, toYear);
                }

                if (newYearBalance != null)
                {
                    newYearBalance.CarriedOverDays = actualCarryOver;
                    newYearBalance.AvailableDays = newYearBalance.AllocatedDays + actualCarryOver + newYearBalance.ManualAdjustment;
                    newYearBalance.RemainingDays = newYearBalance.AvailableDays - newYearBalance.UsedDays - newYearBalance.PendingDays;
                    newYearBalance.UpdatedAt = DateTime.Now;

                    _unitOfWork.LeaveBalances.Update(newYearBalance);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Personel devir işlemi sırasında hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<decimal>> CalculateCarryOverDaysAsync(int personId, int leaveTypeId, int fromYear)
    {
        try
        {
            var balance = await _unitOfWork.LeaveBalances.GetBalanceAsync(personId, leaveTypeId, fromYear);
            if (balance == null)
            {
                return Result<decimal>.Ok(0);
            }

            var remainingDays = balance.RemainingDays;
            return Result<decimal>.Ok(Math.Max(0, remainingDays));
        }
        catch (Exception ex)
        {
            return Result<decimal>.Fail($"Devir günleri hesaplanırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> AdjustBalanceAsync(LeaveBalanceAdjustmentDto adjustment)
    {
        try
        {
            var validationResult = await ValidateAdjustmentAsync(adjustment);
            if (!validationResult.IsSuccess)
            {
                return Result<bool>.Fail(validationResult.Message);
            }

            var balance = await _unitOfWork.LeaveBalances.GetBalanceAsync(adjustment.PersonId, adjustment.LeaveTypeId, adjustment.Year);
            if (balance == null)
            {
                return Result<bool>.Fail("İzin bakiyesi bulunamadı.");
            }

            balance.ManualAdjustment += adjustment.AdjustmentDays;
            balance.AdjustmentReason = adjustment.Reason;
            balance.AdjustmentDate = DateTime.Now;
            // balance.AdjustedById = adjustment.AdjustedById; // Temporarily commented
            balance.AvailableDays = balance.AllocatedDays + balance.CarriedOverDays + balance.ManualAdjustment;
            balance.RemainingDays = balance.AvailableDays - balance.UsedDays - balance.PendingDays;
            balance.UpdatedAt = DateTime.Now;

            _unitOfWork.LeaveBalances.Update(balance);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Bakiye düzeltmesi yapılırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<LeaveBalanceHistoryDto>>> GetBalanceHistoryAsync(int personId, int leaveTypeId, int year)
    {
        try
        {
            // This is a simplified implementation
            // In a real application, you would have a separate BalanceHistory table
            var history = new List<LeaveBalanceHistoryDto>();
            
            var balance = await _unitOfWork.LeaveBalances.GetBalanceAsync(personId, leaveTypeId, year);
            if (balance != null)
            {
                history.Add(new LeaveBalanceHistoryDto
                {
                    Date = balance.CreatedAt,
                    Action = "Bakiye Oluşturuldu",
                    NewValue = balance.AllocatedDays,
                    ChangeDays = balance.AllocatedDays,
                    Reason = "İlk bakiye oluşturma"
                });

                if (balance.ManualAdjustment != 0)
                {
                    history.Add(new LeaveBalanceHistoryDto
                    {
                        Date = balance.AdjustmentDate ?? balance.UpdatedAt ?? DateTime.Now,
                        Action = "Manuel Düzeltme",
                        PreviousValue = balance.AllocatedDays,
                        NewValue = balance.AllocatedDays + balance.ManualAdjustment,
                        ChangeDays = balance.ManualAdjustment,
                        Reason = balance.AdjustmentReason ?? "Manuel düzeltme",
                        AdjustedByName = "TBD" // Temporarily set to placeholder
                    });
                }
            }

            return Result<IEnumerable<LeaveBalanceHistoryDto>>.Ok(history);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveBalanceHistoryDto>>.Fail($"Bakiye geçmişi getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> ValidateAdjustmentAsync(LeaveBalanceAdjustmentDto adjustment)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(adjustment.Reason))
            {
                return Result<bool>.Fail("Düzeltme gerekçesi zorunludur.");
            }

            if (adjustment.AdjustmentDays == 0)
            {
                return Result<bool>.Fail("Düzeltme miktarı sıfır olamaz.");
            }

            var balance = await _unitOfWork.LeaveBalances.GetBalanceAsync(adjustment.PersonId, adjustment.LeaveTypeId, adjustment.Year);
            if (balance == null)
            {
                return Result<bool>.Fail("İzin bakiyesi bulunamadı.");
            }

            // Check if adjustment would make available days negative
            var newAvailableDays = balance.AvailableDays + adjustment.AdjustmentDays;
            if (newAvailableDays < 0)
            {
                return Result<bool>.Fail("Düzeltme sonrası kullanılabilir gün sayısı negatif olamaz.");
            }

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Düzeltme doğrulama sırasında hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> InitializeBalancesForPersonAsync(int personId, int year)
    {
        try
        {
            var activeLeaveTypes = await _unitOfWork.LeaveTypes.GetActiveLeaveTypesAsync();
            
            foreach (var leaveType in activeLeaveTypes)
            {
                await EnsureBalanceExistsAsync(personId, leaveType.Id, year);
            }

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Personel bakiyeleri başlatılırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> InitializeBalancesForYearAsync(int year)
    {
        try
        {
            var activePersons = await _unitOfWork.Persons.GetActivePersonsAsync();
            
            foreach (var person in activePersons)
            {
                await InitializeBalancesForPersonAsync(person.Id, year);
            }

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Yıl bakiyeleri başlatılırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> CreateDefaultBalanceAsync(int personId, int leaveTypeId, int year)
    {
        try
        {
            var leaveType = await _unitOfWork.LeaveTypes.GetByIdAsync(leaveTypeId);
            if (leaveType == null)
            {
                return Result<bool>.Fail("İzin türü bulunamadı.");
            }

            var balance = new LeaveBalance
            {
                PersonId = personId,
                LeaveTypeId = leaveTypeId,
                Year = year,
                AllocatedDays = leaveType.MaxDaysPerYear,
                UsedDays = 0,
                PendingDays = 0,
                CarriedOverDays = 0,
                MonthlyAccrual = leaveType.MaxDaysPerYear > 0 ? (decimal)leaveType.MaxDaysPerYear / 12 : 0,
                AccruedToDate = leaveType.MaxDaysPerYear,
                ManualAdjustment = 0,
                LastAccrualDate = DateTime.Now,
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            balance.AvailableDays = balance.AllocatedDays + balance.CarriedOverDays + balance.ManualAdjustment;
            balance.RemainingDays = balance.AvailableDays - balance.UsedDays - balance.PendingDays;

            await _unitOfWork.LeaveBalances.AddAsync(balance);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Varsayılan bakiye oluşturulurken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> EnsureBalanceExistsAsync(int personId, int leaveTypeId, int year)
    {
        try
        {
            var existingBalance = await _unitOfWork.LeaveBalances.GetBalanceAsync(personId, leaveTypeId, year);
            if (existingBalance != null)
            {
                return Result<bool>.Ok(true);
            }

            var createResult = await CreateDefaultBalanceAsync(personId, leaveTypeId, year);
            return createResult;
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Bakiye varlığı kontrol edilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<Dictionary<string, object>>> GetBalanceStatisticsAsync(int? departmentId = null, int? year = null)
    {
        try
        {
            if (year == null) year = DateTime.Now.Year;

            var statistics = new Dictionary<string, object>();
            IEnumerable<LeaveBalance> balances;

            if (departmentId.HasValue)
            {
                balances = await _unitOfWork.LeaveBalances.GetDepartmentBalancesAsync(departmentId.Value, year.Value);
                statistics["DepartmentId"] = departmentId.Value;
            }
            else
            {
                balances = await _unitOfWork.LeaveBalances.GetBalancesByYearAsync(year.Value);
            }

            statistics["Year"] = year;
            statistics["TotalBalances"] = balances.Count();
            statistics["TotalAllocatedDays"] = balances.Sum(b => b.AllocatedDays);
            statistics["TotalUsedDays"] = balances.Sum(b => b.UsedDays);
            statistics["TotalAvailableDays"] = balances.Sum(b => b.AvailableDays);
            statistics["AverageUsagePercentage"] = balances.Any() ? 
                balances.Where(b => b.AllocatedDays > 0).Average(b => (b.UsedDays / b.AllocatedDays) * 100) : 0;
            statistics["LowBalanceCount"] = balances.Count(b => b.RemainingDays < 2);
            statistics["OverusedCount"] = balances.Count(b => b.RemainingDays < 0);

            return Result<Dictionary<string, object>>.Ok(statistics);
        }
        catch (Exception ex)
        {
            return Result<Dictionary<string, object>>.Fail($"Bakiye istatistikleri hesaplanırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<LeaveBalanceListDto>>> GetLowBalanceAlertsAsync(decimal threshold = 2.0m)
    {
        try
        {
            var lowBalances = await _unitOfWork.LeaveBalances.GetLowBalanceAlertsAsync(threshold);
            var dtos = _mapper.Map<IEnumerable<LeaveBalanceListDto>>(lowBalances);
            return Result<IEnumerable<LeaveBalanceListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveBalanceListDto>>.Fail($"Düşük bakiye uyarıları getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<LeaveBalanceListDto>>> GetOverusedBalancesAsync(int? year = null)
    {
        try
        {
            if (year == null) year = DateTime.Now.Year;

            var overusedBalances = await _unitOfWork.LeaveBalances.GetOverusedBalancesAsync(year.Value);
            var dtos = _mapper.Map<IEnumerable<LeaveBalanceListDto>>(overusedBalances);
            return Result<IEnumerable<LeaveBalanceListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveBalanceListDto>>.Fail($"Aşırı kullanım bakiyeleri getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<LeaveBalanceListDto>>> GetUnusedBalancesAsync(int year, decimal threshold = 0.8m)
    {
        try
        {
            var unusedBalances = await _unitOfWork.LeaveBalances.GetUnusedBalancesAsync(threshold, year);
            var dtos = _mapper.Map<IEnumerable<LeaveBalanceListDto>>(unusedBalances);
            return Result<IEnumerable<LeaveBalanceListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveBalanceListDto>>.Fail($"Kullanılmayan bakiyeler getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> ValidateBalanceAsync(LeaveBalanceCreateDto dto)
    {
        try
        {
            if (dto.PersonId <= 0)
            {
                return Result<bool>.Fail("Geçerli bir personel seçilmelidir.");
            }

            if (dto.LeaveTypeId <= 0)
            {
                return Result<bool>.Fail("Geçerli bir izin türü seçilmelidir.");
            }

            if (dto.Year < 2020 || dto.Year > 2030)
            {
                return Result<bool>.Fail("Yıl 2020-2030 arasında olmalıdır.");
            }

            if (dto.AllocatedDays < 0)
            {
                return Result<bool>.Fail("Tahsisli gün sayısı negatif olamaz.");
            }

            if (dto.CarriedOverDays < 0)
            {
                return Result<bool>.Fail("Devir gün sayısı negatif olamaz.");
            }

            if (dto.MonthlyAccrual < 0)
            {
                return Result<bool>.Fail("Aylık tahakkuk negatif olamaz.");
            }

            var person = await _unitOfWork.Persons.GetByIdAsync(dto.PersonId);
            if (person == null)
            {
                return Result<bool>.Fail("Personel bulunamadı.");
            }

            var leaveType = await _unitOfWork.LeaveTypes.GetByIdAsync(dto.LeaveTypeId);
            if (leaveType == null)
            {
                return Result<bool>.Fail("İzin türü bulunamadı.");
            }

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Bakiye doğrulama sırasında hata oluştu: {ex.Message}");
        }
    }

    public Task<Result<bool>> ValidateBalanceAsync(LeaveBalanceUpdateDto dto)
    {
        try
        {
            if (dto.AllocatedDays < 0)
            {
                return Task.FromResult(Result<bool>.Fail("Tahsisli gün sayısı negatif olamaz."));
            }

            if (dto.CarriedOverDays < 0)
            {
                return Task.FromResult(Result<bool>.Fail("Devir gün sayısı negatif olamaz."));
            }

            if (dto.MonthlyAccrual < 0)
            {
                return Task.FromResult(Result<bool>.Fail("Aylık tahakkuk negatif olamaz."));
            }

            return Task.FromResult(Result<bool>.Ok(true));
        }
        catch (Exception ex)
        {
            return Task.FromResult(Result<bool>.Fail($"Bakiye güncelleme doğrulama sırasında hata oluştu: {ex.Message}"));
        }
    }

    public async Task<Result<bool>> CanDeleteBalanceAsync(int id)
    {
        try
        {
            var balance = await _unitOfWork.LeaveBalances.GetByIdAsync(id);
            if (balance == null)
            {
                return Result<bool>.Fail("İzin bakiyesi bulunamadı.");
            }

            if (balance.UsedDays > 0)
            {
                return Result<bool>.Fail("Kullanılmış günleri olan bakiye silinemez.");
            }

            if (balance.PendingDays > 0)
            {
                return Result<bool>.Fail("Bekleyen günleri olan bakiye silinemez.");
            }

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Silme kontrolü sırasında hata oluştu: {ex.Message}");
        }
    }
}
