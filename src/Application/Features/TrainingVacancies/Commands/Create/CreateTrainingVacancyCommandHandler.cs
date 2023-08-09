using MRA.Jobs.Application.Common.Security;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Commands.Create;
public class CreateTrainingVacancyCommandHandler : IRequestHandler<CreateTrainingVacancyCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public CreateTrainingVacancyCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }
    public async Task<Guid> Handle(CreateTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FindAsync(request.CategoryId);
        _ = category ?? throw new NotFoundException(nameof(VacancyCategory), request.CategoryId);

        var traningModel = _mapper.Map<TrainingVacancy>(request);
        traningModel.Category = category;
        traningModel.Slug = traningModel.Title.ToLower().Trim();
        await _context.TrainingVacancies.AddAsync(traningModel, cancellationToken);

        var timelineEvent = new VacancyTimelineEvent
        {
            VacancyId = traningModel.Id,
            EventType = TimelineEventType.Created,
            Time = _dateTime.Now,
            Note = "Training Model created",
            CreateBy = _currentUserService.GetId() ?? Guid.Empty
        };
        await _context.VacancyTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return traningModel.Id;
    }
}
