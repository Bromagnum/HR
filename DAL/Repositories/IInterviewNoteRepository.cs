using DAL.Entities;

namespace DAL.Repositories;

public interface IInterviewNoteRepository : IRepository<InterviewNote>
{
    Task<IEnumerable<InterviewNote>> GetByJobApplicationIdAsync(int jobApplicationId);
    Task<IEnumerable<InterviewNote>> GetByCandidateIdAsync(int candidateId);
    Task<IEnumerable<InterviewNote>> GetByInterviewerIdAsync(int interviewerId);
    Task<IEnumerable<InterviewNote>> GetByInterviewTypeAsync(InterviewType interviewType);
    Task<IEnumerable<InterviewNote>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<InterviewNote>> GetCompletedInterviewsAsync();
    Task<IEnumerable<InterviewNote>> GetPendingInterviewsAsync();
    Task<IEnumerable<InterviewNote>> GetUpcomingInterviewsAsync(int days = 7);
    Task<IEnumerable<InterviewNote>> GetByScoreRangeAsync(int minScore, int maxScore);
    Task<IEnumerable<InterviewNote>> GetByRecommendationAsync(string recommendation);
    Task<IEnumerable<InterviewNote>> GetTodaysInterviewsAsync();
    Task<decimal> GetAverageScoreByInterviewerAsync(int interviewerId);
    Task<IEnumerable<InterviewNote>> GetHighScoredInterviewsAsync(int minScore = 8);
}
