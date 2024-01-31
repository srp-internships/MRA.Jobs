using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands.Tags;

namespace MRA.Jobs.Application.Features.JobVacancies.Commands.Tags;

public class AddTagToJobVacancyCommandHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ICurrentUserService currentUserService,
    IDateTime dateTime)
    : IRequestHandler<AddTagsToJobVacancyCommand, bool>
{
    private readonly IMapper _mapper = mapper;

    public async Task<bool> Handle(AddTagsToJobVacancyCommand request, CancellationToken cancellationToken)
    {
        var jobVacancy = await context.JobVacancies
            .Include(x => x.Tags)
            .ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(x => x.Slug == request.JobVacancySlug, cancellationToken);

        if (jobVacancy == null)
            throw new NotFoundException(nameof(JobVacancy), request.JobVacancySlug);

        var existingTags = await context.Tags.ToListAsync(cancellationToken);
        var newTags = request.Tags.Where(tagName => !existingTags.Any(t => t.Name == tagName))
            .ToList();

        var tagsToAdd = newTags.Select(tagName => new Tag { Name = tagName }).ToList();
        context.Tags.AddRange(tagsToAdd);

        var vacancyTagsToAdd = tagsToAdd.Select(tag =>
                new VacancyTag { VacancyId = jobVacancy.Id, TagId = tag.Id })
            .ToList();
        context.VacancyTags.AddRange(vacancyTagsToAdd);

        var timelineEvent = new VacancyTimelineEvent
        {
            VacancyId = jobVacancy.Id,
            EventType = TimelineEventType.Created,
            Time = dateTime.Now,
            Note = $"Added tags: {string.Join(", ", tagsToAdd.Select(tag => tag.Name))}",
            CreateBy = currentUserService.GetUserName() ?? string.Empty
        };
        await context.VacancyTimelineEvents.AddAsync(timelineEvent, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}