using DAL.Entities;

namespace DAL.Repositories;

public interface IPersonRepository : IRepository<Person>
{
    Task<Person?> GetByTcKimlikNoAsync(string tcKimlikNo);
    Task<IEnumerable<Person>> GetByDepartmentIdAsync(int departmentId);
    Task<IEnumerable<Person>> GetActiveEmployeesAsync();
}
