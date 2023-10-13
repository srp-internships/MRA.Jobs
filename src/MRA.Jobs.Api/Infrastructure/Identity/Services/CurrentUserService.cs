using Mra.Shared.Common.Constants;
using Microsoft.AspNetCore.Http;

namespace MRA.Jobs.Infrastructure.Identity.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? GetUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user != null)
        {
            //  return Guid.Parse(user.FindFirst(ClaimTypes.Id).Value);
            var claim = user.FindFirst(ClaimTypes.Id);
            if (claim != null)
            {
                return Guid.Parse(claim.Value);
            }
        }
        return Guid.Empty;
    }

    public string GetEmail()
    {
        throw new NotImplementedException();
    }

    public string GetUserName()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user != null)
        {
            var claim = user.FindFirst(ClaimTypes.Username);
            if (claim != null)
            {
                return claim.Value;
            }
        }
        return string.Empty;
    }
    public List<string> GetRoles()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var roleClaims = user?.FindAll(ClaimTypes.Role);

        return roleClaims?.Select(rc => rc.Value).ToList() ?? new List<string>();
    }
}