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

        // CV Job Application mappings
        CreateMap<JobApplicationListDto, JobApplicationListViewModel>();
        CreateMap<JobApplicationDetailDto, JobApplicationDetailViewModel>();
        CreateMap<JobApplicationDetailDto, JobApplicationEditViewModel>();
        CreateMap<ApplicationDocumentDto, ApplicationDocumentViewModel>()
            .ForMember(dest => dest.DocumentTypeName, opt => opt.MapFrom(src => src.DocumentType.ToString()))
            .ForMember(dest => dest.FileSizeFormatted, opt => opt.MapFrom(src => FormatFileSize(src.FileSize)));

        CreateMap<JobApplicationCreateViewModel, JobApplicationCreateDto>();
        CreateMap<JobApplicationEditViewModel, JobApplicationUpdateDto>();
        CreateMap<PublicJobApplicationViewModel, JobApplicationCreateDto>()
            .ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => src.JobPostingId))
            .ForMember(dest => dest.CvFile, opt => opt.Ignore())
            .ForMember(dest => dest.AdditionalDocuments, opt => opt.Ignore());
        CreateMap<JobApplicationFilterViewModel, JobApplicationFilterDto>();

        // Job Posting mappings
        CreateMap<JobPostingListDto, JobPostingListViewModel>()
            .ForMember(dest => dest.EmploymentTypeName, opt => opt.MapFrom(src => src.EmploymentType.ToString()))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.StatusClass, opt => opt.MapFrom(src => GetStatusClass(src.Status)))
            .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.ExpiryDate.HasValue && src.ExpiryDate.Value < DateTime.Now))
            .ForMember(dest => dest.IsApplicationDeadlinePassed, opt => opt.MapFrom(src => src.LastApplicationDate.HasValue && src.LastApplicationDate.Value < DateTime.Now))
            .ForMember(dest => dest.DaysUntilExpiry, opt => opt.MapFrom(src => CalculateDaysUntilExpiry(src.ExpiryDate)))
            .ForMember(dest => dest.SalaryRange, opt => opt.MapFrom(src => FormatSalaryRange(src.MinSalary, src.MaxSalary)));

        CreateMap<JobPostingDetailDto, JobPostingDetailViewModel>()
            .ForMember(dest => dest.EmploymentTypeName, opt => opt.MapFrom(src => src.EmploymentType.ToString()))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.StatusClass, opt => opt.MapFrom(src => GetStatusClass(src.Status)))
            .ForMember(dest => dest.MinEducationName, opt => opt.MapFrom(src => src.MinEducation.ToString()))
            .ForMember(dest => dest.TagList, opt => opt.MapFrom(src => src.Tags))
            .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.ExpiryDate.HasValue && src.ExpiryDate.Value < DateTime.Now))
            .ForMember(dest => dest.IsApplicationDeadlinePassed, opt => opt.MapFrom(src => src.LastApplicationDate.HasValue && src.LastApplicationDate.Value < DateTime.Now))
            .ForMember(dest => dest.DaysUntilExpiry, opt => opt.MapFrom(src => CalculateDaysUntilExpiry(src.ExpiryDate)))
            .ForMember(dest => dest.SalaryRange, opt => opt.MapFrom(src => FormatSalaryRange(src.MinSalary, src.MaxSalary)));

        CreateMap<JobPostingDetailDto, JobPostingEditViewModel>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags));

        CreateMap<PublicJobPostingDto, PublicJobPostingDetailViewModel>()
            .ForMember(dest => dest.EmploymentTypeName, opt => opt.MapFrom(src => src.EmploymentType.ToString()))
            .ForMember(dest => dest.MinEducationName, opt => opt.MapFrom(src => src.MinEducation.ToString()))
            .ForMember(dest => dest.TagList, opt => opt.MapFrom(src => src.Tags))
            .ForMember(dest => dest.ExperienceRange, opt => opt.MapFrom(src => FormatExperienceRange(src.MinExperience, src.MaxExperience)))
            .ForMember(dest => dest.SalaryRange, opt => opt.MapFrom(src => FormatSalaryRange(src.MinSalary, src.MaxSalary)))
            .ForMember(dest => dest.IsNew, opt => opt.MapFrom(src => src.PublishDate >= DateTime.Now.AddDays(-7)))
            .ForMember(dest => dest.IsUrgent, opt => opt.MapFrom(src => src.LastApplicationDate.HasValue && src.LastApplicationDate.Value <= DateTime.Now.AddDays(3)))
            .ForMember(dest => dest.DaysRemaining, opt => opt.MapFrom(src => CalculateDaysUntilExpiry(src.LastApplicationDate ?? src.ExpiryDate)))
            .ForMember(dest => dest.CanApply, opt => opt.MapFrom(src => src.Status == DAL.Entities.JobPostingStatus.Active && 
                (!src.LastApplicationDate.HasValue || src.LastApplicationDate.Value > DateTime.Now) &&
                (!src.ExpiryDate.HasValue || src.ExpiryDate.Value > DateTime.Now)));

        CreateMap<PublicJobPostingDto, PublicJobPostingListViewModel>()
            .ForMember(dest => dest.EmploymentTypeName, opt => opt.MapFrom(src => src.EmploymentType.ToString()))
            .ForMember(dest => dest.TagList, opt => opt.MapFrom(src => src.Tags))
            .ForMember(dest => dest.SalaryRange, opt => opt.MapFrom(src => FormatSalaryRange(src.MinSalary, src.MaxSalary)))
            .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src.Description.Length > 150 ? src.Description.Substring(0, 150) + "..." : src.Description))
            .ForMember(dest => dest.IsNew, opt => opt.MapFrom(src => src.PublishDate >= DateTime.Now.AddDays(-7)))
            .ForMember(dest => dest.IsUrgent, opt => opt.MapFrom(src => src.LastApplicationDate.HasValue && src.LastApplicationDate.Value <= DateTime.Now.AddDays(3)))
            .ForMember(dest => dest.DaysRemaining, opt => opt.MapFrom(src => CalculateDaysUntilExpiry(src.LastApplicationDate ?? src.ExpiryDate)));

        CreateMap<JobPostingCreateViewModel, JobPostingCreateDto>();
        CreateMap<JobPostingEditViewModel, JobPostingUpdateDto>();
        CreateMap<JobPostingFilterViewModel, JobPostingFilterDto>();
        CreateMap<PublicJobPostingFilterViewModel, JobPostingFilterDto>();

        // Summary and Statistics mappings
        CreateMap<JobApplicationSummaryDto, JobApplicationSummaryViewModel>();
        CreateMap<PositionApplicationSummaryDto, PositionApplicationSummaryViewModel>();
        CreateMap<MonthlyApplicationSummaryDto, MonthlyApplicationSummaryViewModel>();
        CreateMap<JobPostingSummaryDto, JobPostingSummaryViewModel>();
        CreateMap<DepartmentPostingSummaryDto, DepartmentPostingSummaryViewModel>();
        CreateMap<MonthlyPostingSummaryDto, MonthlyPostingSummaryViewModel>();
        
        // Job Definition ViewModels
        CreateMap<JobDefinitionListDto, JobDefinitionListViewModel>();
        CreateMap<JobDefinitionDetailDto, JobDefinitionDetailViewModel>();
        CreateMap<JobDefinitionCreateViewModel, JobDefinitionCreateDto>();
        CreateMap<JobDefinitionEditViewModel, JobDefinitionUpdateDto>();
        CreateMap<JobDefinitionDetailDto, JobDefinitionEditViewModel>();
        CreateMap<JobDefinitionFilterViewModel, JobDefinitionFilterDto>();

        // Job Definition Qualification ViewModels
        CreateMap<JobDefinitionQualificationDto, JobDefinitionQualificationViewModel>();
        CreateMap<JobDefinitionQualificationCreateViewModel, JobDefinitionQualificationCreateDto>();

        // Job Required Skill ViewModels
        CreateMap<JobRequiredSkillCreateViewModel, JobRequiredSkillCreateDto>();

        // Qualification Matching Result ViewModels
        CreateMap<QualificationMatchingResultDto, QualificationMatchingResultViewModel>();

        // Job Definition Summary ViewModels
        CreateMap<JobDefinitionSummaryDto, JobDefinitionSummaryViewModel>();

        // Skill Management ViewModels - Added for SkillManagement module
        CreateMap<SkillAnalyticsDto, SkillAnalyticsViewModel>();
        CreateMap<SkillTemplateListDto, SkillTemplateListViewModel>();
        CreateMap<SkillTemplateDetailDto, SkillTemplateDetailViewModel>();
        CreateMap<SkillTemplateCreateViewModel, SkillTemplateCreateDto>();
        CreateMap<SkillTemplateFilterViewModel, SkillTemplateFilterDto>();
        CreateMap<PersonSkillDto, PersonSkillListViewModel>()
            .ForMember(dest => dest.YearsOfExperience, opt => opt.MapFrom(src => src.ExperienceYears))
            .ForMember(dest => dest.HasCertification, opt => opt.MapFrom(src => src.IsCertified))
            .ForMember(dest => dest.CertificationDetails, opt => opt.MapFrom(src => src.IsCertified ? 
                (src.CertificationAuthority + (src.CertificationDate.HasValue ? $" ({src.CertificationDate.Value:dd.MM.yyyy})" : "")) : null));
        CreateMap<PersonSkillFilterViewModel, PersonSkillFilterDto>();
        CreateMap<JobRequiredSkillDto, JobRequiredSkillViewModel>();
        CreateMap<PersonSkillCreateViewModel, PersonSkillCreateDto>();
        CreateMap<PersonSkillUpdateViewModel, PersonSkillUpdateDto>();
    }

    // Helper methods for mapping
    private static string FormatFileSize(long bytes)
    {
        if (bytes == 0) return "0 B";
        string[] sizes = { "B", "KB", "MB", "GB" };
        int order = 0;
        double len = bytes;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }

    private static string GetStatusClass(DAL.Entities.JobPostingStatus status)
    {
        return status switch
        {
            DAL.Entities.JobPostingStatus.Draft => "badge bg-secondary",
            DAL.Entities.JobPostingStatus.Active => "badge bg-success",
            DAL.Entities.JobPostingStatus.Suspended => "badge bg-warning",
            DAL.Entities.JobPostingStatus.Closed => "badge bg-danger",
            DAL.Entities.JobPostingStatus.Expired => "badge bg-dark",
            _ => "badge bg-light"
        };
    }

    private static int CalculateDaysUntilExpiry(DateTime? expiryDate)
    {
        if (!expiryDate.HasValue) return 0;
        var days = (expiryDate.Value - DateTime.Now).Days;
        return Math.Max(0, days);
    }

    private static string FormatSalaryRange(decimal? minSalary, decimal? maxSalary)
    {
        if (minSalary.HasValue && maxSalary.HasValue)
            return $"{minSalary:N0} - {maxSalary:N0} TL";
        else if (minSalary.HasValue)
            return $"{minSalary:N0} TL+";
        else if (maxSalary.HasValue)
            return $"Max {maxSalary:N0} TL";
        else
            return "Maaş belirtilmemiş";
    }

    private static string FormatExperienceRange(int? minExperience, int? maxExperience)
    {
        if (minExperience.HasValue && maxExperience.HasValue)
            return $"{minExperience}-{maxExperience} yıl";
        else if (minExperience.HasValue)
            return $"{minExperience}+ yıl";
        else if (maxExperience.HasValue)
            return $"Max {maxExperience} yıl";
        else
            return "Deneyim belirtilmemiş";
    }
}
