using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries;
using MRA.Jobs.Application.Contracts.Reviewer.Queries;
using MRA.Jobs.Application.Contracts.Reviewer.Response;

namespace MRA.Jobs.Application.Features.Reviewer.Query.GetReviewerById;
using Domain.Entities;

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
        var reviewer =
            await _context.Reviewers.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
        _ = reviewer ?? throw new NotFoundException(nameof(Reviewer), request.Id);
        
        return _mapper.Map<ReviewerDetailsDto>(reviewer);

    }
}