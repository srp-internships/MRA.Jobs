using System.Security.Claims;

using MRA.Jobs.Application.Common.Interfaces;

namespace MRA.Jobs.Web.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

    }

    public Guid UserId
    {
        get
        {
            var id = Guid.Empty;
            Guid.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier), out id);
            return id;
        }
    }
}
