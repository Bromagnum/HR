using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class LeaveBalanceRepository : Repository<LeaveBalance>, ILeaveBalanceRepository
{
    public LeaveBalanceRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<LeaveBalance>> GetBalancesByPersonIdAsync(int personId, int year)
    {
        return await _dbSet
            .Include(lb => lb.Person)
            .Include(lb => lb.LeaveType)
            .Where(lb => lb.PersonId == personId && lb.Year == year)
            .OrderBy(lb => lb.LeaveType.Name)
            .ToListAsync();
    }

    public async Task<LeaveBalance?> GetBalanceAsync(int personId, int leaveTypeId, int year)
    {
        return await _dbSet
            .Include(lb => lb.Person)
            .Include(lb => lb.LeaveType)
            .FirstOrDefaultAsync(lb => lb.PersonId == personId && 
                                     lb.LeaveTypeId == leaveTypeId && 
                                     lb.Year == year);
    }

    public async Task<IEnumerable<LeaveBalance>> GetBalancesByLeaveTypeAsync(int leaveTypeId, int year)
    {
        return await _dbSet
            .Include(lb => lb.Person)
                .ThenInclude(p => p.Department)
            .Include(lb => lb.LeaveType)
            .Where(lb => lb.LeaveTypeId == leaveTypeId && lb.Year == year)
            .OrderBy(lb => lb.Person.FirstName)
            .ToListAsync();
    }

    public async Task<IEnumerable<LeaveBalance>> GetBalancesForAccrualAsync(DateTime cutoffDate)
    {
        return await _dbSet
            .Include(lb => lb.Person)
            .Include(lb => lb.LeaveType)
            .Where(lb => lb.MonthlyAccrual > 0 && 
                        lb.LastAccrualDate < cutoffDate &&
                        lb.Person.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<LeaveBalance>> GetBalancesRequiringCarryOverAsync(int fromYear, int toYear)
    {
        return await _dbSet
            .Include(lb => lb.Person)
            .Include(lb => lb.LeaveType)
            .Where(lb => lb.Year == fromYear &&
                        lb.LeaveType.CanCarryOver &&
                        lb.RemainingDays > 0 &&
                        lb.Person.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<LeaveBalance>> GetDepartmentBalancesAsync(int departmentId, int year)
    {
        return await _dbSet
            .Include(lb => lb.Person)
            .Include(lb => lb.LeaveType)
            .Where(lb => lb.Person.DepartmentId == departmentId && 
                        lb.Year == year &&
                        lb.Person.IsActive)
            .OrderBy(lb => lb.Person.FirstName)
            .ThenBy(lb => lb.LeaveType.Name)
            .ToListAsync();
    }

    public async Task UpdateBalanceAsync(int personId, int leaveTypeId, int year, decimal usedDays, decimal pendingDays)
    {
        var balance = await GetBalanceAsync(personId, leaveTypeId, year);
        if (balance != null)
        {
            balance.UsedDays = usedDays;
            balance.PendingDays = pendingDays;
            balance.UpdatedAt = DateTime.Now;
            _dbSet.Update(balance);
        }
    }

    public async Task CreateOrUpdateBalanceAsync(LeaveBalance balance)
    {
        var existingBalance = await GetBalanceAsync(balance.PersonId, balance.LeaveTypeId, balance.Year);
        
        if (existingBalance != null)
        {
            // Update existing balance
            existingBalance.AllocatedDays = balance.AllocatedDays;
            existingBalance.UsedDays = balance.UsedDays;
            existingBalance.PendingDays = balance.PendingDays;
            existingBalance.CarriedOverDays = balance.CarriedOverDays;
            existingBalance.MonthlyAccrual = balance.MonthlyAccrual;
            existingBalance.AccruedToDate = balance.AccruedToDate;
            existingBalance.ManualAdjustment = balance.ManualAdjustment;
            existingBalance.AdjustmentReason = balance.AdjustmentReason;
            existingBalance.AdjustmentDate = balance.AdjustmentDate;
            // existingBalance.AdjustedById = balance.AdjustedById; // Temporarily commented
            existingBalance.UpdatedAt = DateTime.Now;
            
            _dbSet.Update(existingBalance);
        }
        else
        {
            // Create new balance
            balance.CreatedAt = DateTime.Now;
            await _dbSet.AddAsync(balance);
        }
    }

    public override async Task<IEnumerable<LeaveBalance>> GetAllAsync()
    {
        return await _dbSet
            .Include(lb => lb.Person)
                .ThenInclude(p => p.Department)
            .Include(lb => lb.LeaveType)
            // .Include(lb => lb.AdjustedBy) // Temporarily commented
            .OrderBy(lb => lb.Year)
            .ThenBy(lb => lb.Person.FirstName)
            .ThenBy(lb => lb.LeaveType.Name)
            .ToListAsync();
    }

    public override async Task<LeaveBalance?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(lb => lb.Person)
                .ThenInclude(p => p.Department)
            .Include(lb => lb.LeaveType)
            // .Include(lb => lb.AdjustedBy) // Temporarily commented
            .FirstOrDefaultAsync(lb => lb.Id == id);
    }

    // Additional methods used in services
    public async Task<IEnumerable<LeaveBalance>> GetBalancesByPersonAsync(int personId)
    {
        return await _dbSet
            .Include(lb => lb.Person)
                .ThenInclude(p => p.Department)
            .Include(lb => lb.LeaveType)
            // .Include(lb => lb.AdjustedBy) // Temporarily commented
            .Where(lb => lb.PersonId == personId)
            .OrderBy(lb => lb.Year)
            .ThenBy(lb => lb.LeaveType.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<LeaveBalance>> GetBalancesByPersonAsync(int personId, int year)
    {
        return await GetBalancesByPersonIdAsync(personId, year);
    }

    public async Task<IEnumerable<LeaveBalance>> GetActiveBalancesAsync()
    {
        var currentYear = DateTime.Now.Year;
        return await _dbSet
            .Include(lb => lb.Person)
                .ThenInclude(p => p.Department)
            .Include(lb => lb.LeaveType)
            .Where(lb => lb.Year >= currentYear && lb.Person.IsActive)
            .OrderBy(lb => lb.Person.FirstName)
            .ThenBy(lb => lb.LeaveType.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<LeaveBalance>> GetBalancesByYearAsync(int year)
    {
        return await _dbSet
            .Include(lb => lb.Person)
                .ThenInclude(p => p.Department)
            .Include(lb => lb.LeaveType)
            .Where(lb => lb.Year == year)
            .OrderBy(lb => lb.Person.FirstName)
            .ThenBy(lb => lb.LeaveType.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<LeaveBalance>> GetLowBalanceAlertsAsync(decimal threshold = 5)
    {
        return await _dbSet
            .Include(lb => lb.Person)
                .ThenInclude(p => p.Department)
            .Include(lb => lb.LeaveType)
            .Where(lb => lb.AvailableDays <= threshold && lb.AvailableDays >= 0 && lb.Person.IsActive)
            .OrderBy(lb => lb.AvailableDays)
            .ThenBy(lb => lb.Person.FirstName)
            .ToListAsync();
    }

    public async Task<IEnumerable<LeaveBalance>> GetOverusedBalancesAsync()
    {
        return await _dbSet
            .Include(lb => lb.Person)
                .ThenInclude(p => p.Department)
            .Include(lb => lb.LeaveType)
            .Where(lb => lb.AvailableDays < 0)
            .OrderBy(lb => lb.AvailableDays)
            .ThenBy(lb => lb.Person.FirstName)
            .ToListAsync();
    }

    public async Task<IEnumerable<LeaveBalance>> GetOverusedBalancesAsync(int year)
    {
        return await _dbSet
            .Include(lb => lb.Person)
                .ThenInclude(p => p.Department)
            .Include(lb => lb.LeaveType)
            .Where(lb => lb.AvailableDays < 0 && lb.Year == year)
            .OrderBy(lb => lb.AvailableDays)
            .ThenBy(lb => lb.Person.FirstName)
            .ToListAsync();
    }

    public async Task<IEnumerable<LeaveBalance>> GetUnusedBalancesAsync(decimal threshold = 0)
    {
        return await _dbSet
            .Include(lb => lb.Person)
                .ThenInclude(p => p.Department)
            .Include(lb => lb.LeaveType)
            .Where(lb => lb.UsedDays <= threshold && lb.AllocatedDays > 0)
            .OrderBy(lb => lb.Person.FirstName)
            .ThenBy(lb => lb.LeaveType.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<LeaveBalance>> GetUnusedBalancesAsync(decimal threshold, int year)
    {
        return await _dbSet
            .Include(lb => lb.Person)
                .ThenInclude(p => p.Department)
            .Include(lb => lb.LeaveType)
            .Where(lb => lb.UsedDays <= threshold && lb.AllocatedDays > 0 && lb.Year == year)
            .OrderBy(lb => lb.Person.FirstName)
            .ThenBy(lb => lb.LeaveType.Name)
            .ToListAsync();
    }
}
