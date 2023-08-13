// using MRA.Jobs.Infrastructure.Persistence;
// using MRA.Jobs.Infrastructure.Shared.Pemission.Responces;
// using MRA.Jobs.Infrastructure.Shared.Role.Queries;
//
// namespace MRA.Jobs.Infrastructure.Identity.Features.Role.Queries;
//
// public class GetRolePermissionsQueryHandler : IRequestHandler<GetRolePermissionsQuery, IEnumerable<PermissionResponse>>
// {
//     private readonly ApplicationDbContext _context;
//
//     public GetRolePermissionsQueryHandler(ApplicationDbContext context)
//     {
//         _context = context;
//     }
//
//     public async Task<IEnumerable<PermissionResponse>> Handle(GetRolePermissionsQuery request,
//         CancellationToken cancellationToken)
//     {
//         List<Permission> permissions = await _context.RolePermissions.Where(r => r.RoleId == request.Id)
//             .Select(r => r.Permission).ToListAsync();
//         return permissions.Select(FromEntity).ToArray();
//     }
//
//     private PermissionResponse FromEntity(Permission p)
//     {
//         return new PermissionResponse
//         {
//             Id = p.Id,
//             Name = p.Name,
//             Group = p.Group,
//             DisplayName = p.Name,
//             GroupDisplayName = p.Group
//         };
//     }
// }