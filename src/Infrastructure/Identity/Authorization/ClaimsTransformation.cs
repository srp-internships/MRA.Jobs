﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.JsonWebTokens;
using MRA.Jobs.Infrastructure.Identity.Services;

namespace MRA.Jobs.Infrastructure.Identity.Authorization;

class ClaimsTransformation : IClaimsTransformation
{
    private readonly CurrentUserService _currentUser;

    public ClaimsTransformation(CurrentUserService currentUser)
    {
        _currentUser = currentUser;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.FindFirstValue(JwtRegisteredClaimNames.Sub) is { Length: > 0 } id)
        {
            _currentUser.Id = Guid.Parse(id);
            _currentUser.Email = principal.FindFirstValue(JwtRegisteredClaimNames.Email);
            _currentUser.Email = principal.FindFirstValue(JwtRegisteredClaimNames.Name);
            _currentUser.Roles = principal.FindAll(JwtRegisteredCustomClaimNames.Role).Select(c => c.Value).ToArray();
        }
        return await Task.FromResult(principal);
    }
}

public struct JwtRegisteredCustomClaimNames
{
    public const string Role = "roles";
}