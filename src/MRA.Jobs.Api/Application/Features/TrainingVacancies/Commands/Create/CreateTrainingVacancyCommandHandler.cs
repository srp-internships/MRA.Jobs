using MRA.Jobs.Application.Common.Security;
using MRA.Jobs.Application.Common.SlugGeneratorService;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Commands.Create;

public class CreateTrainingVacancyCommandHandler : IRequestHandler<CreateTrainingVacancyCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;
    private readonly ISlugGeneratorService _slugService;

    public CreateTrainingVacancyCommandHandler(ISlugGeneratorService slugService, IApplicationDbContext context, IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
        _slugService = slugService;
    }

    public async Task<string> Handle(CreateTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        VacancyCategory category = await _context.Categories.FindAsync(request.CategoryId);
        _ = category ?? throw new NotFoundException(nameof(VacancyCategory), request.CategoryId);

        var traningModel = _mapper.Map<TrainingVacancy>(request);
        traningModel.Category = category;
        traningModel.Slug = GenerateSlug(traningModel);
        await _context.TrainingVacancies.AddAsync(traningModel, cancellationToken);

        VacancyTimelineEvent timelineEvent = new VacancyTimelineEvent
        {
            VacancyId = traningModel.Id,
            EventType = TimelineEventType.Created,
            Time = _dateTime.Now,
            Note = "Training Model created",
            CreateBy = _currentUserService.GetId() ?? Guid.Empty
        };
        await _context.VacancyTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return traningModel.Slug;
    }

    private string GenerateSlug(TrainingVacancy vacancy) => _slugService.GenerateSlug($"{vacancy.Title}-{vacancy.PublishDate.Year}-{vacancy.PublishDate.Month}");

}
