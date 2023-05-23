using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Reviewer.Commands;

namespace MRA.Jobs.Application.Features.Reviewer.Command.Tags;
public class RemoveTagsFromReviewerCommandHandler : IRequestHandler<RemoveTagsFromReviewerCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public RemoveTagsFromReviewerCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> Handle(RemoveTagsFromReviewerCommand request, CancellationToken cancellationToken)
    {
        var Reviewer = await _context.Reviewers.FindAsync(new object[] { request.ReviewerId }, cancellationToken);

        if (Reviewer == null)
            throw new NotFoundException(nameof(Reviewer), request.ReviewerId);

        foreach (var tag in request.Tags)
        {
            var ReviewerTag = await _context.UserTags.FindAsync(new object[] {request.ReviewerId, tag }, cancellationToken);

            if (ReviewerTag != null)
                _context.UserTags.Remove(ReviewerTag);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
