using Microsoft.AspNetCore.Components.Forms;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Application.Contracts.Applications.Candidates;
using MRA.Jobs.Application.Contracts.Applications.Commands.AddNote;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplicationStatus;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TimeLineDTO;
using static MRA.Jobs.Application.Contracts.Dtos.Enums.ApplicationStatusDto;

namespace MRA.Jobs.Client.Services.ApplicationService;

public interface IApplicationService
{
    Task<List<ApplicationListStatus>> GetApplicationsByStatus(ApplicationStatus status);
    Task CreateApplication(CreateApplicationCommand application, IBrowserFile cv);
    Task<PagedList<ApplicationListDto>> GetAllApplications();
    Task<bool> UpdateStatus(UpdateApplicationStatusCommand updateApplicationStatusCommand);
    Task<ApplicationDetailsDto> GetApplicationDetails(string applicationSlug);
    Task<string> GetCvLinkAsync(string slug);

    Task<TimeLineDetailsDto> AddNote(AddNoteToApplicationCommand note);
    Task<List<UserResponse>> GetCandidates(GetCandidatesQuery query);
}