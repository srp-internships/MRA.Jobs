using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Reviewer.Queries;
using MRA.Jobs.Application.Contracts.Reviewer.Response;

namespace MRA.Jobs.Application.Features.Reviewer.Query.GetAllReviewer;

public class GetAllReviewerQueryHandler : IRequestHandler<GetAllReviewerQuery, List<ReviewerListDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllReviewerQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<List<ReviewerListDto>> Handle(GetAllReviewerQuery request, CancellationToken cancellationToken)
    {
        var reviewers =
            await _context.Reviewers.ToListAsync();
        
        return _mapper.Map<List<ReviewerListDto>>(reviewers);
    }
}