using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplicationStatus;
using MRA.Jobs.Application.Contracts.Applications.Queries.GetApplicationBySlug;
using MRA.Jobs.Application.Contracts.Applications.Queries.GetApplicationsByStatus;
using MRA.Jobs.Application.Contracts.Applications.Queries.GetApplicationWithPagination;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Client.Identity;
using MRA.Jobs.Client.Services.ContentService;
using MRA.Jobs.Client.Services.HttpClients;
using MudBlazor;
using static MRA.Jobs.Application.Contracts.Dtos.Enums.ApplicationStatusDto;


namespace MRA.Jobs.Client.Services.ApplicationService;

public class ApplicationService(
        IHttpClientService httpClient,
        HttpClient http,
        AuthenticationStateProvider authenticationState,
        ISnackbar snackbar,
        NavigationManager navigationManager,
        IConfiguration configuration,
        IContentService contentService)
    : IApplicationService
{
    private const string ApplicationsEndPoint = "applications";

    public async Task<List<ApplicationListStatus>> GetApplicationsByStatus(ApplicationStatus status, GetApplicationsByStatusQuery getApplicationsByStatusQuery)
    {
        var response = await httpClient.GetAsJsonAsync<List<ApplicationListStatus>>($"{configuration["HttpClient:BaseAddress"]}/{ApplicationsEndPoint}/{status}", getApplicationsByStatusQuery);
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

            var response = await httpClient.PostAsJsonAsync<ApiResponse>($"{configuration["HttpClient:BaseAddress"]}/{ApplicationsEndPoint}", application);
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

    public async Task<PagedList<ApplicationListDto>> GetAllApplications(GetApplicationsQuery getApplicationsQuery)
    {
        await authenticationState.GetAuthenticationStateAsync();
        var response = await httpClient.GetAsJsonAsync<PagedList<ApplicationListDto>>($"{configuration["HttpClient:BaseAddress"]}/{ApplicationsEndPoint}", getApplicationsQuery);
        return response.Success ? response.Result : null;
    }

    public async Task<bool> UpdateStatus(UpdateApplicationStatus updateApplicationStatus)
    {
        await authenticationState.GetAuthenticationStateAsync();
        var response = await httpClient.PutAsJsonAsync<UpdateApplicationStatus>($"{configuration["HttpClient:BaseAddress"]}/{ApplicationsEndPoint}/{updateApplicationStatus.Slug}/update-status", updateApplicationStatus);
        return response.Success;
    }

    public async Task<ApplicationDetailsDto> GetApplicationDetails(string applicationSlug, GetBySlugApplicationQuery getBySlugApplicationQuery)
    {
        await authenticationState.GetAuthenticationStateAsync();
        var response = await httpClient.GetAsJsonAsync<ApplicationDetailsDto>($"{configuration["HttpClient:BaseAddress"]}/{ApplicationsEndPoint}/{applicationSlug}", getBySlugApplicationQuery);
        if (response.HttpStatusCode == HttpStatusCode.OK)
        {
            return response.Result;
        }
        return null;
    }

    public async Task<string> GetCvLinkAsync(string slug, GetBySlugApplicationQuery getBySlugApplicationQuery)
    {
        var applicationSlug = $"{await GetCurrentUserName()}-{slug}".ToLower().Trim();
        var app = await GetApplicationDetails(applicationSlug, getBySlugApplicationQuery);
        if (app == null)
        {
            return "";
        }

        return $"{configuration["HttpClient:BaseAddress"]}applications/downloadCv/{WebUtility.UrlEncode(app.CV)}";
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