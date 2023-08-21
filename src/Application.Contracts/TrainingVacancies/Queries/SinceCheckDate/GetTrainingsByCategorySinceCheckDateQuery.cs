using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using Sieve.Models;

namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Queries.SinceCheckDate;
public class GetTrainingsByCategorySinceCheckDateQuery : SieveModel, IRequest<PagedList<TrainingVacancyListDto>>
{
    public string CategorySlug { get; set; }
}
