using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class EducationRepository : Repository<Education>, IEducationRepository
{
    public EducationRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Education>> GetAllAsync()
    {
        return await _context.Educations
            .Include(e => e.Person)
                .ThenInclude(p => p.Department)
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Education>> GetByPersonIdAsync(int personId)
    {
        return await _context.Educations
            .Where(e => e.PersonId == personId && e.IsActive)
            .Include(e => e.Person)
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Education>> GetOngoingEducationsAsync()
    {
        return await _context.Educations
            .Where(e => e.IsOngoing && e.IsActive)
            .Include(e => e.Person)
            .OrderBy(e => e.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Education>> GetCompletedEducationsAsync()
    {
        return await _context.Educations
            .Where(e => !e.IsOngoing && e.EndDate.HasValue && e.IsActive)
            .Include(e => e.Person)
            .OrderByDescending(e => e.EndDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Education>> GetEducationsByDegreeAsync(string degree)
    {
        return await _context.Educations
            .Where(e => e.Degree.ToLower().Contains(degree.ToLower()) && e.IsActive)
            .Include(e => e.Person)
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
    }

    public async Task<Education?> GetEducationWithPersonAsync(int educationId)
    {
        return await _context.Educations
            .Include(e => e.Person)
            .ThenInclude(p => p.Department)
            .FirstOrDefaultAsync(e => e.Id == educationId && e.IsActive);
    }
}

