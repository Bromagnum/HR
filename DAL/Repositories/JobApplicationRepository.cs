using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class JobApplicationRepository : Repository<JobApplication>, IJobApplicationRepository
{
    public JobApplicationRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<JobApplication>> GetByPositionIdAsync(int positionId)
    {
        return await _dbSet
            .Include(ja => ja.Position)
            .Include(ja => ja.ReviewedBy)
            .Include(ja => ja.Documents)
            .Where(ja => ja.PositionId == positionId)
            .OrderByDescending(ja => ja.ApplicationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobApplication>> GetByStatusAsync(JobApplicationStatus status)
    {
        return await _dbSet
            .Include(ja => ja.Position)
            .Include(ja => ja.ReviewedBy)
            .Include(ja => ja.Documents)
            .Where(ja => ja.Status == status)
            .OrderByDescending(ja => ja.ApplicationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobApplication>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(ja => ja.Position)
            .Include(ja => ja.ReviewedBy)
            .Include(ja => ja.Documents)
            .Where(ja => ja.ApplicationDate >= startDate && ja.ApplicationDate <= endDate)
            .OrderByDescending(ja => ja.ApplicationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobApplication>> GetPendingApplicationsAsync()
    {
        return await _dbSet
            .Include(ja => ja.Position)
            .Include(ja => ja.Documents)
            .Where(ja => ja.Status == JobApplicationStatus.Submitted)
            .OrderBy(ja => ja.ApplicationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobApplication>> GetRecentApplicationsAsync(int count = 10)
    {
        return await _dbSet
            .Include(ja => ja.Position)
            .Include(ja => ja.Documents)
            .OrderByDescending(ja => ja.ApplicationDate)
            .Take(count)
            .ToListAsync();
    }

    public async Task<JobApplication?> GetWithDocumentsAsync(int id)
    {
        return await _dbSet
            .Include(ja => ja.Position)
                .ThenInclude(p => p.Department)
            .Include(ja => ja.ReviewedBy)
            .Include(ja => ja.Documents)
            .FirstOrDefaultAsync(ja => ja.Id == id);
    }

    public async Task<IEnumerable<JobApplication>> GetWithDocumentsAsync()
    {
        return await _dbSet
            .Include(ja => ja.Position)
                .ThenInclude(p => p.Department)
            .Include(ja => ja.ReviewedBy)
            .Include(ja => ja.Documents)
            .OrderByDescending(ja => ja.ApplicationDate)
            .ToListAsync();
    }

    public async Task<bool> HasApplicationForPositionAsync(string email, int positionId)
    {
        return await _dbSet
            .AnyAsync(ja => ja.Email == email && ja.PositionId == positionId);
    }

    public async Task<int> GetApplicationCountByStatusAsync(JobApplicationStatus status)
    {
        return await _dbSet
            .CountAsync(ja => ja.Status == status);
    }

    public async Task<int> GetApplicationCountByPositionAsync(int positionId)
    {
        return await _dbSet
            .CountAsync(ja => ja.PositionId == positionId);
    }

    public async Task<IEnumerable<JobApplication>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllAsync();

        searchTerm = searchTerm.ToLower();

        return await _dbSet
            .Include(ja => ja.Position)
            .Include(ja => ja.ReviewedBy)
            .Include(ja => ja.Documents)
            .Where(ja => 
                ja.FirstName.ToLower().Contains(searchTerm) ||
                ja.LastName.ToLower().Contains(searchTerm) ||
                ja.Email.ToLower().Contains(searchTerm) ||
                ja.Phone.Contains(searchTerm) ||
                (ja.Position.Name != null && ja.Position.Name.ToLower().Contains(searchTerm)) ||
                (ja.CurrentCompany != null && ja.CurrentCompany.ToLower().Contains(searchTerm)))
            .OrderByDescending(ja => ja.ApplicationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobApplication>> GetFilteredAsync(
        JobApplicationStatus? status = null,
        int? positionId = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? searchTerm = null,
        int? reviewedById = null)
    {
        var query = _dbSet
            .Include(ja => ja.Position)
                .ThenInclude(p => p.Department)
            .Include(ja => ja.ReviewedBy)
            .Include(ja => ja.Documents)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(ja => ja.Status == status.Value);

        if (positionId.HasValue)
            query = query.Where(ja => ja.PositionId == positionId.Value);

        if (startDate.HasValue)
            query = query.Where(ja => ja.ApplicationDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(ja => ja.ApplicationDate <= endDate.Value);

        if (reviewedById.HasValue)
            query = query.Where(ja => ja.ReviewedById == reviewedById.Value);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(ja => 
                ja.FirstName.ToLower().Contains(searchTerm) ||
                ja.LastName.ToLower().Contains(searchTerm) ||
                ja.Email.ToLower().Contains(searchTerm) ||
                ja.Phone.Contains(searchTerm) ||
                (ja.Position.Name != null && ja.Position.Name.ToLower().Contains(searchTerm)));
        }

        return await query
            .OrderByDescending(ja => ja.ApplicationDate)
            .ToListAsync();
    }

    public override async Task<IEnumerable<JobApplication>> GetAllAsync()
    {
        return await _dbSet
            .Include(ja => ja.Position)
                .ThenInclude(p => p.Department)
            .Include(ja => ja.ReviewedBy)
            .Include(ja => ja.Documents)
            .OrderByDescending(ja => ja.ApplicationDate)
            .ToListAsync();
    }

    public override async Task<JobApplication?> GetByIdAsync(int id)
    {
        return await GetWithDocumentsAsync(id);
    }
}
