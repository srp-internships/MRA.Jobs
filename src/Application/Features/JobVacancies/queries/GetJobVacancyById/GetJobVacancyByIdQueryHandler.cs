using MRA.Jobs.Application.Contracts.JobVacancies.Queries;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.Application.Features.JobVacancies.queries.GetJobVacancyById;

public class GetJobVacancyByIdQueryHandler : IRequestHandler<GetJobVacancyByIdQuery, JobVacancyDetailsDTO>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetJobVacancyByIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<JobVacancyDetailsDTO> Handle(GetJobVacancyByIdQuery request, CancellationToken cancellationToken)
    {
        var jobVacancy = await _dbContext.JobVacancies.
            FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
        _ = jobVacancy ?? throw new NotFoundException(nameof(JobVacancy), request.Id);
        return _mapper.Map<JobVacancyDetailsDTO>(jobVacancy);
    }
}

