using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplication;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Dtos.Enums;
using MRA.Jobs.Application.Contracts.TimeLineDTO;

namespace MRA.Jobs.Application.Features.Applications;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<Domain.Entities.Application, ApplicationListDto>()
            .ForMember(dest => dest.VacancyResponses, opt => opt.MapFrom(src => src.VacancyResponses))
            .ForMember(dest => dest.TaskResponses, opt => opt.MapFrom(src => src.TaskResponses))
            .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.VacancyTitle, opt => opt.MapFrom(src => src.Vacancy.Title))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.ApplicantUsername));

        CreateMap<Domain.Entities.Application, ApplicationDetailsDto>()
            .ForMember(dest => dest.VacancyResponses, opt => opt.MapFrom(src => src.VacancyResponses))
            .ForMember(dest => dest.TaskResponses, opt => opt.MapFrom(src => src.TaskResponses))
            .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History));
        CreateMap<CreateApplicationCommand, Domain.Entities.Application>()
            .ForMember(dest => dest.VacancyResponses, opt => opt.MapFrom(src => src.VacancyResponses))
            .ForMember(dest => dest.CV, opt => opt.Ignore());
        CreateMap<CreateApplicationNoVacancyCommand, Domain.Entities.Application>()
            .ForMember(dest => dest.VacancyResponses, opt => opt.MapFrom(src => src.VacancyResponses))
            .ForMember(dest => dest.CV, opt => opt.Ignore());
        CreateMap<UpdateApplicationCommand, Domain.Entities.Application>();
        MappingConfiguration.ConfigureUserMap<ApplicationTimelineEvent, TimeLineDetailsDto>(this);
        CreateMap<Domain.Entities.Application, ApplicationListStatus>()
            .ForMember(dest => dest.VacancyTitle, opt => opt.MapFrom(src => src.Vacancy.Title));
        CreateMap<ApplicationStatus, ApplicationStatusDto.ApplicationStatus>();
    }
}