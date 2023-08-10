using MRA.Jobs.Application.Common.Seive;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Infrastructure.Persistence;
using MRA.Jobs.Infrastructure.Shared.Role.Responses;

namespace MRA.Jobs.Infrastructure.Identity.Features.Role.Queries;

public class GetRolesQueryHandler : IRequestHandler<PaggedListQuery<RoleResponse>, PaggedList<RoleResponse>>
{
    private readonly ApplicationDbContext _context;
    private readonly IApplicationSieveProcessor _sieveProcessor;

    public GetRolesQueryHandler(ApplicationDbContext context, IApplicationSieveProcessor sieveProcessor)
    {
        _context = context;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<PaggedList<RoleResponse>> Handle(PaggedListQuery<RoleResponse> request, CancellationToken cancellationToken)
    {
        var result = _sieveProcessor.ApplyAdnGetPaggedList(request, _context.Roles, FromEntity);
        return await Task.FromResult(result);
    }

    private RoleResponse FromEntity(ApplicationRole p)
    {
        return new RoleResponse()
        {
            Name = p.Name,
            Id = p.Id
        };
    }
}
