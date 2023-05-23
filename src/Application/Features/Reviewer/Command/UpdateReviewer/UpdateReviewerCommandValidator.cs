using MRA.Jobs.Application.Contracts.Reviewer.Command;

namespace MRA.Jobs.Application.Features.Reviewer.Command.UpdateReviewer;

public class UpdateReviewerCommandValidator : AbstractValidator<UpdateReviewerCommand>
{
    public UpdateReviewerCommandValidator()
    {
        RuleFor(a => a.Id)
            .NotEmpty();
        RuleFor(a => a.Avatar)
            .MaximumLength(100)
            .NotEmpty();
        RuleFor(a => a.LastName)
            .MaximumLength(100)
            .NotEmpty();
        RuleFor(a => a.FirstName)
            .MaximumLength(100)
            .NotEmpty();
        RuleFor(a => a.Patronymic)
            .MaximumLength(100)
            .NotEmpty();
        RuleFor(a => a.DateOfBirth)
            .NotEmpty();
        RuleFor(a => a.Email)
            .EmailAddress()
            .NotEmpty();
        RuleFor(a => a.PhoneNumber)
            .NotEmpty();
    }
}