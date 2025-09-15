using DAL.Entities;

namespace DAL.Repositories;

public interface ISkillAssessmentRepository : IRepository<SkillAssessment>
{
    Task<IEnumerable<SkillAssessment>> GetByPersonSkillIdAsync(int personSkillId);
    Task<IEnumerable<SkillAssessment>> GetByPersonIdAsync(int personId);
    Task<IEnumerable<SkillAssessment>> GetByTypeAsync(AssessmentType type);
}
