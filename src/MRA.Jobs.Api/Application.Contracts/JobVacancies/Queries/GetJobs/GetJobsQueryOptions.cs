using System.Collections;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Queries.GetJobs;

public class GetJobsQueryOptions : PagedListQuery<JobVacancyListDto>
{
    public string SearchText { get; set; }
    public string CategorySlug { get; set; }
    public bool CheckDate { get; set; }
    public List<string> Tags { get; set; }
}