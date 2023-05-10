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
public class UpdateTimelineEventCommandHandler : IRequestHandler<UpdateTimelineEventCommand, long>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateTimelineEventCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<long> Handle(UpdateTimelineEventCommand request, CancellationToken cancellationToken)
    {
        var result = _mapper.Map<TimelineEvent>(request);
        _context.TimelineEvents.Update(result);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Id;
    }
}
