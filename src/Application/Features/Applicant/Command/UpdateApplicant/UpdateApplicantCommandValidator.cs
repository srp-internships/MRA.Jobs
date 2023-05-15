using MRA.Jobs.Application.Contracts.Applicant.Commands;

namespace MRA.Jobs.Application.Features.Applicant.Command.UpdateApplicant;

public class UpdateApplicantCommandValidator : AbstractValidator<UpdateApplicantCommand>
{
    public UpdateApplicantCommandValidator()
    {
        RuleFor(a => a.Id).NotEmpty();
        RuleFor(a => a.LastName).NotEmpty();
        RuleFor(a => a.FirstName).NotEmpty();
        RuleFor(a => a.DateOfBrith).NotEmpty();
        RuleFor(a => a.Email).NotEmpty();
        RuleFor(a => a.PhoneNumber).NotEmpty();
    }
}