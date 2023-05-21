using MRA.Jobs.Application.Contracts.TrainingModels.Commands;

namespace MRA.Jobs.Application.Features.TraningModels.Commands.Tags;
public class AddTagToTrainingModelCommandHandler : IRequestHandler<AddTagToTrainingModelCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public AddTagToTrainingModelCommandHandler(IApplicationDbContext context, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = context;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }
    public async Task<bool> Handle(AddTagToTrainingModelCommand request, CancellationToken cancellationToken)
    {
        var trainingModel = await _context.TrainingModels.FindAsync(new object[] { request.TrainingModelId }, cancellationToken: cancellationToken);
        var tag = await _context.Tags.FindAsync(new object[] { request.TagId }, cancellationToken: cancellationToken);

        var vacancyTag = new VacancyTag
        {
            VacancyId = trainingModel?.Id ?? throw new NotFoundException(nameof(TrainingModel), request.TrainingModelId),
            TagId = tag?.Id ?? throw new NotFoundException(nameof(Tag), request.TagId)
        };

        var timeLineEvent = new VacancyTimelineEvent
        {
            VacancyId = trainingModel.Id,
            EventType = TimelineEventType.Created,
            Time = _dateTime.Now,
            Note = $"Added '{tag.Name}' tag",
            CreateBy = _currentUserService.UserId
        };
        await _context.VacancyTimelineEvents.AddAsync(timeLineEvent, cancellationToken);

        await _context.VacancyTags.AddAsync(vacancyTag, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
