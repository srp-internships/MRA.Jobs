using MediatR;
using MRA.Identity.Application.Contract.User.Responses;

namespace MRA.Identity.Application.Contract.User.Commands.GoogleAuth;

#nullable disable
public class GoogleAuthCommand: IRequest<JwtTokenResponse>
{
    public string Code { get; set; }
    public string Application { get; set; }
    public string Role { get; set; }
}