using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class CandidateRepository : Repository<Candidate>, ICandidateRepository
{
    public CandidateRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Candidate?> GetByTcKimlikNoAsync(string tcKimlikNo)
    {
        return await _context.Candidates
            .FirstOrDefaultAsync(c => c.TcKimlikNo == tcKimlikNo && c.IsActive);
    }

    public async Task<Candidate?> GetByEmailAsync(string email)
    {
        return await _context.Candidates
            .FirstOrDefaultAsync(c => c.Email == email && c.IsActive);
    }

    public async Task<IEnumerable<Candidate>> GetByStatusAsync(string status)
    {
        return await _context.Candidates
            .Where(c => c.Status == status && c.IsActive)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Candidate>> SearchAsync(string searchTerm)
    {
        var term = searchTerm.ToLower();
        return await _context.Candidates
            .Where(c => c.IsActive && (
                c.FirstName.ToLower().Contains(term) ||
                c.LastName.ToLower().Contains(term) ||
                c.Email.ToLower().Contains(term) ||
                c.Phone.Contains(term) ||
                c.TcKimlikNo.Contains(term) ||
                (c.CurrentCompany != null && c.CurrentCompany.ToLower().Contains(term)) ||
                (c.CurrentPosition != null && c.CurrentPosition.ToLower().Contains(term))
            ))
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Candidate>> GetWithEducationsAsync()
    {
        return await _context.Candidates
            .Include(c => c.Educations)
            .Where(c => c.IsActive)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Candidate>> GetWithExperiencesAsync()
    {
        return await _context.Candidates
            .Include(c => c.Experiences)
            .Where(c => c.IsActive)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Candidate>> GetWithSkillsAsync()
    {
        return await _context.Candidates
            .Include(c => c.Skills)
            .Where(c => c.IsActive)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Candidate>> GetWithJobApplicationsAsync()
    {
        return await _context.Candidates
            .Include(c => c.JobApplications)
                .ThenInclude(ja => ja.Position)
            .Where(c => c.IsActive)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Candidate?> GetWithAllDetailsAsync(int id)
    {
        return await _context.Candidates
            .Include(c => c.Educations)
            .Include(c => c.Experiences)
            .Include(c => c.Skills)
            .Include(c => c.JobApplications)
                .ThenInclude(ja => ja.Position)
                    .ThenInclude(p => p.Department)
            .Include(c => c.JobApplications)
                .ThenInclude(ja => ja.InterviewNotes)
            .Include(c => c.InterviewNotes)
                .ThenInclude(i => i.Interviewer)
            .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
    }

    public async Task<IEnumerable<Candidate>> GetByExperienceRangeAsync(int minYears, int maxYears)
    {
        return await _context.Candidates
            .Where(c => c.IsActive && 
                       c.ExperienceYears >= minYears && 
                       c.ExperienceYears <= maxYears)
            .OrderByDescending(c => c.ExperienceYears)
            .ToListAsync();
    }

    public async Task<IEnumerable<Candidate>> GetBySkillAsync(string skillName)
    {
        return await _context.Candidates
            .Where(c => c.IsActive && 
                       c.Skills.Any(s => s.SkillName.ToLower().Contains(skillName.ToLower()) && s.IsActive))
            .Include(c => c.Skills.Where(s => s.SkillName.ToLower().Contains(skillName.ToLower()) && s.IsActive))
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Candidate>> GetByLocationAsync(string city)
    {
        return await _context.Candidates
            .Where(c => c.IsActive && c.City != null && c.City.ToLower().Contains(city.ToLower()))
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Candidate>> GetBySalaryRangeAsync(decimal minSalary, decimal maxSalary)
    {
        return await _context.Candidates
            .Where(c => c.IsActive && 
                       c.ExpectedSalary >= minSalary && 
                       c.ExpectedSalary <= maxSalary)
            .OrderByDescending(c => c.ExpectedSalary)
            .ToListAsync();
    }

    public async Task<IEnumerable<Candidate>> GetRecentCandidatesAsync(int days = 30)
    {
        var cutoffDate = DateTime.Now.AddDays(-days);
        return await _context.Candidates
            .Where(c => c.IsActive && c.CreatedAt >= cutoffDate)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Candidate>> GetBlacklistedCandidatesAsync()
    {
        return await _context.Candidates
            .Where(c => c.Status == "Blacklisted")
            .OrderByDescending(c => c.UpdatedAt)
            .ToListAsync();
    }
}
