using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Applications.Queries.GetTimelineEvents;
using MRA.Jobs.Application.Contracts.TimeLineDTO;

namespace MRA.Jobs.Application.Features.Applications.Query.GetApplicationTimelineEvents;

public class GetListApplicationTimelineEventsQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper)
    : IRequestHandler<GetListApplicationTimelineEventsQuery, List<TimeLineDetailsDto>>
{
    public async Task<List<TimeLineDetailsDto>> Handle(GetListApplicationTimelineEventsQuery request,
        CancellationToken cancellationToken)
    {
        var timeLines = await dbContext.ApplicationTimelineEvents
            .Include(t => t.Application)
            .Where(a => a.Application.Slug.Equals(request.ApplicationSlug))
            .ToListAsync(cancellationToken);

        return timeLines.Select(mapper.Map<TimeLineDetailsDto>).ToList();
    }
}