using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Reviewer.Commands;

namespace MRA.Jobs.Application.Features.Reviewer.Command.Tags;

using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Domain.Entities;
using MRA.Jobs.Domain.Enums;

public class AddTagToReviewerCommandHandler : IRequestHandler<AddTagsToReviewerCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public AddTagToReviewerCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(AddTagsToReviewerCommand request, CancellationToken cancellationToken)
    {
        var reviewer = await _context.Reviewers
          .Include(x => x.Tags)
          .ThenInclude(t => t.Tag)
          .FirstOrDefaultAsync(x => x.Id == request.ReviewerId, cancellationToken);

        if (reviewer == null)
            throw new NotFoundException(nameof(JobVacancy), request.ReviewerId);

        foreach (var tagName in request.Tags)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name.Equals(tagName), cancellationToken);

            if (tag == null)
            {
                tag = new Tag { Name = tagName };
                _context.Tags.Add(tag);
            }

            var ReviewerTag = reviewer.Tags.FirstOrDefault(t => t.Tag.Name == tagName);

            if (ReviewerTag == null)
            {
                ReviewerTag = new UserTag { UserId = request.ReviewerId, TagId = tag.Id };
                _context.UserTags.Add(ReviewerTag);

                var timelineEvent = new UserTimelineEvent
                {
                    UserId = reviewer.Id,
                    EventType = TimelineEventType.Created,
                    Time = _dateTime.Now,
                    Note = $"Added '{tag.Name}' tag",
                    CreateBy = _currentUserService.UserId
                };
                await _context.UserTimelineEvents.AddAsync(timelineEvent, cancellationToken);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}