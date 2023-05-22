namespace MRA.Jobs.Application.Contracts.Reviewer.Command;
public class AddTagToReviewerCommand : IRequest<bool>
{
    public Guid ReviewerId { get; set; }
    public Guid TagId { get; set; }
}
