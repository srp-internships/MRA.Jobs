using MRA.Jobs.Application.Contracts.TrainingModels.Responses;

namespace MRA.Jobs.Application.Contracts.TrainingModels.Queries;
public class GetTrainingVacancyByIdQuery : IRequest<TrainingModelDetailsDTO>
{
    public Guid Id { get; set; }
}
