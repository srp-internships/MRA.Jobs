using Microsoft.AspNetCore.Http;
using MRA.Configurations.Common.Constants;

namespace MRA.Jobs.Infrastructure.Services;

public class UserHttpContextAccessor(IHttpContextAccessor httpContextAccessor) : IUserHttpContextAccessor
{
    public bool IsAuthenticated()
    {
        var user = httpContextAccessor.HttpContext?.User;
        return user?.Identity?.IsAuthenticated ?? false;
    }
    
    public Guid GetUserId()
    {
        var user = httpContextAccessor.HttpContext?.User;

        var idClaim = user?.FindFirst(ClaimTypes.Id);

        if (idClaim != null && Guid.TryParse(idClaim.Value, out Guid id))
            return id;

        return Guid.Empty;
    }

    public string GetUserName()
    {
        var user = httpContextAccessor.HttpContext?.User;
        var userNameClaim = user?.FindFirst(ClaimTypes.Username);

        return userNameClaim != null ? userNameClaim.Value : string.Empty;
    }

    public List<string> GetUserRoles()
    {
        var user = httpContextAccessor.HttpContext?.User;
        var roleClaims = user?.FindAll(ClaimTypes.Role);

        return roleClaims?.Select(rc => rc.Value).ToList() ?? new List<string>();
    }
}