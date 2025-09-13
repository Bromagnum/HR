using BLL.DTOs;
using BLL.Utilities;

namespace BLL.Services;

public interface IOrganizationService
{
    Task<Result<IEnumerable<OrganizationListDto>>> GetAllAsync();
    Task<Result<OrganizationDetailDto>> GetByIdAsync(int id);
    Task<Result<OrganizationDetailDto>> CreateAsync(OrganizationCreateDto dto);
    Task<Result<OrganizationDetailDto>> UpdateAsync(OrganizationUpdateDto dto);
    Task<Result<bool>> DeleteAsync(int id);
    Task<Result<IEnumerable<OrganizationListDto>>> GetFilteredAsync(OrganizationFilterDto filter);
    Task<Result<IEnumerable<OrganizationTreeDto>>> GetOrganizationTreeAsync();
    Task<Result<IEnumerable<OrganizationListDto>>> GetRootOrganizationsAsync();
    Task<Result<IEnumerable<OrganizationListDto>>> GetSubOrganizationsAsync(int parentId);
}
