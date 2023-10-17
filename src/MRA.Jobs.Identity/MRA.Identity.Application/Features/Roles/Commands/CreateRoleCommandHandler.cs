using MediatR;
using Microsoft.AspNetCore.Identity;
using MRA.Identity.Application.Contract.ApplicationRoles.Commands;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Roles.Commands;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Guid>
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public CreateRoleCommandHandler(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = new ApplicationRole
        {
            Id = Guid.NewGuid(),
            Name = request.RoleName,
            NormalizedName = request.RoleName.ToLower(),
            Slug = request.RoleName.ToLower(),
        };
        var result = await _roleManager.CreateAsync(role);
        return role.Id;
    }
}