using System.Security.Claims;
using Google.Apis.Auth;
using MRA.Identity.Application.Contract.Admin.Responses;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Common.Interfaces.Services;

public interface IJwtTokenService
{
    internal string CreateTokenByClaims(IList<Claim> user);
    internal string CreateRefreshToken(IList<Claim> user);
}