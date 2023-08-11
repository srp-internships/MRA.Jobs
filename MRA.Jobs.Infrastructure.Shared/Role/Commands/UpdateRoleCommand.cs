using MediatR;
using MRA.Jobs.Infrastructure.Shared.Role.Responses;

namespace MRA.Jobs.Infrastructure.Shared.Role.Commands;

public class UpdateRoleCommand : IRequest<RoleResponse>
{
    public Guid Id { get; set; }

    public string Name { get; set; }
}