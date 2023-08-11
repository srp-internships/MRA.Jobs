using MRA.Jobs.Application.Contracts.Applications.Commands;

namespace MRA.Jobs.Application.Features.Applications.Command.UpdateApplication;

public class UpdateApplicationCommandHadler : IRequestHandler<UpdateApplicationCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private readonly IMapper _mapper;

    public UpdateApplicationCommandHadler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(UpdateApplicationCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Application application = await _context.Applications.FindAsync(request.Id);
        _ = application ?? throw new NotFoundException(nameof(Domain.Entities.Application), request.Id);
        ;

        _mapper.Map(request, application);

        ApplicationTimelineEvent timelineEvent = new ApplicationTimelineEvent
        {
            ApplicationId = application.Id,
            EventType = TimelineEventType.Updated,
            Time = _dateTime.Now,
            Note = "Application vacancy updated",
            CreateBy = _currentUserService.GetId() ?? Guid.Empty
        };
        await _context.ApplicationTimelineEvents.AddAsync(timelineEvent);
        await _context.SaveChangesAsync(cancellationToken);

        return application.Id;
    }
}