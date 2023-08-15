using MediatR;

namespace MRA.Jobs.Infrastructure.Shared.Auth.Commands;

public class RevokeRefreshTokenCommand : IRequest<Unit>
{
    public RevokeRefreshTokenCommand(string token)
    {
        Token = token;
    }

    public string Token { get; }
}