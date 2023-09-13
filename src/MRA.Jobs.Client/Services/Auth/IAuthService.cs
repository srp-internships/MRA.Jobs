using MRA.Identity.Application.Contract.User.Commands;

namespace MRA.Jobs.Client.Services.Auth;

public interface IAuthService
{
    Task<bool> RegisterUserAsync(RegisterUserCommand command);
}
