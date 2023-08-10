﻿using MRA.Jobs.Application.Contracts.Applications.Commands;

namespace MRA.Jobs.Application.Features.Applications.Command.DeleteApplication;
using MRA.Jobs.Domain.Entities;
public class DeleteApplicationCommandHandler : IRequestHandler<DeleteApplicationCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public DeleteApplicationCommandHandler(IApplicationDbContext context ,IDateTime dateTime,ICurrentUserService currentUserService)
    {
        _context = context;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(DeleteApplicationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Applications.FindAsync(new object[] { request.Id }, cancellationToken)
            ?? throw new NotFoundException(nameof(Application), request.Id);

        var timelineEvent = new ApplicationTimelineEvent
        {
            ApplicationId = entity.Id,
            EventType = TimelineEventType.Deleted,
            Time = _dateTime.Now,
            Note = "Application vacancy deleted",
            CreateBy = _currentUserService.GetId() ?? Guid.Empty
        };

        _context.Applications.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
