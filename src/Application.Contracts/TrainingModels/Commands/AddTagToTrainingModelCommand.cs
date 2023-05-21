namespace MRA.Jobs.Application.Contracts.TrainingModels.Commands;
public class AddTagToTrainingModelCommand : IRequest<bool>
{
    public Guid TrainingModelId { get; set; }
    public Guid TagId { get; set; }
}