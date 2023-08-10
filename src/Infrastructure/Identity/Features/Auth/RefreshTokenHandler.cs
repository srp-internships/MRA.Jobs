using FluentValidation;
using MRA.Jobs.Infrastructure.Shared.Auth.Commands;
using MRA.Jobs.Infrastructure.Shared.Auth.Responses;

namespace MRA.Jobs.Infrastructure.Identity.Features.Auth;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.Token).NotNull().NotEmpty();
    }
}

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, JwtTokenResponse>
{
    private readonly TokenService _tokenService;

    public RefreshTokenHandler(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<JwtTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return await _tokenService.RefreshToken(request.Token);
    }
}