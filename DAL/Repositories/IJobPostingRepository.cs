using DAL.Entities;

namespace DAL.Repositories;

public interface IJobPostingRepository : IRepository<JobPosting>
{
    Task<IEnumerable<JobPosting>> GetActivePostingsAsync();
    Task<IEnumerable<JobPosting>> GetByStatusAsync(JobPostingStatus status);
    Task<IEnumerable<JobPosting>> GetByDepartmentIdAsync(int departmentId);
    Task<IEnumerable<JobPosting>> GetByPositionIdAsync(int positionId);
    Task<IEnumerable<JobPosting>> GetExpiringPostingsAsync(int daysAhead = 7);
    Task<IEnumerable<JobPosting>> GetRecentPostingsAsync(int count = 10);
    Task<JobPosting?> GetBySlugAsync(string slug);
    Task<JobPosting?> GetWithApplicationsAsync(int id);
    Task<IEnumerable<JobPosting>> GetWithApplicationsAsync();
    Task<bool> IsSlugUniqueAsync(string slug, int? excludeId = null);
    Task<IEnumerable<JobPosting>> SearchAsync(string searchTerm);
    Task<IEnumerable<JobPosting>> GetFilteredAsync(
        JobPostingStatus? status = null,
        int? departmentId = null,
        int? positionId = null,
        EmploymentType? employmentType = null,
        decimal? minSalary = null,
        decimal? maxSalary = null,
        string? location = null,
        string? searchTerm = null,
        bool? isRemoteWork = null);
    Task IncrementViewCountAsync(int id);
    Task UpdateApplicationCountAsync(int id);
}
