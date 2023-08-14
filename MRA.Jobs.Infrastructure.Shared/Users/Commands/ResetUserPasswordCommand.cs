using MediatR;

namespace MRA.Jobs.Infrastructure.Shared.Users.Commands;

public class ResetUserPasswordCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
}