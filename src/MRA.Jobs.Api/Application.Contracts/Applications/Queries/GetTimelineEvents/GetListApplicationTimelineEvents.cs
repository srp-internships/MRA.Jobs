using MRA.Jobs.Application.Contracts.TimeLineDTO;

namespace MRA.Jobs.Application.Contracts.Applications.Queries.GetTimelineEvents
;

public class GetListApplicationTimelineEventsQuery : IRequest<List<TimeLineDetailsDto>>
{
    public string ApplicationSlug { get; set; }
}