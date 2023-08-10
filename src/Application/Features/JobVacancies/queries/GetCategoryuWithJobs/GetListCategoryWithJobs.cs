using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Application.Features.JobVacancies.queries.GetCategoryuWithJobs;
public class GetJobsPagedByCategoryQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryResponseCount>>
{
    private readonly IApplicationDbContext _dbContext;


    public GetJobsPagedByCategoryQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

    }

    
   public async Task<List<CategoryResponseCount>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        DateTime datenov = DateTime.Now;
        var result = await _dbContext.JobVacancies
            .Where(j=>j.PublishDate<= datenov && j.EndDate>= datenov)
            .GroupBy(j=>j.CategoryId)
            .Select(x=>new CategoryResponseCount
            {
                Name=x.First().Category.Name,
                Id=x.Key,
                Cout=x.Count()
            })
           .ToListAsync(cancellationToken);

        return result;

    }
}
