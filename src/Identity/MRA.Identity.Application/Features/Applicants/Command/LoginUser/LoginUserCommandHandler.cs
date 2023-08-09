using MediatR;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Application.Contract.User.Responses;

namespace MRA.Identity.Application.Features.Applicants.Command.LoginUser;

public class LoginUserCommandHandler:IRequestHandler<LoginUserCommand,JwtTokenResponse>
{
    public Task<JwtTokenResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}