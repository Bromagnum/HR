using DAL.Entities;

namespace DAL.Repositories
{
    public interface IWorkLogRepository : IRepository<WorkLog>
    {
        Task<IEnumerable<WorkLog>> GetByPersonIdAsync(int personId);
        Task<IEnumerable<WorkLog>> GetByPersonIdAndDateRangeAsync(int personId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<WorkLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<WorkLog?> GetByPersonIdAndDateAsync(int personId, DateTime date);
        Task<IEnumerable<WorkLog>> GetPendingApprovalsAsync();
        Task<IEnumerable<WorkLog>> GetByApproverIdAsync(int approverId);
        Task<IEnumerable<WorkLog>> GetOvertimeWorkLogsAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<WorkLog>> GetLateArrivalsAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<WorkLog>> GetByWorkTypeAsync(string workType, DateTime startDate, DateTime endDate);
        Task<IEnumerable<WorkLog>> GetActiveWorkLogsAsync();
        Task<WorkLog?> GetWorkLogWithPersonAsync(int id);
        Task<decimal> GetTotalHoursByPersonAsync(int personId, DateTime startDate, DateTime endDate);
        Task<decimal> GetOvertimeHoursByPersonAsync(int personId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<WorkLog>> GetWeeklyReportAsync(int personId, DateTime weekStart);
        Task<IEnumerable<WorkLog>> GetMonthlyReportAsync(int personId, int year, int month);
    }
}
