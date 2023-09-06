using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Domain.Entities;
using Google.Apis.Auth;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Identity;
using MRA.Identity.Application.Contract.Admin.Responses;

namespace MRA.Identity.Application.Services;

internal class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;
    

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateTokenByClaims(IList<Claim> claims)
    {
        SymmetricSecurityKey key = new(Encoding.UTF8
            .GetBytes(_configuration.GetSection("JwtSettings")["SecurityKey"]!));

        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);

        JwtSecurityToken token = new(
            claims: claims,
            expires: DateTime.Now.AddDays(int.Parse(_configuration.GetSection("JwtSettings")["ExpireDayFromNow"]!)),
            signingCredentials: creds);

        string? jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
    public string CreateRefreshToken(IList<Claim> claims)
    {
         SymmetricSecurityKey key = new(Encoding.UTF8
            .GetBytes(_configuration.GetSection("JwtSettings")["SecurityKey"]!));

        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);

        JwtSecurityToken token = new(
            claims: claims,
            expires: DateTime.Now.AddDays(60),
            signingCredentials: creds);

        string? jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}