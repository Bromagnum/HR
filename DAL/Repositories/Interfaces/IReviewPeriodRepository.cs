using DAL.Entities;

namespace DAL.Repositories.Interfaces;

public interface IReviewPeriodRepository : IRepository<ReviewPeriod>
{
    Task<IEnumerable<ReviewPeriod>> GetActivePeriodsAsync();
    Task<IEnumerable<ReviewPeriod>> GetByTypeAsync(ReviewPeriodType type);
    Task<ReviewPeriod?> GetCurrentPeriodAsync();
    Task<IEnumerable<ReviewPeriod>> GetUpcomingPeriodsAsync();
    Task<IEnumerable<ReviewPeriod>> GetPastPeriodsAsync();
    Task<bool> IsNameUniqueAsync(string name, int? excludeId = null);
    Task<bool> HasOverlappingPeriodAsync(DateTime startDate, DateTime endDate, int? excludeId = null);
}
