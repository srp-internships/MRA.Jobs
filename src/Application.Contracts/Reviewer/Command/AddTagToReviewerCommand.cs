namespace MRA.Jobs.Application.Contracts.Reviewer.Commands;
public class AddTagToReviewerCommand : IRequest<bool>
{
    public Guid ReviewerId { get; set; }
    public Guid TagId { get; set; }
}
