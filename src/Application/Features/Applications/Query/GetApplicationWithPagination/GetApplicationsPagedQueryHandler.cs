using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Applications.Queries;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Infrastructure;

namespace MRA.Jobs.Application.Features.Applications.Query.GetApplicationWithPagination;
using MRA.Jobs.Domain.Entities;
public class GetApplicationsPagedQueryHandler : IRequestHandler<GetApplicationsQuery, List<ApplicationListDTO>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly SieveService<Application> _sieveService;

    public GetApplicationsPagedQueryHandler(IApplicationDbContext dbContext, IMapper mapper, SieveService<Application> sieveService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _sieveService = sieveService;
    }

    public async Task<List<ApplicationListDTO>> Handle(GetApplicationsQuery query, CancellationToken cancellationToken)
    {
        var queryable = _dbContext.Applications.AsQueryable();
        var sievedQuery = _sieveService.ApplySieve(queryable, query.SieveQuery);
        var applications = await sievedQuery.ToListAsync(cancellationToken);

        var result = _mapper.Map<List<ApplicationListDTO>>(applications);
        return result;
    }
}
