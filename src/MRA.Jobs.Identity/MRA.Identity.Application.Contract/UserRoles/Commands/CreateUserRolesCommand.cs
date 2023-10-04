using MediatR;

namespace MRA.Identity.Application.Contract.UserRoles.Commands;
public class CreateUserRolesCommand : IRequest<ApplicationResponse<string>>
{
    public string RoleName{ get; set; }
    public Guid UserId { get; set; }

}
