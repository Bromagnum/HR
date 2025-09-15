using DAL.Entities;

namespace DAL.Repositories;

public interface IPersonSkillRepository : IRepository<PersonSkill>
{
    Task<IEnumerable<PersonSkill>> GetByPersonIdAsync(int personId);
    Task<PersonSkill?> GetByPersonAndSkillTemplateAsync(int personId, int skillTemplateId);
    Task<IEnumerable<PersonSkill>> GetByPersonAndTypeAsync(int personId, SkillType type);
    Task<IEnumerable<PersonSkill>> GetBySkillTemplateIdAsync(int skillTemplateId);
}
