using DAL.Entities;

namespace DAL.Repositories;

public interface IJobDefinitionRepository : IRepository<JobDefinition>
{
    Task<IEnumerable<JobDefinition>> GetByPositionIdAsync(int positionId);
    Task<IEnumerable<JobDefinition>> GetApprovedAsync();
    Task<IEnumerable<JobDefinition>> GetPendingApprovalAsync();
    Task<JobDefinition?> GetByPositionIdAndVersionAsync(int positionId, string version);
    Task<IEnumerable<JobDefinition>> GetByDepartmentIdAsync(int departmentId);
    Task<string> GetNextVersionAsync(int positionId);
    Task<IEnumerable<JobDefinition>> GetWithRequiredQualificationsAsync();
    Task<IEnumerable<JobDefinition>> GetWithMatchingResultsAsync();
    Task<JobDefinition?> GetWithDetailsAsync(int id);
    Task<bool> HasApprovedVersionAsync(int positionId);
    Task<IEnumerable<JobDefinition>> SearchAsync(string searchTerm);
}

public interface IJobDefinitionQualificationRepository : IRepository<JobDefinitionQualification>
{
    Task<IEnumerable<JobDefinitionQualification>> GetByJobDefinitionIdAsync(int jobDefinitionId);
    Task<IEnumerable<JobDefinitionQualification>> GetByCategoryAsync(string category);
    Task<IEnumerable<JobDefinitionQualification>> GetByImportanceAsync(QualificationImportance importance);
    Task DeleteByJobDefinitionIdAsync(int jobDefinitionId);
}

public interface IQualificationMatchingResultRepository : IRepository<QualificationMatchingResult>
{
    Task<IEnumerable<QualificationMatchingResult>> GetByJobDefinitionIdAsync(int jobDefinitionId);
    Task<IEnumerable<QualificationMatchingResult>> GetByPersonIdAsync(int personId);
    Task<QualificationMatchingResult?> GetByJobDefinitionAndPersonAsync(int jobDefinitionId, int personId);
    Task<IEnumerable<QualificationMatchingResult>> GetHighMatchesAsync(decimal minPercentage = 80);
    Task<IEnumerable<QualificationMatchingResult>> GetByStatusAsync(MatchingStatus status);
    Task<IEnumerable<QualificationMatchingResult>> GetPendingReviewAsync();
    Task<IEnumerable<QualificationMatchingResult>> GetByMatchPercentageRangeAsync(decimal minPercentage, decimal maxPercentage);
    Task<bool> ExistsAsync(int jobDefinitionId, int personId);
    Task DeleteByJobDefinitionIdAsync(int jobDefinitionId);
    Task DeleteByPersonIdAsync(int personId);
}
