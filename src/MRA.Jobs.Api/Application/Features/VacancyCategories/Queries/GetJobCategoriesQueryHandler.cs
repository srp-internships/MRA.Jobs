using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries.GetJobCategories;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Application.Features.VacancyCategories.Queries;
public class GetJobCategoriesQueryHandler : IRequestHandler<GetJobCategoriesQuery, List<JobCategoriesResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetJobCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<JobCategoriesResponse>> Handle(GetJobCategoriesQuery request, CancellationToken cancellationToken)
    {

        IEnumerable<JobVacancy> jobsQuery = _context.JobVacancies;

        if (request.CheckDate)
        {
            DateTime now = DateTime.UtcNow;
            jobsQuery = _context.JobVacancies.Where(j => j.PublishDate <= now && j.EndDate >= now);
        }

        var sortredJobs = (from j in jobsQuery
                           group j by j.CategoryId).ToList();

        var jobs = jobsQuery.ToList();

        var jobsWithCategory = new List<JobCategoriesResponse>();
        var categories = await _context.Categories.ToListAsync();

        foreach (var job in sortredJobs)
        {
            var category = categories.Where(c => c.Id == job.Key).FirstOrDefault();

            jobsWithCategory.Add(new JobCategoriesResponse
            {
                CategoryId = job.Key,
                Category = _mapper.Map<CategoryResponse>(category),
                JobsCount = jobs.Count()
            });
        }

        return jobsWithCategory;
    }
}
