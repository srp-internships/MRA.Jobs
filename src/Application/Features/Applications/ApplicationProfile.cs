using MRA.Jobs.Application.Contracts.Applications.Commands;
using MRA.Jobs.Application.Contracts.Applications.Responses;

namespace MRA.Jobs.Application.Features.Applications;
using MRA.Jobs.Domain.Entities;

public class ApplicationProfile: Profile
{
    public ApplicationProfile()
    {
        CreateMap<Application, ApplicationListDTO>();
        CreateMap<Application, ApplicationDetailsDTO>();
        CreateMap<CreateApplicationCommand, Application>();
        CreateMap<CreateApplicationWithoutApplicantIdCommand, Application>();
        CreateMap<UpdateApplicationCommand, Application>();
    }
}
