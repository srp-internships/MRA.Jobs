using MRA.Identity.Application.Contract.User.Commands.ChangePassword;
using MRA.Identity.Application.Contract.User.Commands.LoginUser;
using MRA.Identity.Application.Contract.User.Commands.RegisterUser;
using MRA.Identity.Application.Contract.User.Commands.ResetPassword;
using MRA.Identity.Application.Contract.User.Queries.CheckUserDetails;
using MRA.Identity.Application.Contract.User.Queries.GetUserNameByPhoneNymber;
using MRA.Identity.Application.Contract.User.Responses;

namespace MRA.Jobs.SSR.Client.Services.Auth;

public interface IAuthService
{
    public const string TokenLocalStorageKey = "TokenLocalStorageKey";
    Task<string> RegisterUserAsync(RegisterUserCommand command);
    Task<string> LoginUserAsync(LoginUserCommand command, bool newRegister = false);
    Task<ApiResponse<bool>> ChangePassword(ChangePasswordUserCommand command);
    Task<ApiResponse<bool>> IsAvailableUserPhoneNumber(IsAvailableUserPhoneNumberQuery query);
    Task<ApiResponse<bool>> ResetPassword(ResetPasswordCommand command);
    Task<ApiResponse> CheckUserName(string userName);
    Task<ApiResponse<UserDetailsResponse>> CheckUserDetails(CheckUserDetailsQuery checkUserDetailsQuery);
    Task<ApiResponse> ResendVerificationEmail();
}
