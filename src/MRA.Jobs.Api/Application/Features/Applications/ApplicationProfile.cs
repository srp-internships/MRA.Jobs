using MRA.Jobs.Application.Contracts.Applications.Responses;

namespace MRA.Jobs.Application.Features.Applications;

using MRA.Jobs.Application;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplication;
using MRA.Jobs.Application.Contracts.Dtos.Enums;
using MRA.Jobs.Application.Contracts.TimeLineDTO;
using MRA.Jobs.Domain.Entities;

public class ApplicationProfile : Profile
{
    private readonly ICurrentUserService _currentUserService;

    public ApplicationProfile(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }
    public ApplicationProfile()
    {
        CreateMap<Application, ApplicationListDto>()
            .ForMember(dest => dest.VacancyResponses, opt => opt.MapFrom(src => src.VacancyResponses))
            .ForMember(dest => dest.TaskResponses, opt => opt.MapFrom(src => src.TaskResponses))
            .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.VacancyTitle, opt => opt.MapFrom(src => src.Vacancy.Title))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.ApplicantUsername));

        CreateMap<Application, ApplicationDetailsDto>()
            .ForMember(dest => dest.VacancyResponses, opt => opt.MapFrom(src => src.VacancyResponses))
            .ForMember(dest => dest.TaskResponses, opt => opt.MapFrom(src => src.TaskResponses))
            .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History));
        CreateMap<CreateApplicationCommand, Application>()
            .ForMember(dest => dest.VacancyResponses, opt => opt.MapFrom(src => src.VacancyResponses));

        CreateMap<CreateApplicationWithoutApplicantIdCommand, Application>();
        CreateMap<UpdateApplicationCommand, Application>();
        MappingConfiguration.ConfigureUserMap<ApplicationTimelineEvent, TimeLineDetailsDto>(this);
        CreateMap<Application, ApplicationListStatus>()
            .ForMember(dest => dest.VacancyTitle, opt => opt.MapFrom(src => src.Vacancy.Title));
        CreateMap<ApplicationStatus, ApplicationStatusDto.ApplicationStatus>();
        //CreateMap<Domain.Entities.VacancyQuestion, VacancyQuestionDto>();
    }
}