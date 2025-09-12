using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class JobApplicationRepository : Repository<JobApplication>, IJobApplicationRepository
{
    public JobApplicationRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<JobApplication>> GetByStatusAsync(ApplicationStatus status)
    {
        return await _context.JobApplications
            .Include(ja => ja.Candidate)
            .Include(ja => ja.Position)
                .ThenInclude(p => p.Department)
            .Where(ja => ja.Status == status && ja.IsActive)
            .OrderByDescending(ja => ja.ApplicationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobApplication>> GetByCandidateIdAsync(int candidateId)
    {
        return await _context.JobApplications
            .Include(ja => ja.Position)
                .ThenInclude(p => p.Department)
            .Include(ja => ja.ReviewedBy)
            .Include(ja => ja.Interviewer)
            .Where(ja => ja.CandidateId == candidateId && ja.IsActive)
            .OrderByDescending(ja => ja.ApplicationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobApplication>> GetByPositionIdAsync(int positionId)
    {
        return await _context.JobApplications
            .Include(ja => ja.Candidate)
            .Include(ja => ja.ReviewedBy)
            .Where(ja => ja.PositionId == positionId && ja.IsActive)
            .OrderByDescending(ja => ja.ApplicationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobApplication>> GetPendingApplicationsAsync()
    {
        var pendingStatuses = new[] 
        { 
            ApplicationStatus.Applied, 
            ApplicationStatus.UnderReview,
            ApplicationStatus.Shortlisted,
            ApplicationStatus.InterviewScheduled
        };

        return await _context.JobApplications
            .Include(ja => ja.Candidate)
            .Include(ja => ja.Position)
                .ThenInclude(p => p.Department)
            .Where(ja => pendingStatuses.Contains(ja.Status) && ja.IsActive)
            .OrderBy(ja => ja.ApplicationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobApplication>> GetApplicationsForReviewAsync()
    {
        return await _context.JobApplications
            .Include(ja => ja.Candidate)
            .Include(ja => ja.Position)
            .Where(ja => ja.Status == ApplicationStatus.Applied && ja.IsActive)
            .OrderBy(ja => ja.ApplicationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobApplication>> GetApplicationsForInterviewAsync()
    {
        return await _context.JobApplications
            .Include(ja => ja.Candidate)
            .Include(ja => ja.Position)
            .Where(ja => ja.Status == ApplicationStatus.InterviewScheduled && ja.IsActive)
            .OrderBy(ja => ja.InterviewDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobApplication>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.JobApplications
            .Include(ja => ja.Candidate)
            .Include(ja => ja.Position)
            .Where(ja => ja.ApplicationDate >= startDate && 
                        ja.ApplicationDate <= endDate && 
                        ja.IsActive)
            .OrderByDescending(ja => ja.ApplicationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobApplication>> GetByReviewerIdAsync(int reviewerId)
    {
        return await _context.JobApplications
            .Include(ja => ja.Candidate)
            .Include(ja => ja.Position)
            .Where(ja => ja.ReviewedById == reviewerId && ja.IsActive)
            .OrderByDescending(ja => ja.ReviewedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobApplication>> GetByInterviewerIdAsync(int interviewerId)
    {
        return await _context.JobApplications
            .Include(ja => ja.Candidate)
            .Include(ja => ja.Position)
            .Where(ja => ja.InterviewerId == interviewerId && ja.IsActive)
            .OrderByDescending(ja => ja.InterviewDate)
            .ToListAsync();
    }

    public async Task<JobApplication?> GetWithCandidateAsync(int id)
    {
        return await _context.JobApplications
            .Include(ja => ja.Candidate)
                .ThenInclude(c => c.Educations)
            .Include(ja => ja.Candidate)
                .ThenInclude(c => c.Experiences)
            .Include(ja => ja.Candidate)
                .ThenInclude(c => c.Skills)
            .FirstOrDefaultAsync(ja => ja.Id == id && ja.IsActive);
    }

    public async Task<JobApplication?> GetWithPositionAsync(int id)
    {
        return await _context.JobApplications
            .Include(ja => ja.Position)
                .ThenInclude(p => p.Department)
            .FirstOrDefaultAsync(ja => ja.Id == id && ja.IsActive);
    }

    public async Task<JobApplication?> GetWithAllDetailsAsync(int id)
    {
        return await _context.JobApplications
            .Include(ja => ja.Candidate)
                .ThenInclude(c => c.Educations)
            .Include(ja => ja.Candidate)
                .ThenInclude(c => c.Experiences)
            .Include(ja => ja.Candidate)
                .ThenInclude(c => c.Skills)
            .Include(ja => ja.Position)
                .ThenInclude(p => p.Department)
            .Include(ja => ja.ReviewedBy)
            .Include(ja => ja.Interviewer)
            .Include(ja => ja.DecisionBy)
            .Include(ja => ja.InterviewNotes)
                .ThenInclude(i => i.Interviewer)
            .Include(ja => ja.Documents)
            .FirstOrDefaultAsync(ja => ja.Id == id && ja.IsActive);
    }

    public async Task<IEnumerable<JobApplication>> GetWithCandidateAndPositionAsync()
    {
        return await _context.JobApplications
            .Include(ja => ja.Candidate)
            .Include(ja => ja.Position)
                .ThenInclude(p => p.Department)
            .Where(ja => ja.IsActive)
            .OrderByDescending(ja => ja.ApplicationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobApplication>> GetExpiredOffersAsync()
    {
        return await _context.JobApplications
            .Include(ja => ja.Candidate)
            .Include(ja => ja.Position)
            .Where(ja => ja.Status == ApplicationStatus.Offered && 
                        ja.OfferExpiryDate.HasValue && 
                        ja.OfferExpiryDate < DateTime.Now &&
                        ja.IsActive)
            .OrderBy(ja => ja.OfferExpiryDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobApplication>> GetLongPendingApplicationsAsync(int days = 30)
    {
        var cutoffDate = DateTime.Now.AddDays(-days);
        var pendingStatuses = new[] 
        { 
            ApplicationStatus.Applied, 
            ApplicationStatus.UnderReview,
            ApplicationStatus.Shortlisted
        };

        return await _context.JobApplications
            .Include(ja => ja.Candidate)
            .Include(ja => ja.Position)
            .Where(ja => pendingStatuses.Contains(ja.Status) && 
                        ja.ApplicationDate <= cutoffDate &&
                        ja.IsActive)
            .OrderBy(ja => ja.ApplicationDate)
            .ToListAsync();
    }

    public async Task<int> GetApplicationCountByStatusAsync(ApplicationStatus status)
    {
        return await _context.JobApplications
            .CountAsync(ja => ja.Status == status && ja.IsActive);
    }

    public async Task<IEnumerable<JobApplication>> GetTopScoredApplicationsAsync(int count = 10)
    {
        return await _context.JobApplications
            .Include(ja => ja.Candidate)
            .Include(ja => ja.Position)
            .Where(ja => ja.OverallScore.HasValue && ja.IsActive)
            .OrderByDescending(ja => ja.OverallScore)
            .Take(count)
            .ToListAsync();
    }

    public async Task<bool> HasCandidateAppliedToPositionAsync(int candidateId, int positionId)
    {
        return await _context.JobApplications
            .AnyAsync(ja => ja.CandidateId == candidateId && 
                           ja.PositionId == positionId && 
                           ja.IsActive);
    }

    public async Task<IEnumerable<JobApplication>> GetDuplicateApplicationsAsync(int candidateId, int positionId)
    {
        return await _context.JobApplications
            .Include(ja => ja.Candidate)
            .Include(ja => ja.Position)
            .Where(ja => ja.CandidateId == candidateId && 
                        ja.PositionId == positionId && 
                        ja.IsActive)
            .OrderByDescending(ja => ja.ApplicationDate)
            .ToListAsync();
    }
}
