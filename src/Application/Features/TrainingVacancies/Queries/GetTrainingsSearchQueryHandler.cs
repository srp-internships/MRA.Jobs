using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries;
public class GetTrainingsSearchQueryHandler : IRequestHandler<GetTrainingsSearchQuery, List<TrainingVacancyListDto>>
{
    IApplicationDbContext _context;
    IMapper _mapper;
    public GetTrainingsSearchQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<TrainingVacancyListDto>> Handle(GetTrainingsSearchQuery request, CancellationToken cancellationToken)
    {
        var allTrainings = await _context.TrainingVacancies.ToListAsync();

        var trainings = (from t in allTrainings
                         where t.Title.ToLower().Trim().Contains(request.SearchInout.ToLower().Trim())
                         select _mapper.Map<TrainingVacancyListDto>(t)).ToList();

        return trainings;
    }
}
