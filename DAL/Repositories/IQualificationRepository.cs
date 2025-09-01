using DAL.Entities;

namespace DAL.Repositories;

public interface IQualificationRepository : IRepository<Qualification>
{
    Task<IEnumerable<Qualification>> GetByPersonIdAsync(int personId);
    Task<IEnumerable<Qualification>> GetExpiringSoonAsync(int days = 30);
    Task<IEnumerable<Qualification>> GetExpiredAsync();
    Task<IEnumerable<Qualification>> GetByCategoryAsync(string category);
    Task<Qualification?> GetQualificationWithPersonAsync(int id);
}
