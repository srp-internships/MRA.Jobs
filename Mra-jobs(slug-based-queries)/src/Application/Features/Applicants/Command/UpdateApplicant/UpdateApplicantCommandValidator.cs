using MRA.Jobs.Application.Contracts.Applicant.Commands;

namespace MRA.Jobs.Application.Features.Applicants.Command.UpdateApplicant;

public class UpdateApplicantCommandValidator : AbstractValidator<UpdateApplicantCommand>
{
    public UpdateApplicantCommandValidator()
    {
        RuleFor(a => a.Id).NotEmpty();
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