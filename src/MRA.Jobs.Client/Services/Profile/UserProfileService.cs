using System.Net;
using System.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using MRA.BlazorComponents.Configuration;
using MRA.BlazorComponents.HttpClient.Responses;
using MRA.BlazorComponents.HttpClient.Services;
using MRA.BlazorComponents.Snackbar.Extensions;
using MRA.Identity.Application.Contract.Common;
using MRA.Identity.Application.Contract.Educations.Responses;
using MRA.Identity.Application.Contract.Experiences.Responses;
using MRA.Identity.Application.Contract.Profile.Responses;
using MRA.Identity.Application.Contract.Skills.Responses;
using MRA.Identity.Application.Contract.User.Queries;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Client.Services.ContentService;
using MudBlazor;

namespace MRA.Jobs.Client.Services.Profile;

public class UserProfileService(
    IHttpClientService identityHttpClient,
    AuthenticationStateProvider authenticationStateProvider,
    ISnackbar snackbar,
    IConfiguration configuration,
    IContentService contentService)
    : IUserProfileService
{
    public async Task<UserProfileResponse> Get(string userName = null)
    {
        await authenticationStateProvider.GetAuthenticationStateAsync();
        var result =
            await identityHttpClient.GetFromJsonAsync<UserProfileResponse>(
                configuration.GetIdentityUrl(
                    $"Profile{(userName != null ? "?userName=" + Uri.EscapeDataString(userName) : "")}"));
        if (result.Success)
        {
            return result.Result;
        }

        if (result.HttpStatusCode == HttpStatusCode.NotFound)
        {
            snackbar.Add("User not found", Severity.Error);
            return null;
        }

        return null;
    }

    public async Task<ApiResponse<List<UserEducationResponse>>> GetEducationsByUser(string userName)
    {
        var result =
            await identityHttpClient.GetFromJsonAsync<List<UserEducationResponse>>(
                configuration.GetIdentityUrl("Profile/GetEducationsByUser"));
        return result;
    }

    public async Task<ApiResponse<List<UserExperienceResponse>>> GetExperiencesByUser(string userName)
    {
        var result =
            await identityHttpClient.GetFromJsonAsync<List<UserExperienceResponse>>(
                configuration.GetIdentityUrl("Profile/GetExperiencesByUser"));
        return result;
    }

    public async Task<UserSkillsResponse> GetUserSkills(string userName)
    {
        var response = await identityHttpClient
            .GetFromJsonAsync<UserSkillsResponse>(
                configuration.GetIdentityUrl(
                    $"Profile/GetUserSkills{(userName != null ? "?userName=" + Uri.EscapeDataString(userName) : "")}"));
        return response.Result;
    }

    public async Task<UserSkillsResponse> GetAllSkills()
    {
        var response =
            await identityHttpClient.GetFromJsonAsync<UserSkillsResponse>(
                configuration.GetIdentityUrl("Profile/GetAllSkills"));
        snackbar.ShowIfError(response, contentService["Profile:Servernotrespondingtry"]);
        return response.Result!;
    }

    public async Task<PagedList<UserResponse>> GetCandidatesAsync(GetPagedListUsersQuery query)
    {
        var queryParam = HttpUtility.ParseQueryString(string.Empty);
        if (!query.Skills.IsNullOrEmpty()) queryParam["Skills"] = query.Skills;
        queryParam["Page"] = query.Page.ToString();
        queryParam["PageSize"] = query.PageSize.ToString();
        if (!query.Filters.IsNullOrEmpty()) queryParam["Filters"] = query.Filters;
        var response =
            await identityHttpClient.GetFromJsonAsync<PagedList<UserResponse>>(
                configuration.GetIdentityUrl($"user?{queryParam}"));
        snackbar.ShowIfError(response, contentService["Profile:Servernotrespondingtry"]);
        return response.Result;
    }
}