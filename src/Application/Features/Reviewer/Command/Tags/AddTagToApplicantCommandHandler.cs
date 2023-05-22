using MRA.Jobs.Application.Contracts.Reviewer.Command;

namespace MRA.Jobs.Application.Features.Reviewer.Command.Tags;
public class AddTagToReviewerCommandHandler : IRequestHandler<AddTagToReviewerCommand, bool>
{
    private readonly IApplicationDbContext _dbContext;
  
    public AddTagToReviewerCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(AddTagToReviewerCommand request, CancellationToken cancellationToken)
    {
        var Reviewer = await _dbContext.Reviewers.FindAsync(new object[] { request.ReviewerId }, cancellationToken);
        var tag = await _dbContext.Tags.FindAsync(new object[] { request.TagId }, cancellationToken: cancellationToken);

        var userTag = new UserTag
        {
            UserId = Reviewer?.Id ?? throw new NotFoundException(nameof(Reviewer), request.ReviewerId),
            TagId = tag?.Id ?? throw new NotFoundException(nameof(Tag), request.TagId)
        };

        await _dbContext.UserTags.AddAsync(userTag, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
