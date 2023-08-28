using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.UserRoles.Commands;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.UserRoles.Commands;
public class CreateUserRoleCommandHandler : IRequestHandler<CreateUserRolesCommand, ApplicationResponse<Guid>>
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    public CreateUserRoleCommandHandler(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }
    public async Task<ApplicationResponse<Guid>> Handle(CreateUserRolesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var roleName = request.RoleName;

            var exestingRole = await _roleManager.FindByNameAsync(roleName);
            if (exestingRole != null)
            {
                return new ApplicationResponseBuilder<Guid>().SetErrorMessage("The role already exists.").Success(false).Build();
            }

            var newRole = new ApplicationRole
            {
                Name = roleName
            };
            var result = await _roleManager.CreateAsync(newRole);

            if (result.Succeeded)
            {
                return new ApplicationResponseBuilder<Guid>().SetResponse(newRole.Id).Build();
            }

            return new ApplicationResponseBuilder<Guid>().SetErrorMessage("Failed to create role.").Success(false).Build();
        }
        catch (Exception e)
        {
            return new ApplicationResponseBuilder<Guid>().SetException(e).Success(false).Build();
        }

    }
}
