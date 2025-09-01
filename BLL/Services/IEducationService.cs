using BLL.DTOs;
using BLL.Utilities;

namespace BLL.Services;

public interface IEducationService
{
    Task<Result<IEnumerable<EducationListDto>>> GetAllAsync();
    Task<Result<EducationDetailDto?>> GetByIdAsync(int id);
    Task<Result<IEnumerable<EducationListDto>>> GetByPersonIdAsync(int personId);
    Task<Result<IEnumerable<EducationListDto>>> GetOngoingEducationsAsync();
    Task<Result<IEnumerable<EducationListDto>>> GetCompletedEducationsAsync();
    Task<Result<IEnumerable<EducationListDto>>> GetEducationsByDegreeAsync(string degree);
    Task<Result<EducationDetailDto>> CreateAsync(EducationCreateDto educationCreateDto);
    Task<Result<EducationDetailDto>> UpdateAsync(EducationUpdateDto educationUpdateDto);
    Task<Result> DeleteAsync(int id);
    Task<Result> ChangeStatusAsync(int id, bool isActive);
}

