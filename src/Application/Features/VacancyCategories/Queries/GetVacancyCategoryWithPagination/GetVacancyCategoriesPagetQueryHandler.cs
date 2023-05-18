using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using MRA.Jobs.Infrastructure;

namespace MRA.Jobs.Application.Features.VacancyCategories.Queries.GetVacancyCategoryWithPagination;
public class GetVacancyCategoriesPageQueryHandler : IRequestHandler<PaggedListQuery<VacancyCategoryListDTO>, PaggedList<VacancyCategoryListDTO>>
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

    public async Task<PaggedList<VacancyCategoryListDTO>> Handle(PaggedListQuery<VacancyCategoryListDTO> request, CancellationToken cancellationToken)
    {
        var result = _sieveProcessor.ApplyAdnGetPaggedList(request, _dbContext.Categories.AsNoTracking(), _mapper.Map<VacancyCategoryListDTO>);
        return await Task.FromResult(result);
    }
}
