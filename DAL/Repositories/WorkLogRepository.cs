using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class WorkLogRepository : Repository<WorkLog>, IWorkLogRepository
    {
        public WorkLogRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<WorkLog>> GetByPersonIdAsync(int personId)
        {
            return await _context.WorkLogs
                .Include(w => w.Person)
                .Include(w => w.ApprovedBy)
                .Where(w => w.PersonId == personId && w.IsActive)
                .OrderByDescending(w => w.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkLog>> GetByPersonIdAndDateRangeAsync(int personId, DateTime startDate, DateTime endDate)
        {
            return await _context.WorkLogs
                .Include(w => w.Person)
                .Include(w => w.ApprovedBy)
                .Where(w => w.PersonId == personId && 
                           w.Date >= startDate && 
                           w.Date <= endDate)
                .OrderBy(w => w.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.WorkLogs
                .Include(w => w.Person)
                    .ThenInclude(p => p.Department)
                .Include(w => w.ApprovedBy)
                .Where(w => w.Date >= startDate && 
                           w.Date <= endDate)
                .OrderBy(w => w.Date)
                .ThenBy(w => w.Person.FirstName)
                .ToListAsync();
        }

        public async Task<WorkLog?> GetByPersonIdAndDateAsync(int personId, DateTime date)
        {
            return await _context.WorkLogs
                .Include(w => w.Person)
                .Include(w => w.ApprovedBy)
                .FirstOrDefaultAsync(w => w.PersonId == personId && 
                                         w.Date.Date == date.Date && 
                                         w.IsActive);
        }

        public async Task<IEnumerable<WorkLog>> GetPendingApprovalsAsync()
        {
            return await _context.WorkLogs
                .Include(w => w.Person)
                    .ThenInclude(p => p.Department)
                .Where(w => w.Status == "Pending" && w.IsActive)
                .OrderBy(w => w.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkLog>> GetByApproverIdAsync(int approverId)
        {
            return await _context.WorkLogs
                .Include(w => w.Person)
                    .ThenInclude(p => p.Department)
                .Include(w => w.ApprovedBy)
                .Where(w => w.ApprovedById == approverId && w.IsActive)
                .OrderByDescending(w => w.ApprovedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkLog>> GetOvertimeWorkLogsAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.WorkLogs
                .Include(w => w.Person)
                    .ThenInclude(p => p.Department)
                .Where(w => w.IsOvertime && 
                           w.Date >= startDate && 
                           w.Date <= endDate)
                .OrderBy(w => w.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkLog>> GetLateArrivalsAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.WorkLogs
                .Include(w => w.Person)
                    .ThenInclude(p => p.Department)
                .Where(w => w.IsLateArrival && 
                           w.Date >= startDate && 
                           w.Date <= endDate)
                .OrderBy(w => w.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkLog>> GetByWorkTypeAsync(string workType, DateTime startDate, DateTime endDate)
        {
            return await _context.WorkLogs
                .Include(w => w.Person)
                    .ThenInclude(p => p.Department)
                .Where(w => w.WorkType == workType && 
                           w.Date >= startDate && 
                           w.Date <= endDate)
                .OrderBy(w => w.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkLog>> GetActiveWorkLogsAsync()
        {
            return await _context.WorkLogs
                .Include(w => w.Person)
                    .ThenInclude(p => p.Department)
                .Where(w => w.Status == "Active" && 
                           w.EndTime == null && 
                           w.IsActive)
                .OrderBy(w => w.StartTime)
                .ToListAsync();
        }

        public async Task<WorkLog?> GetWorkLogWithPersonAsync(int id)
        {
            return await _context.WorkLogs
                .Include(w => w.Person)
                    .ThenInclude(p => p.Department)
                .Include(w => w.ApprovedBy)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<decimal> GetTotalHoursByPersonAsync(int personId, DateTime startDate, DateTime endDate)
        {
            return await _context.WorkLogs
                .Where(w => w.PersonId == personId && 
                           w.Date >= startDate && 
                           w.Date <= endDate && 
                           w.IsActive)
                .SumAsync(w => w.TotalHours);
        }

        public async Task<decimal> GetOvertimeHoursByPersonAsync(int personId, DateTime startDate, DateTime endDate)
        {
            return await _context.WorkLogs
                .Where(w => w.PersonId == personId && 
                           w.Date >= startDate && 
                           w.Date <= endDate && 
                           w.IsActive)
                .SumAsync(w => w.OvertimeHours);
        }

        public async Task<IEnumerable<WorkLog>> GetWeeklyReportAsync(int personId, DateTime weekStart)
        {
            var weekEnd = weekStart.AddDays(6);
            return await GetByPersonIdAndDateRangeAsync(personId, weekStart, weekEnd);
        }

        public async Task<IEnumerable<WorkLog>> GetMonthlyReportAsync(int personId, int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            return await GetByPersonIdAndDateRangeAsync(personId, startDate, endDate);
        }

        public override async Task<IEnumerable<WorkLog>> GetAllAsync()
        {
            return await _context.WorkLogs
                .Include(w => w.Person)
                    .ThenInclude(p => p.Department)
                .Include(w => w.ApprovedBy)
                .OrderByDescending(w => w.Date)
                .ThenBy(w => w.Person.FirstName)
                .ToListAsync();
        }

        public override async Task<WorkLog?> GetByIdAsync(int id)
        {
            return await _context.WorkLogs
                .Include(w => w.Person)
                    .ThenInclude(p => p.Department)
                .Include(w => w.ApprovedBy)
                .FirstOrDefaultAsync(w => w.Id == id);
        }
    }
}
