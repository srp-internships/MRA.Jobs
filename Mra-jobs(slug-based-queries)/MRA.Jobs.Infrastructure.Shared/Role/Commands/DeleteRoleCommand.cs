using MediatR;

namespace MRA.Jobs.Infrastructure.Shared.Role.Commands;

public class DeleteRoleCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}