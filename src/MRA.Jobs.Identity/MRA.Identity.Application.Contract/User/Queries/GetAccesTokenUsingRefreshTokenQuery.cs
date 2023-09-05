using MediatR;
using MRA.Identity.Application.Contract.Admin.Responses;

namespace MRA.Identity.Application.Contract.User.Queries;
public record GetAccesTokenUsingRefreshTokenQuery : IRequest<ApplicationResponse<JwtTokenResponse>>
{
    public string RefreshToken { get; set; }
    public string AccessToken { get; set; }
}
