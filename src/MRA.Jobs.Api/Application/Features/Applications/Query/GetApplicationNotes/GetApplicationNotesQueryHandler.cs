using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Applications.Queries.GetApplicationNotes;
using MRA.Jobs.Application.Contracts.TimeLineDTO;

namespace MRA.Jobs.Application.Features.Applications.Query.GetApplicationNotes;

public class GetApplicationNotesQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetApplicationNotesQuery, List<TimeLineDetailsDto>>
{
    public async Task<List<TimeLineDetailsDto>> Handle(GetApplicationNotesQuery request,
        CancellationToken cancellationToken)
    {
        var application =
            await context.Applications.FirstOrDefaultAsync(s => s.Slug == request.Slug, cancellationToken) ??
            throw new NotFoundException($"Application with slug {request.Slug} not found.");

        var notes = await context.ApplicationTimelineEvents.Where(s => s.ApplicationId == application.Id)
            .ToListAsync(cancellationToken);
        return notes.Select(mapper.Map<TimeLineDetailsDto>).ToList();
    }
}