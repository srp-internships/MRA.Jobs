using AutoMapper;
using MRA.Jobs.Application.Contracts.Applications.Commands;
using MRA.Jobs.Application.Contracts.Applications.Responses;

namespace MRA.Jobs.Application.Features.Applications;
using MRA.Jobs.Domain.Entities;

public class ApplicationProfile: Profile
{
    public ApplicationProfile()
    {
        CreateMap<CreateApplicationCommand, Application>();
        CreateMap<UpdateApplicationCommand, Application>();
        CreateMap<List<Application>, List<ApplicationResponse>>();
        CreateMap<Application,  ApplicationResponse>();
    }
}
