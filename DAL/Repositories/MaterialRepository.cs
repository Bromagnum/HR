using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class MaterialRepository : Repository<Material>, IMaterialRepository
{
    public MaterialRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Material>> GetByOrganizationAsync(int organizationId)
    {
        return await _dbSet
            .Include(m => m.Organization)
            .Where(m => m.OrganizationId == organizationId && m.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Material>> GetLowStockMaterialsAsync()
    {
        return await _dbSet
            .Include(m => m.Organization)
            .Where(m => m.StockQuantity <= m.MinStockLevel && m.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Material>> GetByCategoryAsync(string category)
    {
        return await _dbSet
            .Include(m => m.Organization)
            .Where(m => m.Category == category && m.IsActive)
            .ToListAsync();
    }

    public override async Task<IEnumerable<Material>> GetAllAsync()
    {
        return await _dbSet
            .Include(m => m.Organization)
            .ToListAsync();
    }

    public override async Task<Material?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(m => m.Organization)
            .FirstOrDefaultAsync(m => m.Id == id);
    }
}
