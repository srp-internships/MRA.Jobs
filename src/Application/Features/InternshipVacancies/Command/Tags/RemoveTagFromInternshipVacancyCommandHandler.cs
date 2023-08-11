using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Command.Tags;

public class RemoveTagFromInternshipVacancyCommandHandler : IRequestHandler<RemoveTagFromInternshipVacancyCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private readonly IMapper _mapper;

    public RemoveTagFromInternshipVacancyCommandHandler(IApplicationDbContext context, IMapper mapper,
        IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(RemoveTagFromInternshipVacancyCommand request, CancellationToken cancellationToken)
    {
        InternshipVacancy internship = await _context.Internships
            .Include(x => x.Tags)
            .ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(x => x.Id == request.InternshipId, cancellationToken);

        if (internship == null)
        {
            throw new NotFoundException(nameof(internship), request.InternshipId);
        }

        foreach (string tagName in request.Tags)
        {
            VacancyTag vacancyTag = internship.Tags.FirstOrDefault(t => t.Tag.Name == tagName);

            if (vacancyTag == null)
            {
                continue;
            }

            _context.VacancyTags.Remove(vacancyTag);

            VacancyTimelineEvent timelineEvent = new VacancyTimelineEvent
            {
                VacancyId = internship.Id,
                EventType = TimelineEventType.Deleted,
                Time = _dateTime.Now,
                Note = $"Removed '{tagName}' tag",
                CreateBy = _currentUserService.GetId() ?? Guid.Empty
            };
            await _context.VacancyTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}