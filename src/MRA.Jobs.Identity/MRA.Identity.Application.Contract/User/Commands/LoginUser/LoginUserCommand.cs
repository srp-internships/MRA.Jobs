using MediatR;
using MRA.Identity.Application.Contract.User.Responses;

namespace MRA.Identity.Application.Contract.User.Commands.LoginUser;

public class LoginUserCommand : IRequest<ApplicationResponse<JwtTokenResponse>>
{
    public string Username { get; set; }
    public string Password { get; set; }
}