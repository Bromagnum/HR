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
                       opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null));

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
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.Person != null ? $"{src.Person.FirstName} {src.Person.LastName}" : ""))
            .ForMember(dest => dest.PersonEmployeeNumber, opt => opt.MapFrom(src => src.Person != null ? src.Person.EmployeeNumber : ""));

        CreateMap<WorkLog, WorkLogDetailDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.Person != null ? $"{src.Person.FirstName} {src.Person.LastName}" : ""))
            .ForMember(dest => dest.PersonEmployeeNumber, opt => opt.MapFrom(src => src.Person != null ? src.Person.EmployeeNumber : ""));

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
            .ForMember(dest => dest.ApprovedByName, opt => opt.MapFrom(src => src.ApprovedBy != null ? $"{src.ApprovedBy.FirstName} {src.ApprovedBy.LastName}" : null))
            .ForMember(dest => dest.HandoverToPersonName, opt => opt.MapFrom(src => src.HandoverToPerson != null ? $"{src.HandoverToPerson.FirstName} {src.HandoverToPerson.LastName}" : null));

        CreateMap<Leave, LeaveDetailDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.Person != null ? $"{src.Person.FirstName} {src.Person.LastName}" : ""))
            .ForMember(dest => dest.LeaveTypeName, opt => opt.MapFrom(src => src.LeaveType != null ? src.LeaveType.Name : ""))
            .ForMember(dest => dest.ApprovedByName, opt => opt.MapFrom(src => src.ApprovedBy != null ? $"{src.ApprovedBy.FirstName} {src.ApprovedBy.LastName}" : null))
            .ForMember(dest => dest.HandoverToPersonName, opt => opt.MapFrom(src => src.HandoverToPerson != null ? $"{src.HandoverToPerson.FirstName} {src.HandoverToPerson.LastName}" : null));

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
    }
}
