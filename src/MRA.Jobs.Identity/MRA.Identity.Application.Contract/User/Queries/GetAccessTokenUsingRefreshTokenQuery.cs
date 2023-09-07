using MediatR;
using MRA.Identity.Application.Contract.Admin.Responses;

namespace MRA.Identity.Application.Contract.User.Queries;
#nullable disable
public record GetAccessTokenUsingRefreshTokenQuery : IRequest<ApplicationResponse<JwtTokenResponse>>
{
    public string RefreshToken { get; set; }
    public string AccessToken { get; set; }
}
