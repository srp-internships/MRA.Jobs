using MediatR;
using MRA.Identity.Application.Contract.UserRoles.Response;

namespace MRA.Identity.Application.Contract.UserRoles.Queries;
public class GetUserRolesQuery : IRequest<List<UserRolesResponse>>
{
    public string Role { get; set; }
    public string UserName { get; set; }


}
