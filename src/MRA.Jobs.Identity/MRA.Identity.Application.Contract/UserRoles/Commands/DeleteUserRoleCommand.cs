using MediatR;

namespace MRA.Identity.Application.Contract.UserRoles.Commands;
public class DeleteUserRoleCommand : IRequest<bool>
{
    public string Slug { get; set; }
}
