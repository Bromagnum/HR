using DAL.Entities;

namespace DAL.Repositories;

public interface ISkillAssessmentRepository : IRepository<SkillAssessment>
{
    Task<IEnumerable<SkillAssessment>> GetByPersonSkillIdAsync(int personSkillId);
    Task<IEnumerable<SkillAssessment>> GetByPersonIdAsync(int personId);
    Task<IEnumerable<SkillAssessment>> GetByAssessorIdAsync(int assessorId);
    Task<IEnumerable<SkillAssessment>> GetByTypeAsync(AssessmentType type);
    Task<IEnumerable<SkillAssessment>> GetValidAssessmentsAsync();
    Task<IEnumerable<SkillAssessment>> GetExpiredAssessmentsAsync();
    Task<SkillAssessment?> GetLatestAssessmentAsync(int personSkillId);
    Task<IEnumerable<SkillAssessment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<SkillAssessment>> GetRecentAssessmentsAsync(int count = 10);
}
