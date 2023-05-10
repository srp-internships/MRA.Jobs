using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.TimelineEvents.Queries;
using MRA.Jobs.Application.Contracts.TimelineEvents.Responses;

namespace MRA.Jobs.Application.Features.TimelineEvents.Queries;
public class GetTimelineEventsQueryHandler : IRequestHandler<GetTimelineEventsQuery, List<GetTimelineEventsResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTimelineEventsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<List<GetTimelineEventsResponse>> Handle(GetTimelineEventsQuery request, CancellationToken cancellationToken)
    {
        var TimelineEvents = _context.TimelineEvents.ToList();
        var result = _mapper.Map<List<GetTimelineEventsResponse>>(TimelineEvents);
        return result;
    }
}
