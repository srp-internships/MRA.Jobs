using MRA.Jobs.Application.Contracts.TrainingModels.Queries;
using MRA.Jobs.Application.Contracts.TrainingModels.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries.GetTrainingModelById;
public class GetTrainingModelByIdQueryHandler : IRequestHandler<GetTrainingVacancyByIdQuery, TrainingModelDetailsDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTrainingModelByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<TrainingModelDetailsDTO> Handle(GetTrainingVacancyByIdQuery request, CancellationToken cancellationToken)
    {
        var trainingModel = await _context.TrainingModels.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
        _ = trainingModel ?? throw new NotFoundException(nameof(TrainingVacancy), request.Id);
        return _mapper.Map<TrainingModelDetailsDTO>(trainingModel);
    }
}