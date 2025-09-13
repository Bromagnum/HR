using BLL.DTOs;
using BLL.Utilities;
using DAL.Entities;

namespace BLL.Services;

public interface ICandidateSkillService
{
    // CRUD Operations
    Task<Result<IEnumerable<CandidateSkillDto>>> GetAllAsync();
    Task<Result<CandidateSkillDto>> GetByIdAsync(int id);
    Task<Result<CandidateSkillDto>> CreateAsync(CandidateSkillCreateDto dto);
    Task<Result<CandidateSkillDto>> UpdateAsync(CandidateSkillUpdateDto dto);
    Task<Result<bool>> DeleteAsync(int id);

    // Candidate Specific Operations
    Task<Result<IEnumerable<CandidateSkillDto>>> GetByCandidateIdAsync(int candidateId);

    // Filter Operations
    Task<Result<IEnumerable<CandidateSkillDto>>> GetBySkillNameAsync(string skillName);
    Task<Result<IEnumerable<CandidateSkillDto>>> GetByCategoryAsync(SkillCategory category);
    Task<Result<IEnumerable<CandidateSkillDto>>> GetByLevelAsync(SkillLevel level);
    Task<Result<IEnumerable<CandidateSkillDto>>> GetVerifiedSkillsAsync();
    Task<Result<IEnumerable<CandidateSkillDto>>> GetSkillsWithCertificationsAsync();
    Task<Result<IEnumerable<CandidateSkillDto>>> GetExpiredCertificationsAsync();
    Task<Result<IEnumerable<CandidateSkillDto>>> GetExpiringSoonCertificationsAsync(int days = 30);
    Task<Result<IEnumerable<CandidateSkillDto>>> GetByExperienceRangeAsync(int minYears, int maxYears);

    // Statistics
    Task<Result<IEnumerable<string>>> GetPopularSkillsAsync(int count = 20);
    Task<Result<IEnumerable<SkillStatisticsDto>>> GetSkillStatisticsAsync();

    // Validation
    Task<Result<bool>> ValidateSkillAsync(CandidateSkillCreateDto dto);
    Task<Result<bool>> ValidateSkillAsync(CandidateSkillUpdateDto dto);
    Task<Result<bool>> VerifySkillAsync(int id, int verifiedById);
}
