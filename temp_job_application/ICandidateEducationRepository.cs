using DAL.Entities;

namespace DAL.Repositories;

public interface ICandidateEducationRepository : IRepository<CandidateEducation>
{
    Task<IEnumerable<CandidateEducation>> GetByCandidateIdAsync(int candidateId);
    Task<IEnumerable<CandidateEducation>> GetByDegreeAsync(string degree);
    Task<IEnumerable<CandidateEducation>> GetByFieldOfStudyAsync(string fieldOfStudy);
    Task<IEnumerable<CandidateEducation>> GetBySchoolNameAsync(string schoolName);
    Task<IEnumerable<CandidateEducation>> GetOngoingEducationsAsync();
    Task<IEnumerable<CandidateEducation>> GetByGpaRangeAsync(decimal minGpa, decimal maxGpa);
    Task<IEnumerable<CandidateEducation>> GetRecentGraduatesAsync(int years = 2);
}
