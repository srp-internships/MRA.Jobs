using MRA.Jobs.Application.Contracts.TrainingModels.Commands;
using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Application.Features.TraningModels.Commands.CreateTraningModel;
public class CreateTrainingModelCommandHandler : IRequestHandler<CreateTrainingModelCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public CreateTrainingModelCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }
    public async Task<Guid> Handle(CreateTrainingModelCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FindAsync(request.CategoryId);
        _ = category ?? throw new NotFoundException(nameof(VacancyCategory), request.CategoryId);

        var traningModel = _mapper.Map<TrainingModel>(request);
        traningModel.Category = category;
        await _context.TrainingModels.AddAsync(traningModel, cancellationToken);

        var timelineEvent = new VacancyTimelineEvent
        {
            VacancyId = traningModel.Id,
            EventType = TimelineEventType.Created,
            Time = _dateTime.Now,
            Note = "Training Model created",
            CreateBy = _currentUserService.UserId
        };
        await _context.VacancyTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return traningModel.Id;
    }
}
