using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MRA.Identity.Application.Contract.UserRoles.Commands;
public class CreateUserRolesCommand : IRequest<ApplicationResponse<string>>
{
    public Guid RoleId { get; set; }
    public Guid UserId { get; set; }

}
