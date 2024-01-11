using AltairCA.Blazor.WebAssembly.Cookie;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using MRA.Identity.Application.Contract.User.Commands.ChangePassword;
using MRA.Identity.Application.Contract.User.Commands.LoginUser;
using MRA.Identity.Application.Contract.User.Commands.RegisterUser;
using MRA.Identity.Application.Contract.User.Commands.ResetPassword;
using MRA.Identity.Application.Contract.User.Queries.CheckUserDetails;
using MRA.Identity.Application.Contract.User.Queries.GetUserNameByPhoneNymber;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Identity.Client.Services.Profile;
using System.Net;
using System.Net.Http.Json;

namespace MRA.Identity.Client.Services.Auth;

public class AuthService(HttpClient httpClient,
        AuthenticationStateProvider authenticationStateProvider, NavigationManager navigationManager,
        IAltairCABlazorCookieUtil cookieUtil, LayoutService layoutService, IUserProfileService userProfileService, IConfiguration configuration)
    : IAuthService
{
    public async Task<HttpResponseMessage> ChangePassword(ChangePasswordUserCommand command)
    {

        var result = await httpClient.PutAsJsonAsync("Auth/ChangePassword", command);
        return result;
    }

    public async Task<HttpResponseMessage> IsAvailableUserPhoneNumber(IsAvailableUserPhoneNumberQuery query)
    {
        var result = await httpClient.GetAsync($"Auth/IsAvailableUserPhoneNumber/{Uri.EscapeDataString(query.PhoneNumber)}");
        return result;
    }

    public async Task<string> LoginUserAsync(LoginUserCommand command, bool newRegister = false)
    {
        string errorMessage = null;
        try
        {
            var result = await httpClient.PostAsJsonAsync("Auth/login", command);
            if (result.IsSuccessStatusCode)
            {
                string callbackUrl = string.Empty;
                var currentUri = navigationManager.ToAbsoluteUri(navigationManager.Uri);
                if (QueryHelpers.ParseQuery(currentUri.Query).TryGetValue("callback", out var param))
                {
                    callbackUrl = param;
                }
                if (callbackUrl.IsNullOrEmpty()) callbackUrl = configuration["HttpClient:JobsClient"];

                var response = await result.Content.ReadFromJsonAsync<JwtTokenResponse>();
                
                navigationManager.NavigateTo($"{callbackUrl}?atoken={response.AccessToken}&rtoken={response.RefreshToken}&vdate={response.AccessTokenValidateTo}");
                
                await cookieUtil.SetValueAsync("authToken", response);
                await authenticationStateProvider.GetAuthenticationStateAsync();
                layoutService.User = await userProfileService.Get();
                if (!newRegister)
                    navigationManager.NavigateTo("/");
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
            command.PhoneNumber = command.PhoneNumber.Trim();
            if (command.PhoneNumber.Length == 9) command.PhoneNumber = "+992" + command.PhoneNumber.Trim();
            else if (command.PhoneNumber.Length == 12 && command.PhoneNumber[0] != '+') command.PhoneNumber = "+" + command.PhoneNumber;

            var result = await httpClient.PostAsJsonAsync("Auth/register", command);
            if (result.IsSuccessStatusCode)
            {
                await LoginUserAsync(new LoginUserCommand()
                {
                    Password = command.Password,
                    Username = command.Username
                });
                layoutService.User = await userProfileService.Get();
                navigationManager.NavigateTo("/");

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
        var result = await httpClient.PostAsJsonAsync("Auth/ResetPassword", command);
        return result;
    }

    public async Task<HttpResponseMessage> CheckUserName(string userName)
    {
        var result = await httpClient.GetAsync($"User/CheckUserName/{userName}");
        return result;
    }

    public async Task<HttpResponseMessage> CheckUserDetails(CheckUserDetailsQuery query)
    {
        var result = await httpClient.GetAsync($"User/CheckUserDetails/{query.UserName}/{query.PhoneNumber}/{query.Email}");
        return result;
    }

    public async Task<HttpResponseMessage> ResendVerificationEmail()
    {
        var result = await httpClient.PostAsync("Auth/VerifyEmail", null);
        return result;
    }

    public async Task SendVerificationEmailToken(string token,string userId)
    {
        await httpClient.GetAsync($"Auth/verify?token={WebUtility.UrlEncode(token)}&userid={userId}");
    }
}