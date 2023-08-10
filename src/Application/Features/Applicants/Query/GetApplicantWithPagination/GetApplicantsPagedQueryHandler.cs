using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Applicant.Responses;
using MRA.Jobs.Application.Contracts.Common;

namespace MRA.Jobs.Application.Features.Applicants.Query.GetApplicantWithPagination;

public class GetApplicantsPagedQueryHandler : IRequestHandler<PaggedListQuery<ApplicantListDto>, PaggedList<ApplicantListDto>>
{
    private readonly IMapper _mapper;
    private readonly IApplicationSieveProcessor _sieveProcessor;
    private readonly IApplicationDbContext _dbContext;

    public GetApplicantsPagedQueryHandler(IApplicationDbContext context, IMapper mapper, IApplicationSieveProcessor sieveProcessor)
    {
        _dbContext = context;
        _mapper = mapper;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<PaggedList<ApplicantListDto>> Handle(PaggedListQuery<ApplicantListDto> request, CancellationToken cancellationToken)
    {
        var result = _sieveProcessor.ApplyAdnGetPagedList(request, _dbContext.Applicants.AsNoTracking(), _mapper.Map<ApplicantListDto>);
        return await Task.FromResult(result);
    }
}