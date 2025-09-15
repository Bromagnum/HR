using BLL.DTOs;
using BLL.Utilities;

namespace BLL.Services;

public interface IJobDefinitionService
{
    // CRUD Operations
    Task<Result<IEnumerable<JobDefinitionListDto>>> GetAllAsync();
    Task<Result<JobDefinitionDetailDto>> GetByIdAsync(int id);
    Task<Result<JobDefinitionDetailDto>> CreateAsync(JobDefinitionCreateDto dto);
    Task<Result<JobDefinitionDetailDto>> UpdateAsync(JobDefinitionUpdateDto dto);
    Task<Result<bool>> DeleteAsync(int id);
    Task<Result<IEnumerable<JobDefinitionListDto>>> GetFilteredAsync(JobDefinitionFilterDto filter);

    // Position and Department Related
    Task<Result<IEnumerable<JobDefinitionListDto>>> GetByPositionIdAsync(int positionId);
    Task<Result<IEnumerable<JobDefinitionListDto>>> GetByDepartmentIdAsync(int departmentId);
    Task<Result<JobDefinitionDetailDto>> GetByPositionAndVersionAsync(int positionId, string version);

    // Approval Workflow
    Task<Result<IEnumerable<JobDefinitionListDto>>> GetApprovedAsync();
    Task<Result<IEnumerable<JobDefinitionListDto>>> GetPendingApprovalAsync();
    Task<Result<bool>> ApproveAsync(int id, int approvedById);
    Task<Result<bool>> RejectAsync(int id, string reason);

    // Version Management
    Task<Result<JobDefinitionDetailDto>> CreateNewVersionAsync(int baseId, JobDefinitionUpdateDto dto);
    Task<Result<string>> GetNextVersionAsync(int positionId);
    Task<Result<bool>> HasApprovedVersionAsync(int positionId);

    // Qualification Management
    Task<Result<IEnumerable<JobDefinitionQualificationDto>>> GetQualificationsAsync(int jobDefinitionId);
    Task<Result<bool>> AddQualificationAsync(int jobDefinitionId, JobDefinitionQualificationCreateDto dto);
    Task<Result<bool>> RemoveQualificationAsync(int jobDefinitionId, int qualificationId);
    Task<Result<bool>> UpdateQualificationAsync(int qualificationId, JobDefinitionQualificationCreateDto dto);

    // Skill Management
    Task<Result<IEnumerable<JobRequiredSkillDto>>> GetRequiredSkillsAsync(int jobDefinitionId);
    Task<Result<bool>> AddRequiredSkillAsync(int jobDefinitionId, JobRequiredSkillCreateDto dto);
    Task<Result<bool>> RemoveRequiredSkillAsync(int jobDefinitionId, int skillId);
    Task<Result<bool>> UpdateRequiredSkillAsync(int skillId, JobRequiredSkillCreateDto dto);

    // Matching and Analysis
    Task<Result<IEnumerable<QualificationMatchingResultDto>>> GetMatchingResultsAsync(int jobDefinitionId);
    Task<Result<QualificationMatchingResultDto>> CalculateMatchAsync(int jobDefinitionId, int personId);
    Task<Result<IEnumerable<QualificationMatchingResultDto>>> CalculateAllMatchesAsync(JobDefinitionMatchingRequestDto request);
    Task<Result<IEnumerable<QualificationMatchingResultDto>>> GetTopMatchesAsync(int jobDefinitionId, int count = 10);

    // Statistics and Reporting
    Task<Result<JobDefinitionSummaryDto>> GetSummaryAsync();
    Task<Result<DepartmentJobDefinitionSummaryDto>> GetDepartmentSummaryAsync(int departmentId);
    Task<Result<IEnumerable<JobDefinitionListDto>>> SearchAsync(string searchTerm);

    // Export
    Task<Result<byte[]>> ExportDefinitionAsync(int id);
    Task<Result<byte[]>> ExportMatchingResultsAsync(int jobDefinitionId);
    Task<Result<byte[]>> ExportSummaryAsync();

    // Validation
    Task<Result<bool>> ValidateDefinitionAsync(JobDefinitionCreateDto dto);
    Task<Result<List<string>>> GetValidationErrorsAsync(JobDefinitionCreateDto dto);
}
