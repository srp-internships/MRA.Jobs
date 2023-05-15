using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Applicant.Responses;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Reviewer.Queries;
using MRA.Jobs.Application.Contracts.Reviewer.Response;
using MRA.Jobs.Infrastructure;

namespace MRA.Jobs.Application.Features.Reviewer.Query.GetReviewerWithPagination;

public class GetAllReviewerCommandHandler : IRequestHandler<PaggedListQuery<ReviewerListDTO>, PaggedList<ReviewerListDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IApplicationSieveProcessor _applicationSieveProcessor;

    public GetAllReviewerCommandHandler(IApplicationDbContext context, IMapper mapper, IApplicationSieveProcessor applicationSieveProcessor)
    {
        _context = context;
        _mapper = mapper;
        _applicationSieveProcessor = applicationSieveProcessor;
    }
    
    public async Task<PaggedList<ReviewerListDTO>> Handle(PaggedListQuery<ReviewerListDTO> request, CancellationToken cancellationToken)
    {
        var reviewer = _applicationSieveProcessor.ApplyAdnGetPaggedList(request, _context.Applicants.AsNoTracking(), _mapper.Map<ReviewerListDTO>);
        return await Task.FromResult(reviewer);
    }
}