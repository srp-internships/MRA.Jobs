using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.UserRoles.Commands;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.UserRoles.Commands;
public class DeleteUserRoleCommandHandler : IRequestHandler<DeleteUserRoleCommand, ApplicationResponse<bool>>
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    public DeleteUserRoleCommandHandler(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }
    public async Task<ApplicationResponse<bool>> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Slug == request.Slug);
        if (role == null)
        {
            return new ApplicationResponseBuilder<bool>().SetErrorMessage("Role not found").Success(false).Build();
        }

        await _roleManager.DeleteAsync(role);
        return new ApplicationResponseBuilder<bool>().Success(true).Build();
    }
}

