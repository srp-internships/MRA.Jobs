using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Commands.Tags;
public class AddTagToTrainingVacancyCommandHandler : IRequestHandler<AddTagToTrainingVacancyCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public AddTagToTrainingVacancyCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }
    public async Task<bool> Handle(AddTagToTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        var trainingVacancy = await _context.TrainingVacancies.FindAsync(new object[] { request.TrainingVacancySlug }, cancellationToken);

        if (trainingVacancy == null)
            throw new NotFoundException(nameof(JobVacancy), request.TrainingVacancySlug);

        foreach (var tagName in request.Tags)
        {
            var tag = await _context.Tags.FindAsync(new object[] { tagName }, cancellationToken);

            if (tag == null)
            {
                tag = new Tag { Name = tagName };
                _context.Tags.Add(tag);
            }

            var vacancyTag = await _context.VacancyTags.FindAsync(new object[] { request.TrainingVacancySlug, tag.Id }, cancellationToken);

            if (vacancyTag == null)
            {
                vacancyTag = new VacancyTag { VacancyId = trainingVacancy.Id, TagId = tag.Id };
                _context.VacancyTags.Add(vacancyTag);

                var timelineEvent = new VacancyTimelineEvent
                {
                    VacancyId = trainingVacancy.Id,
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
