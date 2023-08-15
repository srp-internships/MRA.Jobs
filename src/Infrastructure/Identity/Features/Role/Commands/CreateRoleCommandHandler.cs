using FluentValidation;
using Microsoft.AspNetCore.Identity;
using MRA.Jobs.Infrastructure.Shared.Role.Commands;
using MRA.Jobs.Infrastructure.Shared.Role.Responses;

namespace MRA.Jobs.Infrastructure.Identity.Features.Role.Commands;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
    }
}

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleResponse>
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public CreateRoleCommandHandler(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<RoleResponse> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        IdentityResult result = await _roleManager.CreateAsync(new ApplicationRole(request.Name));
        if (!result.Succeeded)
        {
            throw new Exception("Error on creating role");
        }

        return new RoleResponse { Name = request.Name };
    }
}