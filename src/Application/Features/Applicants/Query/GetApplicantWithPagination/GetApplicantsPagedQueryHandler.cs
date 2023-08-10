using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Applicant.Responses;
using MRA.Jobs.Application.Contracts.Common;

namespace MRA.Jobs.Application.Features.Applicants.Query.GetApplicantWithPagination;

public class
    GetApplicantsPagedQueryHandler : IRequestHandler<PagedListQuery<ApplicantListDto>, PagedList<ApplicantListDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IApplicationSieveProcessor _sieveProcessor;

    public GetApplicantsPagedQueryHandler(IApplicationDbContext context, IMapper mapper,
        IApplicationSieveProcessor sieveProcessor)
    {
        _dbContext = context;
        _mapper = mapper;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<PagedList<ApplicantListDto>> Handle(PagedListQuery<ApplicantListDto> request,
        CancellationToken cancellationToken)
    {
        PagedList<ApplicantListDto> result = _sieveProcessor.ApplyAdnGetPagedList(request,
            _dbContext.Applicants.AsNoTracking(), _mapper.Map<ApplicantListDto>);
        return await Task.FromResult(result);
    }
}