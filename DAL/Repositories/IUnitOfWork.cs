namespace DAL.Repositories;

public interface IUnitOfWork : IDisposable
{
    IPersonRepository Persons { get; }
    IDepartmentRepository Departments { get; }
    IEducationRepository Educations { get; }
    IQualificationRepository Qualifications { get; }
    IPositionRepository Positions { get; }
    IWorkLogRepository WorkLogs { get; }
    ILeaveTypeRepository LeaveTypes { get; }
    ILeaveRepository Leaves { get; }
    ILeaveBalanceRepository LeaveBalances { get; }
    IPayrollRepository Payrolls { get; }
    
    // TMK Repositories
    IOrganizationRepository Organizations { get; }
    IMaterialRepository Materials { get; }
    
    // CV and Job Application Repositories
    IJobApplicationRepository JobApplications { get; }
    IJobPostingRepository JobPostings { get; }
    
    // Job Definition and Skill Management Repositories
    IJobDefinitionRepository JobDefinitions { get; }
    IJobDefinitionQualificationRepository JobDefinitionQualifications { get; }
    IQualificationMatchingResultRepository QualificationMatchingResults { get; }
    ISkillTemplateRepository SkillTemplates { get; }
    IPersonSkillRepository PersonSkills { get; }
    IJobRequiredSkillRepository JobRequiredSkills { get; }
    ISkillAssessmentRepository SkillAssessments { get; }
    
    // Performance Review Repositories
    IPerformanceReviewRepository PerformanceReviews { get; }
    IReviewPeriodRepository ReviewPeriods { get; }
    IPerformanceGoalRepository PerformanceGoals { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
