using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class PersonRepository : Repository<Person>, IPersonRepository
{
    public PersonRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Person?> GetByTcKimlikNoAsync(string tcKimlikNo)
    {
        return await _dbSet
            .Include(p => p.Department)
            .FirstOrDefaultAsync(p => p.TcKimlikNo == tcKimlikNo);
    }

    public async Task<IEnumerable<Person>> GetByDepartmentIdAsync(int departmentId)
    {
        return await _dbSet
            .Include(p => p.Department)
            .Where(p => p.DepartmentId == departmentId && p.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Person>> GetActiveEmployeesAsync()
    {
        return await _dbSet
            .Include(p => p.Department)
            .Where(p => p.IsActive)
            .ToListAsync();
    }

    public override async Task<Person?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(p => p.Department)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public override async Task<IEnumerable<Person>> GetAllAsync()
    {
        return await _dbSet
            .Include(p => p.Department)
            .ToListAsync();
    }
}
