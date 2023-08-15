using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
public class GetTrainingVacancyBySlugQuery : IRequest<TrainingVacancyDetailedResponse>
{
    public string Slug { get; set; }
}
