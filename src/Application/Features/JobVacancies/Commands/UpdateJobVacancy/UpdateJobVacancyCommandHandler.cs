using MRA.Jobs.Application.Contracts.JobVacancies.Commands;

namespace MRA.Jobs.Application.Features.JobVacancies.Commands.UpdateJobVacancy;

public class UpdateJobVacancyCommandHandler : IRequestHandler<UpdateJobVacancyCommand, Guid>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public UpdateJobVacancyCommandHandler(IApplicationDbContext dbContext, IMapper mapper, IDateTime dateTime,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(UpdateJobVacancyCommand request, CancellationToken cancellationToken)
    {
        JobVacancy jobVacancy = await _dbContext.JobVacancies.FindAsync(new object[] { request.Id }, cancellationToken);
        _ = jobVacancy ?? throw new NotFoundException(nameof(JobVacancy), request.Id);

        VacancyCategory category =
            await _dbContext.Categories.FindAsync(new object[] { request.CategoryId }, cancellationToken);
        _ = category ?? throw new NotFoundException(nameof(VacancyCategory), request.CategoryId);


        _mapper.Map(request, jobVacancy);

        VacancyTimelineEvent timelineEvent = new VacancyTimelineEvent
        {
            VacancyId = jobVacancy.Id,
            Vacancy = jobVacancy,
            EventType = TimelineEventType.Updated,
            Time = _dateTime.Now,
            Note = "Job vacancy updated",
            CreateBy = _currentUserService.GetId() ?? Guid.Empty
        };
        await _dbContext.VacancyTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return jobVacancy.Id;
    }
}