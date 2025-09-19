using DAL.Entities;

namespace DAL.Repositories.Interfaces;

public interface IPerformanceReviewRepository : IRepository<PerformanceReview>
{
    Task<IEnumerable<PerformanceReview>> GetByPersonIdAsync(int personId);
    Task<IEnumerable<PerformanceReview>> GetByReviewerIdAsync(int reviewerId);
    Task<IEnumerable<PerformanceReview>> GetByReviewPeriodIdAsync(int reviewPeriodId);
    Task<IEnumerable<PerformanceReview>> GetByStatusAsync(ReviewStatus status);
    Task<PerformanceReview?> GetByPersonAndPeriodAsync(int personId, int reviewPeriodId);
    Task<IEnumerable<PerformanceReview>> GetPendingReviewsAsync();
    Task<IEnumerable<PerformanceReview>> GetCompletedReviewsAsync();
    Task<IEnumerable<PerformanceReview>> GetByDepartmentIdAsync(int departmentId);
    Task<decimal> GetAverageScoreByPersonAsync(int personId);
    Task<decimal> GetAverageScoreByDepartmentAsync(int departmentId);
    Task<IEnumerable<PerformanceReview>> GetReviewsForApprovalAsync(int approverId);
}
