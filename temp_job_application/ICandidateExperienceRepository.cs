using DAL.Entities;

namespace DAL.Repositories;

public interface ICandidateExperienceRepository : IRepository<CandidateExperience>
{
    Task<IEnumerable<CandidateExperience>> GetByCandidateIdAsync(int candidateId);
    Task<IEnumerable<CandidateExperience>> GetByCompanyNameAsync(string companyName);
    Task<IEnumerable<CandidateExperience>> GetByJobTitleAsync(string jobTitle);
    Task<IEnumerable<CandidateExperience>> GetCurrentJobsAsync();
    Task<IEnumerable<CandidateExperience>> GetByEmploymentTypeAsync(string employmentType);
    Task<IEnumerable<CandidateExperience>> GetByDurationRangeAsync(int minMonths, int maxMonths);
    Task<IEnumerable<CandidateExperience>> GetBySalaryRangeAsync(decimal minSalary, decimal maxSalary);
    Task<IEnumerable<CandidateExperience>> GetByTechnologyAsync(string technology);
}
