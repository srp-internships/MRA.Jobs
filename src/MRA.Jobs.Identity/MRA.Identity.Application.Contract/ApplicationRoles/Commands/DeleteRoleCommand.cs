using MediatR;

namespace MRA.Identity.Application.Contract.ApplicationRoles.Commands;
public class DeleteRoleCommand : IRequest<bool>
{
    public string Slug { get; set; }
}
