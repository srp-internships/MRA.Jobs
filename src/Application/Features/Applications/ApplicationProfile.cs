using MRA.Jobs.Application.Contracts.Applications.Commands;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.TimeLineDTO;

namespace MRA.Jobs.Application.Features.Applications;

public class ApplicationProfile : Profile
{
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
            .ForMember(dest => dest.ApplicantFullName,
                opt => opt.MapFrom(src => src.Applicant.FirstName + " " + src.Applicant.LastName))
            .ForMember(dest => dest.VacancyTitle, opt => opt.MapFrom(src => src.Vacancy.Title));
    }
}