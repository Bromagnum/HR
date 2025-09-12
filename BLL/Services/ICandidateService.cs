using BLL.DTOs;
using BLL.Utilities;

namespace BLL.Services;

public interface ICandidateService
{
    // CRUD Operations
    Task<Result<IEnumerable<CandidateListDto>>> GetAllAsync();
    Task<Result<CandidateSearchResultDto>> GetFilteredAsync(CandidateFilterDto filter);
    Task<Result<CandidateDetailDto>> GetByIdAsync(int id);
    Task<Result<CandidateDetailDto>> CreateAsync(CandidateCreateDto dto);
    Task<Result<CandidateDetailDto>> UpdateAsync(CandidateUpdateDto dto);
    Task<Result<bool>> DeleteAsync(int id);
    Task<Result<bool>> ToggleStatusAsync(int id);

    // Search Operations
    Task<Result<IEnumerable<CandidateListDto>>> SearchAsync(string searchTerm);
    Task<Result<CandidateDetailDto>> GetByTcKimlikNoAsync(string tcKimlikNo);
    Task<Result<CandidateDetailDto>> GetByEmailAsync(string email);

    // Filter Operations
    Task<Result<IEnumerable<CandidateListDto>>> GetByStatusAsync(string status);
    Task<Result<IEnumerable<CandidateListDto>>> GetByLocationAsync(string city);
    Task<Result<IEnumerable<CandidateListDto>>> GetByExperienceRangeAsync(int minYears, int maxYears);
    Task<Result<IEnumerable<CandidateListDto>>> GetBySalaryRangeAsync(decimal minSalary, decimal maxSalary);
    Task<Result<IEnumerable<CandidateListDto>>> GetBySkillAsync(string skillName);

    // Detailed Operations
    Task<Result<CandidateDetailDto>> GetWithAllDetailsAsync(int id);
    Task<Result<IEnumerable<CandidateListDto>>> GetRecentCandidatesAsync(int days = 30);
    Task<Result<IEnumerable<CandidateListDto>>> GetBlacklistedCandidatesAsync();

    // Validation
    Task<Result<bool>> ValidateCandidateAsync(CandidateCreateDto dto);
    Task<Result<bool>> ValidateCandidateAsync(CandidateUpdateDto dto);
    Task<Result<bool>> IsTcKimlikNoUniqueAsync(string tcKimlikNo, int? excludeId = null);
    Task<Result<bool>> IsEmailUniqueAsync(string email, int? excludeId = null);

    // Statistics
    Task<Result<Dictionary<string, int>>> GetCandidateStatisticsAsync();
    Task<Result<Dictionary<string, int>>> GetCandidatesBySourceAsync();
    Task<Result<Dictionary<string, int>>> GetCandidatesByLocationAsync();
}
