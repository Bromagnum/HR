using DAL.Context;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class PerformanceReviewRepository : Repository<PerformanceReview>, IPerformanceReviewRepository
{
    public PerformanceReviewRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<PerformanceReview>> GetAllAsync()
    {
        return await _context.PerformanceReviews
            .Include(pr => pr.Person)
                .ThenInclude(p => p.Department)
            .Include(pr => pr.ReviewPeriod)
            .Include(pr => pr.Reviewer)
            .Include(pr => pr.ApprovedBy)
            .Include(pr => pr.Goals_Navigation)
            .OrderByDescending(pr => pr.CreatedAt)
            .ToListAsync();
    }

    public override async Task<PerformanceReview?> GetByIdAsync(int id)
    {
        return await _context.PerformanceReviews
            .Include(pr => pr.Person)
                .ThenInclude(p => p.Department)
            .Include(pr => pr.ReviewPeriod)
            .Include(pr => pr.Reviewer)
            .Include(pr => pr.ApprovedBy)
            .Include(pr => pr.Goals_Navigation)
            .FirstOrDefaultAsync(pr => pr.Id == id);
    }

    public async Task<IEnumerable<PerformanceReview>> GetByPersonIdAsync(int personId)
    {
        return await _context.PerformanceReviews
            .Include(pr => pr.Person)
                .ThenInclude(p => p.Department)
            .Include(pr => pr.ReviewPeriod)
            .Include(pr => pr.Reviewer)
            .Include(pr => pr.ApprovedBy)
            .Include(pr => pr.Goals_Navigation)
            .Where(pr => pr.PersonId == personId)
            .OrderByDescending(pr => pr.ReviewPeriod.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<PerformanceReview>> GetByReviewerIdAsync(int reviewerId)
    {
        return await _context.PerformanceReviews
            .Include(pr => pr.Person)
                .ThenInclude(p => p.Department)
            .Include(pr => pr.ReviewPeriod)
            .Include(pr => pr.Reviewer)
            .Include(pr => pr.ApprovedBy)
            .Include(pr => pr.Goals_Navigation)
            .Where(pr => pr.ReviewerId == reviewerId)
            .OrderByDescending(pr => pr.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<PerformanceReview>> GetByReviewPeriodIdAsync(int reviewPeriodId)
    {
        return await _context.PerformanceReviews
            .Include(pr => pr.Person)
                .ThenInclude(p => p.Department)
            .Include(pr => pr.ReviewPeriod)
            .Include(pr => pr.Reviewer)
            .Include(pr => pr.ApprovedBy)
            .Include(pr => pr.Goals_Navigation)
            .Where(pr => pr.ReviewPeriodId == reviewPeriodId)
            .OrderBy(pr => pr.Person.FirstName)
            .ThenBy(pr => pr.Person.LastName)
            .ToListAsync();
    }

    public async Task<IEnumerable<PerformanceReview>> GetByStatusAsync(ReviewStatus status)
    {
        return await _context.PerformanceReviews
            .Include(pr => pr.Person)
                .ThenInclude(p => p.Department)
            .Include(pr => pr.ReviewPeriod)
            .Include(pr => pr.Reviewer)
            .Include(pr => pr.ApprovedBy)
            .Include(pr => pr.Goals_Navigation)
            .Where(pr => pr.Status == status)
            .OrderByDescending(pr => pr.CreatedAt)
            .ToListAsync();
    }

    public async Task<PerformanceReview?> GetByPersonAndPeriodAsync(int personId, int reviewPeriodId)
    {
        return await _context.PerformanceReviews
            .Include(pr => pr.Person)
                .ThenInclude(p => p.Department)
            .Include(pr => pr.ReviewPeriod)
            .Include(pr => pr.Reviewer)
            .Include(pr => pr.ApprovedBy)
            .Include(pr => pr.Goals_Navigation)
            .FirstOrDefaultAsync(pr => pr.PersonId == personId && pr.ReviewPeriodId == reviewPeriodId);
    }

    public async Task<IEnumerable<PerformanceReview>> GetPendingReviewsAsync()
    {
        var pendingStatuses = new[] { ReviewStatus.Draft, ReviewStatus.InProgress, ReviewStatus.EmployeeReview, ReviewStatus.ManagerReview };
        
        return await _context.PerformanceReviews
            .Include(pr => pr.Person)
                .ThenInclude(p => p.Department)
            .Include(pr => pr.ReviewPeriod)
            .Include(pr => pr.Reviewer)
            .Include(pr => pr.ApprovedBy)
            .Include(pr => pr.Goals_Navigation)
            .Where(pr => pendingStatuses.Contains(pr.Status))
            .OrderBy(pr => pr.ReviewPeriod.ReviewEndDate)
            .ThenBy(pr => pr.Person.FirstName)
            .ToListAsync();
    }

    public async Task<IEnumerable<PerformanceReview>> GetCompletedReviewsAsync()
    {
        var completedStatuses = new[] { ReviewStatus.Completed, ReviewStatus.Approved };
        
        return await _context.PerformanceReviews
            .Include(pr => pr.Person)
                .ThenInclude(p => p.Department)
            .Include(pr => pr.ReviewPeriod)
            .Include(pr => pr.Reviewer)
            .Include(pr => pr.ApprovedBy)
            .Include(pr => pr.Goals_Navigation)
            .Where(pr => completedStatuses.Contains(pr.Status))
            .OrderByDescending(pr => pr.ApprovedAt ?? pr.UpdatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<PerformanceReview>> GetByDepartmentIdAsync(int departmentId)
    {
        return await _context.PerformanceReviews
            .Include(pr => pr.Person)
                .ThenInclude(p => p.Department)
            .Include(pr => pr.ReviewPeriod)
            .Include(pr => pr.Reviewer)
            .Include(pr => pr.ApprovedBy)
            .Include(pr => pr.Goals_Navigation)
            .Where(pr => pr.Person.DepartmentId == departmentId)
            .OrderByDescending(pr => pr.CreatedAt)
            .ToListAsync();
    }

    public async Task<decimal> GetAverageScoreByPersonAsync(int personId)
    {
        var completedStatuses = new[] { ReviewStatus.Completed, ReviewStatus.Approved };
        
        var reviews = await _context.PerformanceReviews
            .Where(pr => pr.PersonId == personId && completedStatuses.Contains(pr.Status))
            .ToListAsync();

        if (!reviews.Any())
            return 0;

        return (decimal)reviews.Average(pr => pr.OverallScore);
    }

    public async Task<decimal> GetAverageScoreByDepartmentAsync(int departmentId)
    {
        var completedStatuses = new[] { ReviewStatus.Completed, ReviewStatus.Approved };
        
        var reviews = await _context.PerformanceReviews
            .Include(pr => pr.Person)
            .Where(pr => pr.Person.DepartmentId == departmentId && completedStatuses.Contains(pr.Status))
            .ToListAsync();

        if (!reviews.Any())
            return 0;

        return (decimal)reviews.Average(pr => pr.OverallScore);
    }

    public async Task<IEnumerable<PerformanceReview>> GetReviewsForApprovalAsync(int approverId)
    {
        return await _context.PerformanceReviews
            .Include(pr => pr.Person)
                .ThenInclude(p => p.Department)
            .Include(pr => pr.ReviewPeriod)
            .Include(pr => pr.Reviewer)
            .Include(pr => pr.ApprovedBy)
            .Include(pr => pr.Goals_Navigation)
            .Where(pr => pr.Status == ReviewStatus.Completed && 
                        (pr.Person.Department != null && 
                         pr.Person.Department.ManagerId == approverId || 
                         pr.ReviewerId == approverId))
            .OrderBy(pr => pr.ReviewPeriod.ReviewEndDate)
            .ToListAsync();
    }
}
