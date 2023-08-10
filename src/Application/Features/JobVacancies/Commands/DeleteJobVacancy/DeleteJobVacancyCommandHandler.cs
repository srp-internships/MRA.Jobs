﻿using MRA.Jobs.Application.Contracts.JobVacancies.Commands;

namespace MRA.Jobs.Application.Features.JobVacancies.Commands.DeleteJobVacancy;

public class DeleteJobVacancyCommandHandler : IRequestHandler<DeleteJobVacancyCommand, bool>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private readonly IApplicationDbContext _dbContext;

    public DeleteJobVacancyCommandHandler(IApplicationDbContext dbContext, IDateTime dateTime,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(DeleteJobVacancyCommand request, CancellationToken cancellationToken)
    {
        JobVacancy jobVacancy = await _dbContext.JobVacancies.FindAsync(new object[] { request.Id }, cancellationToken);

        if (jobVacancy == null)
        {
            throw new NotFoundException(nameof(JobVacancy), request.Id);
        }

        VacancyTimelineEvent timelineEvent = new VacancyTimelineEvent
        {
            VacancyId = jobVacancy.Id,
            Vacancy = jobVacancy,
            EventType = TimelineEventType.Deleted,
            Time = _dateTime.Now,
            Note = "Job vacancy deleted",
            CreateBy = _currentUserService.GetId() ?? Guid.Empty
        };

        _dbContext.JobVacancies.Remove(jobVacancy);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}