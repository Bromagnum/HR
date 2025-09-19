using DAL.Context;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class ReviewPeriodRepository : Repository<ReviewPeriod>, IReviewPeriodRepository
{
    public ReviewPeriodRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<ReviewPeriod>> GetAllAsync()
    {
        return await _context.ReviewPeriods
            .Include(rp => rp.PerformanceReviews)
            .OrderByDescending(rp => rp.StartDate)
            .ToListAsync();
    }

    public override async Task<ReviewPeriod?> GetByIdAsync(int id)
    {
        return await _context.ReviewPeriods
            .Include(rp => rp.PerformanceReviews)
                .ThenInclude(pr => pr.Person)
            .FirstOrDefaultAsync(rp => rp.Id == id);
    }

    public async Task<IEnumerable<ReviewPeriod>> GetActivePeriodsAsync()
    {
        return await _context.ReviewPeriods
            .Include(rp => rp.PerformanceReviews)
            .Where(rp => rp.IsActive)
            .OrderByDescending(rp => rp.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<ReviewPeriod>> GetByTypeAsync(ReviewPeriodType type)
    {
        return await _context.ReviewPeriods
            .Include(rp => rp.PerformanceReviews)
            .Where(rp => rp.Type == type)
            .OrderByDescending(rp => rp.StartDate)
            .ToListAsync();
    }

    public async Task<ReviewPeriod?> GetCurrentPeriodAsync()
    {
        var now = DateTime.Now;
        
        return await _context.ReviewPeriods
            .Include(rp => rp.PerformanceReviews)
            .Where(rp => rp.IsActive && 
                        rp.StartDate <= now && 
                        rp.EndDate >= now)
            .OrderByDescending(rp => rp.StartDate)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ReviewPeriod>> GetUpcomingPeriodsAsync()
    {
        var now = DateTime.Now;
        
        return await _context.ReviewPeriods
            .Include(rp => rp.PerformanceReviews)
            .Where(rp => rp.IsActive && rp.StartDate > now)
            .OrderBy(rp => rp.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<ReviewPeriod>> GetPastPeriodsAsync()
    {
        var now = DateTime.Now;
        
        return await _context.ReviewPeriods
            .Include(rp => rp.PerformanceReviews)
            .Where(rp => rp.EndDate < now)
            .OrderByDescending(rp => rp.EndDate)
            .ToListAsync();
    }

    public async Task<bool> IsNameUniqueAsync(string name, int? excludeId = null)
    {
        var query = _context.ReviewPeriods.Where(rp => rp.Name.ToLower() == name.ToLower());
        
        if (excludeId.HasValue)
        {
            query = query.Where(rp => rp.Id != excludeId.Value);
        }
        
        return !await query.AnyAsync();
    }

    public async Task<bool> HasOverlappingPeriodAsync(DateTime startDate, DateTime endDate, int? excludeId = null)
    {
        var query = _context.ReviewPeriods.Where(rp => 
            rp.IsActive && 
            ((rp.StartDate <= startDate && rp.EndDate >= startDate) ||
             (rp.StartDate <= endDate && rp.EndDate >= endDate) ||
             (rp.StartDate >= startDate && rp.EndDate <= endDate)));
        
        if (excludeId.HasValue)
        {
            query = query.Where(rp => rp.Id != excludeId.Value);
        }
        
        return await query.AnyAsync();
    }
}
