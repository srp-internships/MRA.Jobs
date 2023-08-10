namespace MRA.Jobs.Application.Contracts.Reviewer.Command;

public class AddTagsToReviewerCommand : IRequest<bool>
{
    public Guid ReviewerId { get; set; }
    public string[] Tags { get; set; }
}