using MRA.Jobs.Application.Contracts.Reviewer.Command;

namespace MRA.Jobs.Application.Features.Reviewer.Command.DeleteReviewer;
using Domain.Entities;

public class DeleteReviewerCommandHandler : IRequestHandler<DeleteReviewerCommand, bool>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public DeleteReviewerCommandHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    
    public async Task<bool> Handle(DeleteReviewerCommand request, CancellationToken cancellationToken)
    {
        var reviewer =
            await _context.Reviewers.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
       
        if (reviewer == null)
            throw new NotFoundException(nameof(Reviewer), request.Id);
        
        _context.Reviewers.Remove(reviewer);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}