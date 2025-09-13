using AutoMapper;
using BLL.DTOs;
using DAL.Entities;

namespace BLL.Mapping;

public class CandidateMappingProfile : Profile
{
    public CandidateMappingProfile()
    {
        // Candidate Mappings
        CreateMap<Candidate, CandidateListDto>()
            .ForMember(dest => dest.JobApplicationCount, opt => opt.MapFrom(src => src.JobApplications.Count));

        CreateMap<Candidate, CandidateDetailDto>()
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => 
                src.BirthDate.HasValue ? DateTime.Now.Year - src.BirthDate.Value.Year : 0))
            .ForMember(dest => dest.Educations, opt => opt.MapFrom(src => src.Educations))
            .ForMember(dest => dest.Experiences, opt => opt.MapFrom(src => src.Experiences))
            .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.Skills))
            .ForMember(dest => dest.JobApplications, opt => opt.MapFrom(src => src.JobApplications));

        CreateMap<CandidateCreateDto, Candidate>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

        CreateMap<CandidateUpdateDto, Candidate>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

        // CandidateEducation Mappings
        CreateMap<CandidateEducation, CandidateEducationDto>();
        CreateMap<CandidateEducationCreateDto, CandidateEducation>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        CreateMap<CandidateEducationUpdateDto, CandidateEducation>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

        // CandidateExperience Mappings
        CreateMap<CandidateExperience, CandidateExperienceDto>();
        CreateMap<CandidateExperienceCreateDto, CandidateExperience>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        CreateMap<CandidateExperienceUpdateDto, CandidateExperience>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

        // CandidateSkill Mappings
        CreateMap<CandidateSkill, CandidateSkillDto>();
        CreateMap<CandidateSkillCreateDto, CandidateSkill>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        CreateMap<CandidateSkillUpdateDto, CandidateSkill>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));
    }
}
