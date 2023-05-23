using MRA.Jobs.Application.Contracts.Reviewer.Command;

namespace MRA.Jobs.Application.Features.Reviewer.Command.Tags;

public class RemoveTagFromReviewerCommandValidator : AbstractValidator<RemoveTagsFromReviewerCommand>
{
    public RemoveTagFromReviewerCommandValidator()
    {
        RuleFor(x => x.ReviewerId).NotEmpty();
        RuleFor(x => x.Tags).NotEmpty();
    }
}