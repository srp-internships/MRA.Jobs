using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.ApplicationRoles.Commands;
using MRA.Identity.Domain.Entities;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace MRA.Identity.Application.Features.Roles.Commands;
public class UpdateRolCommandHandler : IRequestHandler<UpdateRoleCommand, Guid>
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    public UpdateRolCommandHandler(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<Guid> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Slug == request.Slug);
        _ = role ?? throw new NotFoundException("Role not found");
        role.Name = request.NewRoleName;
        var result = await _roleManager.UpdateAsync(role);
        return result.Succeeded
                 ? role.Id
                 : throw new ValidationException("Failed to update role.");
    }
}
