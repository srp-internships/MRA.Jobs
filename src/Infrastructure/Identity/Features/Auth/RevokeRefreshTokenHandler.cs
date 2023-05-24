using FluentValidation;
using MRA.Jobs.Infrastructure.Shared.Auth.Commands;

namespace MRA.Jobs.Infrastructure.Identity.Features.Auth;

public class RevokeRefreshTokenCommandValidator : AbstractValidator<RevokeRefreshTokenCommand>
{
    public RevokeRefreshTokenCommandValidator()
    {
        RuleFor(x => x.Token).NotNull().NotEmpty();
    }
}

public class RevokeRefreshTokenHandler : IRequestHandler<RevokeRefreshTokenCommand, Unit>
{
    private readonly TokenService _tokenService;

    public RevokeRefreshTokenHandler(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<Unit> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        await _tokenService.RevokeRefreshToken(request.Token);

        return await Unit.Task;
    }
}
