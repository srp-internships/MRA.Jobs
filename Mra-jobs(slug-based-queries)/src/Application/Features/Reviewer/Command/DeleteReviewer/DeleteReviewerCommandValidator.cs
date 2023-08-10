using MRA.Jobs.Application.Contracts.Reviewer.Command;

namespace MRA.Jobs.Application.Features.Reviewer.Command.DeleteReviewer;

public class DeleteReviewerCommandValidator : AbstractValidator<DeleteReviewerCommand>
{
    public DeleteReviewerCommandValidator()
    {
        RuleFor(a => a.Id).NotEmpty();
    }
}