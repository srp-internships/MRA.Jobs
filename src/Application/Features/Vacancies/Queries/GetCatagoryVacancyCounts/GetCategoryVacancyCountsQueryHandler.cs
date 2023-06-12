using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Vacncies.Queries;
using MRA.Jobs.Application.Contracts.Vacncies.Responses;

namespace MRA.Jobs.Application.Features.Vacancies.Queries.GetCatagoryVacancyCounts;
public class GetCategoryVacancyCountsQueryHandler : IRequestHandler<GetCategoryVacancyCountsQuery, List<CategoryVacancyCountDTO>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetCategoryVacancyCountsQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<CategoryVacancyCountDTO>> Handle(GetCategoryVacancyCountsQuery request, CancellationToken cancellationToken)
    {
        var currentDate = DateTime.Now;

        var categoryVacancyCounts = await _dbContext.Vacancies
            .Where(v => v.PublishDate <= currentDate && v.EndDate >= currentDate)
            .GroupBy(v => v.CategoryId)
            .Select(g => new CategoryVacancyCountDTO
            {
                CategoryId = g.Key,
                CategoryName = g.First().Category.Name,
                VacancyCount = g.Count()
            })
            .ToListAsync(cancellationToken);

        return categoryVacancyCounts;
    }
}
