using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.SlugGeneratorService;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Create;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Commands.Create;

public class CreateTrainingVacancyCommandHandler : IRequestHandler<CreateTrainingVacancyCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;
    private readonly ISlugGeneratorService _slugService;

    public CreateTrainingVacancyCommandHandler(ISlugGeneratorService slugService, IApplicationDbContext context,
        IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService)
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

        var trainingModel = _mapper.Map<TrainingVacancy>(request);

        trainingModel.Category = category;
        trainingModel.Slug = GenerateSlug(trainingModel);
        await ThrowIfApplicationExist(trainingModel.Slug);
        trainingModel.CreatedByEmail = _currentUserService.GetEmail();
        trainingModel.LastModifiedBy = trainingModel.CreatedBy = _currentUserService.GetUserId() ?? Guid.NewGuid();
        trainingModel.LastModifiedAt = trainingModel.CreatedAt = _dateTime.Now;
        
        await _context.TrainingVacancies.AddAsync(trainingModel, cancellationToken);

        VacancyTimelineEvent timelineEvent = new VacancyTimelineEvent
        {
            VacancyId = trainingModel.Id,
            EventType = TimelineEventType.Created,
            Time = _dateTime.Now,
            Note = "Training Model Created",
            CreateBy = _currentUserService.GetUserId() ?? Guid.Empty
        };
        await _context.VacancyTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return trainingModel.Slug;
    }
    private async Task ThrowIfApplicationExist(string applicationSlug)
    {
        if (await _context.TrainingVacancies.AnyAsync(v => v.Slug == applicationSlug))
        {
            throw new ConflictException("Duplicate Training. Training with this title already exists!");
        }
    }
    private string GenerateSlug(TrainingVacancy vacancy) =>
        _slugService.GenerateSlug($"{vacancy.Title}-{vacancy.PublishDate.Year}-{vacancy.PublishDate.Month}");
}