using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries.GetJobVacancyBySlug;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.Application.Features.JobVacancies.queries.GetJobVacancyById;

public class GetJobVacancyBySlugQueryHandler : IRequestHandler<GetJobVacancyBySlugQuery, JobVacancyDetailsDto>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public GetJobVacancyBySlugQueryHandler(IApplicationDbContext dbContext, IMapper mapper, ICurrentUserService currentUser)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<JobVacancyDetailsDto> Handle(GetJobVacancyBySlugQuery request, CancellationToken cancellationToken)
    {
        //var jobVacancy = await _dbContext.JobVacancies.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
        JobVacancy jobVacancy = await _dbContext.JobVacancies.Include(i => i.History)
            .Include(i => i.VacancyQuestions)
            .Include(i => i.VacancyTasks)
            .Include(i => i.Tags)
            .ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(i => i.Slug == request.Slug);
        _ = jobVacancy ?? throw new NotFoundException(nameof(JobVacancy), request.Slug);
        var mapped = _mapper.Map<JobVacancyDetailsDto>(jobVacancy);
        mapped.IsApplied = await _dbContext.Applications.AnyAsync(s => s.ApplicantId == _currentUser.GetUserId());
        return mapped;
    }
}