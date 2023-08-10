using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries;
public class GetTrainingVacanciesQueryHandler : IRequestHandler<PaggedListQuery<TrainingVacancyListDTO>, PaggedList<TrainingVacancyListDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IApplicationSieveProcessor _sieveProcessor;

    public GetTrainingVacanciesQueryHandler(IApplicationDbContext context, IMapper mapper, IApplicationSieveProcessor sieveProcessor)
    {
        _context = context;
        _mapper = mapper;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<PaggedList<TrainingVacancyListDTO>> Handle(PaggedListQuery<TrainingVacancyListDTO> request, CancellationToken cancellationToken)
    {
        var result = _sieveProcessor.ApplyAdnGetPagedList(request, _context.TrainingVacancies.AsNoTracking(), _mapper.Map<TrainingVacancyListDTO>);
        return await Task.FromResult(result);
    }
}
