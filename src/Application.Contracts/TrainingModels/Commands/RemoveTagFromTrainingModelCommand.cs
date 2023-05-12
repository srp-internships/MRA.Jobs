using MediatR;

namespace MRA.Jobs.Application.Contracts.TrainingModels.Commands;
public class RemoveTagFromTrainingModelCommand : IRequest<bool>
{
    public Guid TrainingModelId { get; set; }
    public Guid TagId { get; set; }
}
