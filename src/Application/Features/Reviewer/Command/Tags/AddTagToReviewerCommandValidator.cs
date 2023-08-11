using MRA.Jobs.Application.Contracts.Reviewer.Command;

namespace MRA.Jobs.Application.Features.Reviewer.Command.Tags;

public class AddTagToReviewerCommandValidator : AbstractValidator<AddTagsToReviewerCommand>
{
    public AddTagToReviewerCommandValidator()
    {
        RuleFor(x => x.ReviewerId).NotEmpty();
        RuleFor(x => x.Tags).NotEmpty();
    }
}