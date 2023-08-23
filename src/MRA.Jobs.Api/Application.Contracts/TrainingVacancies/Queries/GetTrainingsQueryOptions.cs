using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
public class GetTrainingsQueryOptions : PagedListQuery<TrainingVacancyListDto>
{
    public string SearchText { get; set; }
    public string CategorySlug { get; set; }
    public bool CheckDate { get; set; }
}
