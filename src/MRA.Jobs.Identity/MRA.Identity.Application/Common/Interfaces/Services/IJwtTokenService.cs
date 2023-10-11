using System.Security.Claims;

namespace MRA.Identity.Application.Common.Interfaces.Services;

public interface IJwtTokenService
{
    public string CreateTokenByClaims(IList<Claim> user, out DateTime expireDate);
    public string CreateRefreshToken(IList<Claim> user);
}