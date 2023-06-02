using MRA.Jobs.Application.Contracts.JobVacancies.Queries;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.Application.Features.JobVacancies.queries.GetJobVacancyByTitle;

public class GetJobVacancyByTitleQueryHandler : IRequestHandler<GetJobVacancyByTitleQuery, JobVacancyListDTO>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public GetJobVacancyByTitleQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<JobVacancyListDTO> Handle(GetJobVacancyByTitleQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.JobVacancies.FindAsync(request.Title, cancellationToken);
        _ = result ?? throw new NotFoundException(nameof(JobVacancies), request.Title);
        return _mapper.Map<JobVacancyListDTO>(result);
    }
}