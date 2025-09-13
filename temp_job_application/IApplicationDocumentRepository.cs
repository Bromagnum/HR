using DAL.Entities;

namespace DAL.Repositories;

public interface IApplicationDocumentRepository : IRepository<ApplicationDocument>
{
    Task<IEnumerable<ApplicationDocument>> GetByJobApplicationIdAsync(int jobApplicationId);
    Task<IEnumerable<ApplicationDocument>> GetByDocumentTypeAsync(DocumentType documentType);
    Task<IEnumerable<ApplicationDocument>> GetByUploadDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<ApplicationDocument>> GetUnverifiedDocumentsAsync();
    Task<IEnumerable<ApplicationDocument>> GetVerifiedDocumentsAsync();
    Task<IEnumerable<ApplicationDocument>> GetByUploaderIdAsync(int uploaderId);
    Task<IEnumerable<ApplicationDocument>> GetByVerifierIdAsync(int verifierId);
    Task<IEnumerable<ApplicationDocument>> GetLargeFilesAsync(long minSizeBytes);
    Task<IEnumerable<ApplicationDocument>> GetByFileExtensionAsync(string extension);
    Task<long> GetTotalFileSizeAsync();
    Task<int> GetDownloadCountAsync();
    Task<IEnumerable<ApplicationDocument>> GetMostDownloadedAsync(int count = 10);
    Task<IEnumerable<ApplicationDocument>> GetRecentUploadsAsync(int days = 7);
}
