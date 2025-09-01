using DAL.Entities;

namespace DAL.Repositories
{
    public interface IPositionRepository : IRepository<Position>
    {
        Task<IEnumerable<Position>> GetByDepartmentIdAsync(int departmentId);
        Task<IEnumerable<Position>> GetAvailablePositionsAsync();
        Task<IEnumerable<Position>> GetByLevelAsync(string level);
        Task<IEnumerable<Position>> GetByEmploymentTypeAsync(string employmentType);
        Task<Position?> GetPositionWithDepartmentAsync(int id);
        Task<IEnumerable<Position>> GetPositionsWithPersonCountAsync();
    }
}
