using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Common;

namespace MRA.Jobs.Application.Features.Applications.Query.GetApplicationWithPagination;

public class
    GetApplicationsPagedQueryHandler : IRequestHandler<PagedListQuery<ApplicationListDto>,
        PagedList<ApplicationListDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IApplicationSieveProcessor _sieveProcessor;

    public GetApplicationsPagedQueryHandler(IApplicationDbContext dbContext, IApplicationSieveProcessor sieveProcessor,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _sieveProcessor = sieveProcessor;
        _mapper = mapper;
    }

    public Task<PagedList<ApplicationListDto>> Handle(PagedListQuery<ApplicationListDto> request,
        CancellationToken cancellationToken)
    {
        PagedList<ApplicationListDto> result = _sieveProcessor.ApplyAdnGetPagedList(request,
            _dbContext.Applications.AsNoTracking().Include(a => a.VacancyResponses), _mapper.Map<ApplicationListDto>);
        return Task.FromResult(result);
    }
}