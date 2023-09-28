using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Client.Identity;

namespace MRA.Jobs.Client.Pages.Admin;

[Authorize(ApplicationPolicies.Administrator)]
public partial class UserManager
{
    private List<UserResponse> _applicationUsers = null!;
    [Inject] public IdentityHttpClient _client { get; set; }
    [Inject] public ILocalStorageService _localStorageService { get; set; }
    [Inject] public AuthenticationStateProvider _authStateProvider { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Console.WriteLine(_client.BaseAddress);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            (await _localStorageService.GetItemAsync<JwtTokenResponse>("authToken")).AccessToken);
        _applicationUsers = await _client.GetFromJsonAsync<List<UserResponse>>("user");
        
    }
}