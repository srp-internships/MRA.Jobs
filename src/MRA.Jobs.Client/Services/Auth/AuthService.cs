using System.Net;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Admin.Responses;
using MRA.Identity.Application.Contract.User.Commands;
using Blazored.LocalStorage;

namespace MRA.Jobs.Client.Services.Auth;


public class AuthService : IAuthService
{
    private readonly IdentityHttpClient _identityHttpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly NavigationManager _navigationManager;

    public AuthService(IdentityHttpClient identityHttpClient, ILocalStorageService localStorage,
        AuthenticationStateProvider authenticationStateProvider, NavigationManager navigationManager)
    {
        _identityHttpClient = identityHttpClient;
        _localStorage = localStorage;
        _authenticationStateProvider = authenticationStateProvider;
        _navigationManager = navigationManager;
    }

    public async Task<string> LoginUserAsync(LoginUserCommand command)
    {
        string errorMessage = null;
        try
        {
            var result = await _identityHttpClient.PostAsJsonAsync("Auth/login", command);
            if (result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadFromJsonAsync<JwtTokenResponse>();
                await _localStorage.SetItemAsync<JwtTokenResponse>("authToken", response);
                await _authenticationStateProvider.GetAuthenticationStateAsync();
                _navigationManager.NavigateTo("/");
                return null;
            }
            else
            {
                // Обработка ошибок, которые возвращает сервер
                switch ((int)result.StatusCode)
                {
                    case 400:
                        // Bad Request
                        return "Your username or password is incorrect";
                    case 404:
                        // Not Found
                        return "User not found";
                    case 503:
                        // Service Unavailable
                        return "The server is temporarily unavailable";
                    case 401:
                        return "Unauthorized";
                    default:
                        // Other errors
                        return "An error occurred";

                }
            }

        }
        catch (Exception)
        {
            errorMessage = "An error occurred";
        }
        return errorMessage;
    }

    public async Task<bool> RegisterUserAsync(RegisterUserCommand command)
    {
        var result = await _identityHttpClient.PostAsJsonAsync("Auth/register", command);
        if (result.StatusCode == HttpStatusCode.OK)
            return true;
        return false;
    }
}



