using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class InterviewNoteRepository : Repository<InterviewNote>, IInterviewNoteRepository
{
    public InterviewNoteRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<InterviewNote>> GetByJobApplicationIdAsync(int jobApplicationId)
    {
        return await _context.InterviewNotes
            .Include(i => i.Interviewer)
            .Where(i => i.JobApplicationId == jobApplicationId && i.IsActive)
            .OrderByDescending(i => i.InterviewDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<InterviewNote>> GetByCandidateIdAsync(int candidateId)
    {
        return await _context.InterviewNotes
            .Include(i => i.Interviewer)
            .Include(i => i.JobApplication)
                .ThenInclude(ja => ja.Position)
            .Where(i => i.CandidateId == candidateId && i.IsActive)
            .OrderByDescending(i => i.InterviewDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<InterviewNote>> GetByInterviewerIdAsync(int interviewerId)
    {
        return await _context.InterviewNotes
            .Include(i => i.Candidate)
            .Include(i => i.JobApplication)
                .ThenInclude(ja => ja.Position)
            .Where(i => i.InterviewerId == interviewerId && i.IsActive)
            .OrderByDescending(i => i.InterviewDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<InterviewNote>> GetByInterviewTypeAsync(InterviewType interviewType)
    {
        return await _context.InterviewNotes
            .Include(i => i.Candidate)
            .Include(i => i.Interviewer)
            .Include(i => i.JobApplication)
                .ThenInclude(ja => ja.Position)
            .Where(i => i.InterviewType == interviewType && i.IsActive)
            .OrderByDescending(i => i.InterviewDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<InterviewNote>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.InterviewNotes
            .Include(i => i.Candidate)
            .Include(i => i.Interviewer)
            .Include(i => i.JobApplication)
                .ThenInclude(ja => ja.Position)
            .Where(i => i.InterviewDate >= startDate && 
                       i.InterviewDate <= endDate && 
                       i.IsActive)
            .OrderByDescending(i => i.InterviewDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<InterviewNote>> GetCompletedInterviewsAsync()
    {
        return await _context.InterviewNotes
            .Include(i => i.Candidate)
            .Include(i => i.Interviewer)
            .Include(i => i.JobApplication)
                .ThenInclude(ja => ja.Position)
            .Where(i => i.IsCompleted && i.IsActive)
            .OrderByDescending(i => i.CompletedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<InterviewNote>> GetPendingInterviewsAsync()
    {
        return await _context.InterviewNotes
            .Include(i => i.Candidate)
            .Include(i => i.Interviewer)
            .Include(i => i.JobApplication)
                .ThenInclude(ja => ja.Position)
            .Where(i => !i.IsCompleted && i.InterviewDate >= DateTime.Now && i.IsActive)
            .OrderBy(i => i.InterviewDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<InterviewNote>> GetUpcomingInterviewsAsync(int days = 7)
    {
        var endDate = DateTime.Now.AddDays(days);
        return await _context.InterviewNotes
            .Include(i => i.Candidate)
            .Include(i => i.Interviewer)
            .Include(i => i.JobApplication)
                .ThenInclude(ja => ja.Position)
            .Where(i => i.InterviewDate >= DateTime.Now && 
                       i.InterviewDate <= endDate && 
                       !i.IsCompleted && 
                       i.IsActive)
            .OrderBy(i => i.InterviewDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<InterviewNote>> GetByScoreRangeAsync(int minScore, int maxScore)
    {
        return await _context.InterviewNotes
            .Include(i => i.Candidate)
            .Include(i => i.Interviewer)
            .Include(i => i.JobApplication)
                .ThenInclude(ja => ja.Position)
            .Where(i => i.OverallScore >= minScore && 
                       i.OverallScore <= maxScore && 
                       i.IsActive)
            .OrderByDescending(i => i.OverallScore)
            .ToListAsync();
    }

    public async Task<IEnumerable<InterviewNote>> GetByRecommendationAsync(string recommendation)
    {
        return await _context.InterviewNotes
            .Include(i => i.Candidate)
            .Include(i => i.Interviewer)
            .Include(i => i.JobApplication)
                .ThenInclude(ja => ja.Position)
            .Where(i => i.Recommendation != null && 
                       i.Recommendation.ToLower().Contains(recommendation.ToLower()) && 
                       i.IsActive)
            .OrderByDescending(i => i.InterviewDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<InterviewNote>> GetTodaysInterviewsAsync()
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        
        return await _context.InterviewNotes
            .Include(i => i.Candidate)
            .Include(i => i.Interviewer)
            .Include(i => i.JobApplication)
                .ThenInclude(ja => ja.Position)
            .Where(i => i.InterviewDate >= today && 
                       i.InterviewDate < tomorrow && 
                       i.IsActive)
            .OrderBy(i => i.InterviewDate)
            .ToListAsync();
    }

    public async Task<decimal> GetAverageScoreByInterviewerAsync(int interviewerId)
    {
        var scores = await _context.InterviewNotes
            .Where(i => i.InterviewerId == interviewerId && 
                       i.OverallScore.HasValue && 
                       i.IsActive)
            .Select(i => i.OverallScore!.Value)
            .ToListAsync();

        return scores.Any() ? (decimal)scores.Average() : 0;
    }

    public async Task<IEnumerable<InterviewNote>> GetHighScoredInterviewsAsync(int minScore = 8)
    {
        return await _context.InterviewNotes
            .Include(i => i.Candidate)
            .Include(i => i.Interviewer)
            .Include(i => i.JobApplication)
                .ThenInclude(ja => ja.Position)
            .Where(i => i.OverallScore >= minScore && i.IsActive)
            .OrderByDescending(i => i.OverallScore)
            .ToListAsync();
    }
}
