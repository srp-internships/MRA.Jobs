using MediatR;

namespace MRA.Jobs.Infrastructure.Shared.Users.Commands.Verifications;

public class ChangeEmailCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public string NewEmail { get; set; }
}