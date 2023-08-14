using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Domain.Entities;
using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Application.Features.JobVacancies.Commands.Tags;


public class RemoveTagsFromJobVacancyCommandHandler : IRequestHandler<RemoveTagsFromJobVacancyCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    public RemoveTagsFromJobVacancyCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService, IDateTime dateTime)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }


    public async Task<bool> Handle(RemoveTagsFromJobVacancyCommand request, CancellationToken cancellationToken)
    {
        var jobVacancy = await _context.JobVacancies
          .Include(x => x.Tags)
          .ThenInclude(t => t.Tag)
          .FirstOrDefaultAsync(x => x.Slug == request.JobVacancySlug, cancellationToken);

        if (jobVacancy == null)
            throw new NotFoundException(nameof(JobVacancy), request.JobVacancySlug);

        foreach (var tagName in request.Tags)
        {
            var vacancyTag = jobVacancy.Tags.FirstOrDefault(t => t.Tag.Name == tagName);

            if (vacancyTag == null)
                continue;

            _context.VacancyTags.Remove(vacancyTag);

            var timelineEvent = new VacancyTimelineEvent
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
