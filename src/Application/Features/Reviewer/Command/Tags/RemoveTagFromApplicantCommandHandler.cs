using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Reviewer.Commands;

namespace MRA.Jobs.Application.Features.Reviewer.Command.Tags;
public class RemoveTagFromReviewerCommandHandler : IRequestHandler<RemoveTagFromReviewerCommand, bool>
{
    private readonly IApplicationDbContext _dbContext;

    public RemoveTagFromReviewerCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(RemoveTagFromReviewerCommand request, CancellationToken cancellationToken)
    {
        var userTag = await _dbContext.UserTags
          .FirstOrDefaultAsync(vt => vt.UserId == request.ReviewerId && vt.TagId == request.TagId, cancellationToken);

        _ = userTag ?? throw new NotFoundException(nameof(VacancyTag), request.TagId);

        _ = _dbContext.UserTags.Remove(userTag);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
