using MediatR;

namespace MRA.Jobs.Infrastructure.Shared.Role.Commands;

public class RoleGrantPermissionCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public Guid[] Permissions { get; set; }
}