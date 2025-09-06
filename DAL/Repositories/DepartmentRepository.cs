using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class DepartmentRepository : Repository<Department>, IDepartmentRepository
{
    public DepartmentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Department>> GetRootDepartmentsAsync()
    {
        return await _dbSet
            .Include(d => d.SubDepartments)
            .Include(d => d.Employees)
            .Where(d => d.ParentDepartmentId == null && d.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Department>> GetSubDepartmentsAsync(int parentId)
    {
        return await _dbSet
            .Include(d => d.SubDepartments)
            .Include(d => d.Employees)
            .Where(d => d.ParentDepartmentId == parentId && d.IsActive)
            .ToListAsync();
    }

    public override async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await _dbSet
            .Include(d => d.ParentDepartment)
            .Include(d => d.SubDepartments)
            .Include(d => d.Employees)
            .ToListAsync();
    }

    public override async Task<Department?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(d => d.ParentDepartment)
            .Include(d => d.SubDepartments)
            .Include(d => d.Employees)
            .FirstOrDefaultAsync(d => d.Id == id);
    }
}
