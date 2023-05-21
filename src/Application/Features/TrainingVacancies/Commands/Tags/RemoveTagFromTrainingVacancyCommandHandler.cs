using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Security;
using MRA.Jobs.Application.Contracts.TrainingModels.Commands;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Commands.Tags;
public class RemoveTagFromTrainingVacancyCommandHandler : IRequestHandler<RemoveTagFromTrainingVacancyCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    public RemoveTagFromTrainingVacancyCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IDateTime dateTime)
    {
        _context = context;
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    public async Task<bool> Handle(RemoveTagFromTrainingVacancyCommand request, CancellationToken cancellationToken)
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
