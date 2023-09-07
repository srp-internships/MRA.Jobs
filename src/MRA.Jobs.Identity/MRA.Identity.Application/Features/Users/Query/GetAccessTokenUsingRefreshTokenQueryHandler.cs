using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using MRA.Identity.Application.Contract.Admin.Responses;
using MRA.Identity.Application.Contract.User.Queries;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract;
using ClaimTypes = Mra.Shared.Common.Constants.ClaimTypes;

namespace MRA.Identity.Application.Features.Users.Query;

public class GetAccessTokenUsingRefreshTokenQueryHandler : IRequestHandler<GetAccessTokenUsingRefreshTokenQuery,
    ApplicationResponse<JwtTokenResponse>>
{
    private readonly IJwtTokenService _tokenService;

    public GetAccessTokenUsingRefreshTokenQueryHandler(IJwtTokenService tokenService)
    {
        _tokenService = tokenService;
    }

    Task<ApplicationResponse<JwtTokenResponse>>
        IRequestHandler<GetAccessTokenUsingRefreshTokenQuery, ApplicationResponse<JwtTokenResponse>>.Handle(
            GetAccessTokenUsingRefreshTokenQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (!AreTokensRelated(request))
            {
                return Task.FromResult(new ApplicationResponseBuilder<JwtTokenResponse>().Success(false)
                    .SetErrorMessage("Tokens are not related").Build());
            }

            var claims = GetTokenClaims(request.AccessToken);
            if (claims.Count != 0)
            {
                return Task.FromResult<ApplicationResponse<JwtTokenResponse>>(
                    new ApplicationResponseBuilder<JwtTokenResponse>().SetResponse(new JwtTokenResponse
                    {
                        AccessToken = _tokenService.CreateTokenByClaims(claims),
                        RefreshToken = _tokenService.CreateRefreshToken(claims)
                    }));
            }

            return Task.FromResult(new ApplicationResponseBuilder<JwtTokenResponse>().Success(false)
                .SetErrorMessage("Could not validate token").Build());
        }
        catch (Exception ex)
        {
            return Task.FromResult(new ApplicationResponseBuilder<JwtTokenResponse>().Success(false)
                .SetErrorMessage(ex.Message)
                .Build());
        }
    }

    private bool AreTokensRelated(GetAccessTokenUsingRefreshTokenQuery query)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var refreshClaims = tokenHandler.ReadJwtToken(query.RefreshToken);
        var refreshUserId = refreshClaims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Id);

        var accessClaims = tokenHandler.ReadJwtToken(query.AccessToken);
        var accessUserId = accessClaims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Id);

        return refreshUserId != null &&
               accessUserId != null &&
               refreshUserId.Value == accessUserId.Value;
    }

    private List<Claim> GetTokenClaims(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        if (!tokenHandler.CanReadToken(token))
        {
            throw new Exception("Token is not valid! Can not read it");
        }

        var jwtToken = tokenHandler.ReadJwtToken(token);
        List<Claim> claims = jwtToken.Claims.ToList();

        return claims;
    }
}