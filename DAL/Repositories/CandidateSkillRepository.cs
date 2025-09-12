using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class CandidateSkillRepository : Repository<CandidateSkill>, ICandidateSkillRepository
{
    public CandidateSkillRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CandidateSkill>> GetByCandidateIdAsync(int candidateId)
    {
        return await _context.CandidateSkills
            .Where(cs => cs.CandidateId == candidateId && cs.IsActive)
            .OrderByDescending(cs => cs.Level)
            .ThenBy(cs => cs.SkillName)
            .ToListAsync();
    }

    public async Task<IEnumerable<CandidateSkill>> GetBySkillNameAsync(string skillName)
    {
        return await _context.CandidateSkills
            .Include(cs => cs.Candidate)
            .Where(cs => cs.SkillName.ToLower().Contains(skillName.ToLower()) && cs.IsActive)
            .OrderByDescending(cs => cs.Level)
            .ToListAsync();
    }

    public async Task<IEnumerable<CandidateSkill>> GetByCategoryAsync(SkillCategory category)
    {
        return await _context.CandidateSkills
            .Include(cs => cs.Candidate)
            .Where(cs => cs.Category == category && cs.IsActive)
            .OrderByDescending(cs => cs.Level)
            .ThenBy(cs => cs.SkillName)
            .ToListAsync();
    }

    public async Task<IEnumerable<CandidateSkill>> GetByLevelAsync(SkillLevel level)
    {
        return await _context.CandidateSkills
            .Include(cs => cs.Candidate)
            .Where(cs => cs.Level == level && cs.IsActive)
            .OrderBy(cs => cs.SkillName)
            .ToListAsync();
    }

    public async Task<IEnumerable<CandidateSkill>> GetVerifiedSkillsAsync()
    {
        return await _context.CandidateSkills
            .Include(cs => cs.Candidate)
            .Where(cs => cs.IsVerified && cs.IsActive)
            .OrderByDescending(cs => cs.Level)
            .ThenBy(cs => cs.SkillName)
            .ToListAsync();
    }

    public async Task<IEnumerable<CandidateSkill>> GetSkillsWithCertificationsAsync()
    {
        return await _context.CandidateSkills
            .Include(cs => cs.Candidate)
            .Where(cs => !string.IsNullOrEmpty(cs.CertificationName) && cs.IsActive)
            .OrderByDescending(cs => cs.CertificationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<CandidateSkill>> GetExpiredCertificationsAsync()
    {
        return await _context.CandidateSkills
            .Include(cs => cs.Candidate)
            .Where(cs => cs.CertificationExpiry.HasValue && 
                        cs.CertificationExpiry < DateTime.Now && 
                        cs.IsActive)
            .OrderBy(cs => cs.CertificationExpiry)
            .ToListAsync();
    }

    public async Task<IEnumerable<CandidateSkill>> GetExpiringSoonCertificationsAsync(int days = 30)
    {
        var cutoffDate = DateTime.Now.AddDays(days);
        return await _context.CandidateSkills
            .Include(cs => cs.Candidate)
            .Where(cs => cs.CertificationExpiry.HasValue && 
                        cs.CertificationExpiry <= cutoffDate && 
                        cs.CertificationExpiry > DateTime.Now &&
                        cs.IsActive)
            .OrderBy(cs => cs.CertificationExpiry)
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetPopularSkillsAsync(int count = 20)
    {
        return await _context.CandidateSkills
            .Where(cs => cs.IsActive)
            .GroupBy(cs => cs.SkillName)
            .OrderByDescending(g => g.Count())
            .Take(count)
            .Select(g => g.Key)
            .ToListAsync();
    }

    public async Task<IEnumerable<CandidateSkill>> GetByExperienceRangeAsync(int minYears, int maxYears)
    {
        return await _context.CandidateSkills
            .Include(cs => cs.Candidate)
            .Where(cs => cs.YearsOfExperience >= minYears && 
                        cs.YearsOfExperience <= maxYears && 
                        cs.IsActive)
            .OrderByDescending(cs => cs.YearsOfExperience)
            .ToListAsync();
    }
}
