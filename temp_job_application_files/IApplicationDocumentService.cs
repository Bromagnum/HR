using BLL.DTOs;
using BLL.Utilities;
using DAL.Entities;

namespace BLL.Services;

public interface IApplicationDocumentService
{
    // CRUD Operations
    Task<Result<IEnumerable<ApplicationDocumentDto>>> GetAllAsync();
    Task<Result<ApplicationDocumentDto>> GetByIdAsync(int id);
    Task<Result<ApplicationDocumentDto>> CreateAsync(ApplicationDocumentCreateDto dto);
    Task<Result<ApplicationDocumentDto>> UpdateAsync(ApplicationDocumentUpdateDto dto);
    Task<Result<bool>> DeleteAsync(int id);

    // Application Specific Operations
    Task<Result<IEnumerable<ApplicationDocumentDto>>> GetByJobApplicationIdAsync(int jobApplicationId);

    // Filter Operations
    Task<Result<IEnumerable<ApplicationDocumentDto>>> GetByDocumentTypeAsync(DocumentType documentType);
    Task<Result<IEnumerable<ApplicationDocumentDto>>> GetByUploadDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<Result<IEnumerable<ApplicationDocumentDto>>> GetByFileExtensionAsync(string extension);
    Task<Result<IEnumerable<ApplicationDocumentDto>>> GetLargeFilesAsync(long minSizeBytes);

    // Verification Operations
    Task<Result<IEnumerable<ApplicationDocumentDto>>> GetUnverifiedDocumentsAsync();
    Task<Result<IEnumerable<ApplicationDocumentDto>>> GetVerifiedDocumentsAsync();
    Task<Result<ApplicationDocumentDto>> VerifyDocumentAsync(DocumentVerificationDto dto);
    Task<Result<IEnumerable<ApplicationDocumentDto>>> GetByVerifierIdAsync(int verifierId);

    // Upload Operations
    Task<Result<IEnumerable<ApplicationDocumentDto>>> GetByUploaderIdAsync(int uploaderId);
    Task<Result<IEnumerable<ApplicationDocumentDto>>> GetRecentUploadsAsync(int days = 7);

    // Download Operations
    Task<Result<ApplicationDocumentDto>> RecordDownloadAsync(int id, int downloadedById);
    Task<Result<IEnumerable<ApplicationDocumentDto>>> GetMostDownloadedAsync(int count = 10);

    // Statistics
    Task<Result<DocumentStatisticsDto>> GetStatisticsAsync();
    Task<Result<long>> GetTotalFileSizeAsync();
    Task<Result<int>> GetTotalDownloadCountAsync();

    // Validation
    Task<Result<bool>> ValidateDocumentAsync(ApplicationDocumentCreateDto dto);
    Task<Result<bool>> ValidateFileTypeAsync(string fileName, string mimeType);
    Task<Result<bool>> ValidateFileSizeAsync(long fileSizeBytes);

    // File Management
    Task<Result<byte[]>> GetFileContentAsync(int id);
    Task<Result<string>> GetFilePathAsync(int id);
    Task<Result<bool>> FileExistsAsync(int id);
}
