namespace MRA.Jobs.Application.Contracts.Reviewer.Command;

public class DeleteReviewerCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}