using MediatR;

namespace MRA.Jobs.Infrastructure.Shared.Users.Commands.Verifications;

public class ConfirmPhoneNumberChangeCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public string NewPhoneNumber { get; set; }
    public string Code { get; set; }
}