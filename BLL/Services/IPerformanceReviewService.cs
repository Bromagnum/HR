using BLL.DTOs;
using BLL.Utilities;
using DAL.Entities;

namespace BLL.Services;

public interface IPerformanceReviewService
{
    // Performance Review Operations
    Task<Result<IEnumerable<PerformanceReviewListDto>>> GetAllAsync();
    Task<Result<PerformanceReviewDetailDto?>> GetByIdAsync(int id);
    Task<Result<PerformanceReviewDetailDto>> CreateAsync(PerformanceReviewCreateDto dto);
    Task<Result<PerformanceReviewDetailDto>> UpdateAsync(PerformanceReviewUpdateDto dto);
    Task<Result> DeleteAsync(int id);

    // Review Management
    Task<Result<IEnumerable<PerformanceReviewListDto>>> GetByPersonIdAsync(int personId);
    Task<Result<IEnumerable<PerformanceReviewListDto>>> GetByReviewerIdAsync(int reviewerId);
    Task<Result<IEnumerable<PerformanceReviewListDto>>> GetByReviewPeriodIdAsync(int reviewPeriodId);
    Task<Result<IEnumerable<PerformanceReviewListDto>>> GetByStatusAsync(ReviewStatus status);
    Task<Result<IEnumerable<PerformanceReviewListDto>>> GetPendingReviewsAsync();
    Task<Result<IEnumerable<PerformanceReviewListDto>>> GetCompletedReviewsAsync();
    Task<Result<IEnumerable<PerformanceReviewListDto>>> GetByDepartmentIdAsync(int departmentId);

    // Review Status Management
    Task<Result> SubmitReviewAsync(int reviewId);
    Task<Result> ApproveReviewAsync(int reviewId, int approverId);
    Task<Result> RejectReviewAsync(int reviewId, string reason);
    Task<Result> StartEmployeeReviewAsync(int reviewId);
    Task<Result> CompleteEmployeeReviewAsync(int reviewId);

    // Self Assessment
    Task<Result> CompleteSelfAssessmentAsync(SelfAssessmentDto dto);
    Task<Result<PerformanceReviewDetailDto?>> GetSelfAssessmentAsync(int personId, int reviewPeriodId);

    // Analytics and Reporting
    Task<Result<PerformanceAnalyticsDto>> GetPerformanceAnalyticsAsync(int? departmentId = null, int? year = null);
    Task<Result<decimal>> GetAverageScoreByPersonAsync(int personId);
    Task<Result<decimal>> GetAverageScoreByDepartmentAsync(int departmentId);
    Task<Result<IEnumerable<PerformanceReviewListDto>>> GetReviewsForApprovalAsync(int approverId);

    // Review Period Operations
    Task<Result<IEnumerable<ReviewPeriodListDto>>> GetAllPeriodsAsync();
    Task<Result<ReviewPeriodDetailDto?>> GetPeriodByIdAsync(int id);
    Task<Result<ReviewPeriodDetailDto>> CreatePeriodAsync(ReviewPeriodCreateDto dto);
    Task<Result<ReviewPeriodDetailDto>> UpdatePeriodAsync(ReviewPeriodUpdateDto dto);
    Task<Result> DeletePeriodAsync(int id);
    Task<Result<IEnumerable<ReviewPeriodListDto>>> GetActivePeriodsAsync();
    Task<Result<ReviewPeriodDetailDto?>> GetCurrentPeriodAsync();

    // Goal Management
    Task<Result<IEnumerable<PerformanceGoalDto>>> GetGoalsByReviewIdAsync(int performanceReviewId);
    Task<Result<IEnumerable<PerformanceGoalDto>>> GetGoalsByPersonIdAsync(int personId);
    Task<Result<PerformanceGoalDto>> CreateGoalAsync(int performanceReviewId, PerformanceGoalCreateDto dto);
    Task<Result<PerformanceGoalDto>> UpdateGoalAsync(PerformanceGoalUpdateDto dto);
    Task<Result> DeleteGoalAsync(int goalId);
    Task<Result<IEnumerable<PerformanceGoalDto>>> GetOverdueGoalsAsync();
    Task<Result<IEnumerable<PerformanceGoalDto>>> GetUpcomingGoalsAsync(int days = 30);

    // Validation
    Task<Result<bool>> CanCreateReviewAsync(int personId, int reviewPeriodId);
    Task<Result<bool>> CanEditReviewAsync(int reviewId, int userId);
    Task<Result<bool>> CanApproveReviewAsync(int reviewId, int approverId);
}
