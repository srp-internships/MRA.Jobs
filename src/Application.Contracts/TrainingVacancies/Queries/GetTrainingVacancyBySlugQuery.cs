using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
public class GetTrainingVacancyBySlugQuery : IRequest<TrainingVacancyDetailedResponce>
{
    public string Slug { get; set; }
}
