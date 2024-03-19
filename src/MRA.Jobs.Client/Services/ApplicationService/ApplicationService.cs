using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MRA.BlazorComponents.Configuration;
using MRA.BlazorComponents.HttpClient.Services;
using MRA.BlazorComponents.Snackbar.Extensions;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Application.Contracts.Applications.Candidates;
using MRA.Jobs.Application.Contracts.Applications.Commands.AddNote;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplicationStatus;
using MRA.Jobs.Application.Contracts.Applications.Queries.GetApplicationWithPagination;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TimeLineDTO;
using MRA.Jobs.Client.Identity;
using MRA.Jobs.Client.Services.ContentService;
using MudBlazor;
using static MRA.Jobs.Application.Contracts.Dtos.Enums.ApplicationStatusDto;


namespace MRA.Jobs.Client.Services.ApplicationService;

public class ApplicationService(
    IHttpClientService httpClient,
    AuthenticationStateProvider authenticationState,
    ISnackbar snackbar,
    NavigationManager navigationManager,
    IConfiguration configuration,
    IContentService contentService)
    : IApplicationService
{
    private readonly string _applicationsEndPoint = configuration.GetJobsUrl("applications");

    public async Task<List<ApplicationListStatus>> GetApplicationsByStatus(ApplicationStatus status)
    {
        var response =
            await httpClient.GetFromJsonAsync<List<ApplicationListStatus>>($"{_applicationsEndPoint}/{status}");
        return response.Success ? response.Result : null;
    }


    public async Task CreateApplication(CreateApplicationCommand application, IBrowserFile file)
    {
        //set cv
        if (application.Cv.IsUploadCvMode)
        {
            var fileBytes = await GetFileBytesAsync(file);
            application.Cv.CvBytes = fileBytes;
            application.Cv.FileName = file.Name;
        }
        //set cv

        var response = await httpClient.PostAsJsonAsync<Guid>(_applicationsEndPoint, application);
        snackbar.ShowIfError(response, contentService["ServerIsNotResponding"],
            contentService["Application:Success"]);

        if (response.HttpStatusCode == HttpStatusCode.OK)
        {
            navigationManager.NavigateTo(navigationManager.Uri.Replace("/apply/", "/"));
        }
    }

    private async Task<byte[]> GetFileBytesAsync(IBrowserFile file)
    {
        var allowedSize = int.Parse(configuration["CvSettings:MaxFileSize"]!);
        if (file.Size <= allowedSize)
        {
            var ms = new MemoryStream();
            await file.OpenReadStream(allowedSize).CopyToAsync(ms);
            var res = ms.ToArray();
            return res;
        }

        return null;
    }

    public async Task<PagedList<ApplicationListDto>> GetAllApplications(GetApplicationsByFiltersQuery query)
    {
        await authenticationState.GetAuthenticationStateAsync();
        var response = await httpClient.GetFromJsonAsync<PagedList<ApplicationListDto>>(_applicationsEndPoint, query);
        return response.Success ? response.Result : null;
    }

    public async Task<bool> UpdateStatus(UpdateApplicationStatusCommand updateApplicationStatusCommand)
    {
        await authenticationState.GetAuthenticationStateAsync();
        var response = await httpClient.PutAsJsonAsync(
            $"{_applicationsEndPoint}/{updateApplicationStatusCommand.Slug}/update-status",
            updateApplicationStatusCommand);

        if (response.HttpStatusCode != null)
            return (int)response.HttpStatusCode > 199 && (int)response.HttpStatusCode < 300;
        return false;
    }

    public async Task<ApplicationDetailsDto> GetApplicationDetails(string applicationSlug)
    {
        var r = await authenticationState.GetAuthenticationStateAsync();
        var response =
            await httpClient.GetFromJsonAsync<ApplicationDetailsDto>($"{_applicationsEndPoint}/{applicationSlug}");
        return response.Result;
    }

    public async Task<string> GetCvLinkAsync(string slug)
    {
        var applicationSlug = $"{await GetCurrentUserName()}-{slug}".ToLower().Trim();
        var app = await GetApplicationDetails(applicationSlug);
        string baseAddress = configuration["HttpClient:BaseAddress"];
        return app == null ? "" : $"{baseAddress}applications/downloadCv/{WebUtility.UrlEncode(app.CV)}";
    }

    public async Task<TimeLineDetailsDto> AddNote(AddNoteToApplicationCommand note)
    {
        var response =
            await httpClient.PostAsJsonAsync<TimeLineDetailsDto>($"{_applicationsEndPoint}/add-note", note);
        snackbar.ShowIfError(response, contentService["ServerIsNotResponding"], "Successfully");

        if (response.HttpStatusCode == HttpStatusCode.OK)
        {
            return response.Result;
        }

        return null;
    }

    public async Task<List<UserResponse>> GetCandidates(GetCandidatesQuery query)
    {
        var response =
            await httpClient.GetFromJsonAsync<List<UserResponse>>(configuration.GetJobsUrl("applications/candidates"),
                query);
        snackbar.ShowIfError(response, contentService["ServerIsNotResponding"]);
        return response.Result;
    }

    private async Task<string> GetCurrentUserName()
    {
        var authState = await authenticationState.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity.IsAuthenticated)
        {
            var userNameClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Username);
            if (userNameClaim != null)
                return userNameClaim.Value;
        }

        return null;
    }
}