using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using MRA.Jobs.Infrastructure;

namespace MRA.Jobs.Application.Features.VacancyCategories.Queries.GetVacancyCategoryWithPagination;
public class GetVacancyCategoriesPageQueryHandler : IRequestHandler<PaggedListQuery<CategoryResponse>, PaggedList<CategoryResponse>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IApplicationSieveProcessor _sieveProcessor;
    private readonly IMapper _mapper;

    public GetVacancyCategoriesPageQueryHandler(IApplicationDbContext dbContext, IApplicationSieveProcessor sieveProcessor, IMapper mapper)
    {
        _dbContext = dbContext;
        _sieveProcessor = sieveProcessor;
        _mapper = mapper;
    }

    public async Task<PaggedList<CategoryResponse>> Handle(PaggedListQuery<CategoryResponse> request, CancellationToken cancellationToken)
    {
        var result = _sieveProcessor.ApplyAdnGetPaggedList(request, _dbContext.Categories.AsNoTracking(), _mapper.Map<CategoryResponse>);
        return await Task.FromResult(result);
    }
}
