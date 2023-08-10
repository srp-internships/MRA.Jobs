using MediatR;
using MRA.Jobs.Infrastructure.Shared.Role.Responses;

namespace MRA.Jobs.Infrastructure.Shared.Role.Commands;

public class CreateRoleCommand : IRequest<RoleResponse>
{
    public string Name { get; set; }
}
