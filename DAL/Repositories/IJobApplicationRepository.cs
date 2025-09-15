using DAL.Entities;

namespace DAL.Repositories;

public interface IJobApplicationRepository : IRepository<JobApplication>
{
    Task<IEnumerable<JobApplication>> GetByPositionIdAsync(int positionId);
    Task<IEnumerable<JobApplication>> GetByStatusAsync(JobApplicationStatus status);
    Task<IEnumerable<JobApplication>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<JobApplication>> GetPendingApplicationsAsync();
    Task<IEnumerable<JobApplication>> GetRecentApplicationsAsync(int count = 10);
    Task<JobApplication?> GetWithDocumentsAsync(int id);
    Task<IEnumerable<JobApplication>> GetWithDocumentsAsync();
    Task<bool> HasApplicationForPositionAsync(string email, int positionId);
    Task<int> GetApplicationCountByStatusAsync(JobApplicationStatus status);
    Task<int> GetApplicationCountByPositionAsync(int positionId);
    Task<IEnumerable<JobApplication>> SearchAsync(string searchTerm);
    Task<IEnumerable<JobApplication>> GetFilteredAsync(
        JobApplicationStatus? status = null,
        int? positionId = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? searchTerm = null,
        int? reviewedById = null);
}
