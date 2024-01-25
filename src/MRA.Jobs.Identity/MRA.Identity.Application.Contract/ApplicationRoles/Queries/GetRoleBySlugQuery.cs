using MediatR;
using MRA.Identity.Application.Contract.ApplicationRoles.Responses;

namespace MRA.Identity.Application.Contract.ApplicationRoles.Queries;
public class GetRoleBySlugQuery : IRequest<RoleNameResponse>
{
    public string Slug { get; set; }
}
