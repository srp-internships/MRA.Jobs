using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Applicant.Commands;

namespace MRA.Jobs.Application.Features.Applicants.Command.Tags;
public class RemoveTagsFromApplicantCommandHandler : IRequestHandler<RemoveTagsFromApplicantCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public RemoveTagsFromApplicantCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(RemoveTagsFromApplicantCommand request, CancellationToken cancellationToken)
    {
        var applicant = await _context.Applicants
            .Include(x => x.Tags)
            .ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(x => x.Id == request.ApplicantId, cancellationToken);

        if (applicant == null)
            throw new NotFoundException(nameof(Applicant), request.ApplicantId);

        foreach (var tag in request.Tags)
        {
            var applicantTag = applicant.Tags.FirstOrDefault(t => t.Tag.Name == tag);

            if (applicantTag != null)
                _context.UserTags.Remove(applicantTag);

            var timelineEvent = new UserTimelineEvent
            {
                UserId = applicant.Id,
                EventType = TimelineEventType.Deleted,
                Time = _dateTime.Now,
                Note = $"Removed '{tag}' tag",
                CreateBy = _currentUserService.GetId() ?? Guid.Empty
            };
            await _context.UserTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
