using BLL.DTOs;
using BLL.Utilities;

namespace BLL.Services;

public interface IPersonService
{
    Task<Result<IEnumerable<PersonListDto>>> GetAllAsync();
    Task<Result<PersonDetailDto>> GetByIdAsync(int id);
    Task<Result<PersonDetailDto>> GetByTcKimlikNoAsync(string tcKimlikNo);
    Task<Result<IEnumerable<PersonListDto>>> GetByDepartmentIdAsync(int departmentId);
    Task<Result<IEnumerable<PersonListDto>>> GetActiveEmployeesAsync();
    Task<Result<PersonDetailDto>> CreateAsync(PersonCreateDto dto);
    Task<Result<PersonDetailDto>> UpdateAsync(PersonUpdateDto dto);
    Task<Result> DeleteAsync(int id);
    Task<Result> SetActiveStatusAsync(int id, bool isActive);
}
