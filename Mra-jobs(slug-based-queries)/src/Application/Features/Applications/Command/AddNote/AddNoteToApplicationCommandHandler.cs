﻿using MRA.Jobs.Application.Contracts.Applications.Commands;

namespace MRA.Jobs.Application.Features.Applications.Command.AddNote;
public class AddNoteToApplicationCommandHandler : IRequestHandler<AddNoteToApplicationCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public AddNoteToApplicationCommandHandler(IApplicationDbContext dbContext, IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = dbContext;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }
    public async Task<bool> Handle(AddNoteToApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await _context.Applications.FindAsync(request.Id);
        _ = application ?? throw new NotFoundException(nameof(Application), request.Id); ;

        var timelineEvent = new ApplicationTimelineEvent
        {
            ApplicationId = application.Id,
            Application = application,
            EventType = TimelineEventType.Updated,
            Time = _dateTime.Now,
            Note = request.Note,
            CreateBy = _currentUserService.GetId() ?? Guid.Empty
        };
        await _context.ApplicationTimelineEvents.AddAsync(timelineEvent);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
