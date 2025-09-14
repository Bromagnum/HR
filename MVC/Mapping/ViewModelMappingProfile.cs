using AutoMapper;
using BLL.DTOs;
using MVC.Models;

namespace MVC.Mapping;

public class ViewModelMappingProfile : Profile
{
    public ViewModelMappingProfile()
    {
        // Person DTO to ViewModel mappings
        CreateMap<PersonListDto, PersonListViewModel>();
        CreateMap<PersonDetailDto, PersonDetailViewModel>();
        CreateMap<PersonDetailDto, PersonEditViewModel>();
        
        // ViewModel to DTO mappings
        CreateMap<PersonCreateViewModel, PersonCreateDto>();
        CreateMap<PersonEditViewModel, PersonUpdateDto>();
        
        // Department DTO to ViewModel mappings
        CreateMap<DepartmentListDto, DepartmentListViewModel>()
            .ForMember(dest => dest.Level, opt => opt.Ignore())
            .ForMember(dest => dest.HasChildren, opt => opt.Ignore());
            
        CreateMap<DepartmentDetailDto, DepartmentDetailViewModel>();
        CreateMap<DepartmentDetailDto, DepartmentEditViewModel>();
        
        // ViewModel to DTO mappings
        CreateMap<DepartmentCreateViewModel, DepartmentCreateDto>();
        CreateMap<DepartmentEditViewModel, DepartmentUpdateDto>();
        
        // Filter and Search mappings
        CreateMap<DepartmentFilterViewModel, DepartmentFilterDto>();
        CreateMap<DepartmentFilterDto, DepartmentFilterViewModel>();
        CreateMap<DepartmentSearchResultDto, DepartmentSearchResultViewModel>();
        
        // Education DTO to ViewModel mappings
        CreateMap<EducationListDto, EducationListViewModel>();
        CreateMap<EducationDetailDto, EducationDetailViewModel>();
        CreateMap<EducationDetailDto, EducationEditViewModel>();
        
        // ViewModel to DTO mappings
        CreateMap<EducationCreateViewModel, EducationCreateDto>();
        CreateMap<EducationEditViewModel, EducationUpdateDto>();
        
        // Qualification DTO to ViewModel mappings
        CreateMap<QualificationListDto, QualificationListViewModel>();
        CreateMap<QualificationDetailDto, QualificationDetailViewModel>();
        CreateMap<QualificationDetailDto, QualificationEditViewModel>();
        
        // ViewModel to DTO mappings
        CreateMap<QualificationCreateViewModel, QualificationCreateDto>();
        CreateMap<QualificationEditViewModel, QualificationUpdateDto>();
        
        // Position DTO to ViewModel mappings
        CreateMap<PositionListDto, PositionListViewModel>();
        CreateMap<PositionDetailDto, PositionDetailViewModel>();
        CreateMap<PositionDetailDto, PositionUpdateViewModel>();
        CreateMap<PositionPersonDto, PositionPersonViewModel>();
        
        // ViewModel to DTO mappings
        CreateMap<PositionCreateViewModel, PositionCreateDto>();
        CreateMap<PositionUpdateViewModel, PositionUpdateDto>();

        // WorkLog DTO to ViewModel mappings
        CreateMap<WorkLogListDto, WorkLogListViewModel>();
        CreateMap<WorkLogDetailDto, WorkLogDetailViewModel>();
        CreateMap<WorkLogDetailDto, WorkLogUpdateViewModel>();
        CreateMap<WorkLogTimeSheetDto, WorkLogTimeSheetViewModel>()
            .ForMember(dest => dest.WorkLogs, opt => opt.MapFrom(src => src.WorkLogs));

        // ViewModel to DTO mappings
        CreateMap<WorkLogCreateViewModel, WorkLogCreateDto>();
        CreateMap<WorkLogUpdateViewModel, WorkLogUpdateDto>();
        CreateMap<WorkLogCheckInViewModel, WorkLogCheckInDto>();
        CreateMap<WorkLogCheckOutViewModel, WorkLogCheckOutDto>();
        CreateMap<WorkLogApprovalViewModel, WorkLogApprovalDto>();

        // LeaveType DTO to ViewModel mappings
        CreateMap<LeaveTypeListDto, LeaveTypeListViewModel>();
        CreateMap<LeaveTypeDetailDto, LeaveTypeDetailViewModel>();
        CreateMap<LeaveTypeDetailDto, LeaveTypeEditViewModel>();

        // ViewModel to DTO mappings
        CreateMap<LeaveTypeCreateViewModel, LeaveTypeCreateDto>();
        CreateMap<LeaveTypeEditViewModel, LeaveTypeUpdateDto>();

        // Leave DTO to ViewModel mappings
        CreateMap<LeaveListDto, LeaveListViewModel>();
        CreateMap<LeaveDetailDto, LeaveDetailViewModel>();
        CreateMap<LeaveDetailDto, LeaveEditViewModel>();
        CreateMap<LeaveCalendarDto, LeaveCalendarViewModel>();
        
        // Missing mapping for RecentLeaveViewModel
        CreateMap<LeaveListDto, RecentLeaveViewModel>()
            .ForMember(dest => dest.StatusText, opt => opt.MapFrom(src => src.StatusText));

        // ViewModel to DTO mappings
        CreateMap<LeaveCreateViewModel, LeaveCreateDto>();
        CreateMap<LeaveEditViewModel, LeaveUpdateDto>();
        CreateMap<LeaveApprovalViewModel, LeaveApprovalDto>();
        CreateMap<LeaveFilterViewModel, LeaveFilterDto>();

        // LeaveBalance DTO to ViewModel mappings
        CreateMap<LeaveBalanceListDto, LeaveBalanceListViewModel>();
        CreateMap<LeaveBalanceDetailDto, LeaveBalanceDetailViewModel>();
        CreateMap<LeaveBalanceDetailDto, LeaveBalanceEditViewModel>();
        CreateMap<LeaveBalanceSummaryDto, LeaveBalanceSummaryViewModel>()
            .ForMember(dest => dest.Balances, opt => opt.MapFrom(src => src.Balances));

        // ViewModel to DTO mappings
        CreateMap<LeaveBalanceCreateViewModel, LeaveBalanceCreateDto>();
        CreateMap<LeaveBalanceEditViewModel, LeaveBalanceUpdateDto>();
        CreateMap<LeaveBalanceAdjustmentViewModel, LeaveBalanceAdjustmentDto>();
        
        // Auth DTO to ViewModel mappings
        CreateMap<UserProfileDto, UserProfileViewModel>();
        CreateMap<LoginRequestDto, LoginViewModel>().ReverseMap();
        CreateMap<RegisterRequestDto, RegisterViewModel>().ReverseMap();

        // Payroll DTO to ViewModel mappings
        CreateMap<PayrollListDto, PayrollListViewModel>();
        CreateMap<PayrollDetailDto, PayrollDetailViewModel>();
        CreateMap<PayrollDetailDto, PayrollEditViewModel>();
        CreateMap<PayrollSummaryDto, PayrollSummaryViewModel>();
        CreateMap<PersonYearlyPayrollSummaryDto, PersonYearlyPayrollSummaryViewModel>();
        
        // ViewModel to DTO mappings
        CreateMap<PayrollCreateViewModel, PayrollCreateDto>();
        CreateMap<PayrollEditViewModel, PayrollUpdateDto>();
        CreateMap<PayrollFilterViewModel, PayrollFilterDto>();

        // TMK Organization mappings
        CreateMap<OrganizationListDto, OrganizationListViewModel>();
        CreateMap<OrganizationDetailDto, OrganizationDetailViewModel>();
        CreateMap<OrganizationDetailDto, OrganizationEditViewModel>();
        CreateMap<OrganizationTreeDto, OrganizationTreeViewModel>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "Organizasyon"))
            .ForMember(dest => dest.Level, opt => opt.MapFrom(src => 1))
            .ForMember(dest => dest.SubOrganizations, opt => opt.MapFrom(src => src.Children));
        
        CreateMap<OrganizationCreateViewModel, OrganizationCreateDto>();
        CreateMap<OrganizationEditViewModel, OrganizationUpdateDto>();
        CreateMap<OrganizationFilterViewModel, OrganizationFilterDto>();

        // TMK Material mappings
        CreateMap<MaterialListDto, MaterialListViewModel>();
        CreateMap<MaterialDetailDto, MaterialDetailViewModel>();
        CreateMap<MaterialDetailDto, MaterialEditViewModel>();
        CreateMap<MaterialStockSummaryDto, MaterialStockSummaryViewModel>();
        CreateMap<MaterialCategorySummaryDto, MaterialCategorySummaryViewModel>();
        
        CreateMap<MaterialCreateViewModel, MaterialCreateDto>();
        CreateMap<MaterialEditViewModel, MaterialUpdateDto>();
        CreateMap<MaterialFilterViewModel, MaterialFilterDto>();
    }
}
