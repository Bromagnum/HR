using AutoMapper;
using BLL.DTOs;
using DAL.Entities;

namespace BLL.Mapping;

public class JobApplicationMappingProfile : Profile
{
    public JobApplicationMappingProfile()
    {
        // JobApplication Mappings
        CreateMap<JobApplication, JobApplicationListDto>()
            .ForMember(dest => dest.CandidateName, opt => opt.MapFrom(src => src.Candidate != null ? src.Candidate.FullName : ""))
            .ForMember(dest => dest.CandidateEmail, opt => opt.MapFrom(src => src.Candidate != null ? src.Candidate.Email : ""))
            .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position != null ? src.Position.Name : ""))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Position != null && src.Position.Department != null ? src.Position.Department.Name : ""))
            .ForMember(dest => dest.ReviewedByName, opt => opt.MapFrom(src => src.ReviewedBy != null ? src.ReviewedBy.FullName : null))
            .ForMember(dest => dest.InterviewerName, opt => opt.MapFrom(src => src.Interviewer != null ? src.Interviewer.FullName : null));

        CreateMap<JobApplication, JobApplicationDetailDto>()
            .ForMember(dest => dest.Candidate, opt => opt.MapFrom(src => src.Candidate))
            .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
            .ForMember(dest => dest.ReviewedByName, opt => opt.MapFrom(src => src.ReviewedBy != null ? src.ReviewedBy.FullName : null))
            .ForMember(dest => dest.InterviewerName, opt => opt.MapFrom(src => src.Interviewer != null ? src.Interviewer.FullName : null))
            .ForMember(dest => dest.DecisionByName, opt => opt.MapFrom(src => src.DecisionBy != null ? src.DecisionBy.FullName : null))
            .ForMember(dest => dest.LastViewedByName, opt => opt.MapFrom(src => src.LastViewedBy != null ? src.LastViewedBy.FullName : null))
            .ForMember(dest => dest.InterviewNotes, opt => opt.MapFrom(src => src.InterviewNotes))
            .ForMember(dest => dest.Documents, opt => opt.MapFrom(src => src.Documents));

        CreateMap<JobApplicationCreateDto, JobApplication>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ApplicationStatus.Applied))
            .ForMember(dest => dest.ApplicationDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

        CreateMap<JobApplicationUpdateDto, JobApplication>()
            .ForMember(dest => dest.ApplicationDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

        // InterviewNote Mappings
        CreateMap<InterviewNote, InterviewNoteDto>()
            .ForMember(dest => dest.CandidateName, opt => opt.MapFrom(src => src.Candidate != null ? src.Candidate.FullName : null))
            .ForMember(dest => dest.InterviewerName, opt => opt.MapFrom(src => src.Interviewer != null ? src.Interviewer.FullName : ""));

        CreateMap<InterviewNoteCreateDto, InterviewNote>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

        CreateMap<InterviewNoteUpdateDto, InterviewNote>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

        // ApplicationDocument Mappings
        CreateMap<ApplicationDocument, ApplicationDocumentDto>()
            .ForMember(dest => dest.UploadedByName, opt => opt.MapFrom(src => src.UploadedBy != null ? src.UploadedBy.FullName : null))
            .ForMember(dest => dest.VerifiedByName, opt => opt.MapFrom(src => src.VerifiedBy != null ? src.VerifiedBy.FullName : null))
            .ForMember(dest => dest.LastDownloadedByName, opt => opt.MapFrom(src => src.LastDownloadedBy != null ? src.LastDownloadedBy.FullName : null));

        CreateMap<ApplicationDocumentCreateDto, ApplicationDocument>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UploadDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.DownloadCount, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => false));

        CreateMap<ApplicationDocumentUpdateDto, ApplicationDocument>()
            .ForMember(dest => dest.UploadDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

        // Interview Schedule Mapping
        CreateMap<InterviewNote, InterviewScheduleDto>()
            .ForMember(dest => dest.CandidateName, opt => opt.MapFrom(src => src.Candidate != null ? src.Candidate.FullName : ""))
            .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.JobApplication != null && src.JobApplication.Position != null ? src.JobApplication.Position.Name : ""))
            .ForMember(dest => dest.InterviewerName, opt => opt.MapFrom(src => src.Interviewer != null ? src.Interviewer.FullName : ""));
    }
}
