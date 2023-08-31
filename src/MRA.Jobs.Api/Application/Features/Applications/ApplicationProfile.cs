using MRA.Jobs.Application.Contracts.Applications.Commands;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.TimeLineDTO;

namespace MRA.Jobs.Application.Features.Applications;

using MRA.Jobs.Application;
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
        CreateMap<Domain.Entities.Application, ApplicationListDto>();
        CreateMap<Domain.Entities.Application, ApplicationDetailsDto>()
            .ForMember(dest => dest.Histiry, opt => opt.MapFrom(src => src.History));
        CreateMap<CreateApplicationCommand, Domain.Entities.Application>();
        CreateMap<CreateApplicationWithoutApplicantIdCommand, Domain.Entities.Application>();
        CreateMap<UpdateApplicationCommand, Domain.Entities.Application>();
        MappingConfiguration.ConfigureUserMap<ApplicationTimelineEvent, TimeLineDetailsDto>(this);
        CreateMap<Domain.Entities.Application, ApplicationListStatus>()
            .ForMember(dest => dest.VacancyTitle, opt => opt.MapFrom(src => src.Vacancy.Title));
        CreateMap<ApplicationStatus, ApplicationStatusDto.ApplicationStatus>();
        CreateMap<Domain.Entities.VacancyQuestion, VacancyQuestionDto>();
    }
}