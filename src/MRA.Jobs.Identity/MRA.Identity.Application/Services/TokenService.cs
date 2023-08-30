

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.Admin.Responses;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Services;

public class TokenService : IGoogleTokenService
{
    private readonly IConfiguration _configuration;
    private readonly IConfigurationSection _jwtSettings;
    private readonly IConfigurationSection _goolgeSettings;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _dbContext;
    private readonly TimeSpan _accessTokenexparationTime;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TimeSpan _refreshTokenexparationTime;

    public TokenService(IConfiguration configuration, UserManager<ApplicationUser> userManager, IApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _configuration = configuration;
        _jwtSettings = _configuration.GetSection("Jwt");
        _goolgeSettings = _configuration.GetSection("Auth:Google");
        _accessTokenexparationTime = new TimeSpan(24, 0, 0);
        _refreshTokenexparationTime = new TimeSpan(30, 0, 0, 0);
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(GoogleAuthCommand externalAuth)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new[] { _goolgeSettings["ClientId"] }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.Token, settings);
            return payload;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<JwtTokenResponse> GenerateTokens(ApplicationUser user)
    {
        var token = await GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken(user.Id);
        user.RefreshTokens.Add(refreshToken);
        _dbContext.Update(user);
        await _dbContext.SaveChangesAsync();
        return new JwtTokenResponse()
        {
            AccessToken = token.Token,
            AccessTokenValidTo = token.ValidTo,
            RefreshToken = refreshToken.Token,
            RefreshTokenValidTo = refreshToken.ExpiryOn
        };
    }

    private async Task<(string Token, DateTime ValidTo)> GenerateAccessToken(ApplicationUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Sid,user.SecurityStamp),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings["SecretKey"]));
        var credentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtSettings["Issuer"],
            audience: _jwtSettings["Issuer"],
            expires: DateTime.UtcNow.Add(_accessTokenexparationTime),
            claims: authClaims,
            signingCredentials: credentials
        );
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
            ExpiryOn = DateTime.UtcNow.Add(_refreshTokenexparationTime),
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

    public DateTime GetValidToDate(DateTime? from = null) => (from ?? DateTime.UtcNow).Add(_accessTokenexparationTime);

    public RefreshToken GetValidRefreshToken(string token, ApplicationUser identityUser)
    {
        if (identityUser == null)
            return null;

        var existingToken = identityUser.RefreshTokens.FirstOrDefault(x => x.Token == token);
        return IsRefreshTokenValid(existingToken) ? existingToken : null;
    }

    public async Task<JwtTokenResponse> RefreshToken(string token, ApplicationUser user)
    {
        var existingRefreshToken = GetValidRefreshToken(token, user);
        if (existingRefreshToken == null)
            throw new BadHttpRequestException("Invalid refresh token!");

        var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        existingRefreshToken.RevokedByIp = ipAddress;
        existingRefreshToken.RevokedOn = DateTime.UtcNow;
        return await GenerateTokens(user);
    }

    public async Task RevokeRefreshToken(string token, ApplicationUser user)
    {
        var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        var existingToken = user.RefreshTokens.FirstOrDefault(x => x.Token == token);
        existingToken.RevokedByIp = ipAddress;
        existingToken.RevokedOn = DateTime.UtcNow;
        _dbContext.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RevokeRefreshToken(string userName)
    {
        var user = _dbContext.Users.Find(userName);
        if (user != null)
        {
            var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var tokens = user.RefreshTokens.Where(t => string.IsNullOrEmpty(t.RevokedByIp) && t.ExpiryOn > DateTime.UtcNow && t.CreatedByIp == ipAddress);
            foreach (var token in tokens)
            {
                token.RevokedByIp = ipAddress;
                token.RevokedOn = DateTime.UtcNow;
            }
            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}