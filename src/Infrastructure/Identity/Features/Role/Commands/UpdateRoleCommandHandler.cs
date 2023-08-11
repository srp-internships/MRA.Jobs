using FluentValidation;
using Microsoft.AspNetCore.Identity;
using MRA.Jobs.Infrastructure.Shared.Role.Commands;
using MRA.Jobs.Infrastructure.Shared.Role.Responses;

namespace MRA.Jobs.Infrastructure.Identity.Features.Role.Commands;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
    }
}

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, RoleResponse>
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public UpdateRoleCommandHandler(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<RoleResponse> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        ApplicationRole role = await _roleManager.FindByIdAsync(request.Id.ToString());
        if (role == null)
        {
            throw new NotFoundException(nameof(ApplicationRole), request.Id);
        }

        role.Name = request.Name;
        IdentityResult result = await _roleManager.UpdateAsync(role);
        if (!result.Succeeded)
        {
            throw new Exception("Error on creating role");
        }

        return new RoleResponse { Name = request.Name };
    }
}