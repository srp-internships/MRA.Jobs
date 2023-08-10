using MRA.Jobs.Application.Contracts.Reviewer.Command;

namespace MRA.Jobs.Application.Features.Reviewer.Command.CreateReviewer;

public class CreateReviewerCommandValidator : AbstractValidator<CreateReviewerCommand>
{
    public CreateReviewerCommandValidator()
    {
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
        RuleFor(r => r.DateOfBirth)
            .NotEmpty();
    }
}