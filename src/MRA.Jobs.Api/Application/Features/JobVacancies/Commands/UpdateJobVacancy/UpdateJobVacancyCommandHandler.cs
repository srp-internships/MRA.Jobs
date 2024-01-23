using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.SlugGeneratorService;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands.Update;

namespace MRA.Jobs.Application.Features.JobVacancies.Commands.UpdateJobVacancy;

public class UpdateJobVacancyCommandHandler : IRequestHandler<UpdateJobVacancyCommand, string>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ISlugGeneratorService _slugService;

    public UpdateJobVacancyCommandHandler(IApplicationDbContext dbContext, IMapper mapper, IDateTime dateTime,
        ICurrentUserService currentUserService, ISlugGeneratorService slugService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
        _slugService = slugService;
    }

    public async Task<string> Handle(UpdateJobVacancyCommand request, CancellationToken cancellationToken)
    {
        var jobVacancy = await _dbContext.JobVacancies
            .Include(j => j.Category)
            .Include(j => j.VacancyQuestions)
            .Include(i => i.VacancyTasks)
            .FirstOrDefaultAsync(j => j.Slug == request.Slug, cancellationToken);
        _ = jobVacancy ?? throw new NotFoundException(nameof(JobVacancy), request.Slug);
        
        TimeZoneInfo zone = TimeZoneInfo.Local;
        jobVacancy.EndDate = TimeZoneInfo.ConvertTimeFromUtc(jobVacancy.EndDate, zone);

        //VacancyCategory category =
        //    await _dbContext.Categories.FirstOrDefaultAsync(j => j.Id == request.CategoryId, cancellationToken);
        //_ = category ?? throw new NotFoundException(nameof(VacancyCategory), request.CategoryId);


        _mapper.Map(request, jobVacancy);
    
        VacancyTimelineEvent timelineEvent = new VacancyTimelineEvent
        {
            VacancyId = jobVacancy.Id,
            Vacancy = jobVacancy,
            EventType = TimelineEventType.Updated,
            Time = _dateTime.Now,
            Note = "Job vacancy updated",
            CreateBy = _currentUserService.GetUserId() ?? Guid.Empty
        };
        await _dbContext.VacancyTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return jobVacancy.Slug;
    }
}