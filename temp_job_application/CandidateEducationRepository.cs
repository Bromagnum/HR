using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class CandidateEducationRepository : Repository<CandidateEducation>, ICandidateEducationRepository
{
    public CandidateEducationRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CandidateEducation>> GetByCandidateIdAsync(int candidateId)
    {
        return await _context.CandidateEducations
            .Where(ce => ce.CandidateId == candidateId && ce.IsActive)
            .OrderByDescending(ce => ce.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<CandidateEducation>> GetByDegreeAsync(string degree)
    {
        return await _context.CandidateEducations
            .Include(ce => ce.Candidate)
            .Where(ce => ce.Degree.ToLower().Contains(degree.ToLower()) && ce.IsActive)
            .OrderByDescending(ce => ce.EndDate ?? DateTime.MaxValue)
            .ToListAsync();
    }

    public async Task<IEnumerable<CandidateEducation>> GetByFieldOfStudyAsync(string fieldOfStudy)
    {
        return await _context.CandidateEducations
            .Include(ce => ce.Candidate)
            .Where(ce => ce.FieldOfStudy.ToLower().Contains(fieldOfStudy.ToLower()) && ce.IsActive)
            .OrderByDescending(ce => ce.EndDate ?? DateTime.MaxValue)
            .ToListAsync();
    }

    public async Task<IEnumerable<CandidateEducation>> GetBySchoolNameAsync(string schoolName)
    {
        return await _context.CandidateEducations
            .Include(ce => ce.Candidate)
            .Where(ce => ce.SchoolName.ToLower().Contains(schoolName.ToLower()) && ce.IsActive)
            .OrderByDescending(ce => ce.EndDate ?? DateTime.MaxValue)
            .ToListAsync();
    }

    public async Task<IEnumerable<CandidateEducation>> GetOngoingEducationsAsync()
    {
        return await _context.CandidateEducations
            .Include(ce => ce.Candidate)
            .Where(ce => ce.IsOngoing && ce.IsActive)
            .OrderByDescending(ce => ce.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<CandidateEducation>> GetByGpaRangeAsync(decimal minGpa, decimal maxGpa)
    {
        return await _context.CandidateEducations
            .Include(ce => ce.Candidate)
            .Where(ce => ce.GPA >= minGpa && ce.GPA <= maxGpa && ce.IsActive)
            .OrderByDescending(ce => ce.GPA)
            .ToListAsync();
    }

    public async Task<IEnumerable<CandidateEducation>> GetRecentGraduatesAsync(int years = 2)
    {
        var cutoffDate = DateTime.Now.AddYears(-years);
        return await _context.CandidateEducations
            .Include(ce => ce.Candidate)
            .Where(ce => ce.EndDate >= cutoffDate && !ce.IsOngoing && ce.IsActive)
            .OrderByDescending(ce => ce.EndDate)
            .ToListAsync();
    }
}
