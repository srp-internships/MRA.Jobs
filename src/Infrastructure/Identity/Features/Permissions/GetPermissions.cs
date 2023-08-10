using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Infrastructure.Persistence;
using MRA.Jobs.Infrastructure.Shared.Pemission.Responces;

namespace MRA.Jobs.Infrastructure.Identity.Features.Permissions;

public class GetPermissionsHandler : IRequestHandler<PagedListQuery<PermissionResponse>, PagedList<PermissionResponse>>
{
    private readonly ApplicationDbContext _context;
    private readonly IApplicationSieveProcessor _sieveProcessor;

    public GetPermissionsHandler(ApplicationDbContext context, IApplicationSieveProcessor sieveProcessor)
    {
        _context = context;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<PagedList<PermissionResponse>> Handle(PagedListQuery<PermissionResponse> request,
        CancellationToken cancellationToken)
    {
        PagedList<PermissionResponse> result =
            _sieveProcessor.ApplyAdnGetPagedList(request, _context.Permissions, FromEntity);
        return await Task.FromResult(result);
    }

    private PermissionResponse FromEntity(Permission p)
    {
        return new PermissionResponse
        {
            Id = p.Id,
            Name = p.Name,
            Group = p.Group,
            DisplayName = p.Name,
            GroupDisplayName = p.Group
        };
    }
}