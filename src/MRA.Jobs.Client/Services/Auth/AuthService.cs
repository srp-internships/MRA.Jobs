using System.Net;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;
using MRA.Identity.Application.Contract.User.Commands.RegisterUser;
using MRA.Identity.Application.Contract.User.Commands.LoginUser;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Client.Identity;
using MRA.Identity.Application.Contract.User.Commands.ChangePassword;
using MRA.Identity.Application.Contract.User.Commands.ResetPassword;
using MRA.Identity.Application.Contract.User.Queries.GetUserNameByPhoneNymber;

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

    public async Task<HttpResponseMessage> ChangePassword(ChangePasswordUserCommand command)
    {

        var result = await _identityHttpClient.PutAsJsonAsync("Auth/ChangePassword", command);
        return result;
    }

    public async Task<HttpResponseMessage> GetUserNameByPhoneNumber(GetUserNameByPhoneNumberQuery query)
    {
        var result = await _identityHttpClient.GetAsync($"Auth/GetUserNameByPhoneNumber/{Uri.EscapeDataString(query.PhoneNumber)}");
        return result;
    }

    public async Task<string> LoginUserAsync(LoginUserCommand command, bool newRegister = false)
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
                if (!newRegister)
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
                await LoginUserAsync(new LoginUserCommand()
                {
                    Password = command.Password,
                    Username = command.Username
                });

                _navigationManager.NavigateTo("/profile");

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

    public async Task<HttpResponseMessage> ResetPassword(ResetPasswordCommand command)
    {
        var result = await _identityHttpClient.PostAsJsonAsync("Auth/ResetPassword", command);
        return result;
    }

    public async Task<HttpResponseMessage> CheckUserName(string userName)
    {
        var result = await _identityHttpClient.GetAsync($"User/CheckUserName/{userName}");
        return result;
    }
}