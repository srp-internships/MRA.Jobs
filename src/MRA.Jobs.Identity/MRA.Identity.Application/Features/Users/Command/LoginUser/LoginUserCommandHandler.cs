using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.User.Commands.LoginUser;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Users.Command.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, JwtTokenResponse>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly UserManager<ApplicationUser> _userManager;

    public LoginUserCommandHandler(IJwtTokenService jwtTokenService, UserManager<ApplicationUser> userManager)
    {
        _jwtTokenService = jwtTokenService;
        _userManager = userManager;
    }

    public async Task<JwtTokenResponse> Handle(LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        ApplicationUser? user =
            await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.Username, cancellationToken);
        _ = user ?? throw new NotFoundException(nameof(user), request.Username);

        bool success = await _userManager.CheckPasswordAsync(user, request.Password);
      
        if (success)
        {
            return new JwtTokenResponse
            {
                RefreshToken = _jwtTokenService.CreateRefreshToken(await _userManager.GetClaimsAsync(user)),
                AccessToken = _jwtTokenService.CreateTokenByClaims(await _userManager.GetClaimsAsync(user), out var expireDate),
                AccessTokenValidTo = expireDate
            };
        }
        throw new UnauthorizedAccessException("incorrect password");
    }
}
