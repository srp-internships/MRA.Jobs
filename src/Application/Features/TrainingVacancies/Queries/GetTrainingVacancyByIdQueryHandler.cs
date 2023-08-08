using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries;
public class GetTrainingVacancyBySlugQueryHandler : IRequestHandler<GetTrainingVacancyBySlugQuery, TrainingVacancyDetailedResponce>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTrainingVacancyBySlugQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<TrainingVacancyDetailedResponce> Handle(GetTrainingVacancyBySlugQuery request, CancellationToken cancellationToken)
    {
       // var trainingVacancy = await _context.TrainingVacancies.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
       var trainingVacancy = await _context.TrainingVacancies.Include(i => i.History)
            .Include(i => i.Tags)
            .ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(i => i.Id == request.Id);
        _ = trainingVacancy ?? throw new NotFoundException(nameof(TrainingVacancy), request.Id);
        return _mapper.Map<TrainingVacancyDetailedResponce>(trainingVacancy);
    }
}