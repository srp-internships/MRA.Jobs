using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;

namespace MRA.Jobs.Application.Features.JobVacancies.Commands.Tags;

public class RemoveTagsFromJobVacancyCommandHandler : IRequestHandler<RemoveTagsFromJobVacancyCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private readonly IMapper _mapper;

    public RemoveTagsFromJobVacancyCommandHandler(IApplicationDbContext context, IMapper mapper,
        ICurrentUserService currentUserService, IDateTime dateTime)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }


    public async Task<bool> Handle(RemoveTagsFromJobVacancyCommand request, CancellationToken cancellationToken)
    {
        JobVacancy jobVacancy = await _context.JobVacancies
            .Include(x => x.Tags)
            .ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(x => x.Id == request.JobVacancyId, cancellationToken);

        if (jobVacancy == null)
        {
            throw new NotFoundException(nameof(JobVacancy), request.JobVacancyId);
        }

        foreach (string tagName in request.Tags)
        {
            VacancyTag vacancyTag = jobVacancy.Tags.FirstOrDefault(t => t.Tag.Name == tagName);

            if (vacancyTag == null)
            {
                continue;
            }

            _context.VacancyTags.Remove(vacancyTag);

            VacancyTimelineEvent timelineEvent = new VacancyTimelineEvent
            {
                VacancyId = jobVacancy.Id,
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