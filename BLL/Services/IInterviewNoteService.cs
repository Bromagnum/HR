using BLL.DTOs;
using BLL.Utilities;
using DAL.Entities;

namespace BLL.Services;

public interface IInterviewNoteService
{
    // CRUD Operations
    Task<Result<IEnumerable<InterviewNoteDto>>> GetAllAsync();
    Task<Result<InterviewNoteDto>> GetByIdAsync(int id);
    Task<Result<InterviewNoteDto>> CreateAsync(InterviewNoteCreateDto dto);
    Task<Result<InterviewNoteDto>> UpdateAsync(InterviewNoteUpdateDto dto);
    Task<Result<bool>> DeleteAsync(int id);

    // Application Specific Operations
    Task<Result<IEnumerable<InterviewNoteDto>>> GetByJobApplicationIdAsync(int jobApplicationId);
    Task<Result<IEnumerable<InterviewNoteDto>>> GetByCandidateIdAsync(int candidateId);

    // Interviewer Operations
    Task<Result<IEnumerable<InterviewNoteDto>>> GetByInterviewerIdAsync(int interviewerId);
    Task<Result<decimal>> GetAverageScoreByInterviewerAsync(int interviewerId);

    // Filter Operations
    Task<Result<IEnumerable<InterviewNoteDto>>> GetByInterviewTypeAsync(InterviewType interviewType);
    Task<Result<IEnumerable<InterviewNoteDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<Result<IEnumerable<InterviewNoteDto>>> GetByScoreRangeAsync(int minScore, int maxScore);
    Task<Result<IEnumerable<InterviewNoteDto>>> GetByRecommendationAsync(string recommendation);

    // Schedule Management
    Task<Result<IEnumerable<InterviewNoteDto>>> GetCompletedInterviewsAsync();
    Task<Result<IEnumerable<InterviewNoteDto>>> GetPendingInterviewsAsync();
    Task<Result<IEnumerable<InterviewNoteDto>>> GetUpcomingInterviewsAsync(int days = 7);
    Task<Result<IEnumerable<InterviewNoteDto>>> GetTodaysInterviewsAsync();
    Task<Result<IEnumerable<InterviewScheduleDto>>> GetInterviewScheduleAsync(DateTime date);

    // Performance Analysis
    Task<Result<IEnumerable<InterviewNoteDto>>> GetHighScoredInterviewsAsync(int minScore = 8);
    Task<Result<Dictionary<string, decimal>>> GetInterviewerPerformanceAsync();

    // Validation
    Task<Result<bool>> ValidateInterviewNoteAsync(InterviewNoteCreateDto dto);
    Task<Result<bool>> ValidateInterviewNoteAsync(InterviewNoteUpdateDto dto);
    Task<Result<bool>> CompleteInterviewAsync(int id, int interviewerId);
}
