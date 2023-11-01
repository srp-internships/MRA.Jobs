using MRA.Identity.Application.Contract.User.Commands.ChangePassword;
using MRA.Identity.Application.Contract.User.Commands.LoginUser;
using MRA.Identity.Application.Contract.User.Commands.RegisterUser;
using MRA.Identity.Application.Contract.User.Commands.ResetPassword;
using MRA.Identity.Application.Contract.User.Queries.GetUserNameByPhoneNymber;

namespace MRA.Jobs.Client.Services.Auth;

public interface IAuthService
{
    Task<string> RegisterUserAsync(RegisterUserCommand command);
    Task<string> LoginUserAsync(LoginUserCommand command, bool newRegister = false);
    Task<HttpResponseMessage> ChangePassword(ChangePasswordUserCommand command);
    Task<HttpResponseMessage> GetUserNameByPhoneNumber(GetUserNameByPhoneNumberQuery query);
    Task<HttpResponseMessage> ResetPassword(ResetPasswordCommand command);
}
