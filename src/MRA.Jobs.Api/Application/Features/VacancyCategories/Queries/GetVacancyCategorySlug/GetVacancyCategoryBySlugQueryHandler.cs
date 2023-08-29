using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.VacancyCategories.Queries;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Application.Features.VacancyCategories.Queries.GetVacancyCategoryBySlug;
public class GetVacancyCategoryBySlugQueryHandler : IRequestHandler<GetVacancyCategoryBySlugQuery, CategoryResponse>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetVacancyCategoryBySlugQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<CategoryResponse> Handle(GetVacancyCategoryBySlugQuery request, CancellationToken cancellationToken)
    {
        var vacancyCategory = await _dbContext.Categories.FirstOrDefaultAsync(v => v.Slug == request.Slug, cancellationToken);
        _ = vacancyCategory ?? throw new NotFoundException(nameof(VacancyCategory), request.Slug);
        return _mapper.Map<CategoryResponse>(vacancyCategory);
    }
}
