using MediatR;
using MRA.Jobs.Application.Contracts.TrainingModels.Responses;

namespace MRA.Jobs.Application.Contracts.TrainingModels.Queries;
public class GetTrainingModelByIdQuery : IRequest<TrainingModelDetailsDTO>
{
    public Guid Id { get; set; }
}
