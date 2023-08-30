using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Users.Command.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ApplicationResponse<JwtTokenResponse>>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly UserManager<ApplicationUser> _userManager;

    public LoginUserCommandHandler(IJwtTokenService jwtTokenService, UserManager<ApplicationUser> userManager)
    {
        _jwtTokenService = jwtTokenService;
        _userManager = userManager;
    }

    public async Task<ApplicationResponse<JwtTokenResponse>> Handle(LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            ApplicationUser? user =
                await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.Username, cancellationToken);

            if (user == null)
            {
                return new ApplicationResponseBuilder<JwtTokenResponse>().Success(false)
                    .SetErrorMessage("incorrect username").Build();
            }

            bool success = await _userManager.CheckPasswordAsync(user, request.Password);

            if (success)
            {
                return new ApplicationResponseBuilder<JwtTokenResponse>().SetResponse(new JwtTokenResponse
                {
                    RefreshToken = _jwtTokenService.CreateRefreshToken(await _userManager.GetClaimsAsync(user)),
                    AccessToken = _jwtTokenService.CreateTokenByClaims(await _userManager.GetClaimsAsync(user))
                });
            }

            return new ApplicationResponseBuilder<JwtTokenResponse>().SetErrorMessage("incorrect password")
                .Success(false).Build();
        }
        catch (Exception e)
        {
            return new ApplicationResponseBuilder<JwtTokenResponse>().Success(false).SetException(e).Build();
        }
    }
}