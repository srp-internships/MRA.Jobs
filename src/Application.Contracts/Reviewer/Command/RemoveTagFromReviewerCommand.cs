namespace MRA.Jobs.Application.Contracts.Reviewer.Command;
public class RemoveTagFromReviewerCommand : IRequest<bool>
{
    public Guid ReviewerId { get; set; }
    public Guid TagId { get; set; }
}
