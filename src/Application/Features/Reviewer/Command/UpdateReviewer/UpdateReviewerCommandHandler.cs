using MRA.Jobs.Application.Contracts.Reviewer.Command;

namespace MRA.Jobs.Application.Features.Reviewer.Command.UpdateReviewer;

public class UpdateReviewerCommandHandler : IRequestHandler<UpdateReviewerCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateReviewerCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(UpdateReviewerCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Reviewer reviewer =
            await _context.Reviewers.FindAsync(new object[] { request.Id }, cancellationToken);
        _ = reviewer ?? throw new NotFoundException(nameof(Domain.Entities.Reviewer), request);

        _mapper.Map(request, reviewer);

        await _context.SaveChangesAsync(cancellationToken);
        return reviewer.Id;
    }
}