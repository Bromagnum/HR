using BLL.DTOs;
using BLL.Utilities;

namespace BLL.Services
{
    public interface IWorkLogService
    {
        // Basic CRUD operations
        Task<Result<IEnumerable<WorkLogListDto>>> GetAllAsync();
        Task<Result<WorkLogDetailDto>> GetByIdAsync(int id);
        Task<Result<WorkLogDetailDto>> CreateAsync(WorkLogCreateDto dto);
        Task<Result<WorkLogDetailDto>> UpdateAsync(WorkLogUpdateDto dto);
        Task<Result> DeleteAsync(int id);
        Task<Result> ToggleStatusAsync(int id);

        // Time tracking operations
        Task<Result<WorkLogDetailDto>> CheckInAsync(WorkLogCheckInDto dto);
        Task<Result<WorkLogDetailDto>> CheckOutAsync(WorkLogCheckOutDto dto);
        Task<Result<WorkLogDetailDto>> StartBreakAsync(int workLogId);
        Task<Result<WorkLogDetailDto>> EndBreakAsync(int workLogId);

        // Person-specific operations
        Task<Result<IEnumerable<WorkLogListDto>>> GetByPersonIdAsync(int personId);
        Task<Result<WorkLogDetailDto>> GetTodayWorkLogAsync(int personId);
        Task<Result<WorkLogDetailDto>> GetActiveWorkLogAsync(int personId);

        // Date range operations
        Task<Result<IEnumerable<WorkLogListDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Result<IEnumerable<WorkLogListDto>>> GetByPersonAndDateRangeAsync(int personId, DateTime startDate, DateTime endDate);

        // Reports and analytics
        Task<Result<WorkLogTimeSheetDto>> GetTimeSheetAsync(int personId, DateTime startDate, DateTime endDate);
        Task<Result<WorkLogTimeSheetDto>> GetWeeklyTimeSheetAsync(int personId, DateTime weekStart);
        Task<Result<WorkLogTimeSheetDto>> GetMonthlyTimeSheetAsync(int personId, int year, int month);
        Task<Result<IEnumerable<WorkLogListDto>>> GetOvertimeReportAsync(DateTime startDate, DateTime endDate);
        Task<Result<IEnumerable<WorkLogListDto>>> GetLateArrivalReportAsync(DateTime startDate, DateTime endDate);

        // Approval operations
        Task<Result<IEnumerable<WorkLogListDto>>> GetPendingApprovalsAsync();
        Task<Result> ApproveWorkLogAsync(WorkLogApprovalDto dto);
        Task<Result> RejectWorkLogAsync(WorkLogApprovalDto dto);

        // Analytics
        Task<Result<decimal>> GetTotalHoursByPersonAsync(int personId, DateTime startDate, DateTime endDate);
        Task<Result<decimal>> GetOvertimeHoursByPersonAsync(int personId, DateTime startDate, DateTime endDate);

        // Validation
        Task<Result<bool>> CanCheckInAsync(int personId, DateTime date);
        Task<Result<bool>> CanCheckOutAsync(int workLogId);
        Task<Result<bool>> HasActiveWorkLogAsync(int personId);
    }
}
