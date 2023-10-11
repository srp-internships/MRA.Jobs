using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries;
public class GetTrainingVacancyBySlugQueryHandler : IRequestHandler<GetTrainingVacancyBySlugQuery, TrainingVacancyDetailedResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTrainingVacancyBySlugQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<TrainingVacancyDetailedResponse> Handle(GetTrainingVacancyBySlugQuery request, CancellationToken cancellationToken)
    {
        var trainingVacancy = await _context.TrainingVacancies.Include(i => i.History).Include(i => i.VacancyQuestions)
             .Include(i => i.Tags)
             .ThenInclude(t => t.Tag)
             .FirstOrDefaultAsync(i => i.Slug == request.Slug);

        _ = trainingVacancy ?? throw new NotFoundException(nameof(TrainingVacancy), request.Slug);
        return _mapper.Map<TrainingVacancyDetailedResponse>(trainingVacancy);
    }
}