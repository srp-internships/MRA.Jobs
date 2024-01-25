using MediatR;
using MRA.Identity.Application.Contract.User.Responses;

namespace MRA.Identity.Application.Contract.User.Queries;
#nullable disable
public record GetAccessTokenUsingRefreshTokenQuery : IRequest<JwtTokenResponse>
{
    public string RefreshToken { get; set; }
    public string AccessToken { get; set; }
}
