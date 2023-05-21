namespace MRA.Jobs.Application.Contracts.TrainingModels.Commands;
public class UpdateTrainingModelCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid CategoryId { get; set; }
    public int Duration { get; set; }
    public int Fees { get; set; }
}
