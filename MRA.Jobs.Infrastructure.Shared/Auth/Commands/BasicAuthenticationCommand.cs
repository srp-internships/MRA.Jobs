using MediatR;
using MRA.Jobs.Infrastructure.Shared.Auth.Responses;

namespace MRA.Jobs.Infrastructure.Shared.Auth.Commands;

public class BasicAuthenticationCommand : IRequest<JwtTokenResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}