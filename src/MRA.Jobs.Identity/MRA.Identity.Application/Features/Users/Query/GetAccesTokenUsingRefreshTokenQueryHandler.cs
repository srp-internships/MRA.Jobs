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
using Azure.Core;

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
            if (AreTokensRelated(request))
            {
                var claims = await GetTokenClaims(request.AccessToken);
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
            else return new ApplicationResponseBuilder<JwtTokenResponse>().Success(false).SetErrorMessage("Tokens are not realted").Build();
        }
        catch (Exception ex)
        {
            return new ApplicationResponseBuilder<JwtTokenResponse>().Success(false).SetErrorMessage(ex.Message).Build();
        }
    }

    private bool AreTokensRelated(GetAccesTokenUsingRefreshTokenQuery query)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var refreshClaims = tokenHandler.ReadJwtToken(query.RefreshToken);
        string refreshUserId = refreshClaims.Claims.Where(c=>c.Type==ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;

        var accessClaims = tokenHandler.ReadJwtToken(query.AccessToken);
        string accessUserId = accessClaims.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;

        return refreshUserId == accessUserId;
    }
    private async Task<List<Claim>> GetTokenClaims(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        if (tokenHandler.CanReadToken(token))
        {
            var jwtToken = tokenHandler.ReadJwtToken(token);
            List<Claim> claims = jwtToken.Claims.ToList();

            return claims;
        }
        else throw new Exception("Token is not valid! Can not read it");
    }
}