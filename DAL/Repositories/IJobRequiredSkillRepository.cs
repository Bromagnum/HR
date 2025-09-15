using DAL.Entities;

namespace DAL.Repositories;

public interface IJobRequiredSkillRepository : IRepository<JobRequiredSkill>
{
    Task<IEnumerable<JobRequiredSkill>> GetByJobDefinitionIdAsync(int jobDefinitionId);
    Task<JobRequiredSkill?> GetByJobDefinitionAndSkillTemplateAsync(int jobDefinitionId, int skillTemplateId);
    Task<IEnumerable<JobRequiredSkill>> GetBySkillTemplateIdAsync(int skillTemplateId);
    Task DeleteByJobDefinitionIdAsync(int jobDefinitionId);
}
