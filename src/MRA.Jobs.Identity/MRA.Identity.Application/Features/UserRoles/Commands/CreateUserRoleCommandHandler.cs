using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.UserRoles.Commands;

namespace MRA.Identity.Application.Features.UserRoles.Commands;
public class CreateUserRoleCommandHandler : IRequestHandler<CreateUserRolesCommand, ApplicationResponse<Guid>>
{
   private readonly RoleManager<ApplicationResponse<Guid>> _roleManager;
    public CreateUserRoleCommandHandler(CreateUserRolesCommand, ApplicationResponse)
    {
        
    }
    public Task<ApplicationResponse<Guid>> Handle(CreateUserRolesCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
