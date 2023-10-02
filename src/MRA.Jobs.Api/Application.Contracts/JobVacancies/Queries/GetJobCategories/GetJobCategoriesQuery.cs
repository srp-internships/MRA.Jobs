using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Queries.GetJobCategories;
public class GetJobCategoriesQuery : IRequest<List<JobCategoriesResponse>>
{
    public bool CheckDate { get; set; } = true;
}
