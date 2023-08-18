using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
public class GetTrainingsSearchQuery : IRequest<List<TrainingVacancyListDto>>
{
    public string SearchInout { get; set; }
}
