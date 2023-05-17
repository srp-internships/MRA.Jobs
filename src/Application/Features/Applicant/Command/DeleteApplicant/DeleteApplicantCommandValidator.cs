using MRA.Jobs.Application.Contracts.Applicant.Commands;

namespace MRA.Jobs.Application.Features.Applicant.Command.DeleteApplicant;

public class DeleteApplicantCommandValidator : AbstractValidator<DeleteApplicantCommand>
{
    public DeleteApplicantCommandValidator()
    {
        RuleFor(a => a.Id).NotEmpty();
    }
}