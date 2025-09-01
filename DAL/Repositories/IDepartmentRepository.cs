using DAL.Entities;

namespace DAL.Repositories;

public interface IDepartmentRepository : IRepository<Department>
{
    Task<IEnumerable<Department>> GetRootDepartmentsAsync();
    Task<IEnumerable<Department>> GetSubDepartmentsAsync(int parentId);
}
