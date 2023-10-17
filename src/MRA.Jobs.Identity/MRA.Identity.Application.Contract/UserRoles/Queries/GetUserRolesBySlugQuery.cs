using MediatR;
using MRA.Identity.Application.Contract.UserRoles.Response;

namespace MRA.Identity.Application.Contract.UserRoles.Queries;
public class GetUserRolesBySlugQuery : IRequest<UserRolesResponse>
{
    public string Slug { get; set; }
}
