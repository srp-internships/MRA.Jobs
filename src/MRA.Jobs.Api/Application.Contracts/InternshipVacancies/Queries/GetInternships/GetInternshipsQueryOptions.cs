using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.Application.Contracts.InternshipVacancies.Queries.GetInternships;

public class GetInternshipsQueryOptions : PagedListQuery<InternshipVacancyListResponse>
{
    public string SearchText { get; set; }
    public string CategorySlug { get; set; }
    public bool CheckDate { get; set; }
}