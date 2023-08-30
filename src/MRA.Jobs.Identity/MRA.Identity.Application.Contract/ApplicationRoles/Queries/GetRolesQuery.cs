using MediatR;
using MRA.Identity.Application.Contract.ApplicationRoles.Responses;

namespace MRA.Identity.Application.Contract.ApplicationRoles.Queries;

public class GetRolesQuery : IRequest<ApplicationResponse<List<RoleNameResponse>>>
{
}