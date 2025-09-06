using DAL.Entities;

namespace DAL.Repositories;

public interface ILeaveTypeRepository : IRepository<LeaveType>
{
    Task<IEnumerable<LeaveType>> GetActiveLeaveTypesAsync();
    Task<LeaveType?> GetByNameAsync(string name);
    Task<IEnumerable<LeaveType>> GetLeaveTypesWithBalancesAsync(int personId, int year);
}
