using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Applications.Queries.GetApplicationsByStatus;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Dtos.Enums;

namespace MRA.Jobs.Application.Features.Applications.Query.GetApplicationsByStatus;

public class GetApplicationsByStatusHandler(IApplicationDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetApplicationsByStatusQuery, List<ApplicationListStatus>>
{
    public async Task<List<ApplicationListStatus>> Handle(GetApplicationsByStatusQuery request,
        CancellationToken cancellationToken)
    {
        List<ApplicationListStatus> applications = await dbContext.Applications
            .Where(a => a.Status == mapper.Map<MRA.Jobs.Domain.Enums.ApplicationStatus>(request.Status))
            .ProjectTo<ApplicationListStatus>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return applications;
    }
}