using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingModels.Responses;
using MRA.Jobs.Infrastructure;

namespace MRA.Jobs.Application.Features.TrainingModels.Queries.GetTrainingModelWithPagination;
public class GetTrainingModelsPagedQueryHandler : IRequestHandler<PaggedListQuery<TrainingModelListDTO>, PaggedList<TrainingModelListDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IApplicationSieveProcessor _sieveProcessor;

    public GetTrainingModelsPagedQueryHandler(IApplicationDbContext context, IMapper mapper, IApplicationSieveProcessor sieveProcessor)
    {
        _context = context;
        _mapper = mapper;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<PaggedList<TrainingModelListDTO>> Handle(PaggedListQuery<TrainingModelListDTO> request, CancellationToken cancellationToken)
    {
        var result = _sieveProcessor.ApplyAdnGetPaggedList(request, _context.TrainingModels.AsNoTracking(), _mapper.Map<TrainingModelListDTO>);
        return await Task.FromResult(result);
    }
}
