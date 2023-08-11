namespace MRA.Jobs.Application.Features.Applications.Command.UpdateApplicationStatus;

public class
    UpdateApplicationStatusCommandHandler : IRequestHandler<Contracts.Applications.Commands.UpdateApplicationStatus,
        bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private readonly IMapper _mapper;

    public UpdateApplicationStatusCommandHandler(IApplicationDbContext dbContext, IMapper mapper, IDateTime dateTime,
        ICurrentUserService currentUserService)
    {
        _context = dbContext;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(Contracts.Applications.Commands.UpdateApplicationStatus request,
        CancellationToken cancellationToken)
    {
        Domain.Entities.Application application = await _context.Applications.FindAsync(request.Id);
        _ = application ?? throw new NotFoundException(nameof(Application), request.Id);
        ;

        application.Status = (ApplicationStatus)request.StatusId;

        ApplicationTimelineEvent timelineEvent = new ApplicationTimelineEvent
        {
            ApplicationId = application.Id,
            Application = application,
            EventType = TimelineEventType.Updated,
            Time = _dateTime.Now,
            Note = "Application updated",
            CreateBy = _currentUserService.GetId() ?? Guid.Empty
        };
        await _context.ApplicationTimelineEvents.AddAsync(timelineEvent);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}