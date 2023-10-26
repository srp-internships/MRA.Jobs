using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using MRA.Identity.Application.Common.Interfaces.Services;

namespace MRA.Identity.Application.Services;

public class TokenService : IGoogleTokenService
{
    private readonly IConfigurationSection _googleSettings;

    public TokenService(IConfiguration configuration)
    {
        _googleSettings = configuration.GetSection("Google");
    }

    public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string token)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new[] { _googleSettings["ClientId"] }
        };
        var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
        return payload;
    }
}