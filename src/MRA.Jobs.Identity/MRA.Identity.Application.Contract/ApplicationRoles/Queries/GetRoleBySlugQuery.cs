using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MRA.Identity.Application.Contract.ApplicationRoles.Responses;

namespace MRA.Identity.Application.Contract.ApplicationRoles.Queries;
public class GetRoleBySlugQuery : IRequest<ApplicationResponse<RoleNameResponse>>
{
    public string Slug { get; set; }
}
