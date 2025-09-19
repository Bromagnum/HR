using AutoMapper;
using BLL.DTOs;
using MVC.Models;

namespace MVC.Mapping;

public class MvcMappingProfile : Profile
{
    public MvcMappingProfile()
    {
        // Performance Review ViewModel Mappings
        CreateMap<PerformanceReviewCreateViewModel, PerformanceReviewCreateDto>();
        CreateMap<PerformanceReviewEditViewModel, PerformanceReviewUpdateDto>();
        CreateMap<PerformanceReviewDetailDto, PerformanceReviewEditViewModel>();
        CreateMap<SelfAssessmentViewModel, SelfAssessmentDto>();

        // Review Period ViewModel Mappings
        CreateMap<ReviewPeriodCreateViewModel, ReviewPeriodCreateDto>();
        CreateMap<ReviewPeriodEditViewModel, ReviewPeriodUpdateDto>();
        CreateMap<ReviewPeriodDetailDto, ReviewPeriodEditViewModel>();

        // Performance Goal ViewModel Mappings
        CreateMap<PerformanceGoalCreateViewModel, PerformanceGoalCreateDto>();
        CreateMap<PerformanceGoalEditViewModel, PerformanceGoalUpdateDto>();
        CreateMap<PerformanceGoalDto, PerformanceGoalEditViewModel>();
    }
}
