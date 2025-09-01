using BLL.DTOs;
using BLL.Utilities;

namespace BLL.Services;

public interface IQualificationService
{
    Task<Result<IEnumerable<QualificationListDto>>> GetAllAsync();
    Task<Result<QualificationDetailDto>> GetByIdAsync(int id);
    Task<Result<QualificationDetailDto>> CreateAsync(QualificationCreateDto qualificationCreateDto);
    Task<Result<QualificationDetailDto>> UpdateAsync(QualificationUpdateDto qualificationUpdateDto);
    Task<Result> DeleteAsync(int id);
    Task<Result> ChangeStatusAsync(int id, bool isActive);
    
    // Additional methods for specific queries
    Task<Result<IEnumerable<QualificationListDto>>> GetByPersonIdAsync(int personId);
    Task<Result<IEnumerable<QualificationListDto>>> GetExpiringSoonAsync(int days = 30);
    Task<Result<IEnumerable<QualificationListDto>>> GetExpiredAsync();
    Task<Result<IEnumerable<QualificationListDto>>> GetByCategoryAsync(string category);
}
