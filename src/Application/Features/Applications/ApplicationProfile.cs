using MRA.Jobs.Application.Contracts.Applications.Commands;
using MRA.Jobs.Application.Contracts.Applications.Responses;

namespace MRA.Jobs.Application.Features.Applications;

using MRA.Jobs.Application.Contracts.TimeLineDTO;
using MRA.Jobs.Domain.Entities;

public class ApplicationProfile: Profile
{
    public ApplicationProfile()
    {
        CreateMap<Application, ApplicationListDTO>();
        CreateMap<Application, ApplicationDetailsDTO>()
              .ForMember(dest => dest.Histiry, opt => opt.MapFrom(src => src.History));
        CreateMap<CreateApplicationCommand, Application>();
        CreateMap<CreateApplicationWithoutApplicantIdCommand, Application>();
        CreateMap<UpdateApplicationCommand, Application>();
        MappingConfiguration.ConfigureUserMap<ApplicationTimelineEvent, TimeLineDetailsDto>(this);
        CreateMap<Application, ApplicationListStatus>()
           .ForMember(dest => dest.ApplicantFullName, opt => opt.MapFrom(src => src.Applicant.FirstName + " " + src.Applicant.LastName))
           .ForMember(dest => dest.VacancyTitle, opt => opt.MapFrom(src => src.Vacancy.Title));
    }
}
