using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.ApplicationRoles.Commands;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Roles.Commands;
public class UpdateRolCommandHandler : IRequestHandler<UpdateRoleCommand, ApplicationResponse<Guid>>
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    public UpdateRolCommandHandler(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<ApplicationResponse<Guid>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Slug == request.Slug);
            if (role == null)
            {
                return new ApplicationResponseBuilder<Guid>().SetErrorMessage("Role not found.").Success(false).Build();
            }
            role.Name = request.NewRoleName;
            var result = await _roleManager.UpdateAsync(role);


            return result.Succeeded
                     ? new ApplicationResponseBuilder<Guid>().SetResponse(role.Id).Build()
                     : new ApplicationResponseBuilder<Guid>().SetErrorMessage("Failed to update role.").Success(false).Build();

        }
        catch (Exception e)
        {

            return new ApplicationResponseBuilder<Guid>().SetException(e).Success(false).Build();
        }

    }
}
