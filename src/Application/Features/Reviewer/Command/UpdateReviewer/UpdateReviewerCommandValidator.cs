using MRA.Jobs.Application.Contracts.Reviewer.Command;

namespace MRA.Jobs.Application.Features.Reviewer.Command.UpdateReviewer;

public class UpdateReviewerCommandValidator : AbstractValidator<UpdateReviewerCommand>
{
    public UpdateReviewerCommandValidator()
    {
        RuleFor(r => r.Id).NotEmpty();
    }
}