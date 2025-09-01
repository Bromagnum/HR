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
    }
}
