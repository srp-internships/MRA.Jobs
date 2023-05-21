using MediatR;

namespace MRA.Jobs.Infrastructure.Shared.Users.Commands;

public class ChangePhoneNumberCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public string NewPhoneNumber { get; set; }
}

public class ConfirmPhoneNumberChangeCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public string NewPhoneNumber { get; set; }
    public string Token { get; set; }
}
