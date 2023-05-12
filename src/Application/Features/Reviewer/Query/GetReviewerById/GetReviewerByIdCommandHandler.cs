using MRA.Jobs.Application.Contracts.JobVacancies.Queries;
using MRA.Jobs.Application.Contracts.Reviewer.Queries;
using MRA.Jobs.Application.Contracts.Reviewer.Response;

namespace MRA.Jobs.Application.Features.Reviewer.Query.GetReviewerById;
using Domain.Entities;

public class GetReviewerByIdCommandHandler : IRequestHandler<GetReviewerByIdQuery, ReviewerListDTO>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public GetReviewerByIdCommandHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    
    public async Task<ReviewerListDTO> Handle(GetReviewerByIdQuery request, CancellationToken cancellationToken)
    {
        var reviewer = await _context.Reviewers.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
        _ = reviewer ?? throw new NotFoundException(nameof(Reviewer), cancellationToken);
        return _mapper.Map<ReviewerListDTO>(reviewer);

    }
}