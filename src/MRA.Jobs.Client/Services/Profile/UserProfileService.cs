using System.Net;
using Microsoft.AspNetCore.Components.Authorization;
using MRA.Identity.Application.Contract.Educations.Responses;
using MRA.Identity.Application.Contract.Experiences.Responses;
using MRA.Identity.Application.Contract.Profile.Responses;
using MRA.Identity.Application.Contract.Skills.Responses;
using MRA.Jobs.Client.Services.HttpClients;
using MudBlazor;

namespace MRA.Jobs.Client.Services.Profile;

public class UserProfileService(
    IdentityApiHttpClientService identityHttpClient,
    AuthenticationStateProvider authenticationStateProvider,
    ISnackbar snackbar) : IUserProfileService
{
    public async Task<UserProfileResponse> Get(string userName= null)
    {
        await authenticationStateProvider.GetAuthenticationStateAsync();
        var result =
            await identityHttpClient.GetAsJsonAsync<UserProfileResponse>(
                $"Profile{(userName != null ? "?userName=" + Uri.EscapeDataString(userName) : "")}");
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
            await identityHttpClient.GetAsJsonAsync<List<UserEducationResponse>>("Profile/GetEducationsByUser");
        return result;
    }
    
    public async Task<ApiResponse<List<UserExperienceResponse>>> GetExperiencesByUser(string userName)
    {
        var result =
            await identityHttpClient.GetAsJsonAsync<List<UserExperienceResponse>>("Profile/GetExperiencesByUser");
        return result;
    }
    
    public async Task<UserSkillsResponse> GetUserSkills(string userName)
    {
        var response = await identityHttpClient
            .GetAsJsonAsync<UserSkillsResponse>(
                $"Profile/GetUserSkills{(userName != null ? "?userName=" + Uri.EscapeDataString(userName) : "")}");
        return response.Result;
    }
    
}