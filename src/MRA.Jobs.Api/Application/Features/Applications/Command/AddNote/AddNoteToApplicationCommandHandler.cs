using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Applications.Commands.AddNote;

namespace MRA.Jobs.Application.Features.Applications.Command.AddNote;

public class AddNoteToApplicationCommandHandler(
    IApplicationDbContext dbContext,
    IDateTime dateTime,
    ICurrentUserService currentUserService)
    : IRequestHandler<AddNoteToApplicationCommand, bool>
{
    public async Task<bool> Handle(AddNoteToApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await dbContext.Applications.FirstOrDefaultAsync(a => a.Slug == request.Slug, cancellationToken);
        _ = application ?? throw new NotFoundException(nameof(Application), request.Slug); ;

        ApplicationTimelineEvent timelineEvent = new()
        {
            ApplicationId = application.Id,
            Application = application,
            EventType = TimelineEventType.Note,
            Time = dateTime.Now,
            Note = request.Note,
            CreateBy = currentUserService.GetUserId() ?? Guid.Empty
        };
        await dbContext.ApplicationTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}