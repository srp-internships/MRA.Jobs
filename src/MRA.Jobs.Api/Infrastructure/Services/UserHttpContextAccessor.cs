using Microsoft.AspNetCore.Http;

namespace MRA.Jobs.Infrastructure.Services;
public class UserHttpContextAccessor : IUserHttpContextAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public Guid GetUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user is null)
        {
            
        }

        return Guid.NewGuid();
    }
}
