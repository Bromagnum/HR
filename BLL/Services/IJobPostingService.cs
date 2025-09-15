using BLL.DTOs;
using BLL.Utilities;
using DAL.Entities;

namespace BLL.Services;

public interface IJobPostingService
{
    // CRUD Operations
    Task<Result<IEnumerable<JobPostingListDto>>> GetAllAsync();
    Task<Result<JobPostingDetailDto?>> GetByIdAsync(int id);
    Task<Result<JobPostingDetailDto?>> GetBySlugAsync(string slug);
    Task<Result<JobPostingDetailDto>> CreateAsync(JobPostingCreateDto dto);
    Task<Result<JobPostingDetailDto>> UpdateAsync(JobPostingUpdateDto dto);
    Task<Result<bool>> DeleteAsync(int id);
    Task<Result<bool>> SoftDeleteAsync(int id);

    // Public Job Listings
    Task<Result<IEnumerable<PublicJobPostingDto>>> GetActivePostingsAsync();
    Task<Result<IEnumerable<PublicJobPostingDto>>> GetPublicListingsAsync(PublicJobPostingFilterDto? filter = null);
    Task<Result<IEnumerable<PublicJobPostingDto>>> GetPublicPostingsAsync(JobPostingFilterDto? filter = null);
    Task<Result<PublicJobPostingDto?>> GetPublicDetailsAsync(int id);
    Task<Result<PublicJobPostingDto?>> GetPublicPostingBySlugAsync(string slug);
    Task<Result<IEnumerable<PublicJobPostingDto>>> SearchPublicPostingsAsync(string searchTerm);

    // Filtering & Search
    Task<Result<IEnumerable<JobPostingListDto>>> GetFilteredAsync(JobPostingFilterDto filter);
    Task<Result<IEnumerable<JobPostingListDto>>> SearchAsync(string searchTerm);
    Task<Result<IEnumerable<JobPostingListDto>>> GetByStatusAsync(JobPostingStatus status);
    Task<Result<IEnumerable<JobPostingListDto>>> GetByDepartmentIdAsync(int departmentId);
    Task<Result<IEnumerable<JobPostingListDto>>> GetByPositionIdAsync(int positionId);
    Task<Result<IEnumerable<JobPostingListDto>>> GetExpiringPostingsAsync(int daysAhead = 7);
    Task<Result<IEnumerable<JobPostingListDto>>> GetRecentPostingsAsync(int count = 10);

    // Status Management
    Task<Result<bool>> PublishAsync(int id, int publishedById);
    Task<Result<bool>> PublishPostingAsync(int id, int publishedById);
    Task<Result<bool>> SuspendAsync(int id, int suspendedById);
    Task<Result<bool>> PausePostingAsync(int id, int pausedById);
    Task<Result<bool>> CloseAsync(int id, int closedById);
    Task<Result<bool>> ClosePostingAsync(int id, int closedById);
    Task<Result<bool>> MarkAsFilledAsync(int id, int filledById);
    Task<Result<bool>> UpdateStatusAsync(int id, JobPostingStatus status, int updatedById);

    // SEO & URL Management
    Task<Result<string>> GenerateSlugAsync(string title, int? excludeId = null);
    Task<Result<bool>> IsSlugUniqueAsync(string slug, int? excludeId = null);
    Task<Result<bool>> UpdateSlugAsync(int id, string slug);

    // View & Application Tracking
    Task<Result<bool>> IncrementViewCountAsync(int id);
    Task<Result<bool>> UpdateApplicationCountAsync(int id);
    Task<Result<JobPostingDetailDto?>> GetWithApplicationsAsync(int id);

    // Validation & Business Rules
    Task<Result<bool>> CanPublishPostingAsync(int id);
    Task<Result<bool>> IsPostingActiveAsync(int id);
    Task<Result<bool>> IsApplicationDeadlinePassedAsync(int id);
    Task<Result<bool>> HasActivePostingForPositionAsync(int positionId);

    // Statistics & Reports
    Task<Result<JobPostingSummaryDto>> GetPostingSummaryAsync();
    Task<Result<JobPostingSummaryDto>> GetSummaryAsync();
    Task<Result<JobPostingSummaryDto>> GetPostingSummaryByPeriodAsync(DateTime startDate, DateTime endDate);
    Task<Result<IEnumerable<DepartmentPostingSummaryDto>>> GetPostingSummaryByDepartmentAsync();
    Task<Result<IEnumerable<DepartmentPostingSummaryDto>>> GetDepartmentPostingSummaryAsync();
    Task<Result<IEnumerable<MonthlyPostingSummaryDto>>> GetMonthlyPostingSummaryAsync(int year);
    Task<Result<int>> GetPostingCountByStatusAsync(JobPostingStatus status);
    Task<Result<int>> GetPostingCountByDepartmentAsync(int departmentId);

    // Performance Analytics
    Task<Result<object>> GetPostingPerformanceAsync(int id);
    Task<Result<IEnumerable<object>>> GetTopPerformingPostingsAsync(int count = 10);
    Task<Result<IEnumerable<object>>> GetPoorPerformingPostingsAsync(int count = 10);

    // Export Operations
    Task<Result<byte[]>> ExportPostingsAsync(JobPostingFilterDto? filter = null);
    Task<Result<byte[]>> ExportPostingSummaryAsync();
    Task<Result<byte[]>> ExportApplicationsByPostingAsync(int postingId);

    // Bulk Operations
    Task<Result<bool>> BulkUpdateStatusAsync(IEnumerable<int> postingIds, JobPostingStatus status, int updatedById);
    Task<Result<bool>> BulkDeleteAsync(IEnumerable<int> postingIds);
    Task<Result<bool>> BulkExtendExpiryDateAsync(IEnumerable<int> postingIds, DateTime newExpiryDate);

    // Template & Cloning
    Task<Result<JobPostingCreateDto>> ClonePostingAsync(int id);
    Task<Result<JobPostingCreateDto>> CreateFromTemplateAsync(int templateId);

    // Notification & Alerts
    Task<Result<bool>> SendExpiryWarningAsync(int id);
    Task<Result<IEnumerable<JobPostingListDto>>> GetPostingsRequiringAttentionAsync();

    // Dashboard Support
    Task<Result<object>> GetDashboardStatisticsAsync();
    Task<Result<IEnumerable<JobPostingListDto>>> GetTrendingPostingsAsync(int count = 5);

    // Integration Support
    Task<Result<string>> GenerateJobPostingXmlAsync(int id);
    Task<Result<string>> GenerateJobPostingJsonAsync(int id);
    Task<Result<bool>> SyncWithExternalJobBoardsAsync(int id);
}
