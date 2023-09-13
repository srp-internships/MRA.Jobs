using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Mra.Shared.Common.Constants;
using MRA.Identity.Application.Common.Interfaces.Services;

namespace MRA.Identity.Infrastructure.Account.Services;
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
        if (user != null)
        {
            var idClaim = user.Claims.First(s=>s.Type==ClaimTypes.Id);
            
            if (idClaim != null && Guid.TryParse(idClaim.Value, out Guid id))
                return id;
        }
        return Guid.Empty;
    }
}
