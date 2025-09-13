using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class CandidateExperienceRepository : Repository<CandidateExperience>, ICandidateExperienceRepository
{
    public CandidateExperienceRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CandidateExperience>> GetByCandidateIdAsync(int candidateId)
    {
        return await _context.CandidateExperiences
            .Where(ce => ce.CandidateId == candidateId && ce.IsActive)
            .OrderByDescending(ce => ce.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<CandidateExperience>> GetByCompanyNameAsync(string companyName)
    {
        return await _context.CandidateExperiences
            .Include(ce => ce.Candidate)
            .Where(ce => ce.CompanyName.ToLower().Contains(companyName.ToLower()) && ce.IsActive)
            .OrderByDescending(ce => ce.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<CandidateExperience>> GetByJobTitleAsync(string jobTitle)
    {
        return await _context.CandidateExperiences
            .Include(ce => ce.Candidate)
            .Where(ce => ce.JobTitle.ToLower().Contains(jobTitle.ToLower()) && ce.IsActive)
            .OrderByDescending(ce => ce.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<CandidateExperience>> GetCurrentJobsAsync()
    {
        return await _context.CandidateExperiences
            .Include(ce => ce.Candidate)
            .Where(ce => ce.IsCurrentJob && ce.IsActive)
            .OrderByDescending(ce => ce.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<CandidateExperience>> GetByEmploymentTypeAsync(string employmentType)
    {
        return await _context.CandidateExperiences
            .Include(ce => ce.Candidate)
            .Where(ce => ce.EmploymentType != null && 
                        ce.EmploymentType.ToLower().Contains(employmentType.ToLower()) && 
                        ce.IsActive)
            .OrderByDescending(ce => ce.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<CandidateExperience>> GetByDurationRangeAsync(int minMonths, int maxMonths)
    {
        var experiences = await _context.CandidateExperiences
            .Include(ce => ce.Candidate)
            .Where(ce => ce.IsActive)
            .ToListAsync();
            
        return experiences
            .Where(ce => ce.DurationInMonths >= minMonths && ce.DurationInMonths <= maxMonths)
            .OrderByDescending(ce => ce.DurationInMonths)
            .ToList();
    }

    public async Task<IEnumerable<CandidateExperience>> GetBySalaryRangeAsync(decimal minSalary, decimal maxSalary)
    {
        return await _context.CandidateExperiences
            .Include(ce => ce.Candidate)
            .Where(ce => ce.Salary >= minSalary && ce.Salary <= maxSalary && ce.IsActive)
            .OrderByDescending(ce => ce.Salary)
            .ToListAsync();
    }

    public async Task<IEnumerable<CandidateExperience>> GetByTechnologyAsync(string technology)
    {
        return await _context.CandidateExperiences
            .Include(ce => ce.Candidate)
            .Where(ce => ce.TechnologiesUsed != null && 
                        ce.TechnologiesUsed.ToLower().Contains(technology.ToLower()) && 
                        ce.IsActive)
            .OrderByDescending(ce => ce.StartDate)
            .ToListAsync();
    }
}
