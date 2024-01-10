using MRA.Jobs.Application.Contracts.TimeLineDTO;

namespace MRA.Jobs.Application.Features.TimeLineEvents;

public class TimelineEventProfile : Profile
{
    public TimelineEventProfile()
    {
        CreateMap<ApplicationTimelineEvent, TimeLineDetailsDto>();
    }
}