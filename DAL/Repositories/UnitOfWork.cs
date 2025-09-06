using DAL.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace DAL.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;
    
    private IPersonRepository? _persons;
    private IDepartmentRepository? _departments;
    private IEducationRepository? _educations;
    private IQualificationRepository? _qualifications;
    private IPositionRepository? _positions;
    private IWorkLogRepository? _workLogs;
    private ILeaveTypeRepository? _leaveTypes;
    private ILeaveRepository? _leaves;
    private ILeaveBalanceRepository? _leaveBalances;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IPersonRepository Persons =>
        _persons ??= new PersonRepository(_context);

    public IDepartmentRepository Departments =>
        _departments ??= new DepartmentRepository(_context);

    public IEducationRepository Educations =>
        _educations ??= new EducationRepository(_context);

    public IQualificationRepository Qualifications =>
        _qualifications ??= new QualificationRepository(_context);

    public IPositionRepository Positions =>
        _positions ??= new PositionRepository(_context);

    public IWorkLogRepository WorkLogs =>
        _workLogs ??= new WorkLogRepository(_context);

    public ILeaveTypeRepository LeaveTypes =>
        _leaveTypes ??= new LeaveTypeRepository(_context);

    public ILeaveRepository Leaves =>
        _leaves ??= new LeaveRepository(_context);

    public ILeaveBalanceRepository LeaveBalances =>
        _leaveBalances ??= new LeaveBalanceRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
