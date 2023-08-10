using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MRA.Identity.Application.Common.Interfaces.Services;

namespace MRA.Identity.Application.Services;

internal class JwtTokenService:IJwtTokenService
{
    internal static string SecretKey="123";

    public string CreateTokenByClaims(IList<Claim> claims)
    {

        var key = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(SecretKey));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}