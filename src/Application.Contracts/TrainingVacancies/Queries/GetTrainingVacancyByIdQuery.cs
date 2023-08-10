using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;

public class GetTrainingVacancyByIdQuery : IRequest<TrainingVacancyDetailedResponse>
{
    public Guid Id { get; set; }
}