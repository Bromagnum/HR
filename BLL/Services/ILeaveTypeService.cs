using BLL.DTOs;
using BLL.Utilities;

namespace BLL.Services;

public interface ILeaveTypeService
{
    // CRUD Operations
    Task<Result<IEnumerable<LeaveTypeListDto>>> GetAllAsync();
    Task<Result<IEnumerable<LeaveTypeListDto>>> GetActiveAsync();
    Task<Result<LeaveTypeDetailDto>> GetByIdAsync(int id);
    Task<Result<LeaveTypeDetailDto>> CreateAsync(LeaveTypeCreateDto dto);
    Task<Result<LeaveTypeDetailDto>> UpdateAsync(LeaveTypeUpdateDto dto);
    Task<Result<bool>> DeleteAsync(int id);
    Task<Result<bool>> ToggleStatusAsync(int id);
    
    // Business Logic
    Task<Result<LeaveTypeDetailDto>> GetByNameAsync(string name);
    Task<Result<IEnumerable<LeaveTypeListDto>>> GetLeaveTypesWithBalancesAsync(int personId, int year);
    Task<Result<bool>> ValidateLeaveTypeAsync(LeaveTypeCreateDto dto);
    Task<Result<bool>> ValidateLeaveTypeAsync(LeaveTypeUpdateDto dto);
    Task<Result<bool>> CanDeleteLeaveTypeAsync(int id);
    
    // Statistics & Reporting
    Task<Result<Dictionary<string, object>>> GetLeaveTypeStatisticsAsync(int leaveTypeId, int year);
    Task<Result<Dictionary<string, object>>> GetAllLeaveTypesStatisticsAsync(int year);
    Task<Result<IEnumerable<LeaveTypeListDto>>> GetMostUsedLeaveTypesAsync(int year, int count = 5);
}
