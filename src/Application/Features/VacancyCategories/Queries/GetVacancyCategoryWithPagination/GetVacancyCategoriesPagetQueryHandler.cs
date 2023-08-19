using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Application.Features.VacancyCategories.Queries.GetVacancyCategoryWithPagination;

public class
    GetVacancyCategoriesPageQueryHandler : IRequestHandler<PagedListQuery<CategoryResponse>,
        PagedList<CategoryResponse>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IApplicationSieveProcessor _sieveProcessor;

    public GetVacancyCategoriesPageQueryHandler(IApplicationDbContext dbContext,
        IApplicationSieveProcessor sieveProcessor, IMapper mapper)
    {
        _dbContext = dbContext;
        _sieveProcessor = sieveProcessor;
        _mapper = mapper;
    }

    public async Task<PagedList<CategoryResponse>> Handle(PagedListQuery<CategoryResponse> request,
        CancellationToken cancellationToken)
    {
        PagedList<CategoryResponse> result = _sieveProcessor.ApplyAdnGetPagedList(request,
            _dbContext.Categories.AsNoTracking(), _mapper.Map<CategoryResponse>);
        return await Task.FromResult(result);
    }
}