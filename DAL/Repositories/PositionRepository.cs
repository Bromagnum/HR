using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class PositionRepository : Repository<Position>, IPositionRepository
    {
        public PositionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Position>> GetByDepartmentIdAsync(int departmentId)
        {
            return await _context.Positions
                .Include(p => p.Department)
                .Where(p => p.DepartmentId == departmentId)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Position>> GetAvailablePositionsAsync()
        {
            return await _context.Positions
                .Include(p => p.Department)
                .Where(p => p.IsAvailable && p.IsActive)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Position>> GetByLevelAsync(string level)
        {
            return await _context.Positions
                .Include(p => p.Department)
                .Where(p => p.Level == level && p.IsActive)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Position>> GetByEmploymentTypeAsync(string employmentType)
        {
            return await _context.Positions
                .Include(p => p.Department)
                .Where(p => p.EmploymentType == employmentType && p.IsActive)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<Position?> GetPositionWithDepartmentAsync(int id)
        {
            return await _context.Positions
                .Include(p => p.Department)
                .Include(p => p.Persons)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Position>> GetPositionsWithPersonCountAsync()
        {
            return await _context.Positions
                .Include(p => p.Department)
                .Include(p => p.Persons)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }
    }
}
