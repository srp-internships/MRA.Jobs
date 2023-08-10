using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Applicants.Command.LoginUser;

public class LoginUserCommandHandler:IRequestHandler<LoginUserCommand,JwtTokenResponse?>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    public LoginUserCommandHandler(IJwtTokenService jwtTokenService, UserManager<ApplicationUser> userManager)
    {
        _jwtTokenService = jwtTokenService;
        _userManager = userManager;
    }

    public async Task<JwtTokenResponse?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user=await _userManager.Users.FirstOrDefaultAsync(u => u.UserName ==request.Username, cancellationToken: cancellationToken);
        
        var success =user != null && await _userManager.CheckPasswordAsync(user, request.Password);

        if (success)
        {
            return new JwtTokenResponse
            {
                AccessToken = _jwtTokenService.CreateTokenByClaims(await _userManager.GetClaimsAsync(user))
            };
        }
        return null;
    }
}