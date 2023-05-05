using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Application.Features.JobVacancies.Commands.Tags;

public class AddTagToJobVacancyCommandHandler : IRequestHandler<AddTagToJobVacancyCommand, bool>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public AddTagToJobVacancyCommandHandler(IApplicationDbContext dbContext, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(AddTagToJobVacancyCommand request, CancellationToken cancellationToken)
    {
        var jobVacancy = await _dbContext.JobVacancies.FindAsync(new object[] { request.JobVacancyId }, cancellationToken: cancellationToken);
        var tag = await _dbContext.Tags.FindAsync(new object[] { request.TagId }, cancellationToken: cancellationToken);

        var vacancyTag = new VacancyTag
        {
            VacancyId = jobVacancy?.Id ?? throw new NotFoundException(nameof(JobVacancy), request.JobVacancyId),
            TagId = tag?.Id ?? throw new NotFoundException(nameof(Tag), request.TagId)
        };

        var timelineEvent = new VacancyTimelineEvent
        {
            VacancyId = jobVacancy.Id,
            EventType = TimelineEventType.Created,
            Time = _dateTime.Now,
            Note = $"Added '{tag.Name}' tag",
            CreateBy = _currentUserService.UserId
        };
        await _dbContext.VacancyTimelineEvents.AddAsync(timelineEvent, cancellationToken);

        await _dbContext.VacancyTags.AddAsync(vacancyTag, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
