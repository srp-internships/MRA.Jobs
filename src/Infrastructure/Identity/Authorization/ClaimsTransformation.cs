using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.JsonWebTokens;

namespace MRA.Jobs.Infrastructure.Identity.Authorization;

internal class ClaimsTransformation : IClaimsTransformation
{
    private readonly CurrentUserService _currentUser;

    public ClaimsTransformation(ICurrentUserService currentUser)
    {
        _currentUser = currentUser as CurrentUserService;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.FindFirstValue(JwtRegisteredClaimNames.Sub) is { Length: > 0 } id)
        {
            _currentUser.Id = Guid.Parse(id);
            _currentUser.Email = principal.FindFirstValue(JwtRegisteredClaimNames.Email);
            _currentUser.UserName = principal.FindFirstValue(JwtRegisteredClaimNames.Name);
            _currentUser.Roles = principal.FindAll(JwtRegisteredCustomClaimNames.Role).Select(c => c.Value).ToArray();
        }

        return await Task.FromResult(principal);
    }
}

public struct JwtRegisteredCustomClaimNames
{
    public const string Role = "roles";
}