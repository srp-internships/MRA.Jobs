using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Internships.Commands;
using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Application.Features.Internships.Command.Tags;
public class RemoveTagFromInternshipCommandHandler : IRequestHandler<RemoveTagFromInternshipCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public RemoveTagFromInternshipCommandHandler(IApplicationDbContext context, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = context;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }
    public async Task<bool> Handle(RemoveTagFromInternshipCommand request, CancellationToken cancellationToken)
    {
        var vacancyTag = await _context.VacancyTags
            .FirstOrDefaultAsync(vt => vt.VacancyId == request.InternshipId && vt.TagId == request.TagId, cancellationToken);

        _ = vacancyTag ?? throw new NotFoundException(nameof(VacancyTag), request.TagId);

        var timelineEvent = new VacancyTimelineEvent
        {
            VacancyId = vacancyTag.VacancyId,
            EventType = TimelineEventType.Created,
            Time = _dateTime.Now,
            Note = $"Removed '{vacancyTag.Tag.Name}' tag",
            CreateBy = _currentUserService.UserId
        };

        _ = _context.VacancyTags.Remove(vacancyTag);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
