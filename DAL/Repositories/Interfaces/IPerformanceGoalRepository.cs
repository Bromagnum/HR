using DAL.Entities;

namespace DAL.Repositories.Interfaces;

public interface IPerformanceGoalRepository : IRepository<PerformanceGoal>
{
    Task<IEnumerable<PerformanceGoal>> GetByPerformanceReviewIdAsync(int performanceReviewId);
    Task<IEnumerable<PerformanceGoal>> GetByPersonIdAsync(int personId);
    Task<IEnumerable<PerformanceGoal>> GetByStatusAsync(GoalStatus status);
    Task<IEnumerable<PerformanceGoal>> GetByPriorityAsync(GoalPriority priority);
    Task<IEnumerable<PerformanceGoal>> GetOverdueGoalsAsync();
    Task<IEnumerable<PerformanceGoal>> GetUpcomingGoalsAsync(int days = 30);
    Task<decimal> GetCompletionRateByPersonAsync(int personId);
    Task<decimal> GetCompletionRateByReviewAsync(int performanceReviewId);
}
