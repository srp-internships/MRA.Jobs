using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries;
public class GetTrainingVacancyByIdQueryHandler : IRequestHandler<GetTrainingVacancyByIdQuery, TrainingVacancyDetailedResponce>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTrainingVacancyByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<TrainingVacancyDetailedResponce> Handle(GetTrainingVacancyByIdQuery request, CancellationToken cancellationToken)
    {
        var trainingVacancy = await _context.TrainingVacancies.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
        _ = trainingVacancy ?? throw new NotFoundException(nameof(TrainingVacancy), request.Id);
        return _mapper.Map<TrainingVacancyDetailedResponce>(trainingVacancy);
    }
}