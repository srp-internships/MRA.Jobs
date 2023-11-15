using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplicationStatus;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Client.Identity;
using MudBlazor;
using static MRA.Jobs.Application.Contracts.Dtos.Enums.ApplicationStatusDto;


namespace MRA.Jobs.Client.Services.ApplicationService;

public class ApplicationService : IApplicationService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationState;
    private readonly ISnackbar _snackbar;
    private readonly NavigationManager _navigationManager;


    public ApplicationService(HttpClient httpClient, AuthenticationStateProvider authenticationState,
        ISnackbar snackbar, NavigationManager navigationManager)
    {
        _httpClient = httpClient;
        _authenticationState = authenticationState;
        _snackbar = snackbar;
        _navigationManager = navigationManager;
    }

    public async Task<List<ApplicationListStatus>> GetApplicationsByStatus(ApplicationStatus status)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"api/applications/{status}");

        List<ApplicationListStatus> result = await response.Content.ReadFromJsonAsync<List<ApplicationListStatus>>();
        return result;
    }


    public async Task CreateApplication(CreateApplicationCommand application)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/applications", application);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    _snackbar.Add("Applications sent successfully!", Severity.Success);
                    _navigationManager.NavigateTo(_navigationManager.Uri, forceLoad: true);
                    break;
                case HttpStatusCode.Conflict:
                    _snackbar.Add((await response.Content.ReadFromJsonAsync<CustomProblemDetails>()).Detail,
                        Severity.Error);
                    break;
                default:
                    _snackbar.Add("Something went wrong", Severity.Error);
                    break;
            }
        }
        catch (Exception e)
        {
            _snackbar.Add("Server is not responding, please try later", Severity.Error);
            Console.WriteLine(e.Message);
        }
    }

    public async Task<PagedList<ApplicationListDto>> GetAllApplications()
    {
        await _authenticationState.GetAuthenticationStateAsync();
        HttpResponseMessage response = await _httpClient.GetAsync("/api/applications/");
        PagedList<ApplicationListDto>
            result = await response.Content.ReadFromJsonAsync<PagedList<ApplicationListDto>>();
        return result;
    }

    public async Task<bool> UpdateStatus(UpdateApplicationStatus updateApplicationStatus)
    {
        await _authenticationState.GetAuthenticationStateAsync();
        HttpResponseMessage response =
            await _httpClient.PutAsJsonAsync($"/api/applications/{updateApplicationStatus.Slug}/update-status",
                updateApplicationStatus);
        bool result = await response.Content.ReadFromJsonAsync<bool>();
        return result;
    }

    public async Task<ApplicationDetailsDto> GetApplicationDetails(string applicationSlug)
    {
        await _authenticationState.GetAuthenticationStateAsync();
        HttpResponseMessage response = await _httpClient.GetAsync($"/api/applications/{applicationSlug}");
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var application = await response.Content.ReadFromJsonAsync<ApplicationDetailsDto>();
            return application;
        }

        return null;
    }

    public async Task<bool> ApplicationExist(string vacancySlug)
    {
        var applicationSlug = ($"{(await GetCurrentUserName())}-{vacancySlug}").ToLower().Trim();
        var apps = (await GetAllApplications()).Items;
        if (apps.Any(a => a.Slug == applicationSlug))
            return true;
        return false;
    }


    private async Task<string> GetCurrentUserName()
    {
        var authState = await _authenticationState.GetAuthenticationStateAsync();
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