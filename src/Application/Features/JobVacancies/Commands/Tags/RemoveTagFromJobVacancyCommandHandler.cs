using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Security;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;

namespace MRA.Jobs.Application.Features.JobVacancies.Commands.Tags;

public class RemoveTagFromJobVacancyCommandHandler : IRequestHandler<RemoveTagFromJobVacancyCommand, bool>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public RemoveTagFromJobVacancyCommandHandler(IApplicationDbContext dbContext, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(RemoveTagFromJobVacancyCommand request, CancellationToken cancellationToken)
    {
        var vacancyTag = await _dbContext.VacancyTags
            .FirstOrDefaultAsync(vt => vt.VacancyId == request.JobVacancyId && vt.TagId == request.TagId, cancellationToken);

        _ = vacancyTag ?? throw new NotFoundException(nameof(VacancyTag), request.TagId);

        var timelineEvent = new VacancyTimelineEvent
        {
            VacancyId = vacancyTag.VacancyId,
            EventType = TimelineEventType.Created,
            Time = _dateTime.Now,
            Note = $"Removed '{vacancyTag.Tag.Name}' tag",
            CreateBy = _currentUserService.UserId
        };

        _ = _dbContext.VacancyTags.Remove(vacancyTag);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
