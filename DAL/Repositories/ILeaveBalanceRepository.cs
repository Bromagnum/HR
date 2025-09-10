using DAL.Entities;

namespace DAL.Repositories;

public interface ILeaveBalanceRepository : IRepository<LeaveBalance>
{
    Task<IEnumerable<LeaveBalance>> GetBalancesByPersonIdAsync(int personId, int year);
    Task<LeaveBalance?> GetBalanceAsync(int personId, int leaveTypeId, int year);
    Task<IEnumerable<LeaveBalance>> GetBalancesByLeaveTypeAsync(int leaveTypeId, int year);
    Task<IEnumerable<LeaveBalance>> GetBalancesForAccrualAsync(DateTime cutoffDate);
    Task<IEnumerable<LeaveBalance>> GetBalancesRequiringCarryOverAsync(int fromYear, int toYear);
    Task<IEnumerable<LeaveBalance>> GetDepartmentBalancesAsync(int departmentId, int year);
    Task UpdateBalanceAsync(int personId, int leaveTypeId, int year, decimal usedDays, decimal pendingDays);
    Task CreateOrUpdateBalanceAsync(LeaveBalance balance);
    
    // Additional methods used in services
    Task<IEnumerable<LeaveBalance>> GetBalancesByPersonAsync(int personId);
    Task<IEnumerable<LeaveBalance>> GetActiveBalancesAsync();
    Task<IEnumerable<LeaveBalance>> GetBalancesByYearAsync(int year);
    Task<IEnumerable<LeaveBalance>> GetLowBalanceAlertsAsync(decimal threshold = 5);
    Task<IEnumerable<LeaveBalance>> GetOverusedBalancesAsync(int year);
    Task<IEnumerable<LeaveBalance>> GetUnusedBalancesAsync(decimal threshold, int year);
}
