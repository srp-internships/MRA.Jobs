using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries.SinceCheckDate;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries.SinceCheckDate;

public class
    GetTrainingVacanciesSinceCheckDateQueryHandler : IRequestHandler<GetAllSinceCheckDateQuery,
        PagedList<TrainingVacancyListDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IApplicationSieveProcessor _sieveProcessor;

    public GetTrainingVacanciesSinceCheckDateQueryHandler(IApplicationDbContext context, IMapper mapper,
        IApplicationSieveProcessor sieveProcessor)
    {
        _context = context;
        _mapper = mapper;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<PagedList<TrainingVacancyListDto>> Handle(GetAllSinceCheckDateQuery request,
        CancellationToken cancellationToken)
    {
        DateTime now = DateTime.Now;
        var trainings = await (from t in _context.TrainingVacancies
                               where t.PublishDate <= now && t.EndDate >= now
                               select t).Include(t => t.Category).ToListAsync();

        PagedList<TrainingVacancyListDto> result = _sieveProcessor.ApplyAdnGetPagedList(request,
            trainings.AsQueryable(), _mapper.Map<TrainingVacancyListDto>);
        return await Task.FromResult(result);
    }
}