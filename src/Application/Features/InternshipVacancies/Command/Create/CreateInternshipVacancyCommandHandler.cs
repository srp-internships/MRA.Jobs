using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Command.Create;

public class CreateInternshipVacancyCommandHandler : IRequestHandler<CreateInternshipVacancyCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private readonly IMapper _mapper;

    public CreateInternshipVacancyCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateInternshipVacancyCommand request, CancellationToken cancellationToken)
    {
        VacancyCategory category = await _context.Categories.FindAsync(request.CategoryId);
        _ = category ?? throw new NotFoundException(nameof(VacancyCategory), request.CategoryId);

        InternshipVacancy internship = _mapper.Map<InternshipVacancy>(request);
        await _context.Internships.AddAsync(internship, cancellationToken);

        VacancyTimelineEvent timelineEvent = new VacancyTimelineEvent
        {
            VacancyId = internship.Id,
            EventType = TimelineEventType.Created,
            Time = _dateTime.Now,
            Note = "Internship created",
            CreateBy = _currentUserService.GetId() ?? Guid.Empty
        };
        await _context.VacancyTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return internship.Id;
    }
}