using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Tags;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Commands.Tags;

public class AddTagToTrainingVacancyCommandHandler : IRequestHandler<AddTagToTrainingVacancyCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private readonly IMapper _mapper;

    public AddTagToTrainingVacancyCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(AddTagToTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        var trainingVacancy = await _context.TrainingVacancies.FirstOrDefaultAsync(t => t.Slug == request.TrainingVacancySlug, cancellationToken);

        if (trainingVacancy == null)
            throw new NotFoundException(nameof(JobVacancy), request.TrainingVacancySlug);

        foreach (var tagName in request.Tags)
        {
            throw new NotFoundException(nameof(JobVacancy), request.TrainingVacancySlug);
        }

        foreach (string tagName in request.Tags)
        {
            Tag tag = await _context.Tags.FindAsync(new object[] { tagName }, cancellationToken);

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

                VacancyTimelineEvent timelineEvent = new VacancyTimelineEvent
                {
                    VacancyId = trainingVacancy.Id,
                    EventType = TimelineEventType.Created,
                    Time = _dateTime.Now,
                    Note = $"Added '{tag.Name}' tag",
                    CreateBy = _currentUserService.GetUserName() ?? string.Empty
                };
                await _context.VacancyTimelineEvents.AddAsync(timelineEvent, cancellationToken);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}