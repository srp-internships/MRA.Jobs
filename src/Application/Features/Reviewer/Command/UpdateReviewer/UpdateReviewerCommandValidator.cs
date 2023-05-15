using MRA.Jobs.Application.Contracts.Reviewer.Command;

namespace MRA.Jobs.Application.Features.Reviewer.Command.UpdateReviewer;

public class UpdateReviewerCommandValidator : AbstractValidator<UpdateReviewerCommand>
{
    public UpdateReviewerCommandValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty();
        RuleFor(r => r.FirstName)
            .MaximumLength(100)
            .NotEmpty();
        RuleFor(r => r.LastName)
            .MaximumLength(100)
            .NotEmpty();
        RuleFor(r => r.PhoneNumber)
            .NotEmpty();
        RuleFor(r => r.Avatar)
            .NotEmpty();
        RuleFor(r => r.Email)
            .EmailAddress()
            .NotEmpty();
        RuleFor(r => r.DateOfBrith)
            .NotEmpty();
    }
}