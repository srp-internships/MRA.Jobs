using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.TimeLineDTO;

namespace MRA.Jobs.Application.Features.Applications;

using MRA.Jobs.Application;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplication;
using MRA.Jobs.Application.Contracts.Dtos;
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
        CreateMap<Domain.Entities.Application, ApplicationListDto>()
            .ForMember(dest => dest.VacancyResponses, opt => opt.MapFrom(src => src.VacancyResponses));
        CreateMap<Domain.Entities.Application, ApplicationDetailsDto>()
            .ForMember(dest => dest.VacancyResponses, opt => opt.MapFrom(src => src.VacancyResponses))
            .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History));
        CreateMap<CreateApplicationCommand, Domain.Entities.Application>()
            .ForMember(dest => dest.VacancyResponses, opt => opt.MapFrom(src => src.VacancyResponses));

        CreateMap<CreateApplicationWithoutApplicantIdCommand, Domain.Entities.Application>();
        CreateMap<UpdateApplicationCommand, Domain.Entities.Application>();
        MappingConfiguration.ConfigureUserMap<ApplicationTimelineEvent, TimeLineDetailsDto>(this);
        CreateMap<Domain.Entities.Application, ApplicationListStatus>()
            .ForMember(dest => dest.VacancyTitle, opt => opt.MapFrom(src => src.Vacancy.Title));
        CreateMap<ApplicationStatus, ApplicationStatusDto.ApplicationStatus>();
        //CreateMap<Domain.Entities.VacancyQuestion, VacancyQuestionDto>();
    }
}