using MediatR;

namespace MRA.Identity.Application.Contract.ApplicationRoles.Commands;
public class UpdateRoleCommand : IRequest<Guid>
{
    public string Slug { get; set; }
    public string NewRoleName { get; set; }
}
