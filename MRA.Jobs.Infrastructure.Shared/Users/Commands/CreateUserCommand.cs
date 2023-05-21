using MediatR;

namespace MRA.Jobs.Infrastructure.Shared.Users.Commands;

public class CreateUserCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string[] Roles { get; set; }
}
