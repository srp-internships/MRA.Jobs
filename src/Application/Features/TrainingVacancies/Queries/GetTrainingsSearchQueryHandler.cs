using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries;
public class GetTrainingsSearchQueryHandler : IRequestHandler<GetTrainingsSearchQuery, PagedList<TrainingVacancyListDto>>
{
    IApplicationDbContext _context;
    IMapper _mapper;
    IApplicationSieveProcessor _sieveProcessor;
    public GetTrainingsSearchQueryHandler(IApplicationDbContext context, IMapper mapper, IApplicationSieveProcessor sieveProcessor)
    {
        _context = context;
        _mapper = mapper;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<PagedList<TrainingVacancyListDto>> Handle(GetTrainingsSearchQuery request, CancellationToken cancellationToken)
    {
        var allTrainings = await _context.TrainingVacancies.ToListAsync();

        var trainings = (from t in allTrainings
                         where t.Title.ToLower().Trim().Contains(request.SearchInout.ToLower().Trim())
                         select t);

        PagedList<TrainingVacancyListDto> paginatedTrainings = _sieveProcessor.ApplyAdnGetPagedList(request,
            trainings.AsQueryable(), _mapper.Map<TrainingVacancyListDto>);

        return paginatedTrainings;
    }
}
