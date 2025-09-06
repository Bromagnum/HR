using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class LeaveRepository : Repository<Leave>, ILeaveRepository
{
    public LeaveRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Leave>> GetLeavesByPersonIdAsync(int personId)
    {
        return await _dbSet
            .Include(l => l.Person)
            .Include(l => l.LeaveType)
            .Include(l => l.ApprovedBy)
            .Include(l => l.HandoverToPerson)
            .Where(l => l.PersonId == personId)
            .OrderByDescending(l => l.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Leave>> GetLeavesByPersonIdAndYearAsync(int personId, int year)
    {
        return await _dbSet
            .Include(l => l.Person)
            .Include(l => l.LeaveType)
            .Include(l => l.ApprovedBy)
            .Where(l => l.PersonId == personId && l.StartDate.Year == year)
            .OrderByDescending(l => l.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Leave>> GetLeavesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(l => l.Person)
                .ThenInclude(p => p.Department)
            .Include(l => l.LeaveType)
            .Include(l => l.ApprovedBy)
            .Where(l => l.StartDate <= endDate && l.EndDate >= startDate)
            .OrderBy(l => l.StartDate)
            .ThenBy(l => l.Person.FirstName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Leave>> GetLeavesByStatusAsync(LeaveStatus status)
    {
        return await _dbSet
            .Include(l => l.Person)
                .ThenInclude(p => p.Department)
            .Include(l => l.LeaveType)
            .Include(l => l.ApprovedBy)
            .Where(l => l.Status == status)
            .OrderByDescending(l => l.RequestDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Leave>> GetPendingApprovalsAsync()
    {
        return await _dbSet
            .Include(l => l.Person)
                .ThenInclude(p => p.Department)
            .Include(l => l.LeaveType)
            .Where(l => l.Status == LeaveStatus.Pending)
            .OrderBy(l => l.RequestDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Leave>> GetPendingApprovalsByManagerAsync(int managerId)
    {
        return await _dbSet
            .Include(l => l.Person)
                .ThenInclude(p => p.Department)
            .Include(l => l.LeaveType)
            .Where(l => l.Status == LeaveStatus.Pending && 
                       l.Person.Department != null && 
                       l.Person.Department.Employees.Any(e => e.Id == managerId))
            .OrderBy(l => l.RequestDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Leave>> GetActiveLeavesByDateAsync(DateTime date)
    {
        return await _dbSet
            .Include(l => l.Person)
                .ThenInclude(p => p.Department)
            .Include(l => l.LeaveType)
            .Where(l => l.Status == LeaveStatus.InProgress && 
                       l.StartDate <= date && 
                       l.EndDate >= date)
            .OrderBy(l => l.Person.FirstName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Leave>> GetOverlappingLeavesAsync(int personId, DateTime startDate, DateTime endDate, int? excludeLeaveId = null)
    {
        var query = _dbSet
            .Include(l => l.LeaveType)
            .Where(l => l.PersonId == personId &&
                       l.Status != LeaveStatus.Rejected &&
                       l.Status != LeaveStatus.Cancelled &&
                       l.StartDate <= endDate &&
                       l.EndDate >= startDate);

        if (excludeLeaveId.HasValue)
        {
            query = query.Where(l => l.Id != excludeLeaveId.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<decimal> GetUsedLeaveDaysAsync(int personId, int leaveTypeId, int year)
    {
        return await _dbSet
            .Where(l => l.PersonId == personId &&
                       l.LeaveTypeId == leaveTypeId &&
                       l.StartDate.Year == year &&
                       (l.Status == LeaveStatus.Approved || l.Status == LeaveStatus.InProgress || l.Status == LeaveStatus.Completed))
            .SumAsync(l => l.TotalDays);
    }

    public async Task<decimal> GetPendingLeaveDaysAsync(int personId, int leaveTypeId, int year)
    {
        return await _dbSet
            .Where(l => l.PersonId == personId &&
                       l.LeaveTypeId == leaveTypeId &&
                       l.StartDate.Year == year &&
                       l.Status == LeaveStatus.Pending)
            .SumAsync(l => l.TotalDays);
    }

    public async Task<IEnumerable<Leave>> GetTeamLeavesAsync(int departmentId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(l => l.Person)
            .Include(l => l.LeaveType)
            .Where(l => l.Person.DepartmentId == departmentId &&
                       l.StartDate <= endDate &&
                       l.EndDate >= startDate &&
                       (l.Status == LeaveStatus.Approved || l.Status == LeaveStatus.InProgress))
            .OrderBy(l => l.StartDate)
            .ThenBy(l => l.Person.FirstName)
            .ToListAsync();
    }

    public override async Task<IEnumerable<Leave>> GetAllAsync()
    {
        return await _dbSet
            .Include(l => l.Person)
                .ThenInclude(p => p.Department)
            .Include(l => l.LeaveType)
            .Include(l => l.ApprovedBy)
            .Include(l => l.HandoverToPerson)
            .OrderByDescending(l => l.RequestDate)
            .ToListAsync();
    }

    public override async Task<Leave?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(l => l.Person)
                .ThenInclude(p => p.Department)
            .Include(l => l.LeaveType)
            .Include(l => l.ApprovedBy)
            .Include(l => l.HandoverToPerson)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    // Additional methods used in services
    public async Task<IEnumerable<Leave>> GetFilteredLeavesAsync(int? personId = null, int? leaveTypeId = null, int? departmentId = null, LeaveStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, int? year = null)
    {
        var query = _dbSet
            .Include(l => l.Person)
                .ThenInclude(p => p.Department)
            .Include(l => l.LeaveType)
            .Include(l => l.ApprovedBy)
            .Include(l => l.HandoverToPerson)
            .AsQueryable();

        if (personId.HasValue)
            query = query.Where(l => l.PersonId == personId.Value);

        if (leaveTypeId.HasValue)
            query = query.Where(l => l.LeaveTypeId == leaveTypeId.Value);

        if (departmentId.HasValue)
            query = query.Where(l => l.Person.DepartmentId == departmentId.Value);

        if (status.HasValue)
            query = query.Where(l => l.Status == status.Value);

        if (startDate.HasValue)
            query = query.Where(l => l.StartDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(l => l.EndDate <= endDate.Value);

        if (year.HasValue)
            query = query.Where(l => l.StartDate.Year == year.Value);

        return await query.OrderByDescending(l => l.RequestDate).ToListAsync();
    }

    public async Task<IEnumerable<Leave>> GetLeavesByPersonAsync(int personId)
    {
        return await GetLeavesByPersonIdAsync(personId);
    }

    public async Task<IEnumerable<Leave>> GetLeavesByPersonAsync(int personId, int year)
    {
        return await GetLeavesByPersonIdAndYearAsync(personId, year);
    }

    public async Task<IEnumerable<Leave>> GetConflictingLeavesAsync(int personId, DateTime startDate, DateTime endDate, int? excludeLeaveId = null)
    {
        return await GetOverlappingLeavesAsync(personId, startDate, endDate, excludeLeaveId);
    }

    public async Task<IEnumerable<Leave>> GetCalendarDataAsync(DateTime startDate, DateTime endDate, int? departmentId = null)
    {
        var query = _dbSet
            .Include(l => l.Person)
                .ThenInclude(p => p.Department)
            .Include(l => l.LeaveType)
            .Where(l => l.StartDate <= endDate && l.EndDate >= startDate && 
                       (l.Status == LeaveStatus.Approved || l.Status == LeaveStatus.InProgress))
            .AsQueryable();

        if (departmentId.HasValue)
            query = query.Where(l => l.Person.DepartmentId == departmentId.Value);

        return await query.OrderBy(l => l.StartDate).ToListAsync();
    }

    public async Task<IEnumerable<Leave>> GetUpcomingLeavesAsync(int days = 30)
    {
        var futureDate = DateTime.Now.AddDays(days);
        return await _dbSet
            .Include(l => l.Person)
                .ThenInclude(p => p.Department)
            .Include(l => l.LeaveType)
            .Where(l => l.StartDate > DateTime.Now && l.StartDate <= futureDate && 
                       (l.Status == LeaveStatus.Approved || l.Status == LeaveStatus.InProgress))
            .OrderBy(l => l.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Leave>> GetUpcomingLeavesAsync(int days, int? departmentId)
    {
        var futureDate = DateTime.Now.AddDays(days);
        var query = _dbSet
            .Include(l => l.Person)
                .ThenInclude(p => p.Department)
            .Include(l => l.LeaveType)
            .Where(l => l.StartDate > DateTime.Now && l.StartDate <= futureDate && 
                       (l.Status == LeaveStatus.Approved || l.Status == LeaveStatus.InProgress))
            .AsQueryable();

        if (departmentId.HasValue)
            query = query.Where(l => l.Person.DepartmentId == departmentId.Value);

        return await query.OrderBy(l => l.StartDate).ToListAsync();
    }

    public async Task<IEnumerable<Leave>> GetUrgentLeavesAsync()
    {
        var urgentDate = DateTime.Now.AddDays(7);
        return await _dbSet
            .Include(l => l.Person)
                .ThenInclude(p => p.Department)
            .Include(l => l.LeaveType)
            .Where(l => l.StartDate <= urgentDate && l.Status == LeaveStatus.Pending && l.IsUrgent)
            .OrderBy(l => l.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Leave>> GetApprovedLeavesByPersonAndTypeAsync(int personId, int leaveTypeId, int year)
    {
        return await _dbSet
            .Include(l => l.Person)
            .Include(l => l.LeaveType)
            .Where(l => l.PersonId == personId && 
                       l.LeaveTypeId == leaveTypeId && 
                       l.StartDate.Year == year && 
                       (l.Status == LeaveStatus.Approved || l.Status == LeaveStatus.InProgress || l.Status == LeaveStatus.Completed))
            .OrderBy(l => l.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Leave>> GetPendingLeavesByPersonAndTypeAsync(int personId, int leaveTypeId, int year)
    {
        return await _dbSet
            .Include(l => l.Person)
            .Include(l => l.LeaveType)
            .Where(l => l.PersonId == personId && 
                       l.LeaveTypeId == leaveTypeId && 
                       l.StartDate.Year == year && 
                       l.Status == LeaveStatus.Pending)
            .OrderBy(l => l.StartDate)
            .ToListAsync();
    }
}
