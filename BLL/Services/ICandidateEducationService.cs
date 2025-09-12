using BLL.DTOs;
using BLL.Utilities;

namespace BLL.Services;

public interface ICandidateEducationService
{
    // CRUD Operations
    Task<Result<IEnumerable<CandidateEducationDto>>> GetAllAsync();
    Task<Result<CandidateEducationDto>> GetByIdAsync(int id);
    Task<Result<CandidateEducationDto>> CreateAsync(CandidateEducationCreateDto dto);
    Task<Result<CandidateEducationDto>> UpdateAsync(CandidateEducationUpdateDto dto);
    Task<Result<bool>> DeleteAsync(int id);

    // Candidate Specific Operations
    Task<Result<IEnumerable<CandidateEducationDto>>> GetByCandidateIdAsync(int candidateId);

    // Filter Operations
    Task<Result<IEnumerable<CandidateEducationDto>>> GetByDegreeAsync(string degree);
    Task<Result<IEnumerable<CandidateEducationDto>>> GetByFieldOfStudyAsync(string fieldOfStudy);
    Task<Result<IEnumerable<CandidateEducationDto>>> GetBySchoolNameAsync(string schoolName);
    Task<Result<IEnumerable<CandidateEducationDto>>> GetOngoingEducationsAsync();
    Task<Result<IEnumerable<CandidateEducationDto>>> GetByGpaRangeAsync(decimal minGpa, decimal maxGpa);
    Task<Result<IEnumerable<CandidateEducationDto>>> GetRecentGraduatesAsync(int years = 2);

    // Validation
    Task<Result<bool>> ValidateEducationAsync(CandidateEducationCreateDto dto);
    Task<Result<bool>> ValidateEducationAsync(CandidateEducationUpdateDto dto);
}
