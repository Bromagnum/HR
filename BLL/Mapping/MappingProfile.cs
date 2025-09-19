using AutoMapper;
using BLL.DTOs;
using DAL.Entities;

namespace BLL.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Person Mappings
        CreateMap<Person, PersonListDto>()
            .ForMember(dest => dest.DepartmentName, 
                       opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null))
            .ForMember(dest => dest.PositionName, 
                       opt => opt.MapFrom(src => src.Position != null ? src.Position.Name : null));

        CreateMap<Person, PersonDetailDto>()
            .ForMember(dest => dest.DepartmentName, 
                       opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null));

        CreateMap<PersonCreateDto, Person>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.Position, opt => opt.Ignore());

        CreateMap<PersonUpdateDto, Person>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.Position, opt => opt.Ignore());

        // Department Mappings
        CreateMap<Department, DepartmentListDto>()
            .ForMember(dest => dest.ParentDepartmentName, opt => opt.MapFrom(src => src.ParentDepartment != null ? src.ParentDepartment.Name : null))
            .ForMember(dest => dest.EmployeeCount, opt => opt.MapFrom(src => src.Employees.Count(e => e.IsActive)));

        CreateMap<Department, DepartmentDetailDto>()
            .ForMember(dest => dest.ParentDepartmentName, opt => opt.MapFrom(src => src.ParentDepartment != null ? src.ParentDepartment.Name : null))
            .ForMember(dest => dest.Employees, opt => opt.MapFrom(src => src.Employees.Where(e => e.IsActive)))
            .ForMember(dest => dest.SubDepartments, opt => opt.MapFrom(src => src.SubDepartments.Where(d => d.IsActive)));

        CreateMap<DepartmentCreateDto, Department>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ParentDepartment, opt => opt.Ignore())
            .ForMember(dest => dest.SubDepartments, opt => opt.Ignore())
            .ForMember(dest => dest.Employees, opt => opt.Ignore());

        CreateMap<DepartmentUpdateDto, Department>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.ParentDepartment, opt => opt.Ignore())
            .ForMember(dest => dest.SubDepartments, opt => opt.Ignore())
            .ForMember(dest => dest.Employees, opt => opt.Ignore());

        // Education Mappings
        CreateMap<Education, EducationListDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.Person != null ? $"{src.Person.FirstName} {src.Person.LastName}" : ""));

        CreateMap<Education, EducationDetailDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.Person != null ? $"{src.Person.FirstName} {src.Person.LastName}" : ""));

        CreateMap<EducationCreateDto, Education>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Person, opt => opt.Ignore());

        CreateMap<EducationUpdateDto, Education>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Person, opt => opt.Ignore());

        // Qualification Mappings
        CreateMap<Qualification, QualificationListDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.Person != null ? $"{src.Person.FirstName} {src.Person.LastName}" : ""));

        CreateMap<Qualification, QualificationDetailDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.Person != null ? $"{src.Person.FirstName} {src.Person.LastName}" : ""));

        CreateMap<QualificationCreateDto, Qualification>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Person, opt => opt.Ignore());

        CreateMap<QualificationUpdateDto, Qualification>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Person, opt => opt.Ignore());

        // Position Mappings
        CreateMap<Position, PositionListDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : ""))
            .ForMember(dest => dest.PersonCount, opt => opt.MapFrom(src => src.Persons.Count(p => p.IsActive)));

        CreateMap<Position, PositionDetailDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : ""))
            .ForMember(dest => dest.AssignedPersons, opt => opt.MapFrom(src => src.Persons.Where(p => p.IsActive)));

        CreateMap<PositionCreateDto, Position>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.Persons, opt => opt.Ignore());

        CreateMap<PositionUpdateDto, Position>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.Persons, opt => opt.Ignore());

        CreateMap<Person, PositionPersonDto>();

        // WorkLog Mappings
        CreateMap<WorkLog, WorkLogListDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.Person != null ? $"{src.Person.FirstName} {src.Person.LastName}" : ""));

        CreateMap<WorkLog, WorkLogDetailDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.Person != null ? $"{src.Person.FirstName} {src.Person.LastName}" : ""));

        CreateMap<WorkLogCreateDto, WorkLog>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Person, opt => opt.Ignore());

        CreateMap<WorkLogUpdateDto, WorkLog>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Person, opt => opt.Ignore());

        // LeaveType Mappings
        CreateMap<LeaveType, LeaveTypeListDto>();
        CreateMap<LeaveType, LeaveTypeDetailDto>();
        CreateMap<LeaveTypeCreateDto, LeaveType>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        CreateMap<LeaveTypeUpdateDto, LeaveType>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

        // Leave Mappings
        CreateMap<Leave, LeaveListDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.Person != null ? $"{src.Person.FirstName} {src.Person.LastName}" : ""))
            .ForMember(dest => dest.LeaveTypeName, opt => opt.MapFrom(src => src.LeaveType != null ? src.LeaveType.Name : ""))
            .ForMember(dest => dest.ApprovedByName, opt => opt.MapFrom(src => src.ApprovedBy != null ? $"{src.ApprovedBy.FirstName} {src.ApprovedBy.LastName}" : null));

        CreateMap<Leave, LeaveDetailDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.Person != null ? $"{src.Person.FirstName} {src.Person.LastName}" : ""))
            .ForMember(dest => dest.LeaveTypeName, opt => opt.MapFrom(src => src.LeaveType != null ? src.LeaveType.Name : ""))
            .ForMember(dest => dest.ApprovedByName, opt => opt.MapFrom(src => src.ApprovedBy != null ? $"{src.ApprovedBy.FirstName} {src.ApprovedBy.LastName}" : null));

        CreateMap<LeaveCreateDto, Leave>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Person, opt => opt.Ignore())
            .ForMember(dest => dest.LeaveType, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedBy, opt => opt.Ignore())
            .ForMember(dest => dest.HandoverToPerson, opt => opt.Ignore());

        CreateMap<LeaveUpdateDto, Leave>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Person, opt => opt.Ignore())
            .ForMember(dest => dest.LeaveType, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedBy, opt => opt.Ignore())
            .ForMember(dest => dest.HandoverToPerson, opt => opt.Ignore());

        // LeaveBalance Mappings
        CreateMap<LeaveBalance, LeaveBalanceListDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.Person != null ? $"{src.Person.FirstName} {src.Person.LastName}" : ""))
            .ForMember(dest => dest.LeaveTypeName, opt => opt.MapFrom(src => src.LeaveType != null ? src.LeaveType.Name : ""));

        CreateMap<LeaveBalance, LeaveBalanceDetailDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.Person != null ? $"{src.Person.FirstName} {src.Person.LastName}" : ""))
            .ForMember(dest => dest.LeaveTypeName, opt => opt.MapFrom(src => src.LeaveType != null ? src.LeaveType.Name : ""));

        CreateMap<LeaveBalanceCreateDto, LeaveBalance>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Person, opt => opt.Ignore())
            .ForMember(dest => dest.LeaveType, opt => opt.Ignore());

        CreateMap<LeaveBalanceUpdateDto, LeaveBalance>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Person, opt => opt.Ignore())
            .ForMember(dest => dest.LeaveType, opt => opt.Ignore());

        // Payroll Mappings
        CreateMap<Payroll, PayrollListDto>()
            .ForMember(dest => dest.PersonFullName, opt => opt.MapFrom(src => src.Person != null ? $"{src.Person.FirstName} {src.Person.LastName}" : ""))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Person != null && src.Person.Department != null ? src.Person.Department.Name : ""))
            .ForMember(dest => dest.EmployeeNumber, opt => opt.MapFrom(src => src.Person != null ? src.Person.EmployeeNumber ?? "" : ""))
            .ForMember(dest => dest.MonthName, opt => opt.MapFrom(src => src.MonthName))
            .ForMember(dest => dest.PayrollPeriod, opt => opt.MapFrom(src => src.PayrollPeriod))
            .ForMember(dest => dest.GrossSalary, opt => opt.MapFrom(src => src.GrossSalary))
            .ForMember(dest => dest.PreparedByName, opt => opt.MapFrom(src => src.PreparedBy != null ? $"{src.PreparedBy.FirstName} {src.PreparedBy.LastName}" : null));

        CreateMap<Payroll, PayrollDetailDto>()
            .ForMember(dest => dest.PersonFullName, opt => opt.MapFrom(src => src.Person != null ? $"{src.Person.FirstName} {src.Person.LastName}" : ""))
            .ForMember(dest => dest.PersonFirstName, opt => opt.MapFrom(src => src.Person != null ? src.Person.FirstName : ""))
            .ForMember(dest => dest.PersonLastName, opt => opt.MapFrom(src => src.Person != null ? src.Person.LastName : ""))
            .ForMember(dest => dest.PersonTcKimlikNo, opt => opt.MapFrom(src => src.Person != null ? src.Person.TcKimlikNo : ""))
            .ForMember(dest => dest.PersonEmployeeNumber, opt => opt.MapFrom(src => src.Person != null ? src.Person.EmployeeNumber ?? "" : ""))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Person != null && src.Person.Department != null ? src.Person.Department.Name : ""))
            .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Person != null && src.Person.Position != null ? src.Person.Position.Name : ""))
            .ForMember(dest => dest.MonthName, opt => opt.MapFrom(src => src.MonthName))
            .ForMember(dest => dest.PayrollPeriod, opt => opt.MapFrom(src => src.PayrollPeriod))
            .ForMember(dest => dest.GrossSalary, opt => opt.MapFrom(src => src.GrossSalary))
            .ForMember(dest => dest.PreparedByName, opt => opt.MapFrom(src => src.PreparedBy != null ? $"{src.PreparedBy.FirstName} {src.PreparedBy.LastName}" : null));

        CreateMap<PayrollCreateDto, Payroll>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.NetSalary, opt => opt.Ignore()) // Servis tarafından hesaplanacak
            .ForMember(dest => dest.PreparedDate, opt => opt.Ignore()) // Servis tarafından set edilecek
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Person, opt => opt.Ignore())
            .ForMember(dest => dest.PreparedBy, opt => opt.Ignore());

        CreateMap<PayrollUpdateDto, Payroll>()
            .ForMember(dest => dest.NetSalary, opt => opt.Ignore()) // Servis tarafından hesaplanacak
            .ForMember(dest => dest.PreparedDate, opt => opt.Ignore()) // Mevcut değer korunacak
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Person, opt => opt.Ignore())
            .ForMember(dest => dest.PreparedBy, opt => opt.Ignore());

        // TMK Mappings
        CreateMap<Organization, OrganizationListDto>()
            .ForMember(dest => dest.ParentOrganizationName, opt => opt.MapFrom(src => src.ParentOrganization != null ? src.ParentOrganization.Name : null))
            .ForMember(dest => dest.ManagerPersonName, opt => opt.MapFrom(src => src.ManagerPerson != null ? $"{src.ManagerPerson.FirstName} {src.ManagerPerson.LastName}" : null))
            .ForMember(dest => dest.SubOrganizationsCount, opt => opt.MapFrom(src => src.SubOrganizations.Count))
            .ForMember(dest => dest.MaterialsCount, opt => opt.MapFrom(src => src.Materials.Count));

        CreateMap<Organization, OrganizationDetailDto>()
            .ForMember(dest => dest.ParentOrganizationName, opt => opt.MapFrom(src => src.ParentOrganization != null ? src.ParentOrganization.Name : null))
            .ForMember(dest => dest.ManagerPersonName, opt => opt.MapFrom(src => src.ManagerPerson != null ? $"{src.ManagerPerson.FirstName} {src.ManagerPerson.LastName}" : null))
            .ForMember(dest => dest.SubOrganizations, opt => opt.MapFrom(src => src.SubOrganizations.Where(o => o.IsActive)))
            .ForMember(dest => dest.Materials, opt => opt.MapFrom(src => src.Materials.Where(m => m.IsActive)));

        CreateMap<Organization, OrganizationTreeDto>()
            .ForMember(dest => dest.ManagerPersonName, opt => opt.MapFrom(src => src.ManagerPerson != null ? $"{src.ManagerPerson.FirstName} {src.ManagerPerson.LastName}" : ""))
            .ForMember(dest => dest.MaterialsCount, opt => opt.MapFrom(src => src.Materials.Count))
            .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.SubOrganizations));

        CreateMap<OrganizationCreateDto, Organization>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ParentOrganization, opt => opt.Ignore())
            .ForMember(dest => dest.SubOrganizations, opt => opt.Ignore())
            .ForMember(dest => dest.ManagerPerson, opt => opt.Ignore())
            .ForMember(dest => dest.Materials, opt => opt.Ignore());

        CreateMap<OrganizationUpdateDto, Organization>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.ParentOrganization, opt => opt.Ignore())
            .ForMember(dest => dest.SubOrganizations, opt => opt.Ignore())
            .ForMember(dest => dest.ManagerPerson, opt => opt.Ignore())
            .ForMember(dest => dest.Materials, opt => opt.Ignore());

        CreateMap<Material, MaterialListDto>()
            .ForMember(dest => dest.TotalValue, opt => opt.MapFrom(src => src.TotalValue))
            .ForMember(dest => dest.StockStatus, opt => opt.MapFrom(src => src.StockStatus))
            .ForMember(dest => dest.IsLowStock, opt => opt.MapFrom(src => src.IsLowStock))
            .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.Organization != null ? src.Organization.Name : null));

        CreateMap<Material, MaterialDetailDto>()
            .ForMember(dest => dest.TotalValue, opt => opt.MapFrom(src => src.TotalValue))
            .ForMember(dest => dest.StockStatus, opt => opt.MapFrom(src => src.StockStatus))
            .ForMember(dest => dest.IsLowStock, opt => opt.MapFrom(src => src.IsLowStock))
            .ForMember(dest => dest.IsOverStock, opt => opt.MapFrom(src => src.IsOverStock))
            .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.Organization != null ? src.Organization.Name : null));

        CreateMap<MaterialCreateDto, Material>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Organization, opt => opt.Ignore());

        CreateMap<MaterialUpdateDto, Material>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Organization, opt => opt.Ignore());

        // CV and Job Application Mappings
        // JobApplication Mappings
        CreateMap<JobApplication, JobApplicationListDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.Name))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Position.Department.Name))
            .ForMember(dest => dest.StatusText, opt => opt.MapFrom(src => src.StatusText))
            .ForMember(dest => dest.StatusClass, opt => opt.MapFrom(src => src.StatusClass))
            .ForMember(dest => dest.HasCV, opt => opt.MapFrom(src => src.HasCV))
            .ForMember(dest => dest.DocumentCount, opt => opt.MapFrom(src => src.DocumentCount))
            .ForMember(dest => dest.ReviewedByName, opt => opt.MapFrom(src => src.ReviewedBy != null ? $"{src.ReviewedBy.FirstName} {src.ReviewedBy.LastName}" : null))
            .ForMember(dest => dest.ApplicationPeriod, opt => opt.MapFrom(src => src.ApplicationPeriod));

        CreateMap<JobApplication, JobApplicationDetailDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.Name))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Position.Department.Name))
            .ForMember(dest => dest.StatusText, opt => opt.MapFrom(src => src.StatusText))
            .ForMember(dest => dest.StatusClass, opt => opt.MapFrom(src => src.StatusClass))
            .ForMember(dest => dest.HasCV, opt => opt.MapFrom(src => src.HasCV))
            .ForMember(dest => dest.HasCoverLetter, opt => opt.MapFrom(src => src.HasCoverLetter))
            .ForMember(dest => dest.DocumentCount, opt => opt.MapFrom(src => src.DocumentCount))
            .ForMember(dest => dest.ApplicationPeriod, opt => opt.MapFrom(src => src.ApplicationPeriod))
            .ForMember(dest => dest.ReviewedByName, opt => opt.MapFrom(src => src.ReviewedBy != null ? $"{src.ReviewedBy.FirstName} {src.ReviewedBy.LastName}" : null))
            .ForMember(dest => dest.Documents, opt => opt.MapFrom(src => src.Documents));

        CreateMap<JobApplicationCreateDto, JobApplication>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => JobApplicationStatus.Submitted))
            .ForMember(dest => dest.ApplicationDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.Position, opt => opt.Ignore())
            .ForMember(dest => dest.ReviewedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Documents, opt => opt.Ignore());

        CreateMap<JobApplicationUpdateDto, JobApplication>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.ReviewedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ApplicationDate, opt => opt.Ignore())
            .ForMember(dest => dest.Position, opt => opt.Ignore())
            .ForMember(dest => dest.ReviewedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Documents, opt => opt.Ignore());

        // ApplicationDocument Mappings
        CreateMap<ApplicationDocument, ApplicationDocumentDto>()
            .ForMember(dest => dest.DocumentTypeText, opt => opt.MapFrom(src => src.DocumentTypeText))
            .ForMember(dest => dest.FileSizeText, opt => opt.MapFrom(src => src.FileSizeText))
            .ForMember(dest => dest.FileSize, opt => opt.MapFrom(src => src.FileSizeBytes))
            .ForMember(dest => dest.FileExtension, opt => opt.MapFrom(src => src.FileExtension))
            .ForMember(dest => dest.IsImageFile, opt => opt.MapFrom(src => src.IsImageFile))
            .ForMember(dest => dest.IsPdfFile, opt => opt.MapFrom(src => src.IsPdfFile))
            .ForMember(dest => dest.IsDocumentFile, opt => opt.MapFrom(src => src.IsDocumentFile));

        // JobPosting Mappings
        CreateMap<JobPosting, JobPostingListDto>()
            .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.Name))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
            .ForMember(dest => dest.StatusText, opt => opt.MapFrom(src => src.StatusText))
            .ForMember(dest => dest.StatusClass, opt => opt.MapFrom(src => src.StatusClass))
            .ForMember(dest => dest.EmploymentTypeText, opt => opt.MapFrom(src => src.EmploymentTypeText))
            .ForMember(dest => dest.SalaryRange, opt => opt.MapFrom(src => src.SalaryRange))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.IsExpired))
            .ForMember(dest => dest.DaysUntilExpiry, opt => opt.MapFrom(src => src.DaysUntilExpiry))
            .ForMember(dest => dest.DaysUntilApplicationDeadline, opt => opt.MapFrom(src => src.DaysUntilApplicationDeadline))
            .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy != null ? $"{src.CreatedBy.FirstName} {src.CreatedBy.LastName}" : null));

        CreateMap<JobPosting, JobPostingDetailDto>()
            .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.Name))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
            .ForMember(dest => dest.StatusText, opt => opt.MapFrom(src => src.StatusText))
            .ForMember(dest => dest.StatusClass, opt => opt.MapFrom(src => src.StatusClass))
            .ForMember(dest => dest.EmploymentTypeText, opt => opt.MapFrom(src => src.EmploymentTypeText))
            .ForMember(dest => dest.ExperienceRange, opt => opt.MapFrom(src => src.ExperienceRange))
            .ForMember(dest => dest.SalaryRange, opt => opt.MapFrom(src => src.SalaryRange))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.IsExpired))
            .ForMember(dest => dest.IsApplicationDeadlinePassed, opt => opt.MapFrom(src => src.IsApplicationDeadlinePassed))
            .ForMember(dest => dest.DaysUntilExpiry, opt => opt.MapFrom(src => src.DaysUntilExpiry))
            .ForMember(dest => dest.DaysUntilApplicationDeadline, opt => opt.MapFrom(src => src.DaysUntilApplicationDeadline))
            .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy != null ? $"{src.CreatedBy.FirstName} {src.CreatedBy.LastName}" : null))
            .ForMember(dest => dest.UpdatedByName, opt => opt.MapFrom(src => src.UpdatedBy != null ? $"{src.UpdatedBy.FirstName} {src.UpdatedBy.LastName}" : null))
            .ForMember(dest => dest.Applications, opt => opt.MapFrom(src => src.Applications));

        CreateMap<JobPosting, PublicJobPostingDto>()
            .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.Name))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
            .ForMember(dest => dest.EmploymentTypeText, opt => opt.MapFrom(src => src.EmploymentTypeText))
            .ForMember(dest => dest.ExperienceRange, opt => opt.MapFrom(src => src.ExperienceRange))
            .ForMember(dest => dest.SalaryRange, opt => opt.MapFrom(src => src.SalaryRange))
            .ForMember(dest => dest.DaysUntilApplicationDeadline, opt => opt.MapFrom(src => src.DaysUntilApplicationDeadline))
            .ForMember(dest => dest.CanApply, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<JobPostingCreateDto, JobPosting>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => JobPostingStatus.Draft))
            .ForMember(dest => dest.PublishDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.ViewCount, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.ApplicationCount, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.Position, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Applications, opt => opt.Ignore());

        CreateMap<JobPostingUpdateDto, JobPosting>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.PublishDate, opt => opt.Ignore())
            .ForMember(dest => dest.ViewCount, opt => opt.Ignore())
            .ForMember(dest => dest.ApplicationCount, opt => opt.Ignore())
            .ForMember(dest => dest.Position, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Applications, opt => opt.Ignore());
        // Job Definition and Skill Management Mappings
        
        // Job Definition Mappings
        CreateMap<JobDefinition, JobDefinitionListDto>()
            .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position != null ? src.Position.Name : ""))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Position != null && src.Position.Department != null ? src.Position.Department.Name : ""))
            .ForMember(dest => dest.ApprovedByName, opt => opt.MapFrom(src => src.ApprovedBy != null ? $"{src.ApprovedBy.FirstName} {src.ApprovedBy.LastName}" : null))
            .ForMember(dest => dest.RequiredQualificationCount, opt => opt.MapFrom(src => src.RequiredQualifications.Count))
            .ForMember(dest => dest.MatchingResultsCount, opt => opt.MapFrom(src => src.MatchingResults.Count));

        CreateMap<JobDefinition, JobDefinitionDetailDto>()
            .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position != null ? src.Position.Name : ""))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Position != null && src.Position.Department != null ? src.Position.Department.Name : ""))
            .ForMember(dest => dest.ApprovedByName, opt => opt.MapFrom(src => src.ApprovedBy != null ? $"{src.ApprovedBy.FirstName} {src.ApprovedBy.LastName}" : null))
            .ForMember(dest => dest.RequiredQualifications, opt => opt.MapFrom(src => src.RequiredQualifications))
            .ForMember(dest => dest.RequiredSkills, opt => opt.MapFrom(src => src.JobRequiredSkills));

        CreateMap<JobDefinitionCreateDto, JobDefinition>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Version, opt => opt.Ignore())
            .ForMember(dest => dest.IsApproved, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.Position, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedBy, opt => opt.Ignore())
            .ForMember(dest => dest.PreviousVersion, opt => opt.Ignore())
            .ForMember(dest => dest.RequiredQualifications, opt => opt.Ignore())
            .ForMember(dest => dest.JobRequiredSkills, opt => opt.Ignore())
            .ForMember(dest => dest.MatchingResults, opt => opt.Ignore());

        CreateMap<JobDefinitionUpdateDto, JobDefinition>()
            .ForMember(dest => dest.Version, opt => opt.Ignore())
            .ForMember(dest => dest.PositionId, opt => opt.Ignore())
            .ForMember(dest => dest.IsApproved, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

        // Job Definition Qualification Mappings
        CreateMap<JobDefinitionQualification, JobDefinitionQualificationDto>();
        CreateMap<JobDefinitionQualificationCreateDto, JobDefinitionQualification>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.JobDefinitionId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

        // Job Required Skill Mappings
        CreateMap<JobRequiredSkill, JobRequiredSkillDto>()
            .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.SkillTemplate != null ? src.SkillTemplate.Name : ""))
            .ForMember(dest => dest.SkillCategory, opt => opt.MapFrom(src => src.SkillTemplate != null ? src.SkillTemplate.Category : ""))
            .ForMember(dest => dest.SkillType, opt => opt.MapFrom(src => src.SkillTemplate != null ? src.SkillTemplate.Type : SkillType.Technical))
            .ForMember(dest => dest.ImportanceText, opt => opt.MapFrom(src => src.Importance.ToString()))
            .ForMember(dest => dest.PreferredLevel, opt => opt.MapFrom(src => src.PreferredLevel))
            .ForMember(dest => dest.MinExperienceYears, opt => opt.MapFrom(src => src.MinExperienceYears))
            .ForMember(dest => dest.PreferredCertifications, opt => opt.MapFrom(src => src.SpecificRequirements))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.AssessmentCriteria));

        CreateMap<JobRequiredSkillCreateDto, JobRequiredSkill>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.JobDefinitionId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.SpecificRequirements, opt => opt.MapFrom(src => src.PreferredCertifications))
            .ForMember(dest => dest.AssessmentCriteria, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.PreferredExperienceYears, opt => opt.Ignore())
            .ForMember(dest => dest.JobDefinition, opt => opt.Ignore())
            .ForMember(dest => dest.SkillTemplate, opt => opt.Ignore());

        // Qualification Matching Result Mappings
        CreateMap<QualificationMatchingResult, QualificationMatchingResultDto>()
            .ForMember(dest => dest.JobDefinitionTitle, opt => opt.MapFrom(src => src.JobDefinition != null ? src.JobDefinition.Title : ""))
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.Person != null ? $"{src.Person.FirstName} {src.Person.LastName}" : ""))
            .ForMember(dest => dest.PersonEmail, opt => opt.MapFrom(src => src.Person != null ? src.Person.Email : ""))
            .ForMember(dest => dest.StatusText, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.ReviewedByName, opt => opt.MapFrom(src => src.ReviewedBy != null ? $"{src.ReviewedBy.FirstName} {src.ReviewedBy.LastName}" : null));

        // Skill Template Mappings
        CreateMap<SkillTemplate, SkillTemplateListDto>()
            .ForMember(dest => dest.TypeText, opt => opt.MapFrom(src => src.Type.ToString()));

        CreateMap<SkillTemplate, SkillTemplateDetailDto>()
            .ForMember(dest => dest.TypeText, opt => opt.MapFrom(src => src.Type.ToString()));

        CreateMap<SkillTemplateCreateDto, SkillTemplate>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UsageCount, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

        CreateMap<SkillTemplateUpdateDto, SkillTemplate>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UsageCount, opt => opt.Ignore())
            .ForMember(dest => dest.LastUsedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.PersonSkills, opt => opt.Ignore())
            .ForMember(dest => dest.JobRequiredSkills, opt => opt.Ignore());

        // Person Skill Mappings
        CreateMap<PersonSkill, PersonSkillDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.Person != null ? $"{src.Person.FirstName} {src.Person.LastName}" : ""))
            .ForMember(dest => dest.PersonEmail, opt => opt.MapFrom(src => src.Person != null ? src.Person.Email : ""))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Person != null && src.Person.Department != null ? src.Person.Department.Name : ""))
            .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.SkillTemplate != null ? src.SkillTemplate.Name : ""))
            .ForMember(dest => dest.SkillCategory, opt => opt.MapFrom(src => src.SkillTemplate != null ? src.SkillTemplate.Category : ""))
            .ForMember(dest => dest.SkillType, opt => opt.MapFrom(src => src.SkillTemplate != null ? src.SkillTemplate.Type : SkillType.Technical))
            .ForMember(dest => dest.LevelText, opt => opt.MapFrom(src => $"Seviye {src.Level}"))
            .ForMember(dest => dest.TotalExperience, opt => opt.MapFrom(src => src.TotalExperience))
            .ForMember(dest => dest.IsCertificationExpired, opt => opt.MapFrom(src => src.IsCertificationExpired))
            .ForMember(dest => dest.IsCertificationExpiringSoon, opt => opt.MapFrom(src => src.IsCertificationExpiringSoon))
            .ForMember(dest => dest.LastAssessmentDate, opt => opt.MapFrom(src => 
                src.SkillAssessments != null && src.SkillAssessments.Any() ? 
                src.SkillAssessments.OrderByDescending(a => a.AssessmentDate).First().AssessmentDate : 
                (DateTime?)null));

        CreateMap<PersonSkillCreateDto, PersonSkill>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

        // Performance Review Mappings
        CreateMap<PerformanceReview, PerformanceReviewListDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => $"{src.Person.FirstName} {src.Person.LastName}"))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Person.Department != null ? src.Person.Department.Name : ""))
            .ForMember(dest => dest.ReviewPeriodName, opt => opt.MapFrom(src => src.ReviewPeriod.Name))
            .ForMember(dest => dest.ReviewerName, opt => opt.MapFrom(src => $"{src.Reviewer.FirstName} {src.Reviewer.LastName}"))
            .ForMember(dest => dest.StatusText, opt => opt.MapFrom(src => GetStatusText(src.Status)));

        CreateMap<PerformanceReview, PerformanceReviewDetailDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => $"{src.Person.FirstName} {src.Person.LastName}"))
            .ForMember(dest => dest.PersonEmail, opt => opt.MapFrom(src => src.Person.Email))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Person.Department != null ? src.Person.Department.Name : ""))
            .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Person.Position != null ? src.Person.Position.Name : ""))
            .ForMember(dest => dest.ReviewPeriodName, opt => opt.MapFrom(src => src.ReviewPeriod.Name))
            .ForMember(dest => dest.ReviewPeriodStart, opt => opt.MapFrom(src => src.ReviewPeriod.StartDate))
            .ForMember(dest => dest.ReviewPeriodEnd, opt => opt.MapFrom(src => src.ReviewPeriod.EndDate))
            .ForMember(dest => dest.ReviewerName, opt => opt.MapFrom(src => $"{src.Reviewer.FirstName} {src.Reviewer.LastName}"))
            .ForMember(dest => dest.StatusText, opt => opt.MapFrom(src => GetStatusText(src.Status)))
            .ForMember(dest => dest.ApprovedByName, opt => opt.MapFrom(src => src.ApprovedBy != null ? $"{src.ApprovedBy.FirstName} {src.ApprovedBy.LastName}" : null))
            .ForMember(dest => dest.Goals_List, opt => opt.MapFrom(src => src.Goals_Navigation));

        CreateMap<PerformanceReviewCreateDto, PerformanceReview>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.SubmittedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedById, opt => opt.Ignore())
            .ForMember(dest => dest.IsSelfAssessmentCompleted, opt => opt.Ignore())
            .ForMember(dest => dest.SelfAssessmentCompletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.SelfOverallScore, opt => opt.Ignore())
            .ForMember(dest => dest.SelfAssessmentComments, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Person, opt => opt.Ignore())
            .ForMember(dest => dest.ReviewPeriod, opt => opt.Ignore())
            .ForMember(dest => dest.Reviewer, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Goals_Navigation, opt => opt.Ignore());

        CreateMap<PerformanceReviewUpdateDto, PerformanceReview>()
            .ForMember(dest => dest.PersonId, opt => opt.Ignore())
            .ForMember(dest => dest.ReviewPeriodId, opt => opt.Ignore())
            .ForMember(dest => dest.ReviewerId, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.SubmittedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedById, opt => opt.Ignore())
            .ForMember(dest => dest.IsSelfAssessmentCompleted, opt => opt.Ignore())
            .ForMember(dest => dest.SelfAssessmentCompletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.SelfOverallScore, opt => opt.Ignore())
            .ForMember(dest => dest.SelfAssessmentComments, opt => opt.Ignore())
            .ForMember(dest => dest.EmployeeComments, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Person, opt => opt.Ignore())
            .ForMember(dest => dest.ReviewPeriod, opt => opt.Ignore())
            .ForMember(dest => dest.Reviewer, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Goals_Navigation, opt => opt.Ignore());

        // Review Period Mappings
        CreateMap<ReviewPeriod, ReviewPeriodListDto>()
            .ForMember(dest => dest.TypeText, opt => opt.MapFrom(src => GetPeriodTypeText(src.Type)))
            .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.PerformanceReviews.Count))
            .ForMember(dest => dest.CompletedReviewCount, opt => opt.MapFrom(src => src.PerformanceReviews.Count(r => r.Status == ReviewStatus.Completed || r.Status == ReviewStatus.Approved)))
            .ForMember(dest => dest.IsCurrentPeriod, opt => opt.MapFrom(src => src.StartDate <= DateTime.Now && src.EndDate >= DateTime.Now));

        CreateMap<ReviewPeriod, ReviewPeriodDetailDto>()
            .ForMember(dest => dest.TypeText, opt => opt.MapFrom(src => GetPeriodTypeText(src.Type)))
            .ForMember(dest => dest.PerformanceReviews, opt => opt.MapFrom(src => src.PerformanceReviews))
            .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.PerformanceReviews.Count))
            .ForMember(dest => dest.CompletedReviewCount, opt => opt.MapFrom(src => src.PerformanceReviews.Count(r => r.Status == ReviewStatus.Completed || r.Status == ReviewStatus.Approved)))
            .ForMember(dest => dest.CompletionPercentage, opt => opt.MapFrom(src => src.PerformanceReviews.Count > 0 ? (decimal)src.PerformanceReviews.Count(r => r.Status == ReviewStatus.Completed || r.Status == ReviewStatus.Approved) / src.PerformanceReviews.Count * 100 : 0))
            .ForMember(dest => dest.IsCurrentPeriod, opt => opt.MapFrom(src => src.StartDate <= DateTime.Now && src.EndDate >= DateTime.Now));

        CreateMap<ReviewPeriodCreateDto, ReviewPeriod>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.PerformanceReviews, opt => opt.Ignore());

        CreateMap<ReviewPeriodUpdateDto, ReviewPeriod>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.PerformanceReviews, opt => opt.Ignore());

        // Performance Goal Mappings
        CreateMap<PerformanceGoal, PerformanceGoalDto>()
            .ForMember(dest => dest.StatusText, opt => opt.MapFrom(src => GetGoalStatusText(src.Status)))
            .ForMember(dest => dest.PriorityText, opt => opt.MapFrom(src => GetGoalPriorityText(src.Priority)))
            .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.TargetDate < DateTime.Now && src.Status != GoalStatus.Completed && src.Status != GoalStatus.Cancelled))
            .ForMember(dest => dest.IsUpcoming, opt => opt.MapFrom(src => src.TargetDate >= DateTime.Now && src.TargetDate <= DateTime.Now.AddDays(30) && src.Status != GoalStatus.Completed && src.Status != GoalStatus.Cancelled));

        CreateMap<PerformanceGoalCreateDto, PerformanceGoal>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PerformanceReviewId, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.ProgressPercentage, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.PerformanceReview, opt => opt.Ignore());

        CreateMap<PerformanceGoalUpdateDto, PerformanceGoal>()
            .ForMember(dest => dest.PerformanceReviewId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.PerformanceReview, opt => opt.Ignore());

        // ViewModel mappings would be added to a separate MVC mapping profile
        // These are handled in MVC layer with separate mapping profile
    }

    // Helper methods for enum to text conversion
    private static string GetStatusText(ReviewStatus status)
    {
        return status switch
        {
            ReviewStatus.Draft => "Taslak",
            ReviewStatus.InProgress => "Devam Ediyor",
            ReviewStatus.EmployeeReview => "Çalışan Değerlendirmesi",
            ReviewStatus.ManagerReview => "Yönetici Değerlendirmesi",
            ReviewStatus.Completed => "Tamamlandı",
            ReviewStatus.Approved => "Onaylandı",
            _ => "Bilinmiyor"
        };
    }

    private static string GetPeriodTypeText(ReviewPeriodType type)
    {
        return type switch
        {
            ReviewPeriodType.Monthly => "Aylık",
            ReviewPeriodType.Quarterly => "Üç Aylık",
            ReviewPeriodType.SemiAnnual => "Altı Aylık",
            ReviewPeriodType.Annual => "Yıllık",
            ReviewPeriodType.Custom => "Özel",
            _ => "Bilinmiyor"
        };
    }

    private static string GetGoalStatusText(GoalStatus status)
    {
        return status switch
        {
            GoalStatus.NotStarted => "Başlanmadı",
            GoalStatus.InProgress => "Devam Ediyor",
            GoalStatus.Completed => "Tamamlandı",
            GoalStatus.OnHold => "Beklemede",
            GoalStatus.Cancelled => "İptal Edildi",
            _ => "Bilinmiyor"
        };
    }

    private static string GetGoalPriorityText(GoalPriority priority)
    {
        return priority switch
        {
            GoalPriority.Low => "Düşük",
            GoalPriority.Medium => "Orta",
            GoalPriority.High => "Yüksek",
            GoalPriority.Critical => "Kritik",
            _ => "Bilinmiyor"
        };
    }
}
