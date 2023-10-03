﻿using System.Security.AccessControl;
using Microsoft.AspNetCore.Components.Authorization;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplicationStatus;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Client.Identity;
using static MRA.Jobs.Application.Contracts.Dtos.Enums.ApplicationStatusDto;


namespace MRA.Jobs.Client.Services.ApplicationService;

public class ApplicationService : IApplicationService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationState;

    public ApplicationService(HttpClient httpClient, AuthenticationStateProvider authenticationState)
    {
        _httpClient = httpClient;
        _authenticationState = authenticationState;
    }

    public async Task<List<ApplicationListStatus>> GetApplicationsByStatus(ApplicationStatus status)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"api/applications/{status}");

        List<ApplicationListStatus> result = await response.Content.ReadFromJsonAsync<List<ApplicationListStatus>>();
        return result;
    }


    public async Task CreateApplication(CreateApplicationCommand application)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/applications", application);
    }

    public async Task<PagedList<ApplicationListDto>> GetAllApplications()
    {
        await _authenticationState.GetAuthenticationStateAsync();
        HttpResponseMessage response = await _httpClient.GetAsync("/api/applications/");
        PagedList<ApplicationListDto> result = await response.Content.ReadFromJsonAsync<PagedList<ApplicationListDto>>();
        return result;
    }

    public async Task<bool> UpdateStatus(UpdateApplicationStatus updateApplicationStatus)
    {
        await _authenticationState.GetAuthenticationStateAsync();
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"/api/applications/{updateApplicationStatus.Slug}/update-status", updateApplicationStatus);
        bool result = await response.Content.ReadFromJsonAsync<bool>();
        return result;
    }

    public async Task<ApplicationDetailsDto> GetApplicationDetails(string ApplicationSlug)
    {
        await _authenticationState.GetAuthenticationStateAsync();
        HttpResponseMessage response = await _httpClient.GetAsync($"/api/applications/{ApplicationSlug}");
        if(response.StatusCode==System.Net.HttpStatusCode.OK)
        {
            var application = await response.Content.ReadFromJsonAsync<ApplicationDetailsDto>();
            return application;
        }

        return null;
      
    }

    public async Task<bool> ApplicationExist(string vacancySlug)
    {
        var ApplicationSlug = ($"{(await GetCurrentUserName())}-{vacancySlug}").ToLower().Trim();
        if ((await GetApplicationDetails(ApplicationSlug)) != null)
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