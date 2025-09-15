using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class JobPostingRepository : Repository<JobPosting>, IJobPostingRepository
{
    public JobPostingRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<JobPosting>> GetActivePostingsAsync()
    {
        return await _dbSet
            .Include(jp => jp.Position)
            .Include(jp => jp.Department)
            .Include(jp => jp.CreatedBy)
            .Where(jp => jp.Status == JobPostingStatus.Active && 
                        (!jp.ExpiryDate.HasValue || jp.ExpiryDate.Value > DateTime.Now) &&
                        (!jp.LastApplicationDate.HasValue || jp.LastApplicationDate.Value > DateTime.Now))
            .OrderByDescending(jp => jp.PublishDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobPosting>> GetByStatusAsync(JobPostingStatus status)
    {
        return await _dbSet
            .Include(jp => jp.Position)
            .Include(jp => jp.Department)
            .Include(jp => jp.CreatedBy)
            .Where(jp => jp.Status == status)
            .OrderByDescending(jp => jp.PublishDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobPosting>> GetByDepartmentIdAsync(int departmentId)
    {
        return await _dbSet
            .Include(jp => jp.Position)
            .Include(jp => jp.Department)
            .Include(jp => jp.CreatedBy)
            .Where(jp => jp.DepartmentId == departmentId)
            .OrderByDescending(jp => jp.PublishDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobPosting>> GetByPositionIdAsync(int positionId)
    {
        return await _dbSet
            .Include(jp => jp.Position)
            .Include(jp => jp.Department)
            .Include(jp => jp.CreatedBy)
            .Where(jp => jp.PositionId == positionId)
            .OrderByDescending(jp => jp.PublishDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobPosting>> GetExpiringPostingsAsync(int daysAhead = 7)
    {
        var targetDate = DateTime.Now.AddDays(daysAhead);
        return await _dbSet
            .Include(jp => jp.Position)
            .Include(jp => jp.Department)
            .Include(jp => jp.CreatedBy)
            .Where(jp => jp.Status == JobPostingStatus.Active &&
                        jp.ExpiryDate.HasValue &&
                        jp.ExpiryDate.Value <= targetDate &&
                        jp.ExpiryDate.Value > DateTime.Now)
            .OrderBy(jp => jp.ExpiryDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobPosting>> GetRecentPostingsAsync(int count = 10)
    {
        return await _dbSet
            .Include(jp => jp.Position)
            .Include(jp => jp.Department)
            .Include(jp => jp.CreatedBy)
            .Where(jp => jp.Status == JobPostingStatus.Active)
            .OrderByDescending(jp => jp.PublishDate)
            .Take(count)
            .ToListAsync();
    }

    public async Task<JobPosting?> GetBySlugAsync(string slug)
    {
        return await _dbSet
            .Include(jp => jp.Position)
                .ThenInclude(p => p.Department)
            .Include(jp => jp.Department)
            .Include(jp => jp.CreatedBy)
            .Include(jp => jp.Applications)
                .ThenInclude(app => app.Documents)
            .FirstOrDefaultAsync(jp => jp.Slug == slug);
    }

    public async Task<JobPosting?> GetWithApplicationsAsync(int id)
    {
        return await _dbSet
            .Include(jp => jp.Position)
                .ThenInclude(p => p.Department)
            .Include(jp => jp.Department)
            .Include(jp => jp.CreatedBy)
            .Include(jp => jp.UpdatedBy)
            .Include(jp => jp.Applications)
                .ThenInclude(app => app.Documents)
            .FirstOrDefaultAsync(jp => jp.Id == id);
    }

    public async Task<IEnumerable<JobPosting>> GetWithApplicationsAsync()
    {
        return await _dbSet
            .Include(jp => jp.Position)
                .ThenInclude(p => p.Department)
            .Include(jp => jp.Department)
            .Include(jp => jp.CreatedBy)
            .Include(jp => jp.Applications)
            .OrderByDescending(jp => jp.PublishDate)
            .ToListAsync();
    }

    public async Task<bool> IsSlugUniqueAsync(string slug, int? excludeId = null)
    {
        var query = _dbSet.Where(jp => jp.Slug == slug);
        
        if (excludeId.HasValue)
            query = query.Where(jp => jp.Id != excludeId.Value);

        return !await query.AnyAsync();
    }

    public async Task<IEnumerable<JobPosting>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetActivePostingsAsync();

        searchTerm = searchTerm.ToLower();

        return await _dbSet
            .Include(jp => jp.Position)
            .Include(jp => jp.Department)
            .Include(jp => jp.CreatedBy)
            .Where(jp => jp.Status == JobPostingStatus.Active &&
                        (jp.Title.ToLower().Contains(searchTerm) ||
                         jp.Description.ToLower().Contains(searchTerm) ||
                         (jp.Position.Name != null && jp.Position.Name.ToLower().Contains(searchTerm)) ||
                         (jp.Department.Name != null && jp.Department.Name.ToLower().Contains(searchTerm)) ||
                         (jp.Location != null && jp.Location.ToLower().Contains(searchTerm)) ||
                         (jp.Tags != null && jp.Tags.ToLower().Contains(searchTerm))))
            .OrderByDescending(jp => jp.PublishDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobPosting>> GetFilteredAsync(
        JobPostingStatus? status = null,
        int? departmentId = null,
        int? positionId = null,
        EmploymentType? employmentType = null,
        decimal? minSalary = null,
        decimal? maxSalary = null,
        string? location = null,
        string? searchTerm = null,
        bool? isRemoteWork = null)
    {
        var query = _dbSet
            .Include(jp => jp.Position)
                .ThenInclude(p => p.Department)
            .Include(jp => jp.Department)
            .Include(jp => jp.CreatedBy)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(jp => jp.Status == status.Value);

        if (departmentId.HasValue)
            query = query.Where(jp => jp.DepartmentId == departmentId.Value);

        if (positionId.HasValue)
            query = query.Where(jp => jp.PositionId == positionId.Value);

        if (employmentType.HasValue)
            query = query.Where(jp => jp.EmploymentType == employmentType.Value);

        if (minSalary.HasValue)
            query = query.Where(jp => jp.MinSalary >= minSalary.Value);

        if (maxSalary.HasValue)
            query = query.Where(jp => jp.MaxSalary <= maxSalary.Value);

        if (!string.IsNullOrWhiteSpace(location))
            query = query.Where(jp => jp.Location != null && jp.Location.ToLower().Contains(location.ToLower()));

        if (isRemoteWork.HasValue)
            query = query.Where(jp => jp.IsRemoteWork == isRemoteWork.Value);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(jp => 
                jp.Title.ToLower().Contains(searchTerm) ||
                jp.Description.ToLower().Contains(searchTerm) ||
                (jp.Position.Name != null && jp.Position.Name.ToLower().Contains(searchTerm)) ||
                (jp.Department.Name != null && jp.Department.Name.ToLower().Contains(searchTerm)));
        }

        return await query
            .OrderByDescending(jp => jp.PublishDate)
            .ToListAsync();
    }

    public async Task IncrementViewCountAsync(int id)
    {
        var jobPosting = await _dbSet.FindAsync(id);
        if (jobPosting != null)
        {
            jobPosting.ViewCount++;
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateApplicationCountAsync(int id)
    {
        var jobPosting = await _dbSet.FindAsync(id);
        if (jobPosting != null)
        {
            jobPosting.ApplicationCount = await _context.JobApplications
                .CountAsync(app => app.PositionId == jobPosting.PositionId);
            await _context.SaveChangesAsync();
        }
    }

    public override async Task<IEnumerable<JobPosting>> GetAllAsync()
    {
        return await _dbSet
            .Include(jp => jp.Position)
                .ThenInclude(p => p.Department)
            .Include(jp => jp.Department)
            .Include(jp => jp.CreatedBy)
            .OrderByDescending(jp => jp.PublishDate)
            .ToListAsync();
    }

    public override async Task<JobPosting?> GetByIdAsync(int id)
    {
        return await GetWithApplicationsAsync(id);
    }
}
