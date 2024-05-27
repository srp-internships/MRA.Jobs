using System.Collections;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Queries.GetJobs;

public class GetJobsQueryOptions : PagedListQuery<JobVacancyListDto>
{
    public string Tags { get; set; }
}