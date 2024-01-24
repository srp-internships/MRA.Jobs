using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MRA.Identity.Application.Common.Interfaces.Services;

namespace MRA.Identity.Application.Services;

internal class JwtTokenService(IConfiguration configuration) : IJwtTokenService
{
    public string CreateTokenByClaims(IList<Claim> claims, out DateTime expireDate)
    {
        SymmetricSecurityKey key = new(Encoding.UTF8
            .GetBytes(configuration["JWT:Secret"]!));

        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);
        expireDate = DateTime.Now.AddDays(int.Parse(configuration["JWT:RefreshTokenValidityInDays"]!));
        JwtSecurityToken token = new(
            claims: claims,
            expires: expireDate,
            signingCredentials: creds);

        string jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    public string CreateRefreshToken(IList<Claim> claims)
    {
        SymmetricSecurityKey key = new(Encoding.UTF8
            .GetBytes(configuration["JWT:Secret"]!));

        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);

        JwtSecurityToken token = new(
            claims: claims,
            expires: DateTime.Now.AddDays(
                int.Parse(configuration["JWT:RefreshTokenValidityInDays"]!)),
            signingCredentials: creds);

        string jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}