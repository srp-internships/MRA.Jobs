using FluentValidation;
using Microsoft.AspNetCore.Identity;
using MRA.Jobs.Infrastructure.Shared.Role.Commands;

namespace MRA.Jobs.Infrastructure.Identity.Features.Role.Commands;

public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
    }
}

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Unit>
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public DeleteRoleCommandHandler(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.Id.ToString());
        if (role == null)
            throw new NotFoundException(nameof(ApplicationRole), request.Id);

        var result = await _roleManager.DeleteAsync(role);
        if (!result.Succeeded)
            throw new Exception("Error on creating role");

        return Unit.Value;
    }
}
