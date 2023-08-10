using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Common;

namespace MRA.Jobs.Application.Features.Applications.Query.GetApplicationWithPagination;
public class GetApplicationsPagedQueryHandler : IRequestHandler<PaggedListQuery<ApplicationListDTO>, PaggedList<ApplicationListDTO>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IApplicationSieveProcessor _sieveProcessor;
    private readonly IMapper _mapper;

    public GetApplicationsPagedQueryHandler(IApplicationDbContext dbContext, IApplicationSieveProcessor sieveProcessor, IMapper mapper)
    {
        _dbContext = dbContext;
        _sieveProcessor = sieveProcessor;
        _mapper = mapper;
    }

    public Task<PaggedList<ApplicationListDTO>> Handle(PaggedListQuery<ApplicationListDTO> request, CancellationToken cancellationToken)
    {
        var result = _sieveProcessor.ApplyAdnGetPagedList(request, _dbContext.Applications.AsNoTracking(), _mapper.Map<ApplicationListDTO>);
        return Task.FromResult(result);
    }
}
