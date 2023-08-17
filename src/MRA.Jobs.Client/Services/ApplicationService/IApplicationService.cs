using MRA.Jobs.Application.Contracts.Applications.Commands;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Client.Services.ApplicationService;

public interface IApplicationService
{
    Task<List<ApplicationListStatus>> GetApplicationsByStatus(ApplicationStatus status);
    Task<HttpResponseMessage> CreateApplicate(CreateApplicationCommand command);
}