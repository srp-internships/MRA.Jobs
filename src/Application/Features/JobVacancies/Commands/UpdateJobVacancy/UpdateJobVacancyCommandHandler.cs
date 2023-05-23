using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Application.Features.JobVacancies.Commands.UpdateJobVacancy;

public class UpdateJobVacancyCommandHandler : IRequestHandler<UpdateJobVacancyCommand, Guid>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public UpdateJobVacancyCommandHandler(IApplicationDbContext dbContext, IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(UpdateJobVacancyCommand request, CancellationToken cancellationToken)
    {
        var jobVacancy = await _dbContext.Internships.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
        _ = jobVacancy ?? throw new NotFoundException(nameof(JobVacancy), request.Id);

        var category = await _dbContext.Categories.FindAsync(new object[] { request.CategoryId }, cancellationToken: cancellationToken);
        _ = category ?? throw new NotFoundException(nameof(VacancyCategory), request.CategoryId);

        _mapper.Map(request, jobVacancy);

        var timelineEvent = new VacancyTimelineEvent
        {
            VacancyId = jobVacancy.Id,
            EventType = TimelineEventType.Updated,
            Time = _dateTime.Now,
            Note = "Job vacancy updated",
            CreateBy = _currentUserService.UserId
        };
        await _dbContext.VacancyTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return jobVacancy.Id;
    }
}
