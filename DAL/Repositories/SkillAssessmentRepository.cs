using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class SkillAssessmentRepository : Repository<SkillAssessment>, ISkillAssessmentRepository
{
    public SkillAssessmentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<SkillAssessment>> GetByPersonSkillIdAsync(int personSkillId)
    {
        return await _context.SkillAssessments
            .Where(sa => sa.PersonSkillId == personSkillId)
            .Include(sa => sa.PersonSkill)
            .ToListAsync();
    }

    public async Task<IEnumerable<SkillAssessment>> GetByPersonIdAsync(int personId)
    {
        return await _context.SkillAssessments
            .Where(sa => sa.PersonSkill != null && sa.PersonSkill.PersonId == personId)
            .Include(sa => sa.PersonSkill)
            .ThenInclude(ps => ps.SkillTemplate)
            .ToListAsync();
    }

    public async Task<IEnumerable<SkillAssessment>> GetByAssessorIdAsync(int assessorId)
    {
        return await _context.SkillAssessments
            .Where(sa => sa.AssessorId == assessorId)
            .Include(sa => sa.PersonSkill)
            .ThenInclude(ps => ps.SkillTemplate)
            .ToListAsync();
    }

    public async Task<IEnumerable<SkillAssessment>> GetByTypeAsync(AssessmentType type)
    {
        return await _context.SkillAssessments
            .Where(sa => sa.Type == type)
            .Include(sa => sa.PersonSkill)
            .ThenInclude(ps => ps.SkillTemplate)
            .ToListAsync();
    }

    public async Task<IEnumerable<SkillAssessment>> GetValidAssessmentsAsync()
    {
        return await _context.SkillAssessments
            .Where(sa => sa.IsValid && (!sa.ValidUntil.HasValue || sa.ValidUntil > DateTime.Now))
            .Include(sa => sa.PersonSkill)
            .ThenInclude(ps => ps.SkillTemplate)
            .ToListAsync();
    }

    public async Task<IEnumerable<SkillAssessment>> GetExpiredAssessmentsAsync()
    {
        return await _context.SkillAssessments
            .Where(sa => sa.ValidUntil.HasValue && sa.ValidUntil <= DateTime.Now)
            .Include(sa => sa.PersonSkill)
            .ThenInclude(ps => ps.SkillTemplate)
            .ToListAsync();
    }

    public async Task<SkillAssessment?> GetLatestAssessmentAsync(int personSkillId)
    {
        return await _context.SkillAssessments
            .Where(sa => sa.PersonSkillId == personSkillId)
            .OrderByDescending(sa => sa.AssessmentDate)
            .Include(sa => sa.PersonSkill)
            .ThenInclude(ps => ps.SkillTemplate)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<SkillAssessment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.SkillAssessments
            .Where(sa => sa.AssessmentDate >= startDate && sa.AssessmentDate <= endDate)
            .Include(sa => sa.PersonSkill)
            .ThenInclude(ps => ps.SkillTemplate)
            .ToListAsync();
    }

    public async Task<IEnumerable<SkillAssessment>> GetRecentAssessmentsAsync(int count = 10)
    {
        return await _context.SkillAssessments
            .OrderByDescending(sa => sa.AssessmentDate)
            .Take(count)
            .Include(sa => sa.PersonSkill)
            .ThenInclude(ps => ps.SkillTemplate)
            .ToListAsync();
    }
}
