using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries;

public class
    GetTrainingVacanciesQueryHandler : IRequestHandler<GetTrainingsQueryOptions,
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

    public async Task<PagedList<TrainingVacancyListDto>> Handle(GetTrainingsQueryOptions request,
        CancellationToken cancellationToken)
    {
        var trainings = await _context.TrainingVacancies.Include(t => t.Category).ToListAsync();

        if (request.CategorySlug is not null)
        {
            trainings = (from t in trainings
                         where t.Category.Slug == request.CategorySlug
                         select t).ToList();
        }
        else if (request.SearchText is not null)
        {
            trainings = (from t in trainings
                         where t.Title.ToLower().Trim().Contains(request.SearchText.ToLower().Trim())
                         select t).ToList();
        }

        if (request.CheckDate)
        {
            DateTime now = DateTime.Now;
            trainings = (from t in trainings
                         where t.PublishDate <= now && t.EndDate >= now
                         select t).ToList();
        }

        PagedList<TrainingVacancyListDto> result = _sieveProcessor.ApplyAdnGetPagedList(request,
            trainings.AsQueryable(), _mapper.Map<TrainingVacancyListDto>);
        return await Task.FromResult(result);
    }
}