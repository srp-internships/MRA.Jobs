using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Reviewer.Command;

namespace MRA.Jobs.Application.Features.Reviewer.Command.Tags;

public class AddTagToReviewerCommandHandler : IRequestHandler<AddTagsToReviewerCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private readonly IMapper _mapper;

    public AddTagToReviewerCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(AddTagsToReviewerCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Reviewer reviewer = await _context.Reviewers
            .Include(x => x.Tags)
            .ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(x => x.Id == request.ReviewerId, cancellationToken);

        if (reviewer == null)
        {
            throw new NotFoundException(nameof(JobVacancy), request.ReviewerId);
        }

        foreach (string tagName in request.Tags)
        {
            Tag tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name.Equals(tagName), cancellationToken);

            if (tag == null)
            {
                tag = new Tag { Name = tagName };
                _context.Tags.Add(tag);
            }

            UserTag ReviewerTag = reviewer.Tags.FirstOrDefault(t => t.Tag.Name == tagName);

            if (ReviewerTag == null)
            {
                ReviewerTag = new UserTag { UserId = request.ReviewerId, TagId = tag.Id };
                _context.UserTags.Add(ReviewerTag);

                UserTimelineEvent timelineEvent = new UserTimelineEvent
                {
                    UserId = reviewer.Id,
                    EventType = TimelineEventType.Created,
                    Time = _dateTime.Now,
                    Note = $"Added '{tag.Name}' tag",
                    CreateBy = _currentUserService.GetId() ?? Guid.Empty
                };
                await _context.UserTimelineEvents.AddAsync(timelineEvent, cancellationToken);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}