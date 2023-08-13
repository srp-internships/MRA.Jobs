// using Microsoft.AspNetCore.Identity;
// using MRA.Jobs.Infrastructure.Shared.Users.Commands.Roles;
//
// namespace MRA.Jobs.Infrastructure.Identity.Features.User.Commands.Roles;
//
// public class ResetUserRolesCommandHandler : IRequestHandler<ResetUserRolesCommand, Unit>
// {
//     private readonly UserManager<ApplicationUser> _userManager;
//
//     public ResetUserRolesCommandHandler(UserManager<ApplicationUser> userManager)
//     {
//         _userManager = userManager;
//     }
//
//     public async Task<Unit> Handle(ResetUserRolesCommand request, CancellationToken cancellationToken)
//     {
//         ApplicationUser user = await _userManager.FindByIdAsync(request.Id.ToString());
//         if (user == null)
//         {
//             throw new NotFoundException(nameof(ApplicationUser), request.Id);
//         }
//
//         IList<string> existRoles = await _userManager.GetRolesAsync(user);
//         IEnumerable<string> existRolesToRemove = existRoles.Where(r => !request.Roles.Contains(r));
//
//         IdentityResult result = await _userManager.RemoveFromRolesAsync(user, existRolesToRemove);
//         if (!result.Succeeded)
//         {
//             throw new ValidationException(string.Join('\n', result.Errors.Select(r => r.Description)));
//         }
//
//         result = await _userManager.AddToRolesAsync(user, request.Roles.Where(r => !existRoles.Contains(r)));
//         if (!result.Succeeded)
//         {
//             throw new ValidationException(string.Join('\n', result.Errors.Select(r => r.Description)));
//         }
//
//         return Unit.Value;
//     }
// }