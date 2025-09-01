namespace DAL.Repositories;

public interface IUnitOfWork : IDisposable
{
    IPersonRepository Persons { get; }
    IDepartmentRepository Departments { get; }
    IEducationRepository Educations { get; }
    IQualificationRepository Qualifications { get; }
    IPositionRepository Positions { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
