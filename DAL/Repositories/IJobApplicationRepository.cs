using DAL.Entities;

namespace DAL.Repositories;

public interface IJobApplicationRepository : IRepository<JobApplication>
{
    Task<IEnumerable<JobApplication>> GetByStatusAsync(ApplicationStatus status);
    Task<IEnumerable<JobApplication>> GetByCandidateIdAsync(int candidateId);
    Task<IEnumerable<JobApplication>> GetByPositionIdAsync(int positionId);
    Task<IEnumerable<JobApplication>> GetPendingApplicationsAsync();
    Task<IEnumerable<JobApplication>> GetApplicationsForReviewAsync();
    Task<IEnumerable<JobApplication>> GetApplicationsForInterviewAsync();
    Task<IEnumerable<JobApplication>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<JobApplication>> GetByReviewerIdAsync(int reviewerId);
    Task<IEnumerable<JobApplication>> GetByInterviewerIdAsync(int interviewerId);
    Task<JobApplication?> GetWithCandidateAsync(int id);
    Task<JobApplication?> GetWithPositionAsync(int id);
    Task<JobApplication?> GetWithAllDetailsAsync(int id);
    Task<IEnumerable<JobApplication>> GetWithCandidateAndPositionAsync();
    Task<IEnumerable<JobApplication>> GetExpiredOffersAsync();
    Task<IEnumerable<JobApplication>> GetLongPendingApplicationsAsync(int days = 30);
    Task<int> GetApplicationCountByStatusAsync(ApplicationStatus status);
    Task<IEnumerable<JobApplication>> GetTopScoredApplicationsAsync(int count = 10);
    Task<bool> HasCandidateAppliedToPositionAsync(int candidateId, int positionId);
    Task<IEnumerable<JobApplication>> GetDuplicateApplicationsAsync(int candidateId, int positionId);
}
