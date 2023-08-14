﻿using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Seive;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Reviewer.Response;

namespace MRA.Jobs.Application.Features.Reviewer.Query.GetReviewerWithPagination;

public class GetReviewersPagedQueryHandler : IRequestHandler<PaggedListQuery<ReviewerListDto>, PaggedList<ReviewerListDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IApplicationSieveProcessor _sieveProcessor;

    public GetReviewersPagedQueryHandler(IApplicationDbContext context, IMapper mapper, IApplicationSieveProcessor sieveProcessor)
    {
        _dbContext = context;
        _mapper = mapper;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<PaggedList<ReviewerListDto>> Handle(PaggedListQuery<ReviewerListDto> request, CancellationToken cancellationToken)
    {
        var result = _sieveProcessor.ApplyAdnGetPaggedList(request, _dbContext.Reviewers.AsNoTracking(), _mapper.Map<ReviewerListDto>);
        return await Task.FromResult(result);
    }
}