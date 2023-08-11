using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries;

public class
    GetTrainingVacancyByIdQueryHandler : IRequestHandler<GetTrainingVacancyByIdQuery, TrainingVacancyDetailedResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTrainingVacancyByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TrainingVacancyDetailedResponse> Handle(GetTrainingVacancyByIdQuery request,
        CancellationToken cancellationToken)
    {
        // var trainingVacancy = await _context.TrainingVacancies.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
        TrainingVacancy trainingVacancy = await _context.TrainingVacancies.Include(i => i.History)
            .Include(i => i.Tags)
            .ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(i => i.Id == request.Id);
        _ = trainingVacancy ?? throw new NotFoundException(nameof(TrainingVacancy), request.Id);
        return _mapper.Map<TrainingVacancyDetailedResponse>(trainingVacancy);
    }
}