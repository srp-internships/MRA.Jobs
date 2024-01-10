using System.Net;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MRA.Jobs.Application.Contracts.Applications.Commands.AddNote;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplicationStatus;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TimeLineDTO;
using MRA.Jobs.Client.Identity;
using MRA.Jobs.Client.Services.ContentService;
using MudBlazor;
using static MRA.Jobs.Application.Contracts.Dtos.Enums.ApplicationStatusDto;


namespace MRA.Jobs.Client.Services.ApplicationService;

public class ApplicationService(
    HttpClient httpClient,
    AuthenticationStateProvider authenticationState,
    ISnackbar snackbar,
    NavigationManager navigationManager,
    IConfiguration configuration,
    IContentService contentService)
    : IApplicationService
{
    private const string ApplicationsEndPoint = "applications";

    public async Task<List<ApplicationListStatus>> GetApplicationsByStatus(ApplicationStatus status)
    {
        HttpResponseMessage response = await httpClient.GetAsync($"{ApplicationsEndPoint}/{status}");

        List<ApplicationListStatus> result = await response.Content.ReadFromJsonAsync<List<ApplicationListStatus>>();
        return result;
    }


    public async Task CreateApplication(CreateApplicationCommand application, IBrowserFile file)
    {
        try
        {
            //set cv
            if (application.Cv.IsUploadCvMode)
            {
                var fileBytes = await GetFileBytesAsync(file);
                application.Cv.CvBytes = fileBytes;
                application.Cv.FileName = file.Name;
            }
            //set cv

            var response = await httpClient.PostAsJsonAsync(ApplicationsEndPoint, application);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    snackbar.Add(contentService["Application:Success"], Severity.Success);
                    navigationManager.NavigateTo(navigationManager.Uri.Replace("/apply/", "/"));
                    break;
                case HttpStatusCode.Conflict:
                    snackbar.Add((await response.Content.ReadFromJsonAsync<CustomProblemDetails>()).Detail,
                        Severity.Error);
                    break;
                default:
                    snackbar.Add(contentService["SomethingWentWrong"], Severity.Error);
                    break;
            }
        }
        catch (Exception e)
        {
            snackbar.Add(contentService["ServerIsNotResponding"], Severity.Error);
            Console.WriteLine(e.Message);
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

    public async Task<PagedList<ApplicationListDto>> GetAllApplications()
    {
        await authenticationState.GetAuthenticationStateAsync();
        HttpResponseMessage response = await httpClient.GetAsync(ApplicationsEndPoint);
        PagedList<ApplicationListDto>
            result = await response.Content.ReadFromJsonAsync<PagedList<ApplicationListDto>>();
        return result;
    }

    public async Task<bool> UpdateStatus(UpdateApplicationStatus updateApplicationStatus)
    {
        await authenticationState.GetAuthenticationStateAsync();
        HttpResponseMessage response =
            await httpClient.PutAsJsonAsync($"{ApplicationsEndPoint}/{updateApplicationStatus.Slug}/update-status",
                updateApplicationStatus);
        bool result = await response.Content.ReadFromJsonAsync<bool>();
        return result;
    }

    public async Task<ApplicationDetailsDto> GetApplicationDetails(string applicationSlug)
    {
        await authenticationState.GetAuthenticationStateAsync();
        HttpResponseMessage response = await httpClient.GetAsync($"{ApplicationsEndPoint}/{applicationSlug}");
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var application = await response.Content.ReadFromJsonAsync<ApplicationDetailsDto>();
            return application;
        }

        return null;
    }

    public async Task<string> GetCvLinkAsync(string slug)
    {
        var applicationSlug = $"{await GetCurrentUserName()}-{slug}".ToLower().Trim();
        var app = await GetApplicationDetails(applicationSlug);
        if (app == null)
        {
            return "";
        }

        return $"{httpClient.BaseAddress}applications/downloadCv/{WebUtility.UrlEncode(app.CV)}";
    }

    public async Task<TimeLineDetailsDto> AddNote(AddNoteToApplicationCommand note)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync($"{ApplicationsEndPoint}/add-note", note);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadFromJsonAsync<TimeLineDetailsDto>();
            }

            snackbar.Add((await response.Content.ReadFromJsonAsync<CustomProblemDetails>()).Detail, Severity.Error);
        }
        catch (Exception e)
        {
            snackbar.Add(contentService["ServerIsNotResponding"], Severity.Error);
        }

        return null;
    }

    public async Task<List<TimeLineDetailsDto>> GetApplicationTimeLineEvents(string slug)
    {
        try
        {
            var response = await httpClient.GetAsync($"{ApplicationsEndPoint}/GetTimelineEvents/{slug}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadFromJsonAsync<List<TimeLineDetailsDto>>();
            }

            snackbar.Add((await response.Content.ReadFromJsonAsync<CustomProblemDetails>()).Detail, Severity.Error);
        }
        catch (Exception e)
        {
            snackbar.Add(contentService["ServerIsNotResponding"], Severity.Error);
        }

        return null;
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