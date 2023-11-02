using MediatR;

namespace MRA.Identity.Application.Contract.UserRoles.Commands;
public class CreateUserRolesCommand : IRequest<string>
{
    public string RoleName{ get; set; }
    public string UserName { get; set; }

}
