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
using MRA.Jobs.Client.Services.HttpClients;
using MudBlazor;
using static MRA.Jobs.Application.Contracts.Dtos.Enums.ApplicationStatusDto;


namespace MRA.Jobs.Client.Services.ApplicationService;

public class ApplicationService(
        JobsApiHttpClientService httpClient,
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
        var response = await httpClient.GetAsJsonAsync<List<ApplicationListStatus>>($"{ApplicationsEndPoint}/{status}");
        return response.Success ? response.Result : null;
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

            var response = await httpClient.PostAsJsonAsync<Guid>(ApplicationsEndPoint, application);
            switch (response.HttpStatusCode)
            {
                case HttpStatusCode.OK:
                    snackbar.Add(contentService["Application:Success"], Severity.Success);
                    navigationManager.NavigateTo(navigationManager.Uri.Replace("/apply/", "/"));
                    break;
                case HttpStatusCode.Conflict:
                    snackbar.Add(response.Error, Severity.Error);
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
        var response = await httpClient.GetAsJsonAsync<PagedList<ApplicationListDto>>(ApplicationsEndPoint);
        return response.Success ? response.Result : null;
    }

    public async Task<bool> UpdateStatus(UpdateApplicationStatus updateApplicationStatus)
    {
        await authenticationState.GetAuthenticationStateAsync();
        var response = await httpClient.PutAsync($"{ApplicationsEndPoint}/{updateApplicationStatus.Slug}/update-status", updateApplicationStatus);

        if (response.HttpStatusCode != null)
            return (int)response.HttpStatusCode > 199 && (int)response.HttpStatusCode < 300;
        return false;
    }

    public async Task<ApplicationDetailsDto> GetApplicationDetails(string applicationSlug)
    {
        var r=await authenticationState.GetAuthenticationStateAsync();
        var response = await httpClient.GetAsJsonAsync<ApplicationDetailsDto>($"{ApplicationsEndPoint}/{applicationSlug}");
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
        try
        {
            var response = await httpClient.PostAsJsonAsync<TimeLineDetailsDto>($"{ApplicationsEndPoint}/add-note", note);
            switch (response.HttpStatusCode)
            {
                case HttpStatusCode.OK:
                    snackbar.Add("Successfully", Severity.Success);
                    return  response.Result;
                case HttpStatusCode.Conflict:
                    snackbar.Add( response.Error, Severity.Error);
                    break;
                default:
                    snackbar.Add(contentService["SomethingWentWrong"], Severity.Error);
                    break;
            }
        }
        catch (Exception e)
        {
            snackbar.Add(contentService["ServerIsNotResponding"], Severity.Error);
            Console.WriteLine(e);
        }

        return null;
    }

    public async Task<List<TimeLineDetailsDto>> GetApplicationTimeLineEvents(string slug)
    {
        try
        {
            var response = await httpClient.GetAsJsonAsync<List<TimeLineDetailsDto>>($"{ApplicationsEndPoint}/GetApplicationNotes/{slug}");

            return response.Result;
        }
        catch (Exception)
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