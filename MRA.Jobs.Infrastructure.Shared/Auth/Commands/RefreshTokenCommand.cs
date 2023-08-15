using MediatR;
using MRA.Jobs.Infrastructure.Shared.Auth.Responses;

namespace MRA.Jobs.Infrastructure.Shared.Auth.Commands;

public class RefreshTokenCommand : IRequest<JwtTokenResponse>
{
    public string Token { get; set; }
}