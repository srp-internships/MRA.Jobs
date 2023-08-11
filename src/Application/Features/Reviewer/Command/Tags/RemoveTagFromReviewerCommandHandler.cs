using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Reviewer.Command;

namespace MRA.Jobs.Application.Features.Reviewer.Command.Tags;

public class RemoveTagsFromReviewerCommandHandler : IRequestHandler<RemoveTagsFromReviewerCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private readonly IMapper _mapper;

    public RemoveTagsFromReviewerCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(RemoveTagsFromReviewerCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Reviewer reviewer = await _context.Reviewers
            .Include(x => x.Tags)
            .ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(x => x.Id == request.ReviewerId, cancellationToken);

        if (reviewer == null)
        {
            throw new NotFoundException(nameof(reviewer), request.ReviewerId);
        }

        foreach (string tag in request.Tags)
        {
            UserTag reviewerTag = reviewer.Tags.FirstOrDefault(t => t.Tag.Name == tag);

            if (reviewerTag != null)
            {
                _context.UserTags.Remove(reviewerTag);
            }

            UserTimelineEvent timelineEvent = new UserTimelineEvent
            {
                UserId = reviewer.Id,
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