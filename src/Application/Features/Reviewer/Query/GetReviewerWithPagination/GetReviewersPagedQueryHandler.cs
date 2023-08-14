using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Reviewer.Response;

namespace MRA.Jobs.Application.Features.Reviewer.Query.GetReviewerWithPagination;

public class
    GetReviewersPagedQueryHandler : IRequestHandler<PagedListQuery<ReviewerListDto>, PagedList<ReviewerListDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IApplicationSieveProcessor _sieveProcessor;

    public GetReviewersPagedQueryHandler(IApplicationDbContext context, IMapper mapper,
        IApplicationSieveProcessor sieveProcessor)
    {
        _dbContext = context;
        _mapper = mapper;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<PagedList<ReviewerListDto>> Handle(PagedListQuery<ReviewerListDto> request,
        CancellationToken cancellationToken)
    {
        PagedList<ReviewerListDto> result = _sieveProcessor.ApplyAdnGetPagedList(request,
            _dbContext.Reviewers.AsNoTracking(), _mapper.Map<ReviewerListDto>);
        return await Task.FromResult(result);
    }
}