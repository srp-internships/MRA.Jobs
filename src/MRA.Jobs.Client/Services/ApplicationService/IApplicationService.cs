using MRA.Jobs.Application.Contracts.Applications.Commands;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using static MRA.Jobs.Application.Contracts.Dtos.Enums.ApplicationStatusDto;

namespace MRA.Jobs.Client.Services.ApplicationService;

public interface IApplicationService
{
    Task<List<ApplicationListStatus>> GetApplicationsByStatus(ApplicationStatus status);
    Task CreateApplication(CreateApplicationCommand application);
}