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

    public async Task<IEnumerable<SkillAssessment>> GetByTypeAsync(AssessmentType type)
    {
        return await _context.SkillAssessments
            .Where(sa => sa.Type == type)
            .Include(sa => sa.PersonSkill)
            .ThenInclude(ps => ps.SkillTemplate)
            .ToListAsync();
    }
}
