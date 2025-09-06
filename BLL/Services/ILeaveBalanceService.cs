using BLL.DTOs;
using BLL.Utilities;

namespace BLL.Services;

public interface ILeaveBalanceService
{
    // CRUD Operations
    Task<Result<IEnumerable<LeaveBalanceListDto>>> GetAllAsync();
    Task<Result<LeaveBalanceDetailDto>> GetByIdAsync(int id);
    Task<Result<LeaveBalanceDetailDto>> CreateAsync(LeaveBalanceCreateDto dto);
    Task<Result<LeaveBalanceDetailDto>> UpdateAsync(LeaveBalanceUpdateDto dto);
    Task<Result<bool>> DeleteAsync(int id);
    
    // Balance Management
    Task<Result<IEnumerable<LeaveBalanceListDto>>> GetBalancesByPersonAsync(int personId, int year);
    Task<Result<LeaveBalanceSummaryDto>> GetBalanceSummaryAsync(int personId, int year);
    Task<Result<LeaveBalanceDetailDto>> GetBalanceAsync(int personId, int leaveTypeId, int year);
    Task<Result<IEnumerable<LeaveBalanceListDto>>> GetDepartmentBalancesAsync(int departmentId, int year);
    Task<Result<IEnumerable<LeaveBalanceListDto>>> GetBalancesByLeaveTypeAsync(int leaveTypeId, int year);
    
    // Balance Calculations
    Task<Result<bool>> UpdateBalanceAfterLeaveAsync(int personId, int leaveTypeId, int year);
    Task<Result<bool>> RecalculateAllBalancesAsync(int personId, int year);
    Task<Result<bool>> RecalculateBalanceAsync(int personId, int leaveTypeId, int year);
    Task<Result<decimal>> CalculateUsedDaysAsync(int personId, int leaveTypeId, int year);
    Task<Result<decimal>> CalculatePendingDaysAsync(int personId, int leaveTypeId, int year);
    Task<Result<decimal>> CalculateAvailableDaysAsync(int personId, int leaveTypeId, int year);
    
    // Accrual Management
    Task<Result<bool>> ProcessMonthlyAccrualsAsync(DateTime cutoffDate);
    Task<Result<bool>> ProcessAccrualForPersonAsync(int personId, DateTime cutoffDate);
    Task<Result<decimal>> CalculateAccruedDaysAsync(int personId, int leaveTypeId, DateTime asOfDate);
    Task<Result<bool>> UpdateAccrualAsync(int personId, int leaveTypeId, int year, decimal accruedDays);
    
    // Carry Over Management
    Task<Result<bool>> ProcessYearEndCarryOverAsync(int fromYear, int toYear);
    Task<Result<bool>> ProcessCarryOverForPersonAsync(int personId, int fromYear, int toYear);
    Task<Result<decimal>> CalculateCarryOverDaysAsync(int personId, int leaveTypeId, int fromYear);
    
    // Manual Adjustments
    Task<Result<bool>> AdjustBalanceAsync(LeaveBalanceAdjustmentDto adjustment);
    Task<Result<IEnumerable<LeaveBalanceHistoryDto>>> GetBalanceHistoryAsync(int personId, int leaveTypeId, int year);
    Task<Result<bool>> ValidateAdjustmentAsync(LeaveBalanceAdjustmentDto adjustment);
    
    // Initialization & Setup
    Task<Result<bool>> InitializeBalancesForPersonAsync(int personId, int year);
    Task<Result<bool>> InitializeBalancesForYearAsync(int year);
    Task<Result<bool>> CreateDefaultBalanceAsync(int personId, int leaveTypeId, int year);
    Task<Result<bool>> EnsureBalanceExistsAsync(int personId, int leaveTypeId, int year);
    
    // Reporting & Analytics
    Task<Result<Dictionary<string, object>>> GetBalanceStatisticsAsync(int? departmentId = null, int? year = null);
    Task<Result<IEnumerable<LeaveBalanceListDto>>> GetLowBalanceAlertsAsync(decimal threshold = 2.0m);
    Task<Result<IEnumerable<LeaveBalanceListDto>>> GetOverusedBalancesAsync(int? year = null);
    Task<Result<IEnumerable<LeaveBalanceListDto>>> GetUnusedBalancesAsync(int year, decimal threshold = 0.8m);
    
    // Validation
    Task<Result<bool>> ValidateBalanceAsync(LeaveBalanceCreateDto dto);
    Task<Result<bool>> ValidateBalanceAsync(LeaveBalanceUpdateDto dto);
    Task<Result<bool>> CanDeleteBalanceAsync(int id);
}
