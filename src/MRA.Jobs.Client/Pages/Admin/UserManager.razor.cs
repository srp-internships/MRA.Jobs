using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MRA.Identity.Application.Contract.User.Responses;

namespace MRA.Jobs.Client.Pages.Admin;
public sealed partial class UserManager
{
    private List<UserResponse> _applicationUsers = null!;
    [Inject] private IdentityHttpClient _client { get; set; }
    [Inject] private AuthenticationStateProvider _authStateProvider { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await _authStateProvider.GetAuthenticationStateAsync();
        _applicationUsers = await _client.GetFromJsonAsync<List<UserResponse>>("user");
    }
}