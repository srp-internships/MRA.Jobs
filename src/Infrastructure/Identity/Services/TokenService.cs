// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Security.Cryptography;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.Extensions.Options;
// using MRA.Jobs.Infrastructure.Persistence;
// using MRA.Jobs.Infrastructure.Shared.Auth.Responses;
//
// namespace MRA.Jobs.Infrastructure.Identity.Services;
//
// public class TokenService
// {
//     private readonly IHttpContextAccessor _httpContextAccessor;
//     private readonly ApplicationDbContext _identityContext;
//     private readonly JwtSettings _jwtSettings;
//     private readonly UserManager<ApplicationUser> _userManager;
//
//     public TokenService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor,
//         ApplicationDbContext identityContext, IOptions<JwtSettings> jwtSettings)
//     {
//         _userManager = userManager;
//         _httpContextAccessor = httpContextAccessor;
//         _identityContext = identityContext;
//         _jwtSettings = jwtSettings.Value;
//     }
//
//     public async Task<JwtTokenResponse> GenerateTokens(ApplicationUser user,
//         CancellationToken cancellationToken = default)
//     {
//         (string Token, DateTime ValidTo) token = await GenerateAccessToken(user);
//         RefreshToken refreshToken = GenerateRefreshToken(user.Id);
//         await _identityContext.AddAsync(refreshToken, cancellationToken);
//         await _identityContext.SaveChangesAsync(cancellationToken);
//         return new JwtTokenResponse
//         {
//             AccessToken = token.Token,
//             RefreshToken = refreshToken.Token,
//             RefreshTokenValidTo = refreshToken.ExpiryOn
//         };
//     }
//
//     private async Task<(string Token, DateTime ValidTo)> GenerateAccessToken(ApplicationUser user)
//     {
//         IList<string> userRoles = await _userManager.GetRolesAsync(user);
//
//         List<Claim> authClaims = new List<Claim>
//         {
//             new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
//             new(JwtRegisteredClaimNames.Name, user.UserName),
//             new(JwtRegisteredClaimNames.Email, user.Email),
//             new(JwtRegisteredClaimNames.Jti, await _jwtSettings.JtiGenerator()),
//             new(JwtRegisteredClaimNames.Iat,
//                 new DateTimeOffset(_jwtSettings.IssuedAt).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
//         };
//
//         foreach (string userRole in userRoles)
//         {
//             authClaims.Add(new Claim(JwtRegisteredCustomClaimNames.Role, userRole));
//         }
//
//         JwtSecurityToken token = new(
//             _jwtSettings.Issuer,
//             _jwtSettings.Audience,
//             authClaims,
//             _jwtSettings.NotBefore,
//             _jwtSettings.Expiration,
//             _jwtSettings.SigningCredentials);
//
//         return (new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
//     }
//
//     private RefreshToken GenerateRefreshToken(Guid userId)
//     {
//         using RandomNumberGenerator rngCryptoServiceProvider = RandomNumberGenerator.Create();
//         byte[] randomBytes = new byte[64];
//         rngCryptoServiceProvider.GetBytes(randomBytes);
//
//         string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
//         return new RefreshToken
//         {
//             Token = Convert.ToBase64String(randomBytes),
//             ExpiryOn = DateTime.UtcNow.Add(_jwtSettings.RefreshTokenValidFor),
//             CreatedOn = DateTime.UtcNow,
//             CreatedByIp = ipAddress,
//             UserId = userId
//         };
//     }
//
//     public bool IsRefreshTokenValid(RefreshToken existingToken)
//     {
//         if (existingToken.RevokedByIp != null && existingToken.RevokedOn != DateTime.MinValue)
//         {
//             return false;
//         }
//
//         if (existingToken.ExpiryOn <= DateTime.UtcNow)
//         {
//             return false;
//         }
//
//         return true;
//     }
//
//     public async Task<JwtTokenResponse> RefreshToken(string token)
//     {
//         RefreshToken refreshToken = await _identityContext.RefreshTokens.Include(t => t.User)
//             .Where(t => t.Token == token).SingleOrDefaultAsync();
//         if (refreshToken is null || !IsRefreshTokenValid(refreshToken))
//         {
//             throw new BadHttpRequestException("Invalid refresh token!");
//         }
//
//         await RevokeRefreshToken(refreshToken);
//         return await GenerateTokens(refreshToken.User);
//     }
//
//     public async Task RevokeRefreshToken(string token)
//     {
//         RefreshToken refreshToken = await _identityContext.RefreshTokens.Include(t => t.User)
//             .Where(t => t.Token == token).SingleOrDefaultAsync();
//         if (refreshToken is null || !IsRefreshTokenValid(refreshToken))
//         {
//             throw new BadHttpRequestException("Invalid refresh token!");
//         }
//
//         await RevokeRefreshToken(refreshToken);
//     }
//
//     public async Task RevokeRefreshToken(RefreshToken refreshToken)
//     {
//         string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
//         refreshToken.RevokedByIp = ipAddress;
//         refreshToken.RevokedOn = DateTime.UtcNow;
//         _identityContext.Update(refreshToken);
//         await _identityContext.SaveChangesAsync();
//     }
//
//     
// }