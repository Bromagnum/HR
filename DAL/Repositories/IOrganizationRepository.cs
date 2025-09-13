using DAL.Entities;

namespace DAL.Repositories;

public interface IOrganizationRepository : IRepository<Organization>
{
    Task<IEnumerable<Organization>> GetRootOrganizationsAsync();
    Task<IEnumerable<Organization>> GetSubOrganizationsAsync(int parentId);
}
