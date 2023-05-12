using MRA.Jobs.Application.Contracts.Reviewer.Command;

namespace MRA.Jobs.Application.Features.Reviewer.Command.UpdateReviewer;
using Domain.Entities;

public class UpdateReviewerCommandHandler : IRequestHandler<UpdateReviewerCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public UpdateReviewerCommandHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    
    public async Task<Guid> Handle(UpdateReviewerCommand request, CancellationToken cancellationToken)
    {
        var reviewer =
            await _context.Reviewers.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
        _ = reviewer ?? throw new NotFoundException(nameof(Reviewer), request);

        _mapper.Map(request, reviewer);
        
        await _context.SaveChangesAsync(cancellationToken);
        return reviewer.Id;
    }
}