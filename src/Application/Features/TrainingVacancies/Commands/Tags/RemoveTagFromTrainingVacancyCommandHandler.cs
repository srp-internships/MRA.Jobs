using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.TrainingModels.Commands;
using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Application.Features.TrainingModels.Commands.Tags;
public class RemoveTagFromTrainingModelCommandHandler : IRequestHandler<RemoveTagFromTrainingModelCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    public RemoveTagFromTrainingModelCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService, IDateTime dateTime)
    {
        _context = context;
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    public async Task<bool> Handle(RemoveTagFromTrainingModelCommand request, CancellationToken cancellationToken)
    {
        var trainingModel = await _context.TrainingModels
         .Include(x => x.Tags)
         .ThenInclude(t => t.Tag)
         .FirstOrDefaultAsync(x => x.Id == request.TrainingModelId, cancellationToken);

        if (trainingModel == null)
            throw new NotFoundException(nameof(trainingModel), request.TrainingModelId);

        foreach (var tagName in request.Tags)
        {
            var vacancyTag = trainingModel.Tags.FirstOrDefault(t => t.Tag.Name == tagName);

            if (vacancyTag == null)
                continue;

            _context.VacancyTags.Remove(vacancyTag);

            var timelineEvent = new VacancyTimelineEvent
            {
                VacancyId = trainingModel.Id,
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
