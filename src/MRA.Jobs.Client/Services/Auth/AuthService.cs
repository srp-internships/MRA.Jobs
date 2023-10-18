﻿using System.Net;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MRA.Identity.Application.Contract.Admin.Responses;
using Blazored.LocalStorage;
using MRA.Identity.Application.Contract.User.Commands.RegisterUser;
using MRA.Identity.Application.Contract.User.Commands.LoginUser;
using MRA.Jobs.Client.Identity;

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
                await _localStorage.SetItemAsync("authToken", response);
                await _authenticationStateProvider.GetAuthenticationStateAsync();
                _navigationManager.NavigateTo("/");
                return null;
            }

            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                errorMessage = (await result.Content.ReadFromJsonAsync<CustomProblemDetails>()).Detail;
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex);
            errorMessage = "Server is not responding, please try later";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            errorMessage = "An error occurred";
        }

        return errorMessage;
    }

    public async Task<string> RegisterUserAsync(RegisterUserCommand command)
    {
        try
        {
            var result = await _identityHttpClient.PostAsJsonAsync("Auth/register", command);
            if (result.IsSuccessStatusCode)
            {
                _navigationManager.NavigateTo("/sign-in");
                return "";
            }
            if (result.StatusCode is not (HttpStatusCode.Unauthorized or HttpStatusCode.BadRequest))
                return "server error, please try again later";
            
            var response = await result.Content.ReadFromJsonAsync<CustomProblemDetails>();
            return response.Detail;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex);
            return "Server is not responding, please try later";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return "An error occurred";
        }
    }
}