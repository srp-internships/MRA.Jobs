using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;

namespace MRA.Jobs.Application.Features.JobVacancies.Commands.Tags;

public class AddTagToJobVacancyCommandHandler : IRequestHandler<AddTagsToJobVacancyCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private readonly IMapper _mapper;

    public AddTagToJobVacancyCommandHandler(IApplicationDbContext context, IMapper mapper,
        ICurrentUserService currentUserService, IDateTime dateTime)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    public async Task<bool> Handle(AddTagsToJobVacancyCommand request, CancellationToken cancellationToken)
    {
        var jobVacancy = await _context.Internships
          .Include(x => x.Tags)
          .ThenInclude(t => t.Tag)
          .FirstOrDefaultAsync(x => x.Slug == request.JobVacancySlug, cancellationToken);

        if (jobVacancy == null)
            throw new NotFoundException(nameof(JobVacancy), request.JobVacancySlug);

        foreach (var tagName in request.Tags)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name.Equals(tagName), cancellationToken);

            if (tag == null)
            {
                tag = new Tag { Name = tagName };
                _context.Tags.Add(tag);
            }

            VacancyTag vacancyTag = jobVacancy.Tags.FirstOrDefault(t => t.Tag.Name == tagName);

            if (vacancyTag == null)
            {
                vacancyTag = new VacancyTag { VacancyId = jobVacancy.Id, TagId = tag.Id };
                _context.VacancyTags.Add(vacancyTag);

                VacancyTimelineEvent timelineEvent = new VacancyTimelineEvent
                {
                    VacancyId = jobVacancy.Id,
                    EventType = TimelineEventType.Created,
                    Time = _dateTime.Now,
                    Note = $"Added '{tag.Name}' tag",
                    CreateBy = _currentUserService.GetId() ?? Guid.Empty
                };
                await _context.VacancyTimelineEvents.AddAsync(timelineEvent, cancellationToken);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}