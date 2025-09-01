using BLL.DTOs;
using BLL.Utilities;

namespace BLL.Services;

public interface IDepartmentService
{
    Task<Result<IEnumerable<DepartmentListDto>>> GetAllAsync();
    Task<Result<DepartmentDetailDto>> GetByIdAsync(int id);
    Task<Result<IEnumerable<DepartmentListDto>>> GetRootDepartmentsAsync();
    Task<Result<IEnumerable<DepartmentListDto>>> GetSubDepartmentsAsync(int parentId);
    Task<Result<DepartmentDetailDto>> CreateAsync(DepartmentCreateDto dto);
    Task<Result<DepartmentDetailDto>> UpdateAsync(DepartmentUpdateDto dto);
    Task<Result> DeleteAsync(int id);
    Task<Result> SetActiveStatusAsync(int id, bool isActive);
    
    // Arama ve Filtreleme
    Task<Result<DepartmentSearchResultDto>> SearchAsync(DepartmentFilterDto filter);
    Task<Result<IEnumerable<DepartmentListDto>>> GetFilteredAsync(string? searchTerm, bool? isActive = null);
}
