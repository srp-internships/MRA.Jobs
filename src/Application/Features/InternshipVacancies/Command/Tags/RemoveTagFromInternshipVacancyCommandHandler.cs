using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Security;
using MRA.Jobs.Application.Contracts.Internships.Commands;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Command.Tags;
public class RemoveTagFromInternshipVacancyCommandHandler : IRequestHandler<RemoveTagFromInternshipVacancyCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public RemoveTagFromInternshipVacancyCommandHandler(IApplicationDbContext context, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = context;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }
    public async Task<bool> Handle(RemoveTagFromInternshipVacancyCommand request, CancellationToken cancellationToken)
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
            CreateBy = _currentUserService.GetId()
        };

        _ = _context.VacancyTags.Remove(vacancyTag);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
