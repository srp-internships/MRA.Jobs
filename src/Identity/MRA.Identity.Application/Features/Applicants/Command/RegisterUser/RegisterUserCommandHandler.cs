using MediatR;
using MRA.Identity.Application.Contract.User.Commands;

namespace MRA.Identity.Application.Features.Applicants.Command.RegisterUser;

public class RegisterUserCommandHandler:IRequestHandler<RegisterUserCommand,Guid>
{
    public Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}