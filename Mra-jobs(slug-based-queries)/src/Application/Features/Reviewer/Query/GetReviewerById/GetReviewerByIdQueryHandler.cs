using MRA.Jobs.Application.Contracts.Reviewer.Queries;
using MRA.Jobs.Application.Contracts.Reviewer.Response;

namespace MRA.Jobs.Application.Features.Reviewer.Query.GetReviewerById;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class GetReviewerByIdQueryHandler : IRequestHandler<GetReviewerByIdQuery, ReviewerDetailsDto>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public GetReviewerByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ReviewerDetailsDto> Handle(GetReviewerByIdQuery request, CancellationToken cancellationToken)
    {
        //var reviewer =
        //    await _context.Reviewers.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);

        var reviewer =await _context.Reviewers
            .Include(a => a.History)
                 .Include(a => a.Tags)
                   .ThenInclude(t => t.Tag)
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        _ = reviewer ?? throw new NotFoundException(nameof(Reviewer), request.Id);
        
        return _mapper.Map<ReviewerDetailsDto>(reviewer);

    }
}