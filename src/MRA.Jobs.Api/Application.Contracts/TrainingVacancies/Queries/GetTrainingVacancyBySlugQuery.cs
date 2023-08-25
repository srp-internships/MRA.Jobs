using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
public record GetTrainingVacancyBySlugQuery : IRequest<TrainingVacancyDetailedResponse>
{
    public string Slug { get; set; }
}
