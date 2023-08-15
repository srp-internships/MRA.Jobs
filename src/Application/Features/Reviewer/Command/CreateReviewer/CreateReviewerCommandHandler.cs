using MRA.Jobs.Application.Contracts.Reviewer.Command;

namespace MRA.Jobs.Application.Features.Reviewer.Command.CreateReviewer;

public class CreateReviewerCommandHandler : IRequestHandler<CreateReviewerCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateReviewerCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateReviewerCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Reviewer reviewer = _mapper.Map<Domain.Entities.Reviewer>(request);
        await _context.Reviewers.AddAsync(reviewer, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        return reviewer.Id;
    }
}