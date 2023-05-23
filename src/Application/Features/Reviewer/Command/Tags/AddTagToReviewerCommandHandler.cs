using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Reviewer.Commands;

namespace MRA.Jobs.Application.Features.Reviewer.Command.Tags;
using MRA.Jobs.Domain.Entities;
public class AddTagToReviewerCommandHandler : IRequestHandler<AddTagsToReviewerCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AddTagToReviewerCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> Handle(AddTagsToReviewerCommand request, CancellationToken cancellationToken)
    {
        var reviewer = await _context.Reviewers.FindAsync(new object[] { request.ReviewerId }, cancellationToken);

        if (reviewer == null)
            throw new NotFoundException(nameof(Reviewer), request.ReviewerId);

        foreach (var tagName in request.Tags)
        {
            var tag = await _context.Tags.FindAsync(new object[] { tagName }, cancellationToken);

            if (tag == null)
            {
                tag = new Tag { Name = tagName };
                _context.Tags.Add(tag);
            }

            //var ReviewerTag = await _context.UserTags.FirstOrDefaultAsync(at => at.UserId == request.ReviewerId && at.TagId == tag.Id);
            var ReviewerTag = await _context.UserTags.FindAsync(new object[] { request.ReviewerId, tag.Id }, cancellationToken);
            if (ReviewerTag == null)
            {
                ReviewerTag = new UserTag { UserId = request.ReviewerId, TagId = tag.Id };
                _context.UserTags.Add(ReviewerTag);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}