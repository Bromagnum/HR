using DAL.Entities;

namespace DAL.Repositories;

public interface ICandidateSkillRepository : IRepository<CandidateSkill>
{
    Task<IEnumerable<CandidateSkill>> GetByCandidateIdAsync(int candidateId);
    Task<IEnumerable<CandidateSkill>> GetBySkillNameAsync(string skillName);
    Task<IEnumerable<CandidateSkill>> GetByCategoryAsync(SkillCategory category);
    Task<IEnumerable<CandidateSkill>> GetByLevelAsync(SkillLevel level);
    Task<IEnumerable<CandidateSkill>> GetVerifiedSkillsAsync();
    Task<IEnumerable<CandidateSkill>> GetSkillsWithCertificationsAsync();
    Task<IEnumerable<CandidateSkill>> GetExpiredCertificationsAsync();
    Task<IEnumerable<CandidateSkill>> GetExpiringSoonCertificationsAsync(int days = 30);
    Task<IEnumerable<string>> GetPopularSkillsAsync(int count = 20);
    Task<IEnumerable<CandidateSkill>> GetByExperienceRangeAsync(int minYears, int maxYears);
}
