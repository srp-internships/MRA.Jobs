using Google.Apis.Auth;

namespace MRA.Identity.Application.Common.Interfaces.Services;
public interface IGoogleTokenService
{
    Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string Token);
}