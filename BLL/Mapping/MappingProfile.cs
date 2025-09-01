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
            .ForMember(dest => dest.Department, opt => opt.Ignore());

        CreateMap<PersonUpdateDto, Person>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore());

        CreateMap<Person, PersonUpdateDto>();

        // Department Mappings
        CreateMap<Department, DepartmentListDto>()
            .ForMember(dest => dest.ParentDepartmentName, 
                       opt => opt.MapFrom(src => src.ParentDepartment != null ? src.ParentDepartment.Name : null))
            .ForMember(dest => dest.EmployeeCount, 
                       opt => opt.MapFrom(src => src.Employees.Count(e => e.IsActive)));

        CreateMap<Department, DepartmentDetailDto>()
            .ForMember(dest => dest.ParentDepartmentName, 
                       opt => opt.MapFrom(src => src.ParentDepartment != null ? src.ParentDepartment.Name : null))
            .ForMember(dest => dest.SubDepartments, 
                       opt => opt.MapFrom(src => src.SubDepartments.Where(d => d.IsActive)))
            .ForMember(dest => dest.Employees, 
                       opt => opt.MapFrom(src => src.Employees.Where(e => e.IsActive)));

        CreateMap<DepartmentCreateDto, Department>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ParentDepartment, opt => opt.Ignore())
            .ForMember(dest => dest.SubDepartments, opt => opt.Ignore())
            .ForMember(dest => dest.Employees, opt => opt.Ignore());

        CreateMap<DepartmentUpdateDto, Department>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ParentDepartment, opt => opt.Ignore())
            .ForMember(dest => dest.SubDepartments, opt => opt.Ignore())
            .ForMember(dest => dest.Employees, opt => opt.Ignore());

        CreateMap<Department, DepartmentUpdateDto>();

        // Education Mappings
        CreateMap<Education, EducationListDto>()
            .ForMember(dest => dest.PersonName, 
                       opt => opt.MapFrom(src => src.Person != null ? $"{src.Person.FirstName} {src.Person.LastName}" : string.Empty))
            .ForMember(dest => dest.PersonEmployeeNumber, 
                       opt => opt.MapFrom(src => src.Person != null ? src.Person.EmployeeNumber : null));

        CreateMap<Education, EducationDetailDto>()
            .ForMember(dest => dest.PersonName, 
                       opt => opt.MapFrom(src => src.Person != null ? $"{src.Person.FirstName} {src.Person.LastName}" : string.Empty))
            .ForMember(dest => dest.PersonEmployeeNumber, 
                       opt => opt.MapFrom(src => src.Person != null ? src.Person.EmployeeNumber : null))
            .ForMember(dest => dest.PersonEmail, 
                       opt => opt.MapFrom(src => src.Person != null ? src.Person.Email : null))
            .ForMember(dest => dest.PersonPhone, 
                       opt => opt.MapFrom(src => src.Person != null ? src.Person.Phone : null))
            .ForMember(dest => dest.PersonDepartmentName, 
                       opt => opt.MapFrom(src => src.Person != null && src.Person.Department != null ? src.Person.Department.Name : null));

        CreateMap<EducationCreateDto, Education>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Person, opt => opt.Ignore());

        CreateMap<EducationUpdateDto, Education>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Person, opt => opt.Ignore());

        CreateMap<Education, EducationUpdateDto>();

        // Qualification Mappings
        CreateMap<Qualification, QualificationListDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => $"{src.Person.FirstName} {src.Person.LastName}"))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Person.Department != null ? src.Person.Department.Name : null));

        CreateMap<Qualification, QualificationDetailDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => $"{src.Person.FirstName} {src.Person.LastName}"))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Person.Department != null ? src.Person.Department.Name : null));

        CreateMap<QualificationCreateDto, Qualification>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Person, opt => opt.Ignore());

        CreateMap<QualificationUpdateDto, Qualification>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Person, opt => opt.Ignore());

        CreateMap<Qualification, QualificationUpdateDto>();

        // Position Mappings
        CreateMap<Position, PositionListDto>()
            .ForMember(dest => dest.DepartmentName, 
                       opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : string.Empty))
            .ForMember(dest => dest.PersonCount, 
                       opt => opt.MapFrom(src => src.Persons.Count(p => p.IsActive)));

        CreateMap<Position, PositionDetailDto>()
            .ForMember(dest => dest.DepartmentName, 
                       opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : string.Empty))
            .ForMember(dest => dest.AssignedPersons, 
                       opt => opt.MapFrom(src => src.Persons.Where(p => p.IsActive)));

        CreateMap<PositionCreateDto, Position>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.Persons, opt => opt.Ignore());

        CreateMap<PositionUpdateDto, Position>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.Persons, opt => opt.Ignore());

        CreateMap<Position, PositionUpdateDto>();

        CreateMap<Person, PositionPersonDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
    }
}
