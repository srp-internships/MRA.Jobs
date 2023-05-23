using MRA.Jobs.Application.Contracts.Applicant.Commands;

namespace MRA.Jobs.Application.Features.Applicant.Command.Tags;

public class AddTagToApplicantCommandValidator : AbstractValidator<AddTagsToApplicantCommand>
{
    public AddTagToApplicantCommandValidator()
    {
        RuleFor(x => x.ApplicantId).NotEmpty();
        RuleFor(x => x.Tags).NotEmpty();
    }
}
