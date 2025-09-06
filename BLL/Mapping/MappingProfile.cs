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
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.Position, opt => opt.Ignore());

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
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Person.Department != null ? src.Person.Department.Name : ""));

        CreateMap<Qualification, QualificationDetailDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => $"{src.Person.FirstName} {src.Person.LastName}"))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Person.Department != null ? src.Person.Department.Name : ""));

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

        // WorkLog Mappings
        CreateMap<WorkLog, WorkLogListDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => $"{src.Person.FirstName} {src.Person.LastName}"))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Person.Department != null ? src.Person.Department.Name : string.Empty))
            .ForMember(dest => dest.EmployeeNumber, opt => opt.MapFrom(src => src.Person.EmployeeNumber));

        CreateMap<WorkLog, WorkLogDetailDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => $"{src.Person.FirstName} {src.Person.LastName}"))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Person.Department != null ? src.Person.Department.Name : string.Empty))
            .ForMember(dest => dest.EmployeeNumber, opt => opt.MapFrom(src => src.Person.EmployeeNumber))
            .ForMember(dest => dest.ApprovedByName, opt => opt.MapFrom(src => src.ApprovedBy != null ? $"{src.ApprovedBy.FirstName} {src.ApprovedBy.LastName}" : string.Empty));

        CreateMap<WorkLogCreateDto, WorkLog>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Person, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedBy, opt => opt.Ignore())
            .ForMember(dest => dest.TotalHours, opt => opt.Ignore())
            .ForMember(dest => dest.RegularHours, opt => opt.Ignore())
            .ForMember(dest => dest.OvertimeHours, opt => opt.Ignore())
            .ForMember(dest => dest.IsLateArrival, opt => opt.Ignore())
            .ForMember(dest => dest.IsEarlyDeparture, opt => opt.Ignore())
            .ForMember(dest => dest.IsOvertime, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"));

        CreateMap<WorkLogUpdateDto, WorkLog>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Person, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedBy, opt => opt.Ignore())
            .ForMember(dest => dest.TotalHours, opt => opt.Ignore())
            .ForMember(dest => dest.RegularHours, opt => opt.Ignore())
            .ForMember(dest => dest.OvertimeHours, opt => opt.Ignore())
            .ForMember(dest => dest.IsLateArrival, opt => opt.Ignore())
            .ForMember(dest => dest.IsEarlyDeparture, opt => opt.Ignore())
            .ForMember(dest => dest.IsOvertime, opt => opt.Ignore());

        CreateMap<WorkLog, WorkLogUpdateDto>();

        CreateMap<WorkLogCheckInDto, WorkLog>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.Today))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Person, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedBy, opt => opt.Ignore());

        // LeaveType Mappings
        CreateMap<LeaveType, LeaveTypeListDto>()
            .ForMember(dest => dest.TotalLeaves, opt => opt.MapFrom(src => src.Leaves.Count))
            .ForMember(dest => dest.ActiveLeaves, opt => opt.MapFrom(src => src.Leaves.Count(l => l.Status == LeaveStatus.InProgress)));

        CreateMap<LeaveType, LeaveTypeDetailDto>()
            .ForMember(dest => dest.TotalLeaves, opt => opt.MapFrom(src => src.Leaves.Count))
            .ForMember(dest => dest.PendingLeaves, opt => opt.MapFrom(src => src.Leaves.Count(l => l.Status == LeaveStatus.Pending)))
            .ForMember(dest => dest.ApprovedLeaves, opt => opt.MapFrom(src => src.Leaves.Count(l => l.Status == LeaveStatus.Approved)))
            .ForMember(dest => dest.TotalDaysUsed, opt => opt.MapFrom(src => src.Leaves.Where(l => l.Status == LeaveStatus.Approved).Sum(l => l.TotalDays)))
            .ForMember(dest => dest.RecentLeaves, opt => opt.MapFrom(src => src.Leaves.OrderByDescending(l => l.RequestDate).Take(5)))
            .ForMember(dest => dest.Balances, opt => opt.MapFrom(src => src.LeaveBalances));

        CreateMap<LeaveTypeCreateDto, LeaveType>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.Leaves, opt => opt.Ignore())
            .ForMember(dest => dest.LeaveBalances, opt => opt.Ignore());

        CreateMap<LeaveTypeUpdateDto, LeaveType>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Leaves, opt => opt.Ignore())
            .ForMember(dest => dest.LeaveBalances, opt => opt.Ignore());

        CreateMap<LeaveType, LeaveTypeUpdateDto>();

        // Leave Mappings
        CreateMap<Leave, LeaveListDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => $"{src.Person.FirstName} {src.Person.LastName}"))
            .ForMember(dest => dest.EmployeeNumber, opt => opt.MapFrom(src => src.Person.EmployeeNumber ?? ""))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Person.Department != null ? src.Person.Department.Name : ""))
            .ForMember(dest => dest.LeaveTypeName, opt => opt.MapFrom(src => src.LeaveType.Name))
            .ForMember(dest => dest.LeaveTypeColor, opt => opt.MapFrom(src => src.LeaveType.Color))
            .ForMember(dest => dest.StatusText, opt => opt.MapFrom(src => GetLeaveStatusText(src.Status)))
            .ForMember(dest => dest.ApprovedByName, opt => opt.MapFrom(src => src.ApprovedBy != null ? $"{src.ApprovedBy.FirstName} {src.ApprovedBy.LastName}" : null))
            .ForMember(dest => dest.RequiresDocument, opt => opt.MapFrom(src => src.LeaveType.RequiresDocument))
            .ForMember(dest => dest.HasDocument, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.DocumentPath)))
            .ForMember(dest => dest.DaysUntilStart, opt => opt.MapFrom(src => (src.StartDate - DateTime.Today).Days))
            .ForMember(dest => dest.IsUrgent, opt => opt.MapFrom(src => src.Status == LeaveStatus.Pending && (src.StartDate - DateTime.Today).Days <= src.LeaveType.NotificationDays));

        CreateMap<Leave, LeaveDetailDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => $"{src.Person.FirstName} {src.Person.LastName}"))
            .ForMember(dest => dest.EmployeeNumber, opt => opt.MapFrom(src => src.Person.EmployeeNumber ?? ""))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Person.Department != null ? src.Person.Department.Name : ""))
            .ForMember(dest => dest.LeaveTypeName, opt => opt.MapFrom(src => src.LeaveType.Name))
            .ForMember(dest => dest.LeaveTypeColor, opt => opt.MapFrom(src => src.LeaveType.Color))
            .ForMember(dest => dest.LeaveTypeRequiresDocument, opt => opt.MapFrom(src => src.LeaveType.RequiresDocument))
            .ForMember(dest => dest.StatusText, opt => opt.MapFrom(src => GetLeaveStatusText(src.Status)))
            .ForMember(dest => dest.ApprovedByName, opt => opt.MapFrom(src => src.ApprovedBy != null ? $"{src.ApprovedBy.FirstName} {src.ApprovedBy.LastName}" : null))
            .ForMember(dest => dest.HandoverToPersonName, opt => opt.MapFrom(src => src.HandoverToPerson != null ? $"{src.HandoverToPerson.FirstName} {src.HandoverToPerson.LastName}" : null));

        CreateMap<LeaveCreateDto, Leave>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => LeaveStatus.Pending))
            .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Person, opt => opt.Ignore())
            .ForMember(dest => dest.LeaveType, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedBy, opt => opt.Ignore())
            .ForMember(dest => dest.HandoverToPerson, opt => opt.Ignore());

        CreateMap<LeaveUpdateDto, Leave>()
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.RequestDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Person, opt => opt.Ignore())
            .ForMember(dest => dest.LeaveType, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedBy, opt => opt.Ignore())
            .ForMember(dest => dest.HandoverToPerson, opt => opt.Ignore());

        CreateMap<Leave, LeaveUpdateDto>();

        CreateMap<Leave, LeaveCalendarDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => $"{src.Person.FirstName} {src.Person.LastName}"))
            .ForMember(dest => dest.LeaveTypeName, opt => opt.MapFrom(src => src.LeaveType.Name))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.LeaveType.Color));

        // LeaveBalance Mappings
        CreateMap<LeaveBalance, LeaveBalanceListDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => $"{src.Person.FirstName} {src.Person.LastName}"))
            .ForMember(dest => dest.EmployeeNumber, opt => opt.MapFrom(src => src.Person.EmployeeNumber ?? ""))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Person.Department != null ? src.Person.Department.Name : ""))
            .ForMember(dest => dest.LeaveTypeName, opt => opt.MapFrom(src => src.LeaveType.Name))
            .ForMember(dest => dest.LeaveTypeColor, opt => opt.MapFrom(src => src.LeaveType.Color));

        CreateMap<LeaveBalance, LeaveBalanceDetailDto>()
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => $"{src.Person.FirstName} {src.Person.LastName}"))
            .ForMember(dest => dest.EmployeeNumber, opt => opt.MapFrom(src => src.Person.EmployeeNumber ?? ""))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Person.Department != null ? src.Person.Department.Name : ""))
            .ForMember(dest => dest.LeaveTypeName, opt => opt.MapFrom(src => src.LeaveType.Name))
            .ForMember(dest => dest.LeaveTypeColor, opt => opt.MapFrom(src => src.LeaveType.Color))
            .ForMember(dest => dest.AdjustedByName, opt => opt.MapFrom(src => "TBD")); // Temporarily set to placeholder

        CreateMap<LeaveBalanceCreateDto, LeaveBalance>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UsedDays, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.PendingDays, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.AccruedToDate, opt => opt.MapFrom(src => src.AllocatedDays))
            .ForMember(dest => dest.LastAccrualDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Person, opt => opt.Ignore())
            .ForMember(dest => dest.LeaveType, opt => opt.Ignore());
            // .ForMember(dest => dest.AdjustedBy, opt => opt.Ignore()); // Temporarily commented

        CreateMap<LeaveBalanceUpdateDto, LeaveBalance>()
            .ForMember(dest => dest.PersonId, opt => opt.Ignore())
            .ForMember(dest => dest.LeaveTypeId, opt => opt.Ignore())
            .ForMember(dest => dest.Year, opt => opt.Ignore())
            .ForMember(dest => dest.UsedDays, opt => opt.Ignore())
            .ForMember(dest => dest.PendingDays, opt => opt.Ignore())
            .ForMember(dest => dest.AdjustmentDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Person, opt => opt.Ignore())
            .ForMember(dest => dest.LeaveType, opt => opt.Ignore());
            // .ForMember(dest => dest.AdjustedBy, opt => opt.Ignore()); // Temporarily commented

        CreateMap<LeaveBalance, LeaveBalanceUpdateDto>();
    }

    private static string GetLeaveStatusText(LeaveStatus status)
    {
        return status switch
        {
            LeaveStatus.Pending => "Onay Bekliyor",
            LeaveStatus.Approved => "Onaylandı",
            LeaveStatus.Rejected => "Reddedildi",
            LeaveStatus.Cancelled => "İptal Edildi",
            LeaveStatus.InProgress => "Devam Ediyor",
            LeaveStatus.Completed => "Tamamlandı",
            _ => status.ToString()
        };
    }
}
