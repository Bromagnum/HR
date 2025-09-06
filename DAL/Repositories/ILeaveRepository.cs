using DAL.Entities;

namespace DAL.Repositories;

public interface ILeaveRepository : IRepository<Leave>
{
    Task<IEnumerable<Leave>> GetLeavesByPersonIdAsync(int personId);
    Task<IEnumerable<Leave>> GetLeavesByPersonIdAndYearAsync(int personId, int year);
    Task<IEnumerable<Leave>> GetLeavesByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Leave>> GetLeavesByStatusAsync(LeaveStatus status);
    Task<IEnumerable<Leave>> GetPendingApprovalsAsync();
    Task<IEnumerable<Leave>> GetPendingApprovalsByManagerAsync(int managerId);
    Task<IEnumerable<Leave>> GetActiveLeavesByDateAsync(DateTime date);
    Task<IEnumerable<Leave>> GetOverlappingLeavesAsync(int personId, DateTime startDate, DateTime endDate, int? excludeLeaveId = null);
    Task<decimal> GetUsedLeaveDaysAsync(int personId, int leaveTypeId, int year);
    Task<decimal> GetPendingLeaveDaysAsync(int personId, int leaveTypeId, int year);
    Task<IEnumerable<Leave>> GetTeamLeavesAsync(int departmentId, DateTime startDate, DateTime endDate);
    
    // Additional methods used in services
    Task<IEnumerable<Leave>> GetFilteredLeavesAsync(int? personId = null, int? leaveTypeId = null, int? departmentId = null, LeaveStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, int? year = null);
    Task<IEnumerable<Leave>> GetLeavesByPersonAsync(int personId);
    Task<IEnumerable<Leave>> GetLeavesByPersonAsync(int personId, int year);
    Task<IEnumerable<Leave>> GetConflictingLeavesAsync(int personId, DateTime startDate, DateTime endDate, int? excludeLeaveId = null);
    Task<IEnumerable<Leave>> GetCalendarDataAsync(DateTime startDate, DateTime endDate, int? departmentId = null);
    Task<IEnumerable<Leave>> GetUpcomingLeavesAsync(int days = 30);
    Task<IEnumerable<Leave>> GetUpcomingLeavesAsync(int days, int? departmentId);
    Task<IEnumerable<Leave>> GetUrgentLeavesAsync();
    Task<IEnumerable<Leave>> GetApprovedLeavesByPersonAndTypeAsync(int personId, int leaveTypeId, int year);
    Task<IEnumerable<Leave>> GetPendingLeavesByPersonAndTypeAsync(int personId, int leaveTypeId, int year);
}
