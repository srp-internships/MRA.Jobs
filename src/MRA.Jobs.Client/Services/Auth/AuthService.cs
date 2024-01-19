using System.Net;
using AltairCA.Blazor.WebAssembly.Cookie;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MRA.Identity.Application.Contract.User.Commands.RegisterUser;
using MRA.Identity.Application.Contract.User.Commands.LoginUser;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Identity.Application.Contract.User.Commands.ChangePassword;
using MRA.Identity.Application.Contract.User.Commands.ResetPassword;
using MRA.Identity.Application.Contract.User.Queries.GetUserNameByPhoneNymber;
using MRA.Identity.Application.Contract.User.Queries.CheckUserDetails;
using MRA.Jobs.Client.Services.Profile;
using MRA.Jobs.Client.Services.HttpClients;

namespace MRA.Jobs.Client.Services.Auth;

public class AuthService(IdentityApiHttpClientService identityHttpClient,
        AuthenticationStateProvider authenticationStateProvider, NavigationManager navigationManager,
        IAltairCABlazorCookieUtil cookieUtil, LayoutService layoutService, IUserProfileService userProfileService)
    : IAuthService
{
    public async Task<ApiResponse<bool>> ChangePassword(ChangePasswordUserCommand command)
    {

        var result = await identityHttpClient.PutAsJsonAsync<bool>("Auth/ChangePassword", command);
        return result;
    }

    public async Task<ApiResponse<bool>> IsAvailableUserPhoneNumber(IsAvailableUserPhoneNumberQuery query)
    {
        var result = await identityHttpClient.GetAsJsonAsync<bool>($"Auth/IsAvailableUserPhoneNumber/{Uri.EscapeDataString(query.PhoneNumber)}");
        return result;
    }

    public async Task<string> LoginUserAsync(LoginUserCommand command, bool newRegister = false)
    {
        string errorMessage = null;
        try
        {
            var result = await identityHttpClient.PostAsJsonAsync<JwtTokenResponse>("Auth/login", command);
            if (result.Success)
            {
                var response = result.Result;
                await cookieUtil.SetValueAsync("authToken", response, secure:true);
                await authenticationStateProvider.GetAuthenticationStateAsync();
                layoutService.User = await userProfileService.Get();
                if (!newRegister)
                    navigationManager.NavigateTo("/");
                return null;
            }

            if (result.HttpStatusCode == HttpStatusCode.Unauthorized)
            {
                errorMessage = result.Error;
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

            var result = await identityHttpClient.PostAsJsonAsync<Guid>("Auth/register", command);
            if (result.Success)
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
            if (result.HttpStatusCode is not (HttpStatusCode.Unauthorized or HttpStatusCode.BadRequest))
                return "server error, please try again later";

            return result.Error;
         
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

    public async Task<ApiResponse<bool>> ResetPassword(ResetPasswordCommand command)
    {
        var result = await identityHttpClient.PostAsJsonAsync<bool>("Auth/ResetPassword", command);
        return result;
    }

    public async Task<ApiResponse> CheckUserName(string userName)
    {
        var result = await identityHttpClient.GetAsJsonAsync<ApiResponse>($"User/CheckUserName/{userName}");
        return result;
    }

    public async Task<ApiResponse<UserDetailsResponse>> CheckUserDetails(CheckUserDetailsQuery query)
    {
        var result = await identityHttpClient.GetAsJsonAsync<UserDetailsResponse>($"User/CheckUserDetails/{query.UserName}/{query.PhoneNumber}/{query.Email}");
        return result;
    }

    public async Task<ApiResponse> ResendVerificationEmail()
    {
        var result = await identityHttpClient.PostAsJsonAsync<ApiResponse>("Auth/VerifyEmail", null);
        return result;
    }
}