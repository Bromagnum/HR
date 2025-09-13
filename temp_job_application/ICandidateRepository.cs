using DAL.Entities;

namespace DAL.Repositories;

public interface ICandidateRepository : IRepository<Candidate>
{
    Task<Candidate?> GetByTcKimlikNoAsync(string tcKimlikNo);
    Task<Candidate?> GetByEmailAsync(string email);
    Task<IEnumerable<Candidate>> GetByStatusAsync(string status);
    Task<IEnumerable<Candidate>> SearchAsync(string searchTerm);
    Task<IEnumerable<Candidate>> GetWithEducationsAsync();
    Task<IEnumerable<Candidate>> GetWithExperiencesAsync();
    Task<IEnumerable<Candidate>> GetWithSkillsAsync();
    Task<IEnumerable<Candidate>> GetWithJobApplicationsAsync();
    Task<Candidate?> GetWithAllDetailsAsync(int id);
    Task<IEnumerable<Candidate>> GetByExperienceRangeAsync(int minYears, int maxYears);
    Task<IEnumerable<Candidate>> GetBySkillAsync(string skillName);
    Task<IEnumerable<Candidate>> GetByLocationAsync(string city);
    Task<IEnumerable<Candidate>> GetBySalaryRangeAsync(decimal minSalary, decimal maxSalary);
    Task<IEnumerable<Candidate>> GetRecentCandidatesAsync(int days = 30);
    Task<IEnumerable<Candidate>> GetBlacklistedCandidatesAsync();
}
