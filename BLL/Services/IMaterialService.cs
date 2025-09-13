using BLL.DTOs;
using BLL.Utilities;

namespace BLL.Services;

public interface IMaterialService
{
    Task<Result<IEnumerable<MaterialListDto>>> GetAllAsync();
    Task<Result<MaterialDetailDto>> GetByIdAsync(int id);
    Task<Result<MaterialDetailDto>> CreateAsync(MaterialCreateDto dto);
    Task<Result<MaterialDetailDto>> UpdateAsync(MaterialUpdateDto dto);
    Task<Result<bool>> DeleteAsync(int id);
    Task<Result<IEnumerable<MaterialListDto>>> GetFilteredAsync(MaterialFilterDto filter);
    Task<Result<IEnumerable<MaterialListDto>>> GetByOrganizationAsync(int organizationId);
    Task<Result<IEnumerable<MaterialListDto>>> GetLowStockMaterialsAsync();
    Task<Result<IEnumerable<MaterialListDto>>> GetByCategoryAsync(string category);
    Task<Result<MaterialStockSummaryDto>> GetStockSummaryAsync();
    Task<Result<IEnumerable<string>>> GetCategoriesAsync();
}
