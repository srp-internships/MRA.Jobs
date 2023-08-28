using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MRA.Identity.Application.Contract.ApplicationRoles.Responses;

namespace MRA.Identity.Application.Contract.UserRoles.Queries;
public class GetUserRolesBySlugQuery : IRequest<ApplicationResponse<RoleNameResponse>>
{
    private string _slug;

    public GetUserRolesBySlugQuery(string slug)
    {
        _slug = slug;
    }
}
