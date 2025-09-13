using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
{
    public OrganizationRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Organization>> GetRootOrganizationsAsync()
    {
        return await _dbSet
            .Include(o => o.SubOrganizations)
            .Include(o => o.ManagerPerson)
            .Include(o => o.Materials)
            .Where(o => o.ParentOrganizationId == null && o.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Organization>> GetSubOrganizationsAsync(int parentId)
    {
        return await _dbSet
            .Include(o => o.SubOrganizations)
            .Include(o => o.ManagerPerson)
            .Include(o => o.Materials)
            .Where(o => o.ParentOrganizationId == parentId && o.IsActive)
            .ToListAsync();
    }

    public override async Task<IEnumerable<Organization>> GetAllAsync()
    {
        return await _dbSet
            .Include(o => o.ParentOrganization)
            .Include(o => o.SubOrganizations)
            .Include(o => o.ManagerPerson)
            .Include(o => o.Materials)
            .ToListAsync();
    }

    public override async Task<Organization?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(o => o.ParentOrganization)
            .Include(o => o.SubOrganizations)
            .Include(o => o.ManagerPerson)
            .Include(o => o.Materials)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
}
