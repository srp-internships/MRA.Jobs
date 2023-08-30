using System.IdentityModel.Tokens.Jwt;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MRA.Identity.Application.Contract.Admin.Responses;
using MRA.Identity.Application.Contract.User.Queries;
using System.Security.Claims;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Users.Query;
public class GetAccesTokenUsingRefreshTokenQueryHandler : IRequestHandler<GetAccesTokenUsingRefreshTokenQuery, ApplicationResponse<JwtTokenResponse>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenService _tokenService;
    public GetAccesTokenUsingRefreshTokenQueryHandler(IConfiguration configuration, UserManager<ApplicationUser> userManager, IJwtTokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    async Task<ApplicationResponse<JwtTokenResponse>> IRequestHandler<GetAccesTokenUsingRefreshTokenQuery, ApplicationResponse<JwtTokenResponse>>.Handle(GetAccesTokenUsingRefreshTokenQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var claims = await ValidateToken(request.RefreshToken);
            if (claims != null)
            {
                return new ApplicationResponseBuilder<JwtTokenResponse>().SetResponse(new JwtTokenResponse
                {
                    AccessToken = _tokenService.CreateTokenByClaims(claims),
                    RefreshToken = _tokenService.CreateRefreshToken(claims)
                });
            }
            else return new ApplicationResponseBuilder<JwtTokenResponse>().Success(false).SetErrorMessage("Could not validate token").Build();
        }
        catch (Exception ex)
        {
            return new ApplicationResponseBuilder<JwtTokenResponse>().Success(false).SetErrorMessage(ex.Message).Build();
        }
    }
    private async Task<List<Claim>> ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        if (tokenHandler.CanReadToken(token))
        {
            var jwtToken = tokenHandler.ReadJwtToken(token);
            List<Claim> claims = jwtToken.Claims.ToList();

            string? userId = claims.Where(c => c.Type == Mra.Shared.Common.Constants.ClaimTypes.Id).Select(c => c.Value).FirstOrDefault();

            if (userId != null)
            {
                return claims;
            }
            else throw new Exception("Token is not valid! This user does not exist in database anymore");
        }
        else throw new Exception("Token is not valid! Can not read it");
    }
}