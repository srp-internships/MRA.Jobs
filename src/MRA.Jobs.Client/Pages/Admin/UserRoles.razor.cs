using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MRA.Identity.Application.Contract.Profile.Responses;
using MRA.Identity.Application.Contract.Skills.Responses;
using MRA.Identity.Application.Contract.UserRoles.Commands;
using MRA.Identity.Application.Contract.UserRoles.Response;
using MRA.Jobs.Client.Services.Profile;
using MudBlazor;

namespace MRA.Jobs.Client.Pages.Admin;

public partial class UserRoles
{
    [Parameter] public string Username { get; set; }
    private List<UserRolesResponse> Roles { get; set; }
    [Inject] private IdentityHttpClient HttpClient { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public AuthenticationStateProvider AuthStateProvider { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IUserProfileService UserProfileService { get; set; }
    private string NewRoleName { get; set; }

    private UserProfileResponse _personalData = new UserProfileResponse();
    private UserSkillsResponse _userSkills = new UserSkillsResponse();

    protected override async Task OnInitializedAsync()
    {
        await ReloadDataAsync();
        _personalData = await UserProfileService.Get(Username);
        _userSkills= await UserProfileService.GetUserSkills(Username);
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
}