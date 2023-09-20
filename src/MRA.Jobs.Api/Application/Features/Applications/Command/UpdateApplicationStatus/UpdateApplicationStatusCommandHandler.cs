namespace MRA.Jobs.Application.Features.Applications.Command.UpdateApplicationStatus;

using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplicationStatus;
using MRA.Jobs.Domain.Enums;

public class UpdateApplicationStatusCommandHandler : IRequestHandler<UpdateApplicationStatus, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public UpdateApplicationStatusCommandHandler(IApplicationDbContext dbContext, IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = dbContext;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }
    public async Task<bool> Handle(UpdateApplicationStatus request, CancellationToken cancellationToken)
    {
        var application = await _context.Applications.FirstOrDefaultAsync(t => t.Slug == request.Slug, cancellationToken);
        _ = application ?? throw new NotFoundException(nameof(Application), request.Slug); ;

        application.Status = (ApplicationStatus)request.StatusId;

        var timelineEvent = new ApplicationTimelineEvent
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
