namespace MRA.Jobs.Application.Contracts.TrainingModels.Commands;
public class DeleteTrainingModelCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
