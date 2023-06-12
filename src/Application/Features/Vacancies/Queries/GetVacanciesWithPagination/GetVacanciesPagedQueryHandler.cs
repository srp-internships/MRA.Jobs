using MRA.Jobs.Application.Common.Seive;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Vacncies.Responses;

namespace MRA.Jobs.Application.Features.Vacancies.Queries.GetVacanciesWithPagination;
public class GetVacanciesPagedQueryHandler : IRequestHandler<PaggedListQuery<VacancyListDTO>, PaggedList<VacancyListDTO>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IApplicationSieveProcessor _sieveProcessor;
    private readonly IMapper _mapper;

    public GetVacanciesPagedQueryHandler(IApplicationDbContext dbContext, IApplicationSieveProcessor sieveProcessor, IMapper mapper)
    {
        _dbContext = dbContext;
        _sieveProcessor = sieveProcessor;
        _mapper = mapper;
    }

    public async Task<PaggedList<VacancyListDTO>> Handle(PaggedListQuery<VacancyListDTO> request, CancellationToken cancellationToken)
    {
        var currentDate = DateTime.Now;
        var filteredVacancies = _dbContext.Vacancies
            .Where(v => v.PublishDate <= currentDate && v.EndDate >= currentDate);
        var result = _sieveProcessor.ApplyAdnGetPaggedList(request, filteredVacancies, _mapper.Map<VacancyListDTO>);
        return await Task.FromResult(result);
    }
}
