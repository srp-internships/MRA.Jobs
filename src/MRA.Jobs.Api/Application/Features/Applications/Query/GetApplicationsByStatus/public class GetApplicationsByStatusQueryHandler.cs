using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Applications.Queries;
using MRA.Jobs.Application.Contracts.Applications.Responses;

namespace MRA.Jobs.Application.Features.Applications.Query.GetApplicationsByStatus;

public class GetApplicationsByStatusHandler : IRequestHandler<GetApplicationsByStatusQuery, List<ApplicationListStatus>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetApplicationsByStatusHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<ApplicationListStatus>> Handle(GetApplicationsByStatusQuery request,
        CancellationToken cancellationToken)
    {
        List<ApplicationListStatus> applications = await _dbContext.Applications
            .Where(a => a.Status == request.Status)
            .ProjectTo<ApplicationListStatus>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return applications;
    }
}