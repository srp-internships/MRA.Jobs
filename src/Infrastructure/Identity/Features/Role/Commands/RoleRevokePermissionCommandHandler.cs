using FluentValidation;
using MRA.Jobs.Infrastructure.Persistence;
using MRA.Jobs.Infrastructure.Shared.Role.Commands;

namespace MRA.Jobs.Infrastructure.Identity.Features.Role.Commands;

public class RoleRevokePermissionCommandValidator : AbstractValidator<RoleRevokePermissionCommand>
{
    public RoleRevokePermissionCommandValidator()
    {
        RuleFor(x => x.Permissions).NotNull().NotEmpty();
    }
}

public class RoleRevokePermissionCommandHandler : IRequestHandler<RoleRevokePermissionCommand, Unit>
{
    private readonly ApplicationDbContext _context;

    public RoleRevokePermissionCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RoleRevokePermissionCommand request, CancellationToken cancellationToken)
    {
        var role = await _context.Roles.FindAsync(request.Id);
        if (role == null)
        {
            throw new NotFoundException(nameof(ApplicationRole), request.Id);
        }

        var permissions = await _context.RolePermissions.Where(r => r.RoleId == request.Id).ToListAsync();

        foreach (var permissionId in request.Permissions)
        {
            var permission = permissions.FirstOrDefault(p => p.PermissionId == permissionId);
            if (permission == null)
                continue;

            _context.RolePermissions.Remove(permission);
        }

        await _context.SaveChangesAsync();

        //TODO: Update user SecurityStamp
        return Unit.Value;
    }
}
