using MRA.Jobs.Infrastructure.Shared.Auth.Commands;
using MRA.Jobs.Infrastructure.Shared.Auth.Responses;

namespace MRA.Jobs.Client.Services.AuthService;

public interface IAuthService
{
    Task<JwtTokenResponse> Login(string email, string password);
    Task Logout();

}
