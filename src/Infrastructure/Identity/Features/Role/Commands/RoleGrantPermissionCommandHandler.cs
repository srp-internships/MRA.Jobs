// using FluentValidation;
// using MRA.Jobs.Infrastructure.Persistence;
// using MRA.Jobs.Infrastructure.Shared.Role.Commands;
//
// namespace MRA.Jobs.Infrastructure.Identity.Features.Role.Commands;
//
// public class RoleGrantPermissionCommandValidator : AbstractValidator<RoleGrantPermissionCommand>
// {
//     public RoleGrantPermissionCommandValidator()
//     {
//         RuleFor(x => x.Permissions).NotNull().NotEmpty();
//     }
// }
//
// public class RoleGrantPermissionCommandHandler : IRequestHandler<RoleGrantPermissionCommand, Unit>
// {
//     private readonly ApplicationDbContext _context;
//
//     public RoleGrantPermissionCommandHandler(ApplicationDbContext context)
//     {
//         _context = context;
//     }
//
//     public async Task<Unit> Handle(RoleGrantPermissionCommand request, CancellationToken cancellationToken)
//     {
//         ApplicationRole role = await _context.Roles.FindAsync(request.Id);
//         if (role == null)
//         {
//             throw new NotFoundException(nameof(ApplicationRole), request.Id);
//         }
//
//         foreach (Guid permissionId in request.Permissions)
//         {
//             Permission permission = await _context.Permissions.FindAsync(permissionId);
//             if (permission == null)
//             {
//                 continue;
//             }
//
//             await _context.RolePermissions.AddAsync(new RolePermission { Role = role, Permission = permission });
//         }
//
//         await _context.SaveChangesAsync();
//
//         //TODO: Update user SecurityStamp
//         return Unit.Value;
//     }
// }