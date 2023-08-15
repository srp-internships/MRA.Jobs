using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries;

public class
    GetTrainingVacanciesQueryHandler : IRequestHandler<PagedListQuery<TrainingVacancyListDto>,
        PagedList<TrainingVacancyListDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IApplicationSieveProcessor _sieveProcessor;

    public GetTrainingVacanciesQueryHandler(IApplicationDbContext context, IMapper mapper,
        IApplicationSieveProcessor sieveProcessor)
    {
        _context = context;
        _mapper = mapper;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<PagedList<TrainingVacancyListDto>> Handle(PagedListQuery<TrainingVacancyListDto> request,
        CancellationToken cancellationToken)
    {
        PagedList<TrainingVacancyListDto> result = _sieveProcessor.ApplyAdnGetPagedList(request,
            _context.TrainingVacancies.Include(t => t.Category).AsNoTracking(), _mapper.Map<TrainingVacancyListDto>);
        return await Task.FromResult(result);
    }
}