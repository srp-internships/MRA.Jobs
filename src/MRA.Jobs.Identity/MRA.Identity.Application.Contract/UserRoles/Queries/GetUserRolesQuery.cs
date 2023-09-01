using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MRA.Identity.Application.Contract.ApplicationRoles.Responses;
using MRA.Identity.Application.Contract.UserRoles.Response;

namespace MRA.Identity.Application.Contract.UserRoles.Queries;
public class GetUserRolesQuery : IRequest<ApplicationResponse<List<UserRolesResponse>>>
{
    public string Role { get; set; } = null;
    public string UserName { get; set; } = null;


}
