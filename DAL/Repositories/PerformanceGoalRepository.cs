using DAL.Context;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class PerformanceGoalRepository : Repository<PerformanceGoal>, IPerformanceGoalRepository
{
    public PerformanceGoalRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<PerformanceGoal>> GetAllAsync()
    {
        return await _context.PerformanceGoals
            .Include(pg => pg.PerformanceReview)
                .ThenInclude(pr => pr.Person)
            .Include(pg => pg.PerformanceReview)
                .ThenInclude(pr => pr.ReviewPeriod)
            .OrderByDescending(pg => pg.CreatedAt)
            .ToListAsync();
    }

    public override async Task<PerformanceGoal?> GetByIdAsync(int id)
    {
        return await _context.PerformanceGoals
            .Include(pg => pg.PerformanceReview)
                .ThenInclude(pr => pr.Person)
            .Include(pg => pg.PerformanceReview)
                .ThenInclude(pr => pr.ReviewPeriod)
            .FirstOrDefaultAsync(pg => pg.Id == id);
    }

    public async Task<IEnumerable<PerformanceGoal>> GetByPerformanceReviewIdAsync(int performanceReviewId)
    {
        return await _context.PerformanceGoals
            .Include(pg => pg.PerformanceReview)
                .ThenInclude(pr => pr.Person)
            .Include(pg => pg.PerformanceReview)
                .ThenInclude(pr => pr.ReviewPeriod)
            .Where(pg => pg.PerformanceReviewId == performanceReviewId)
            .OrderBy(pg => pg.Priority)
            .ThenBy(pg => pg.TargetDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<PerformanceGoal>> GetByPersonIdAsync(int personId)
    {
        return await _context.PerformanceGoals
            .Include(pg => pg.PerformanceReview)
                .ThenInclude(pr => pr.Person)
            .Include(pg => pg.PerformanceReview)
                .ThenInclude(pr => pr.ReviewPeriod)
            .Where(pg => pg.PerformanceReview.PersonId == personId)
            .OrderByDescending(pg => pg.PerformanceReview.ReviewPeriod.StartDate)
            .ThenBy(pg => pg.Priority)
            .ToListAsync();
    }

    public async Task<IEnumerable<PerformanceGoal>> GetByStatusAsync(GoalStatus status)
    {
        return await _context.PerformanceGoals
            .Include(pg => pg.PerformanceReview)
                .ThenInclude(pr => pr.Person)
            .Include(pg => pg.PerformanceReview)
                .ThenInclude(pr => pr.ReviewPeriod)
            .Where(pg => pg.Status == status)
            .OrderBy(pg => pg.TargetDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<PerformanceGoal>> GetByPriorityAsync(GoalPriority priority)
    {
        return await _context.PerformanceGoals
            .Include(pg => pg.PerformanceReview)
                .ThenInclude(pr => pr.Person)
            .Include(pg => pg.PerformanceReview)
                .ThenInclude(pr => pr.ReviewPeriod)
            .Where(pg => pg.Priority == priority)
            .OrderBy(pg => pg.TargetDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<PerformanceGoal>> GetOverdueGoalsAsync()
    {
        var now = DateTime.Now;
        
        return await _context.PerformanceGoals
            .Include(pg => pg.PerformanceReview)
                .ThenInclude(pr => pr.Person)
            .Include(pg => pg.PerformanceReview)
                .ThenInclude(pr => pr.ReviewPeriod)
            .Where(pg => pg.TargetDate < now && 
                        pg.Status != GoalStatus.Completed && 
                        pg.Status != GoalStatus.Cancelled)
            .OrderBy(pg => pg.TargetDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<PerformanceGoal>> GetUpcomingGoalsAsync(int days = 30)
    {
        var now = DateTime.Now;
        var futureDate = now.AddDays(days);
        
        return await _context.PerformanceGoals
            .Include(pg => pg.PerformanceReview)
                .ThenInclude(pr => pr.Person)
            .Include(pg => pg.PerformanceReview)
                .ThenInclude(pr => pr.ReviewPeriod)
            .Where(pg => pg.TargetDate >= now && 
                        pg.TargetDate <= futureDate && 
                        pg.Status != GoalStatus.Completed && 
                        pg.Status != GoalStatus.Cancelled)
            .OrderBy(pg => pg.TargetDate)
            .ToListAsync();
    }

    public async Task<decimal> GetCompletionRateByPersonAsync(int personId)
    {
        var goals = await _context.PerformanceGoals
            .Include(pg => pg.PerformanceReview)
            .Where(pg => pg.PerformanceReview.PersonId == personId)
            .ToListAsync();

        if (!goals.Any())
            return 0;

        var completedGoals = goals.Count(g => g.Status == GoalStatus.Completed);
        return (decimal)completedGoals / goals.Count * 100;
    }

    public async Task<decimal> GetCompletionRateByReviewAsync(int performanceReviewId)
    {
        var goals = await _context.PerformanceGoals
            .Where(pg => pg.PerformanceReviewId == performanceReviewId)
            .ToListAsync();

        if (!goals.Any())
            return 0;

        var completedGoals = goals.Count(g => g.Status == GoalStatus.Completed);
        return (decimal)completedGoals / goals.Count * 100;
    }
}
