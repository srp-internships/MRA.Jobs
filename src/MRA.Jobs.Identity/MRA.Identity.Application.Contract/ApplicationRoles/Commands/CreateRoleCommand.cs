using MediatR;

namespace MRA.Identity.Application.Contract.ApplicationRoles.Commands;

public class CreateRoleCommand:IRequest<ApplicationResponse<Guid>>
{
    public string RoleName { get; set; }
}