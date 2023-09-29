using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplicationStatus;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Common;
using static MRA.Jobs.Application.Contracts.Dtos.Enums.ApplicationStatusDto;

namespace MRA.Jobs.Client.Services.ApplicationService;

public interface IApplicationService
{
    Task<List<ApplicationListStatus>> GetApplicationsByStatus(ApplicationStatus status);
    Task CreateApplication(CreateApplicationCommand application);
    Task<PagedList<ApplicationListDto>> GetAllApplications();
    Task<bool> UpdateStatus(UpdateApplicationStatus updateApplicationStatus);
    Task<ApplicationDetailsDto> GetApplicationDetails(string ApplicationSlug);
    Task<bool> ApplicationExist(string userName, string vacancySlug);
}