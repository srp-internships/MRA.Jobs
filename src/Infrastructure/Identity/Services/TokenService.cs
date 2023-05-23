using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MRA.Jobs.Infrastructure.Identity.Authorization;
using MRA.Jobs.Infrastructure.Identity.Entities;
using MRA.Jobs.Infrastructure.Identity.Settings;
using MRA.Jobs.Infrastructure.Persistence;
using MRA.Jobs.Infrastructure.Shared.Auth.Responses;

namespace MRA.Jobs.Infrastructure.Identity.Services;

public class TokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _identityContext;
    private readonly JwtSettings _jwtSettings;

    public TokenService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, ApplicationDbContext identityContext, IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _identityContext = identityContext;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<JwtTokenResponse> GenerateTokens(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        var token = await GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken(user.Id);
        await _identityContext.AddAsync(refreshToken, cancellationToken);
        await _identityContext.SaveChangesAsync(cancellationToken);
        return new JwtTokenResponse() { AccessToken = token.Token, RefreshToken = refreshToken.Token, RefreshTokenValidTo = refreshToken.ExpiryOn };
    }

    private async Task<(string Token, DateTime ValidTo)> GenerateAccessToken(ApplicationUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti,await _jwtSettings.JtiGenerator()),
            new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(_jwtSettings.IssuedAt).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        foreach (var userRole in userRoles)
            authClaims.Add(new Claim(JwtRegisteredCustomClaimNames.Role, userRole));

        var token = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            authClaims,
            _jwtSettings.NotBefore,
            _jwtSettings.Expiration,
            _jwtSettings.SigningCredentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
    }

    private RefreshToken GenerateRefreshToken(Guid userId)
    {
        using var rngCryptoServiceProvider = RandomNumberGenerator.Create();
        var randomBytes = new byte[64];
        rngCryptoServiceProvider.GetBytes(randomBytes);

        var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            ExpiryOn = DateTime.UtcNow.Add(_jwtSettings.RefreshTokenValidFor),
            CreatedOn = DateTime.UtcNow,
            CreatedByIp = ipAddress,
            UserId = userId
        };
    }

    public bool IsRefreshTokenValid(RefreshToken existingToken)
    {
        if (existingToken.RevokedByIp != null && existingToken.RevokedOn != DateTime.MinValue)
            return false;

        if (existingToken.ExpiryOn <= DateTime.UtcNow)
            return false;

        return true;
    }

    public async Task<JwtTokenResponse> RefreshToken(string token)
    {
        var refreshToken = await _identityContext.RefreshTokens.Include(t => t.User).Where(t => t.Token == token).SingleOrDefaultAsync();
        if (refreshToken is null || !IsRefreshTokenValid(refreshToken))
            throw new BadHttpRequestException("Invalid refresh token!");

        await RevokeRefreshToken(refreshToken);
        return await GenerateTokens(refreshToken.User);
    }

    public async Task RevokeRefreshToken(string token)
    {
        var refreshToken = await _identityContext.RefreshTokens.Include(t => t.User).Where(t => t.Token == token).SingleOrDefaultAsync();
        if (refreshToken is null || !IsRefreshTokenValid(refreshToken))
            throw new BadHttpRequestException("Invalid refresh token!");

        await RevokeRefreshToken(refreshToken);
    }

    public async Task RevokeRefreshToken(RefreshToken refreshToken)
    {
        var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        refreshToken.RevokedByIp = ipAddress;
        refreshToken.RevokedOn = DateTime.UtcNow;
        _identityContext.Update(refreshToken);
        await _identityContext.SaveChangesAsync();
    }

    public async Task RevokeAllUserRefreshTokens(string userName)
    {
        var user = _identityContext.Users.Find(userName);
        if (user != null)
        {
            var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var tokens = user.RefreshTokens.Where(t => string.IsNullOrEmpty(t.RevokedByIp) && t.ExpiryOn > DateTime.UtcNow && t.CreatedByIp == ipAddress);
            foreach (var token in tokens)
            {
                token.RevokedByIp = ipAddress;
                token.RevokedOn = DateTime.UtcNow;
            }
            _identityContext.Update(user);
            await _identityContext.SaveChangesAsync();
        }
    }
}