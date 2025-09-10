using BLL.DTOs;
using BLL.Utilities;
using DAL.Entities;

namespace BLL.Services;

public interface ILeaveService
{
    // CRUD Operations
    Task<Result<IEnumerable<LeaveListDto>>> GetAllAsync();
    Task<Result<IEnumerable<LeaveListDto>>> GetFilteredAsync(LeaveFilterDto filter);
    Task<Result<LeaveDetailDto>> GetByIdAsync(int id);
    Task<Result<LeaveDetailDto>> CreateAsync(LeaveCreateDto dto);
    Task<Result<LeaveDetailDto>> UpdateAsync(LeaveUpdateDto dto);
    Task<Result<bool>> DeleteAsync(int id);
    Task<Result<bool>> CancelAsync(int id, int userId);
    
    // Leave Management
    Task<Result<IEnumerable<LeaveListDto>>> GetLeavesByPersonAsync(int personId, int? year = null);
    Task<Result<IEnumerable<LeaveListDto>>> GetLeavesByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<Result<IEnumerable<LeaveListDto>>> GetPendingApprovalsAsync();
    Task<Result<IEnumerable<LeaveListDto>>> GetPendingApprovalsByManagerAsync(int managerId);
    Task<Result<IEnumerable<LeaveListDto>>> GetActiveLeavesByDateAsync(DateTime date);
    Task<Result<IEnumerable<LeaveListDto>>> GetTeamLeavesAsync(int departmentId, DateTime startDate, DateTime endDate);
    
    // Approval Workflow
    Task<Result<bool>> ApproveLeaveAsync(LeaveApprovalDto approval);
    Task<Result<bool>> RejectLeaveAsync(LeaveApprovalDto rejection);
    Task<Result<bool>> CanUserApproveLeaveAsync(int leaveId, int userId);
    Task<Result<IEnumerable<LeaveListDto>>> GetLeavesRequiringMyApprovalAsync(int managerId);
    
    // Validation & Business Rules
    Task<Result<bool>> ValidateLeaveRequestAsync(LeaveCreateDto dto);
    Task<Result<bool>> ValidateLeaveUpdateAsync(LeaveUpdateDto dto);
    Task<Result<IEnumerable<LeaveListDto>>> CheckConflictsAsync(int personId, DateTime startDate, DateTime endDate, int? excludeLeaveId = null);
    
    // Calendar & Reports
    Task<Result<IEnumerable<LeaveCalendarDto>>> GetCalendarDataAsync(DateTime startDate, DateTime endDate, int? departmentId = null);
    Task<Result<Dictionary<string, object>>> GetLeaveStatisticsAsync(int? personId = null, int? departmentId = null, int? year = null);
    Task<Result<IEnumerable<LeaveListDto>>> GetUpcomingLeavesAsync(int days = 30);
    Task<Result<IEnumerable<LeaveListDto>>> GetLeavesRequiringAttentionAsync();
    
    // Utility Methods
    Task<Result<int>> CalculateWorkingDaysAsync(DateTime startDate, DateTime endDate);
    Task<Result<bool>> IsWorkingDayAsync(DateTime date);
    Task<Result<DateTime>> GetNextWorkingDayAsync(DateTime date);
    Task<Result<bool>> UpdateLeaveStatusBasedOnDatesAsync();
}
