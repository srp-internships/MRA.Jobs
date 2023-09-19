using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Admin.Responses;
using MRA.Identity.Application.Contract.User.Commands;

namespace MRA.Jobs.Client.Services.Auth;

public interface IAuthService
{
    Task<bool> RegisterUserAsync(RegisterUserCommand command);
    Task<string> LoginUserAsync(LoginUserCommand command);
}
