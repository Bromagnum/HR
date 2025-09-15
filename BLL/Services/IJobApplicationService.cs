using BLL.DTOs;
using BLL.Utilities;
using DAL.Entities;

namespace BLL.Services;

public interface IJobApplicationService
{
    // CRUD Operations
    Task<Result<IEnumerable<JobApplicationListDto>>> GetAllAsync();
    Task<Result<JobApplicationDetailDto?>> GetByIdAsync(int id);
    Task<Result<JobApplicationDetailDto>> CreateAsync(JobApplicationCreateDto dto);
    Task<Result<JobApplicationDetailDto>> UpdateAsync(JobApplicationUpdateDto dto);
    Task<Result<bool>> DeleteAsync(int id);
    Task<Result<bool>> SoftDeleteAsync(int id);

    // Filtering & Search
    Task<Result<IEnumerable<JobApplicationListDto>>> GetFilteredAsync(JobApplicationFilterDto filter);
    Task<Result<IEnumerable<JobApplicationListDto>>> SearchAsync(string searchTerm);
    Task<Result<IEnumerable<JobApplicationListDto>>> GetByStatusAsync(JobApplicationStatus status);
    Task<Result<IEnumerable<JobApplicationListDto>>> GetByPositionIdAsync(int positionId);
    Task<Result<IEnumerable<JobApplicationListDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<Result<IEnumerable<JobApplicationListDto>>> GetPendingApplicationsAsync();
    Task<Result<IEnumerable<JobApplicationListDto>>> GetRecentApplicationsAsync(int count = 10);

    // Status Management
    Task<Result<bool>> UpdateStatusAsync(int id, JobApplicationStatus status, int reviewedById, string? notes = null);
    Task<Result<bool>> ChangeStatusAsync(int id, JobApplicationStatus status, int reviewedById, string? notes = null);
    Task<Result<bool>> ApproveApplicationAsync(int id, int reviewedById, string? notes = null);
    Task<Result<bool>> RejectApplicationAsync(int id, int reviewedById, string? notes = null);
    Task<Result<bool>> ScheduleInterviewAsync(int id, DateTime interviewDate, int reviewedById, string? notes = null);
    Task<Result<bool>> SetRatingAsync(int id, int rating, int reviewedById, string? notes = null);

    // Document Management
    Task<Result<IEnumerable<ApplicationDocumentDto>>> GetApplicationDocumentsAsync(int applicationId);
    Task<Result<bool>> HasRequiredDocumentsAsync(int applicationId);
    Task<Result<bool>> VerifyDocumentAsync(int documentId, int verifiedById, string? notes = null);

    // Validation & Business Rules
    Task<Result<bool>> CanApplyForPositionAsync(string email, int positionId);
    Task<Result<bool>> HasApplicationForPositionAsync(string email, int positionId);
    Task<Result<bool>> IsPositionStillOpenAsync(int positionId);

    // Statistics & Reports
    Task<Result<JobApplicationSummaryDto>> GetApplicationSummaryAsync();
    Task<Result<JobApplicationSummaryDto>> GetSummaryAsync();
    Task<Result<JobApplicationSummaryDto>> GetApplicationSummaryByPeriodAsync(DateTime startDate, DateTime endDate);
    Task<Result<IEnumerable<PositionApplicationSummaryDto>>> GetApplicationSummaryByPositionAsync();
    Task<Result<IEnumerable<PositionApplicationSummaryDto>>> GetPositionApplicationSummaryAsync();
    Task<Result<IEnumerable<MonthlyApplicationSummaryDto>>> GetMonthlyApplicationSummaryAsync(int year);
    Task<Result<int>> GetApplicationCountByStatusAsync(JobApplicationStatus status);
    Task<Result<int>> GetApplicationCountByPositionAsync(int positionId);

    // Export Operations
    Task<Result<byte[]>> ExportApplicationsAsync(JobApplicationFilterDto? filter = null);
    Task<Result<byte[]>> ExportApplicationSummaryAsync();

    // Bulk Operations
    Task<Result<bool>> BulkUpdateStatusAsync(IEnumerable<int> applicationIds, JobApplicationStatus status, int reviewedById);
    Task<Result<bool>> BulkDeleteAsync(IEnumerable<int> applicationIds);

    // Email Integration (Future)
    Task<Result<bool>> SendApplicationConfirmationEmailAsync(int applicationId);
    Task<Result<bool>> SendStatusUpdateEmailAsync(int applicationId);
    Task<Result<bool>> SendInterviewInvitationEmailAsync(int applicationId);

    // Dashboard Support
    Task<Result<object>> GetDashboardStatisticsAsync();
    Task<Result<IEnumerable<JobApplicationListDto>>> GetApplicationsRequiringAttentionAsync();
    Task<Result<IEnumerable<JobApplicationListDto>>> GetUpcomingInterviewsAsync(int daysAhead = 7);
}
