using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MRA.Identity.Application.Contract.UserRoles.Commands;
using MRA.Identity.Application.Contract.UserRoles.Response;
using ClaimTypes = MRA.Jobs.Client.Identity.ClaimTypes;

namespace MRA.Jobs.Client.Pages.Admin;

public partial class UserRoles
{
    [Parameter] public string Username { get; set; }
    private List<UserRolesResponse> Roles { get; set; }
    [Inject] private IdentityHttpClient HttpClient { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public AuthenticationStateProvider AuthStateProvider { get; set; }
    [Inject] private IMatDialogService DialogService { get; set; }
    private string NewRoleName { get; set; }

    private bool _isOpened;

    protected override async Task OnInitializedAsync()
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
            var deleteResult = await HttpClient.DeleteAsync($"UserRoles/{contextSlug}");
            if (!deleteResult.IsSuccessStatusCode)
            {
                await DialogService.AlertAsync("Error deleting");
                return;
            }

            NavigationManager.NavigateTo(NavigationManager.Uri, true);
        }
    }

    private async Task OnAddClick()
    {
        if (!string.IsNullOrWhiteSpace(NewRoleName))
        {
            var state = await AuthStateProvider.GetAuthenticationStateAsync();

            var getIdStatus = Guid.TryParse(state.User.FindFirst(ClaimTypes.Id)?.Value, out Guid id);
            Console.WriteLine(getIdStatus); //todo show error if getIdStatus equal false

            var userRoleCommand = new CreateUserRolesCommand { RoleName = NewRoleName, UserId = id };
            var userRoleResponse = await HttpClient.PostAsJsonAsync("UserRoles", userRoleCommand);
            Console.WriteLine(await userRoleResponse.Content.ReadAsStringAsync()); //todo check userRoleResponse
        }
    }
}