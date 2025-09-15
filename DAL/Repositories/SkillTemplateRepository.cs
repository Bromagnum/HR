using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class SkillTemplateRepository : Repository<SkillTemplate>, ISkillTemplateRepository
{
    public SkillTemplateRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<SkillTemplate>> GetByCategoryAsync(string category)
    {
        return await _context.SkillTemplates
            .Where(x => x.Category == category && x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<SkillTemplate>> GetByTypeAsync(SkillType type)
    {
        return await _context.SkillTemplates
            .Where(x => x.Type == type && x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<SkillTemplate>> GetVerifiableAsync()
    {
        return await _context.SkillTemplates
            .Where(x => x.IsVerifiable && x.IsActive)
            .OrderBy(x => x.Category)
            .ThenBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<SkillTemplate>> GetRequiringCertificationAsync()
    {
        return await _context.SkillTemplates
            .Where(x => x.RequiresCertification && x.IsActive)
            .OrderBy(x => x.Category)
            .ThenBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<SkillTemplate>> SearchAsync(string searchTerm)
    {
        return await _context.SkillTemplates
            .Where(x => x.IsActive && (
                x.Name.Contains(searchTerm) ||
                x.Description!.Contains(searchTerm) ||
                x.Keywords!.Contains(searchTerm) ||
                x.Category.Contains(searchTerm)))
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<SkillTemplate?> GetByNameAsync(string name)
    {
        return await _context.SkillTemplates
            .FirstOrDefaultAsync(x => x.Name == name && x.IsActive);
    }

    public async Task<IEnumerable<SkillTemplate>> GetMostUsedAsync(int count = 10)
    {
        return await _context.SkillTemplates
            .Where(x => x.IsActive)
            .OrderByDescending(x => x.UsageCount)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetCategoriesAsync()
    {
        return await _context.SkillTemplates
            .Where(x => x.IsActive)
            .Select(x => x.Category)
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync();
    }

    public async Task IncrementUsageCountAsync(int id)
    {
        var skill = await _context.SkillTemplates.FindAsync(id);
        if (skill != null)
        {
            skill.UsageCount++;
            skill.LastUsedAt = DateTime.Now;
        }
    }
}
