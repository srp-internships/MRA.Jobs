using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Internships.Commands;
using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Application.Features.Internships.Command.Tags;
public class RemoveTagFromInternshipCommandHandler : IRequestHandler<RemoveTagFromInternshipCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public RemoveTagFromInternshipCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }
    public async Task<bool> Handle(RemoveTagFromInternshipCommand request, CancellationToken cancellationToken)
    {
        var internship = await _context.Internships
           .Include(x => x.Tags)
           .ThenInclude(t => t.Tag)
           .FirstOrDefaultAsync(x => x.Id == request.InternshipId, cancellationToken);

        if (internship == null)
            throw new NotFoundException(nameof(internship), request.InternshipId);

        foreach (var tagName in request.Tags)
        {
            var vacancyTag = internship.Tags.FirstOrDefault(t => t.Tag.Name == tagName);

            if (vacancyTag == null)
                continue;

            _context.VacancyTags.Remove(vacancyTag);

            var timelineEvent = new VacancyTimelineEvent
            {
                VacancyId = internship.Id,
                EventType = TimelineEventType.Deleted,
                Time = _dateTime.Now,
                Note = $"Removed '{tagName}' tag",
                CreateBy = _currentUserService.UserId
            };
            await _context.VacancyTimelineEvents.AddAsync(timelineEvent, cancellationToken);

        }
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
