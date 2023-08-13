// using MRA.Jobs.Application.Common.Sieve;
// using MRA.Jobs.Application.Contracts.Common;
// using MRA.Jobs.Infrastructure.Persistence;
// using MRA.Jobs.Infrastructure.Shared.Role.Responses;
//
// namespace MRA.Jobs.Infrastructure.Identity.Features.Role.Queries;
//
// public class GetRolesQueryHandler : IRequestHandler<PagedListQuery<RoleResponse>, PagedList<RoleResponse>>
// {
//     private readonly ApplicationDbContext _context;
//     private readonly IApplicationSieveProcessor _sieveProcessor;
//
//     public GetRolesQueryHandler(ApplicationDbContext context, IApplicationSieveProcessor sieveProcessor)
//     {
//         _context = context;
//         _sieveProcessor = sieveProcessor;
//     }
//
//     public async Task<PagedList<RoleResponse>> Handle(PagedListQuery<RoleResponse> request,
//         CancellationToken cancellationToken)
//     {
//         PagedList<RoleResponse> result = _sieveProcessor.ApplyAdnGetPagedList(request, _context.Roles, FromEntity);
//         return await Task.FromResult(result);
//     }
//
//     private RoleResponse FromEntity(ApplicationRole p)
//     {
//         return new RoleResponse { Name = p.Name, Id = p.Id };
//     }
// }