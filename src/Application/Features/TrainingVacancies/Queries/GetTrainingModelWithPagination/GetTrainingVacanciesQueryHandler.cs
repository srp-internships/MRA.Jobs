using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingModels.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries.GetTrainingModelWithPagination;
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
        var result = _sieveProcessor.ApplyAdnGetPaggedList(request, _context.TrainingModels.AsNoTracking(), _mapper.Map<TrainingVacancyListDTO>);
        return await Task.FromResult(result);
    }
}
