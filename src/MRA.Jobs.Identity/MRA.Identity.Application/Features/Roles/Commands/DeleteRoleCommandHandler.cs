using System;
using System.Collections.Generic;
using System.Data;
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
public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, ApplicationResponse<bool>>
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public DeleteRoleCommandHandler(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<ApplicationResponse<bool>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {

        var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Slug == request.Slug);
        if (role == null)
        {
            return new ApplicationResponseBuilder<bool>().SetErrorMessage("role not found").Success(false).Build();
        }

        await _roleManager.DeleteAsync(role);
        return new ApplicationResponseBuilder<bool>().Success(true).Build();

    }
}