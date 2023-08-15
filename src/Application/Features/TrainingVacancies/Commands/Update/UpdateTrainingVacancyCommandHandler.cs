using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Commands.Update;

public class UpdateTrainingVacancyCommandHandler : IRequestHandler<UpdateTrainingVacancyCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private readonly IMapper _mapper;

    public UpdateTrainingVacancyCommandHandler(IApplicationDbContext context, IMapper mapper,
        ICurrentUserService currentUserService, IDateTime dateTime)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    public async Task<Guid> Handle(UpdateTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        //var trainingVacancy = await _context.TrainingVacancies.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
        //_ = trainingVacancy ?? throw new NotFoundException(nameof(TrainingVacancy), request.Id);

        //var category = await _context.Categories.FindAsync(new object[] { request.CategoryId }, cancellationToken: cancellationToken);
        //_ = category ?? throw new NotFoundException(nameof(VacancyCategory), request.CategoryId);
        var trainingVacancy = await _context.TrainingVacancies
           .Include(i => i.Category)
           .FirstOrDefaultAsync(i => i.Slug == request.Slug, cancellationToken);
        _ = trainingVacancy ?? throw new NotFoundException(nameof(TrainingVacancy), request.Slug);

        _mapper.Map(request, trainingVacancy);

        VacancyTimelineEvent timeLineEvent = new VacancyTimelineEvent
        {
            VacancyId = trainingVacancy.Id,
            EventType = TimelineEventType.Updated,
            Time = _dateTime.Now,
            Note = "Training Model updated",
            CreateBy = _currentUserService.GetId() ?? Guid.Empty
        };
        await _context.VacancyTimelineEvents.AddAsync(timeLineEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return trainingVacancy.Id;
    }
}