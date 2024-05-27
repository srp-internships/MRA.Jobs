using MRA.Configurations.Common.Constants;
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
        var user = _httpContextAccessor.HttpContext?.User;
        if (user != null)
        {
            return user.FindFirst(ClaimTypes.Email)?.Value ?? throw new UnauthorizedAccessException();
        }

        throw new UnauthorizedAccessException();
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

    public string GetAuthToken()
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers.Authorization;
            if (!string.IsNullOrEmpty(token))
            {
                return token;
            }
        }
        return null;
    }
}