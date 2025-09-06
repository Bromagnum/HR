using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class LeaveTypeRepository : Repository<LeaveType>, ILeaveTypeRepository
{
    public LeaveTypeRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<LeaveType>> GetActiveLeaveTypesAsync()
    {
        return await _dbSet
            .Where(lt => lt.IsActive)
            .OrderBy(lt => lt.Name)
            .ToListAsync();
    }

    public async Task<LeaveType?> GetByNameAsync(string name)
    {
        return await _dbSet
            .FirstOrDefaultAsync(lt => lt.Name == name && lt.IsActive);
    }

    public async Task<IEnumerable<LeaveType>> GetLeaveTypesWithBalancesAsync(int personId, int year)
    {
        return await _dbSet
            .Include(lt => lt.LeaveBalances.Where(lb => lb.PersonId == personId && lb.Year == year))
            .Where(lt => lt.IsActive)
            .OrderBy(lt => lt.Name)
            .ToListAsync();
    }

    public override async Task<IEnumerable<LeaveType>> GetAllAsync()
    {
        return await _dbSet
            .Include(lt => lt.Leaves)
            .Include(lt => lt.LeaveBalances)
            .OrderBy(lt => lt.Name)
            .ToListAsync();
    }

    public override async Task<LeaveType?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(lt => lt.Leaves)
            .Include(lt => lt.LeaveBalances)
            .FirstOrDefaultAsync(lt => lt.Id == id);
    }
}
