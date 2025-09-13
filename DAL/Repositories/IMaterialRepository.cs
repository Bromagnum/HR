using DAL.Entities;

namespace DAL.Repositories;

public interface IMaterialRepository : IRepository<Material>
{
    Task<IEnumerable<Material>> GetByOrganizationAsync(int organizationId);
    Task<IEnumerable<Material>> GetLowStockMaterialsAsync();
    Task<IEnumerable<Material>> GetByCategoryAsync(string category);
}
