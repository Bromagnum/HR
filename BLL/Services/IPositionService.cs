using BLL.DTOs;
using BLL.Utilities;

namespace BLL.Services
{
    public interface IPositionService
    {
        Task<Result<IEnumerable<PositionListDto>>> GetAllAsync();
        Task<Result<PositionDetailDto>> GetByIdAsync(int id);
        Task<Result<PositionDetailDto>> CreateAsync(PositionCreateDto dto);
        Task<Result<PositionDetailDto>> UpdateAsync(PositionUpdateDto dto);
        Task<Result> DeleteAsync(int id);
        Task<Result> ChangeStatusAsync(int id);
        Task<Result<IEnumerable<PositionListDto>>> GetByDepartmentIdAsync(int departmentId);
        Task<Result<IEnumerable<PositionListDto>>> GetAvailablePositionsAsync();
        Task<Result<IEnumerable<PositionListDto>>> GetByLevelAsync(string level);
        Task<Result<IEnumerable<PositionListDto>>> GetByEmploymentTypeAsync(string employmentType);
    }
}
