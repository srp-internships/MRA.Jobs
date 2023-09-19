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
                        // Неверный запрос
                        return "У вас не верный пароль или логин";
                    case 404:
                        // Не найдено
                        return "Юзер не найден";
                    case 503:
                        // Сервис недоступен
                        return "Сервер временно не работает";
                    default:
                        // Другие ошибки
                        return "Произошла ошибка";
                }
            }

        }
        catch (Exception)
        {
            errorMessage = "Произошла ошибка";
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



