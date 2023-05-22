using MRA.Jobs.Application.Contracts.Applicant.Commands;

namespace MRA.Jobs.Application.Features.Applicants.Command.Tags;

public class AddTagToApplicantCommandValidator : AbstractValidator<AddTagToApplicantCommand>
{
    public AddTagToApplicantCommandValidator()
    {
        RuleFor(x => x.ApplicantId).NotEmpty();
        RuleFor(x => x.TagId).NotEmpty();
    }
}
