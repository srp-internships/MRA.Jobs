using Microsoft.AspNetCore.Components.Forms;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplicationStatus;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Common;
using static MRA.Jobs.Application.Contracts.Dtos.Enums.ApplicationStatusDto;

namespace MRA.Jobs.Client.Services.ApplicationService;

public interface IApplicationService
{
    Task<List<ApplicationListStatus>> GetApplicationsByStatus(ApplicationStatus status);
    Task CreateApplication(CreateApplicationCommand application, IBrowserFile cv);
    Task<PagedList<ApplicationListDto>> GetAllApplications();
    Task<bool> UpdateStatus(UpdateApplicationStatus updateApplicationStatus);
    Task<ApplicationDetailsDto> GetApplicationDetails(string applicationSlug);
    Task<string> GetCvLinkAsync(string slug);
}