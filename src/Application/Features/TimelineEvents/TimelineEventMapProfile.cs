using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MRA.Jobs.Application.Contracts.TimelineEvents.Commands;
using MRA.Jobs.Application.Contracts.TimelineEvents.Responses;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Features.TimelineEvents;
public class TimelineEventMapProfile : Profile
{
    public TimelineEventMapProfile()
    {
        CreateMap<CreateTimelineEventCommand, TimelineEvent>();
        CreateMap<DeleteTimelineEventCommand, TimelineEvent>();
        CreateMap<UpdateTimelineEventCommand, TimelineEvent>();
        CreateMap<List<TimelineEvent>, List<GetTimelineEventsResponse>>();
    }
}
