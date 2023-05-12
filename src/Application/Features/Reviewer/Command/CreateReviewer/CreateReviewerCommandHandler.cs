using MRA.Jobs.Application.Contracts.Reviewer.Command;

namespace MRA.Jobs.Application.Features.Reviewer.Command.CreateReviewer;
using Domain.Entities;

public class CreateReviewerCommandHandler : IRequestHandler<CreateReviewerCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public CreateReviewerCommandHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    public async Task<Guid> Handle(CreateReviewerCommand request, CancellationToken cancellationToken)
    {
        var reviewer = _mapper.Map<Reviewer>(request);
        await _context.Reviewers.AddAsync(reviewer, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        return reviewer.Id;
    }
}