using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;

namespace MRA.Jobs.Application.Contracts.InternshipVacancies.Queries;
public class GetInternshipQueryOptions : PagedListQuery<InternshipVacancyListDto>
{
    public string SearchText { get; set; }
    public string CategorySlug { get; set; }
    public bool CheckDate { get; set; }
}
