using MediatR;

namespace MRA.Jobs.Infrastructure.Shared.Users.Commands;

public class ChangePasswordCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}
