using DAL.Entities;

namespace DAL.Repositories;

public interface ISkillTemplateRepository : IRepository<SkillTemplate>
{
    Task<IEnumerable<SkillTemplate>> GetByCategoryAsync(string category);
    Task<IEnumerable<SkillTemplate>> GetByTypeAsync(SkillType type);
    Task<IEnumerable<SkillTemplate>> GetVerifiableAsync();
    Task<IEnumerable<SkillTemplate>> GetRequiringCertificationAsync();
    Task<IEnumerable<SkillTemplate>> SearchAsync(string searchTerm);
    Task<SkillTemplate?> GetByNameAsync(string name);
    Task<IEnumerable<SkillTemplate>> GetMostUsedAsync(int count = 10);
    Task<IEnumerable<string>> GetCategoriesAsync();
    Task IncrementUsageCountAsync(int id);
}
