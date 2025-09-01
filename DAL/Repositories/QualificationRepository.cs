using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class QualificationRepository : Repository<Qualification>, IQualificationRepository
{
    public QualificationRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Qualification>> GetAllAsync()
    {
        return await _context.Qualifications
            .Include(q => q.Person)
                .ThenInclude(p => p.Department)
            .OrderByDescending(q => q.IssueDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Qualification>> GetByPersonIdAsync(int personId)
    {
        return await _context.Qualifications
            .Where(q => q.PersonId == personId && q.IsActive)
            .OrderByDescending(q => q.IssueDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Qualification>> GetExpiringSoonAsync(int days = 30)
    {
        var cutoffDate = DateTime.Now.AddDays(days);
        
        return await _context.Qualifications
            .Include(q => q.Person)
                .ThenInclude(p => p.Department)
            .Where(q => q.IsActive && 
                       q.HasExpiration && 
                       q.ExpirationDate.HasValue &&
                       q.ExpirationDate.Value > DateTime.Now &&
                       q.ExpirationDate.Value <= cutoffDate)
            .OrderBy(q => q.ExpirationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Qualification>> GetExpiredAsync()
    {
        return await _context.Qualifications
            .Include(q => q.Person)
                .ThenInclude(p => p.Department)
            .Where(q => q.IsActive && 
                       q.HasExpiration && 
                       q.ExpirationDate.HasValue &&
                       q.ExpirationDate.Value < DateTime.Now)
            .OrderByDescending(q => q.ExpirationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Qualification>> GetByCategoryAsync(string category)
    {
        return await _context.Qualifications
            .Include(q => q.Person)
                .ThenInclude(p => p.Department)
            .Where(q => q.IsActive && q.Category == category)
            .OrderByDescending(q => q.IssueDate)
            .ToListAsync();
    }

    public async Task<Qualification?> GetQualificationWithPersonAsync(int id)
    {
        return await _context.Qualifications
            .Include(q => q.Person)
                .ThenInclude(p => p.Department)
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    public override async Task<Qualification?> GetByIdAsync(int id)
    {
        return await _context.Qualifications
            .Include(q => q.Person)
                .ThenInclude(p => p.Department)
            .FirstOrDefaultAsync(q => q.Id == id);
    }
}
