using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.Application.Features.JobVacancies.queries.GetJobVacancyById;

public class GetJobVacancyBySlugQueryHandler : IRequestHandler<GetJobVacancyBySlugQuery, JobVacancyDetailsDto>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetJobVacancyBySlugQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<JobVacancyDetailsDto> Handle(GetJobVacancyBySlugQuery request, CancellationToken cancellationToken)
    {
        //var jobVacancy = await _dbContext.JobVacancies.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
        JobVacancy jobVacancy = await _dbContext.JobVacancies.Include(i => i.History)
            .Include(i => i.Tags)
            .ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(i => i.Slug == request.Slug);
        _ = jobVacancy ?? throw new NotFoundException(nameof(JobVacancy), request.Slug);
        return _mapper.Map<JobVacancyDetailsDto>(jobVacancy);
    }
}