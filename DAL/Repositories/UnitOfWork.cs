using DAL.Context;
using DAL.Repositories.Interfaces;
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
    private IPayrollRepository? _payrolls;
    
    // TMK Repositories
    private IOrganizationRepository? _organizations;
    private IMaterialRepository? _materials;
    
    // CV and Job Application Repositories
    private IJobApplicationRepository? _jobApplications;
    private IJobPostingRepository? _jobPostings;
    
    // Job Definition and Skill Management Repositories
    private IJobDefinitionRepository? _jobDefinitions;
    private IJobDefinitionQualificationRepository? _jobDefinitionQualifications;
    private IQualificationMatchingResultRepository? _qualificationMatchingResults;
    private ISkillTemplateRepository? _skillTemplates;
    private IPersonSkillRepository? _personSkills;
    private IJobRequiredSkillRepository? _jobRequiredSkills;
    private ISkillAssessmentRepository? _skillAssessments;
    
    // Performance Review Repositories
    private IPerformanceReviewRepository? _performanceReviews;
    private IReviewPeriodRepository? _reviewPeriods;
    private IPerformanceGoalRepository? _performanceGoals;

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

    public IPayrollRepository Payrolls =>
        _payrolls ??= new PayrollRepository(_context);

    // TMK Repository Properties
    public IOrganizationRepository Organizations =>
        _organizations ??= new OrganizationRepository(_context);

    public IMaterialRepository Materials =>
        _materials ??= new MaterialRepository(_context);

    // CV and Job Application Repository Properties
    public IJobApplicationRepository JobApplications =>
        _jobApplications ??= new JobApplicationRepository(_context);

    public IJobPostingRepository JobPostings =>
        _jobPostings ??= new JobPostingRepository(_context);

    // Job Definition and Skill Management Repository Properties
    public IJobDefinitionRepository JobDefinitions =>
        _jobDefinitions ??= new JobDefinitionRepository(_context);

    public IJobDefinitionQualificationRepository JobDefinitionQualifications =>
        _jobDefinitionQualifications ??= new JobDefinitionQualificationRepository(_context);

    public IQualificationMatchingResultRepository QualificationMatchingResults =>
        _qualificationMatchingResults ??= new QualificationMatchingResultRepository(_context);

    public ISkillTemplateRepository SkillTemplates =>
        _skillTemplates ??= new SkillTemplateRepository(_context);

    public IPersonSkillRepository PersonSkills =>
        _personSkills ??= new PersonSkillRepository(_context);

    public IJobRequiredSkillRepository JobRequiredSkills =>
        _jobRequiredSkills ??= new JobRequiredSkillRepository(_context);

    public ISkillAssessmentRepository SkillAssessments =>
        _skillAssessments ??= new SkillAssessmentRepository(_context);

    // Performance Review Repository Properties
    public IPerformanceReviewRepository PerformanceReviews =>
        _performanceReviews ??= new PerformanceReviewRepository(_context);

    public IReviewPeriodRepository ReviewPeriods =>
        _reviewPeriods ??= new ReviewPeriodRepository(_context);

    public IPerformanceGoalRepository PerformanceGoals =>
        _performanceGoals ??= new PerformanceGoalRepository(_context);

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
