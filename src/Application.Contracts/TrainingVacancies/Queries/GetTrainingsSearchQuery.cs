using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using Sieve.Models;

namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
public class GetTrainingsSearchQuery : SieveModel, IRequest<PagedList<TrainingVacancyListDto>>
{
    public string SearchInout { get; set; }
}
