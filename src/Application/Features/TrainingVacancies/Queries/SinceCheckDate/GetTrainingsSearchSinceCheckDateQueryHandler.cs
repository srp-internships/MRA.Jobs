using System.Data;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries.SinceCheckDate;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Features.TrainingVacancies.SinceCheckDate.Queries;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries;
public class GetTrainingsSearchSinceCheckDateQueryHandler : IRequestHandler<GetSearchedTrainingsSinceCheckDateQuery, PagedList<TrainingVacancyListDto>>
{
    IApplicationDbContext _context;
    IMapper _mapper;
    IApplicationSieveProcessor _sieveProcessor;
    public GetTrainingsSearchSinceCheckDateQueryHandler(IApplicationDbContext context, IMapper mapper, IApplicationSieveProcessor sieveProcessor)
    {
        _context = context;
        _mapper = mapper;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<PagedList<TrainingVacancyListDto>> Handle(GetSearchedTrainingsSinceCheckDateQuery request, CancellationToken cancellationToken)
    {
        var allTrainings = await _context.TrainingVacancies.ToListAsync();

        DateTime now = DateTime.Now;
        var trainings = (from t in allTrainings
                         where t.Title.ToLower().Trim().Contains(request.SearchInput.ToLower().Trim())
                         where t.PublishDate <= now && t.EndDate >= now
                         select t);

        PagedList<TrainingVacancyListDto> paginatedTrainings = _sieveProcessor.ApplyAdnGetPagedList(request,
            trainings.AsQueryable(), _mapper.Map<TrainingVacancyListDto>);

        return paginatedTrainings;
    }
}
