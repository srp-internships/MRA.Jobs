using MRA.Identity.Application.Contract.User.Commands.ChangePassword;
using MRA.Identity.Application.Contract.User.Commands.LoginUser;
using MRA.Identity.Application.Contract.User.Commands.RegisterUser;
using MRA.Identity.Application.Contract.User.Commands.ResetPassword;
using MRA.Identity.Application.Contract.User.Queries.CheckUserDetails;
using MRA.Identity.Application.Contract.User.Queries.GetUserNameByPhoneNymber;

namespace MRA.Jobs.Client.Services.Auth;

public interface IAuthService
{
    public const string TokenLocalStorageKey = "TokenLocalStorageKey";
    Task<string> RegisterUserAsync(RegisterUserCommand command);
    Task<string> LoginUserAsync(LoginUserCommand command, bool newRegister = false);
    Task<HttpResponseMessage> ChangePassword(ChangePasswordUserCommand command);
    Task<HttpResponseMessage> IsAvailableUserPhoneNumber(IsAvailableUserPhoneNumberQuery query);
    Task<HttpResponseMessage> ResetPassword(ResetPasswordCommand command);
    Task<HttpResponseMessage> CheckUserName(string userName);
    Task<HttpResponseMessage> CheckUserDetails(CheckUserDetailsQuery checkUserDetailsQuery);
    Task<HttpResponseMessage> ResendVerificationEmail();
}
