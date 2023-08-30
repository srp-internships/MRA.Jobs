using Google.Apis.Auth;
using MRA.Identity.Application.Contract.Admin.Responses;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Common.Interfaces.Services;
public interface IGoogleTokenService
{
    Task<JwtTokenResponse> GenerateTokens(ApplicationUser user);
    RefreshToken GetValidRefreshToken(string token, ApplicationUser identityUser);
    DateTime GetValidToDate(DateTime? from = null);
    bool IsRefreshTokenValid(RefreshToken existingToken);
    Task<JwtTokenResponse> RefreshToken(string token, ApplicationUser user);
    Task RevokeRefreshToken(string token, ApplicationUser user);
    Task RevokeRefreshToken(string userName);
    Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(GoogleAuthCommand externalAuth);
}