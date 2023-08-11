using MediatR;
using MRA.Jobs.Infrastructure.Shared.Pemission.Responces;

namespace MRA.Jobs.Infrastructure.Shared.Role.Queries;

public class GetRolePermissionsQuery : IRequest<IEnumerable<PermissionResponse>>
{
    public Guid Id { get; set; }
}