using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class PersonSkillRepository : Repository<PersonSkill>, IPersonSkillRepository
{
    public PersonSkillRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PersonSkill>> GetByPersonIdAsync(int personId)
    {
        return await _context.PersonSkills
            .Where(ps => ps.PersonId == personId)
            .Include(ps => ps.SkillTemplate)
            .ToListAsync();
    }

    public async Task<PersonSkill?> GetByPersonAndSkillTemplateAsync(int personId, int skillTemplateId)
    {
        return await _context.PersonSkills
            .FirstOrDefaultAsync(ps => ps.PersonId == personId && ps.SkillTemplateId == skillTemplateId);
    }

    public async Task<IEnumerable<PersonSkill>> GetByPersonAndTypeAsync(int personId, SkillType type)
    {
        return await _context.PersonSkills
            .Where(ps => ps.PersonId == personId && ps.SkillTemplate != null && ps.SkillTemplate.Type == type)
            .Include(ps => ps.SkillTemplate)
            .ToListAsync();
    }

    public async Task<IEnumerable<PersonSkill>> GetBySkillTemplateIdAsync(int skillTemplateId)
    {
        return await _context.PersonSkills
            .Where(ps => ps.SkillTemplateId == skillTemplateId)
            .Include(ps => ps.SkillTemplate)
            .Include(ps => ps.Person)
            .ToListAsync();
    }
}
