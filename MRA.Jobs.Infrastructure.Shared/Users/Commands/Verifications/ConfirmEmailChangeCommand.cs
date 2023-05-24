using MediatR;

namespace MRA.Jobs.Infrastructure.Shared.Users.Commands.Verifications;

public class ConfirmEmailChangeCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public string NewEmail { get; set; }
    public string Token { get; set; }
}
