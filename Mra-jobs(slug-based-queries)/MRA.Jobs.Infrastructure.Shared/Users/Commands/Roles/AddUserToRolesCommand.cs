using MediatR;

namespace MRA.Jobs.Infrastructure.Shared.Users.Commands.Roles;

public class AddUserToRolesCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public IEnumerable<string> Roles { get; set; }
}
