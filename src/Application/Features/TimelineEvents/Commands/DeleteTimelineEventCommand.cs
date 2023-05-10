using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.TimelineEvents.Commands;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Features.TimelineEvents.Commands;
public class DeleteTimelineEventCommandHandler : IRequestHandler<DeleteTimelineEventCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DeleteTimelineEventCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    async Task<bool> IRequestHandler<DeleteTimelineEventCommand, bool>.Handle(DeleteTimelineEventCommand request, CancellationToken cancellationToken)
    {
        var timelineEvent = await _context.TimelineEvents
            .FindAsync(request.Id, cancellationToken)
            ?? throw new NullReferenceException();
        _context.TimelineEvents.Remove(timelineEvent);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
