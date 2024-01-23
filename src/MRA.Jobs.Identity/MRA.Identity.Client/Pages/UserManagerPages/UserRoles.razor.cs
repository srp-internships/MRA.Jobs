using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MRA.Identity.Application.Contract.Claim.Commands;
using MRA.Identity.Application.Contract.Claim.Responses;
using MRA.Identity.Application.Contract.Educations.Responses;
using MRA.Identity.Application.Contract.Experiences.Responses;
using MRA.Identity.Application.Contract.Profile.Responses;
using MRA.Identity.Application.Contract.Skills.Responses;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Identity.Application.Contract.UserRoles.Commands;
using MRA.Identity.Application.Contract.UserRoles.Response;
using MRA.Identity.Client.Services.Profile;
using MudBlazor;

namespace MRA.Identity.Client.Pages.UserManagerPages;

public partial class UserRoles
{
    [Parameter] public string Username { get; set; }
    private List<UserRolesResponse> Roles { get; set; }
    private List<UserClaimsResponse> UserClaims { get; set; }
    [Inject] private HttpClient HttpClient { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public AuthenticationStateProvider AuthStateProvider { get; set; }
    [Inject] private IUserProfileService UserProfileService { get; set; }
    private string NewRoleName { get; set; }

    private UserProfileResponse _personalData = new UserProfileResponse();
    private UserSkillsResponse _userSkills = new UserSkillsResponse();

    private List<UserExperienceResponse> _experiences = new();
    private List<UserEducationResponse> _educations = new();
    private string _claimType;
    private string _claimValue;
    private UserResponse _user;
    private bool _loader;

    protected override async Task OnInitializedAsync()
    {
        await ReloadDataAsync();
        await ReloadUserClaimsAsync();
        _personalData = await UserProfileService.Get(Username);
        _userSkills = await UserProfileService.GetUserSkills(Username);
        _experiences = await UserProfileService.GetExperiencesByUser(Username);
        _educations = await UserProfileService.GetEducationsByUser(Username);
        _user = await (await HttpClient.GetAsync($"User/{Username}")).Content.ReadFromJsonAsync<UserResponse>();
    }

    private async Task ReloadUserClaimsAsync()
    {
        _claimType = "";
        _claimValue = "";
        var userClaimsResponse = await HttpClient.GetAsync($"Claims?username={Username}");
        if (!userClaimsResponse.IsSuccessStatusCode)
        {
            NavigationManager.NavigateTo("/notfound");
            return;
        }

        UserClaims = await userClaimsResponse.Content.ReadFromJsonAsync<List<UserClaimsResponse>>();
    }

    private async Task ReloadDataAsync()
    {
        await AuthStateProvider.GetAuthenticationStateAsync();
        var userRolesResponse = await HttpClient.GetAsync($"UserRoles?userName={Username}");
        if (!userRolesResponse.IsSuccessStatusCode)
        {
            NavigationManager.NavigateTo("/notfound");
            return;
        }

        Roles = await userRolesResponse.Content.ReadFromJsonAsync<List<UserRolesResponse>>();
    }

    private async Task OnDeleteClick(string contextSlug)
    {
        if (!string.IsNullOrWhiteSpace(contextSlug))
        {
            await AuthStateProvider.GetAuthenticationStateAsync();
            try
            {
                var deleteResult = await HttpClient.DeleteAsync($"UserRoles/{contextSlug}");
                if (!deleteResult.IsSuccessStatusCode)
                {
                    Snackbar.Add(ContentService["ErrorDelete"], Severity.Error);
                    return;
                }
            }
            catch (Exception)
            {
                Snackbar.Add(ContentService["ServerIsNotResponding"], Severity.Error);
                return;
            }

            await ReloadDataAsync();
            StateHasChanged();
        }
    }

    private async Task OnAddClick()
    {
        if (!string.IsNullOrWhiteSpace(NewRoleName))
        {
            var userRoleCommand = new CreateUserRolesCommand { RoleName = NewRoleName, UserName = Username };

            await AuthStateProvider.GetAuthenticationStateAsync();
            try
            {
                var userRoleResponse = await HttpClient.PostAsJsonAsync("UserRoles", userRoleCommand);
                if (!userRoleResponse.IsSuccessStatusCode)
                {
                    Snackbar.Add(ContentService["DuplicateRole"]);
                    return;
                }
            }
            catch (Exception)
            {
                Snackbar.Add(ContentService["ServerIsNotResponding"], Severity.Error);
                return;
            }

            await ReloadDataAsync();
            StateHasChanged();
        }
    }

    private async Task OnAddClaimClick()
    {
        if (!string.IsNullOrWhiteSpace(_claimType) && !string.IsNullOrWhiteSpace(_claimValue))
        {
            var command =
                new CreateClaimCommand() { ClaimType = _claimType, ClaimValue = _claimValue, UserId = _user.Id };
            await AuthStateProvider.GetAuthenticationStateAsync();
            try
            {
                _loader = true;
                await HttpClient.PostAsJsonAsync("Claims", command);
                await ReloadUserClaimsAsync();
                _claimType = "";
                _claimValue = "";
                StateHasChanged();
            }
            catch (Exception)
            {
                Snackbar.Add(ContentService["ServerIsNotResponding"], Severity.Error);
            }
            finally
            {
                _loader = false;
            }
        }
    }

    private async Task OnDeleteClaimClick(string slug)
    {
        await AuthStateProvider.GetAuthenticationStateAsync();
        try
        {
            var deleteResult = await HttpClient.DeleteAsync($"Claims/{slug}");
            if (!deleteResult.IsSuccessStatusCode)
            {
                Snackbar.Add(ContentService["ErrorDelete"], Severity.Error);
                return;
            }
        }
        catch (Exception)
        {
            Snackbar.Add(ContentService["ServerIsNotResponding"], Severity.Error);
            return;
        }

        await ReloadUserClaimsAsync();
        StateHasChanged();
    }
}