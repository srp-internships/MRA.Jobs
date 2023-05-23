using MRA.Jobs.Application.Contracts.TrainingModels.Commands;
using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Application.Features.TraningModels.Commands.Tags;
public class AddTagToTrainingModelCommandHandler : IRequestHandler<AddTagToTrainingModelCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public AddTagToTrainingModelCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }
    public async Task<bool> Handle(AddTagToTrainingModelCommand request, CancellationToken cancellationToken)
    {
        var trainingModel = await _context.TrainingModels.FindAsync(new object[] { request.TrainingModelId }, cancellationToken);

        if (trainingModel == null)
            throw new NotFoundException(nameof(JobVacancy), request.TrainingModelId);

        foreach (var tagName in request.Tags)
        {
            var tag = await _context.Tags.FindAsync(new object[] { tagName }, cancellationToken);

            if (tag == null)
            {
                tag = new Tag { Name = tagName };
                _context.Tags.Add(tag);
            }

            var vacancyTag = await _context.VacancyTags.FindAsync(new object[] { request.TrainingModelId, tag.Id }, cancellationToken);

            if (vacancyTag == null)
            {
                vacancyTag = new VacancyTag { VacancyId = request.TrainingModelId, TagId = tag.Id };
                _context.VacancyTags.Add(vacancyTag);

                var timelineEvent = new VacancyTimelineEvent
                {
                    VacancyId = trainingModel.Id,
                    EventType = TimelineEventType.Created,
                    Time = _dateTime.Now,
                    Note = $"Added '{tag.Name}' tag",
                    CreateBy = _currentUserService.UserId
                };
                await _context.VacancyTimelineEvents.AddAsync(timelineEvent, cancellationToken);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
