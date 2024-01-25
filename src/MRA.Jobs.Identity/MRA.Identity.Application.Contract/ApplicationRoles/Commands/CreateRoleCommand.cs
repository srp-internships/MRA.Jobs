using MediatR;

namespace MRA.Identity.Application.Contract.ApplicationRoles.Commands;

public class CreateRoleCommand:IRequest<Guid>
{
    public string RoleName { get; set; }
}