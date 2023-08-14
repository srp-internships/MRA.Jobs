using MRA.Jobs.Application.Common.SlugGeneratorService;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;

namespace MRA.Jobs.Application.Features.JobVacancies.Commands.CreateJobVacancy;

public class CreateJobVacancyCommandHandler : IRequestHandler<CreateJobVacancyCommand, Guid>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;
    private readonly ISlugGeneratorService _slugService;

    public CreateJobVacancyCommandHandler(ISlugGeneratorService slugService, IApplicationDbContext dbContext, IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
        _slugService = slugService;
    }

    public async Task<Guid> Handle(CreateJobVacancyCommand request, CancellationToken cancellationToken)
    {
        VacancyCategory category = await _dbContext.Categories.FindAsync(request.CategoryId);
        _ = category ?? throw new NotFoundException(nameof(VacancyCategory), request.CategoryId);

        var jobVacancy = _mapper.Map<JobVacancy>(request);
        jobVacancy.Category = category;
        jobVacancy.Slug = GenerateSlug(jobVacancy);
        await _dbContext.JobVacancies.AddAsync(jobVacancy, cancellationToken);

        VacancyTimelineEvent timelineEvent = new VacancyTimelineEvent
        {
            VacancyId = jobVacancy.Id,
            Vacancy = jobVacancy,
            EventType = TimelineEventType.Created,
            Time = _dateTime.Now,
            Note = "Job vacancy created",
            CreateBy = _currentUserService.GetId() ?? Guid.Empty
        };
        await _dbContext.VacancyTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return jobVacancy.Id;
    }

    private string GenerateSlug(JobVacancy job) => _slugService.GenerateSlug($"{job.Title}-{job.CreatedAt.Year}-{job.CreatedAt.Month}");
}
