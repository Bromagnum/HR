using BLL.DTOs;
using BLL.Utilities;
using DAL.Entities;

namespace BLL.Services;

public interface IJobApplicationService
{
    // CRUD Operations
    Task<Result<IEnumerable<JobApplicationListDto>>> GetAllAsync();
    Task<Result<IEnumerable<JobApplicationListDto>>> GetFilteredAsync(JobApplicationFilterDto filter);
    Task<Result<JobApplicationDetailDto>> GetByIdAsync(int id);
    Task<Result<JobApplicationDetailDto>> CreateAsync(JobApplicationCreateDto dto);
    Task<Result<JobApplicationDetailDto>> UpdateAsync(JobApplicationUpdateDto dto);
    Task<Result<bool>> DeleteAsync(int id);

    // Status Management
    Task<Result<JobApplicationDetailDto>> ReviewApplicationAsync(JobApplicationReviewDto dto);
    Task<Result<JobApplicationDetailDto>> ScheduleInterviewAsync(JobApplicationInterviewDto dto);
    Task<Result<JobApplicationDetailDto>> MakeDecisionAsync(JobApplicationDecisionDto dto);
    Task<Result<bool>> ChangeStatusAsync(int id, ApplicationStatus status, int userId);

    // Filter Operations
    Task<Result<IEnumerable<JobApplicationListDto>>> GetByStatusAsync(ApplicationStatus status);
    Task<Result<IEnumerable<JobApplicationListDto>>> GetByCandidateIdAsync(int candidateId);
    Task<Result<IEnumerable<JobApplicationListDto>>> GetByPositionIdAsync(int positionId);
    Task<Result<IEnumerable<JobApplicationListDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

    // Workflow Operations
    Task<Result<IEnumerable<JobApplicationListDto>>> GetPendingApplicationsAsync();
    Task<Result<IEnumerable<JobApplicationListDto>>> GetApplicationsForReviewAsync();
    Task<Result<IEnumerable<JobApplicationListDto>>> GetApplicationsForInterviewAsync();
    Task<Result<IEnumerable<JobApplicationListDto>>> GetExpiredOffersAsync();
    Task<Result<IEnumerable<JobApplicationListDto>>> GetLongPendingApplicationsAsync(int days = 30);

    // Manager Operations
    Task<Result<IEnumerable<JobApplicationListDto>>> GetApplicationsByReviewerAsync(int reviewerId);
    Task<Result<IEnumerable<JobApplicationListDto>>> GetApplicationsByInterviewerAsync(int interviewerId);

    // Detailed Operations
    Task<Result<JobApplicationDetailDto>> GetWithAllDetailsAsync(int id);
    Task<Result<IEnumerable<JobApplicationListDto>>> GetTopScoredApplicationsAsync(int count = 10);

    // Validation
    Task<Result<bool>> ValidateApplicationAsync(JobApplicationCreateDto dto);
    Task<Result<bool>> ValidateApplicationAsync(JobApplicationUpdateDto dto);
    Task<Result<bool>> CanCandidateApplyToPositionAsync(int candidateId, int positionId);
    Task<Result<bool>> HasDuplicateApplicationAsync(int candidateId, int positionId);

    // Statistics
    Task<Result<JobApplicationStatisticsDto>> GetStatisticsAsync();
    Task<Result<Dictionary<string, int>>> GetApplicationsByMonthAsync(int year);
    Task<Result<Dictionary<string, int>>> GetApplicationsBySourceAsync();
    Task<Result<List<TopPositionDto>>> GetTopPositionsAsync(int count = 10);

    // Reporting
    Task<Result<int>> GetApplicationCountByStatusAsync(ApplicationStatus status);
    Task<Result<decimal>> GetAverageProcessingTimeAsync();
    Task<Result<decimal>> GetOfferAcceptanceRateAsync();
}
