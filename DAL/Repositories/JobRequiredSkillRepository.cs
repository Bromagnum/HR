using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class JobRequiredSkillRepository : Repository<JobRequiredSkill>, IJobRequiredSkillRepository
{
    public JobRequiredSkillRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<JobRequiredSkill>> GetByJobDefinitionIdAsync(int jobDefinitionId)
    {
        return await _context.JobRequiredSkills
            .Where(jrs => jrs.JobDefinitionId == jobDefinitionId)
            .Include(jrs => jrs.SkillTemplate)
            .ToListAsync();
    }

    public async Task<JobRequiredSkill?> GetByJobDefinitionAndSkillTemplateAsync(int jobDefinitionId, int skillTemplateId)
    {
        return await _context.JobRequiredSkills
            .FirstOrDefaultAsync(jrs => jrs.JobDefinitionId == jobDefinitionId && jrs.SkillTemplateId == skillTemplateId);
    }

    public async Task<IEnumerable<JobRequiredSkill>> GetBySkillTemplateIdAsync(int skillTemplateId)
    {
        return await _context.JobRequiredSkills
            .Where(jrs => jrs.SkillTemplateId == skillTemplateId)
            .Include(jrs => jrs.SkillTemplate)
            .Include(jrs => jrs.JobDefinition)
            .ToListAsync();
    }

    public async Task DeleteByJobDefinitionIdAsync(int jobDefinitionId)
    {
        var skillsToDelete = await _context.JobRequiredSkills
            .Where(jrs => jrs.JobDefinitionId == jobDefinitionId)
            .ToListAsync();
            
        _context.JobRequiredSkills.RemoveRange(skillsToDelete);
    }
}
