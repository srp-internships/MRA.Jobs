// using Microsoft.AspNetCore.Identity;
// using MRA.Jobs.Application.Contracts.Identity.Responces;
// using MRA.Jobs.Infrastructure.Persistence;
//
// namespace MRA.Jobs.Infrastructure.Identity.Services;
//
// public class IdentityService : IIdentityService
// {
//     private readonly ApplicationDbContext _context;
//     private readonly UserManager<ApplicationUser> _userManager;
//
//     public IdentityService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
//     {
//         _context = context;
//         _userManager = userManager;
//     }
//
//     public async Task<bool> HasPermissionAsync(Guid userId, string permissionName, CancellationToken cancellationToken)
//     {
//         ApplicationUser user = await _userManager.FindByIdAsync(userId.ToString());
//         _ = user ?? throw new NotFoundException(nameof(ApplicationUser), userId);
//
//         IList<string> roles = await _userManager.GetRolesAsync(user);
//         bool hasPermission =
//             await _context.RolePermissions.AnyAsync(
//                 p => roles.Contains(p.Role.Name) && p.Permission.Name == permissionName, cancellationToken);
//
//         return hasPermission;
//     }
//
//     public async Task<UserIdentityResponse> GetUserIdentityAsync(Guid userId, CancellationToken cancellationToken)
//     {
//         ApplicationUser user = await _userManager.FindByIdAsync(userId.ToString());
//         _ = user ?? throw new NotFoundException(nameof(ApplicationUser), userId);
//         IList<string> roles = await _userManager.GetRolesAsync(user);
//         string[] permissions = await _context.RolePermissions.Where(p => roles.Contains(p.Role.Name))
//             .Select(p => p.Permission.Name).ToArrayAsync(cancellationToken);
//
//         return new UserIdentityResponse
//         {
//             Email = user.Email,
//             PhoneNumber = user.PhoneNumber,
//             IsActive = true,
//             Roles = roles,
//             Permissions = permissions,
//             TwoFactorEnabled = user.TwoFactorEnabled
//         };
//     }
// }