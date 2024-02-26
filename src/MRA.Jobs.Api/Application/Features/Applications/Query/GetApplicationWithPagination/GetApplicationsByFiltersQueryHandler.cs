using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MRA.Jobs.Application.Contracts.Applications.Queries.GetApplicationWithPagination;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Common;

namespace MRA.Jobs.Application.Features.Applications.Query.GetApplicationWithPagination;

public class GetApplicationsByFiltersQueryHandler(IApplicationDbContext dbContext, 
    IHttpClientFactory clientFactory,
    IConfiguration configuration)
    : IRequestHandler<GetApplicationsByFiltersQuery, PagedList<ApplicationListDto>>
{
    public async Task<PagedList<ApplicationListDto>> Handle(GetApplicationsByFiltersQuery request,
        CancellationToken cancellationToken)
    {
        var application = dbContext.Applications
            .Include(a => a.Vacancy)
            .AsNoTracking();

        var httpClient= clientFactory.CreateClient("Mra.Identity");
        httpClient.BaseAddress =new Uri(configuration[""]);

    }
}