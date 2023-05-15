using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.TrainingModels.Commands;
using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Application.Features.TrainingModels.Commands.Tags;
public class RemoveTagFromTrainingModelCommandHandler : IRequestHandler<RemoveTagFromTrainingModelCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    public RemoveTagFromTrainingModelCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IDateTime dateTime)
    {
        _context = context;
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    public async Task<bool> Handle(RemoveTagFromTrainingModelCommand request, CancellationToken cancellationToken)
    {
        var vacancyTag = await _context.VacancyTags
            .FirstOrDefaultAsync(vt => vt.VacancyId == request.TrainingModelId && vt.TagId == request.TagId, cancellationToken);

        _ = vacancyTag ?? throw new NotFoundException(nameof(VacancyTag), request.TagId);

        var timeLineEvent = new VacancyTimelineEvent
        {
            VacancyId = vacancyTag.VacancyId,
            EventType = TimelineEventType.Created,
            Time = _dateTime.Now,
            Note = $"Removed '{vacancyTag.Tag.Name}' tag",
            CreateBy = _currentUserService.UserId
        };

        _ = _context.VacancyTags.Remove(vacancyTag);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
