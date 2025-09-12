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
    
    // CV and Job Application Repositories
    ICandidateRepository Candidates { get; }
    IJobApplicationRepository JobApplications { get; }
    ICandidateEducationRepository CandidateEducations { get; }
    ICandidateExperienceRepository CandidateExperiences { get; }
    ICandidateSkillRepository CandidateSkills { get; }
    IInterviewNoteRepository InterviewNotes { get; }
    IApplicationDocumentRepository ApplicationDocuments { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
