using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class ApplicationDocumentRepository : Repository<ApplicationDocument>, IApplicationDocumentRepository
{
    public ApplicationDocumentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ApplicationDocument>> GetByJobApplicationIdAsync(int jobApplicationId)
    {
        return await _context.ApplicationDocuments
            .Include(ad => ad.UploadedBy)
            .Include(ad => ad.VerifiedBy)
            .Where(ad => ad.JobApplicationId == jobApplicationId && ad.IsActive)
            .OrderByDescending(ad => ad.UploadDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<ApplicationDocument>> GetByDocumentTypeAsync(DocumentType documentType)
    {
        return await _context.ApplicationDocuments
            .Include(ad => ad.JobApplication)
                .ThenInclude(ja => ja.Candidate)
            .Include(ad => ad.UploadedBy)
            .Where(ad => ad.DocumentType == documentType && ad.IsActive)
            .OrderByDescending(ad => ad.UploadDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<ApplicationDocument>> GetByUploadDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.ApplicationDocuments
            .Include(ad => ad.JobApplication)
                .ThenInclude(ja => ja.Candidate)
            .Include(ad => ad.UploadedBy)
            .Where(ad => ad.UploadDate >= startDate && 
                        ad.UploadDate <= endDate && 
                        ad.IsActive)
            .OrderByDescending(ad => ad.UploadDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<ApplicationDocument>> GetUnverifiedDocumentsAsync()
    {
        return await _context.ApplicationDocuments
            .Include(ad => ad.JobApplication)
                .ThenInclude(ja => ja.Candidate)
            .Include(ad => ad.UploadedBy)
            .Where(ad => !ad.IsVerified && ad.IsActive)
            .OrderBy(ad => ad.UploadDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<ApplicationDocument>> GetVerifiedDocumentsAsync()
    {
        return await _context.ApplicationDocuments
            .Include(ad => ad.JobApplication)
                .ThenInclude(ja => ja.Candidate)
            .Include(ad => ad.VerifiedBy)
            .Where(ad => ad.IsVerified && ad.IsActive)
            .OrderByDescending(ad => ad.VerifiedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<ApplicationDocument>> GetByUploaderIdAsync(int uploaderId)
    {
        return await _context.ApplicationDocuments
            .Include(ad => ad.JobApplication)
                .ThenInclude(ja => ja.Candidate)
            .Where(ad => ad.UploadedById == uploaderId && ad.IsActive)
            .OrderByDescending(ad => ad.UploadDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<ApplicationDocument>> GetByVerifierIdAsync(int verifierId)
    {
        return await _context.ApplicationDocuments
            .Include(ad => ad.JobApplication)
                .ThenInclude(ja => ja.Candidate)
            .Include(ad => ad.UploadedBy)
            .Where(ad => ad.VerifiedById == verifierId && ad.IsActive)
            .OrderByDescending(ad => ad.VerifiedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<ApplicationDocument>> GetLargeFilesAsync(long minSizeBytes)
    {
        return await _context.ApplicationDocuments
            .Include(ad => ad.JobApplication)
                .ThenInclude(ja => ja.Candidate)
            .Where(ad => ad.FileSizeBytes >= minSizeBytes && ad.IsActive)
            .OrderByDescending(ad => ad.FileSizeBytes)
            .ToListAsync();
    }

    public async Task<IEnumerable<ApplicationDocument>> GetByFileExtensionAsync(string extension)
    {
        var ext = extension.StartsWith(".") ? extension : "." + extension;
        return await _context.ApplicationDocuments
            .Include(ad => ad.JobApplication)
                .ThenInclude(ja => ja.Candidate)
            .Where(ad => ad.FileName.ToLower().EndsWith(ext.ToLower()) && ad.IsActive)
            .OrderByDescending(ad => ad.UploadDate)
            .ToListAsync();
    }

    public async Task<long> GetTotalFileSizeAsync()
    {
        return await _context.ApplicationDocuments
            .Where(ad => ad.IsActive)
            .SumAsync(ad => ad.FileSizeBytes);
    }

    public async Task<int> GetDownloadCountAsync()
    {
        return await _context.ApplicationDocuments
            .Where(ad => ad.IsActive)
            .SumAsync(ad => ad.DownloadCount);
    }

    public async Task<IEnumerable<ApplicationDocument>> GetMostDownloadedAsync(int count = 10)
    {
        return await _context.ApplicationDocuments
            .Include(ad => ad.JobApplication)
                .ThenInclude(ja => ja.Candidate)
            .Where(ad => ad.IsActive)
            .OrderByDescending(ad => ad.DownloadCount)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<ApplicationDocument>> GetRecentUploadsAsync(int days = 7)
    {
        var cutoffDate = DateTime.Now.AddDays(-days);
        return await _context.ApplicationDocuments
            .Include(ad => ad.JobApplication)
                .ThenInclude(ja => ja.Candidate)
            .Include(ad => ad.UploadedBy)
            .Where(ad => ad.UploadDate >= cutoffDate && ad.IsActive)
            .OrderByDescending(ad => ad.UploadDate)
            .ToListAsync();
    }
}
