using DAL.Entities;

namespace DAL.Repositories;

public interface IEducationRepository : IRepository<Education>
{
    Task<IEnumerable<Education>> GetByPersonIdAsync(int personId);
    Task<IEnumerable<Education>> GetOngoingEducationsAsync();
    Task<IEnumerable<Education>> GetCompletedEducationsAsync();
    Task<IEnumerable<Education>> GetEducationsByDegreeAsync(string degree);
    Task<Education?> GetEducationWithPersonAsync(int educationId);
}

