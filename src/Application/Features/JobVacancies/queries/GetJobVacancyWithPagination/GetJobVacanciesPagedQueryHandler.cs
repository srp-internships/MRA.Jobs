using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Seive;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.Application.Features.JobVacancies.queries.GetJobVacancyWithPagination;

public class GetJobVacanciesPagedQueryHandler : IRequestHandler<PaggedListQuery<JobVacancyListDTO>, PaggedList<JobVacancyListDTO>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IApplicationSieveProcessor _sieveProcessor;
    private readonly IMapper _mapper;

    public GetJobVacanciesPagedQueryHandler(IApplicationDbContext dbContext, IApplicationSieveProcessor sieveProcessor, IMapper mapper)
    {
        _dbContext = dbContext;
        _sieveProcessor = sieveProcessor;
        _mapper = mapper;
    }

    public async Task<PaggedList<JobVacancyListDTO>> Handle(PaggedListQuery<JobVacancyListDTO> request, CancellationToken cancellationToken)
    {
        var result = _sieveProcessor.ApplyAdnGetPaggedList(request, _dbContext.Internships.AsNoTracking(), _mapper.Map<JobVacancyListDTO>);
        return await Task.FromResult(result);
    }
}
