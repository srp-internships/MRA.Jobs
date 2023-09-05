using System.Security.Claims;

namespace MRA.Identity.Application.Common.Interfaces.Services;

public interface IJwtTokenService
{
    internal string CreateTokenByClaims(IList<Claim> user);
    internal string CreateRefreshToken(IList<Claim> user);
}