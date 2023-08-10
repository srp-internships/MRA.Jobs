using System.Security.Claims;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Common.Interfaces.Services;

public interface IJwtTokenService
{
    internal string CreateTokenByClaims(IList<Claim> user);
}