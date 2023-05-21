using MRA.Jobs.Application.Contracts.Internships.Commands;

namespace MRA.Jobs.Application.Features.Internships.Command.CreateInternship;
public class CreateInternshipCommandHandler : IRequestHandler<CreateInternshipCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public CreateInternshipCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateInternshipCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FindAsync(request.CategoryId);
        _ = category ?? throw new NotFoundException(nameof(VacancyCategory), request.CategoryId);

        var internship = _mapper.Map<Internship>(request);
        await _context.Internships.AddAsync(internship, cancellationToken);

        var timelineEvent = new VacancyTimelineEvent
        {
            VacancyId = internship.Id,
            EventType = TimelineEventType.Created,
            Time = _dateTime.Now,
            Note = "Internship created",
            CreateBy = _currentUserService.UserId
        };
        await _context.VacancyTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return internship.Id;
    }
}